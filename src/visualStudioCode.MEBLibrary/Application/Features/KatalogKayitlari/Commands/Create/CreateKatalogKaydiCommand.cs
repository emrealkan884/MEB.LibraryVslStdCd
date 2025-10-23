using Application.Features.KatalogKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects.Marc;
using MediatR;

namespace Application.Features.KatalogKayitlari.Commands.Create;

public class CreateKatalogKaydiCommand : IRequest<CreatedKatalogKaydiResponse>
{
    public Guid? DeweySiniflamaId { get; set; }
    public Guid? YeniKatalogTalebiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? AltBaslik { get; set; }
    public string? Isbn { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public int? SayfaSayisi { get; set; }
    public string? Dil { get; set; }
    public string? Dizi { get; set; }
    public string? Baski { get; set; }
    public string? Ozet { get; set; }
    public string? Notlar { get; set; }
    public string? KapakResmiYolu { get; set; }
    public MateryalTuru MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public string? Marc21Verisi { get; set; }
    public IList<MarcAlan>? MarcAlanlari { get; set; }
    public bool RdaUyumlu { get; set; }
    public DateTime KayitTarihi { get; set; }
    public IList<CreateKatalogKaydiYazarDto>? Yazarlar { get; set; }
    public IList<CreateKatalogKaydiKonuDto>? Konular { get; set; }

    public class CreateKatalogKaydiCommandHandler : IRequestHandler<CreateKatalogKaydiCommand, CreatedKatalogKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly KatalogKaydiBusinessRules _katalogKaydiBusinessRules;

        public CreateKatalogKaydiCommandHandler(IMapper mapper, IKatalogKaydiRepository katalogKaydiRepository,
                                             IKatalogKaydiYazarRepository katalogKaydiYazarRepository,
                                             IKatalogKonuRepository katalogKonuRepository,
                                             KatalogKaydiBusinessRules katalogKaydiBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiRepository = katalogKaydiRepository;
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _katalogKonuRepository = katalogKonuRepository;
            _katalogKaydiBusinessRules = katalogKaydiBusinessRules;
        }

        public async Task<CreatedKatalogKaydiResponse> Handle(CreateKatalogKaydiCommand request, CancellationToken cancellationToken)
        {
            KatalogKaydi katalogKaydi = _mapper.Map<KatalogKaydi>(request);

            katalogKaydi = await _katalogKaydiRepository.AddAsync(katalogKaydi);

            List<KatalogKaydiYazar> createdYazarlar = new();
            if (request.Yazarlar != null)
            {
                foreach (CreateKatalogKaydiYazarDto yazarDto in request.Yazarlar)
                {
                    KatalogKaydiYazar katalogKaydiYazar = new()
                    {
                        Id = Guid.NewGuid(),
                        KatalogKaydiId = katalogKaydi.Id,
                        YazarId = yazarDto.YazarId,
                        Rol = yazarDto.Rol,
                        Sira = yazarDto.Sira,
                        OtoriteKaydiId = yazarDto.OtoriteKaydiId
                    };
                    await _katalogKaydiYazarRepository.AddAsync(katalogKaydiYazar);
                    createdYazarlar.Add(katalogKaydiYazar);
                }
            }

            List<KatalogKonu> createdKonular = new();
            if (request.Konular != null)
            {
                foreach (CreateKatalogKaydiKonuDto konuDto in request.Konular)
                {
                    KatalogKonu katalogKonu = new()
                    {
                        Id = Guid.NewGuid(),
                        KatalogKaydiId = katalogKaydi.Id,
                        KonuBasligi = konuDto.KonuBasligi,
                        OtoriteKaydiId = konuDto.OtoriteKaydiId
                    };
                    await _katalogKonuRepository.AddAsync(katalogKonu);
                    createdKonular.Add(katalogKonu);
                }
            }

            CreatedKatalogKaydiResponse response = _mapper.Map<CreatedKatalogKaydiResponse>(katalogKaydi);
            response.Yazarlar = createdYazarlar
                .Select(y => new CreatedKatalogKaydiYazarDto(y.Id, y.YazarId, y.Rol, y.Sira, y.OtoriteKaydiId))
                .ToList();
            response.Konular = createdKonular
                .Select(k => new CreatedKatalogKaydiKonuDto(k.Id, k.KonuBasligi, k.OtoriteKaydiId))
                .ToList();
            return response;
        }
    }

    public class CreateKatalogKaydiYazarDto
    {
        public Guid YazarId { get; set; }
        public YazarRolu Rol { get; set; } = YazarRolu.Yazar;
        public int Sira { get; set; } = 1;
        public Guid? OtoriteKaydiId { get; set; }
    }

    public class CreateKatalogKaydiKonuDto
    {
        public required string KonuBasligi { get; set; }
        public Guid? OtoriteKaydiId { get; set; }
    }
}
