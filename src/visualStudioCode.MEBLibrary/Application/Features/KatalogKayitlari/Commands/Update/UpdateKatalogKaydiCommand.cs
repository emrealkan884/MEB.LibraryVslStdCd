using Application.Features.KatalogKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.KatalogKayitlari.Commands.Update;

public class UpdateKatalogKaydiCommand : IRequest<UpdatedKatalogKaydiResponse>
{
    public Guid Id { get; set; }
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
    public bool RdaUyumlu { get; set; }
    public DateTime KayitTarihi { get; set; }

    public class UpdateKatalogKaydiCommandHandler : IRequestHandler<UpdateKatalogKaydiCommand, UpdatedKatalogKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly KatalogKaydiBusinessRules _katalogKaydiBusinessRules;

        public UpdateKatalogKaydiCommandHandler(IMapper mapper, IKatalogKaydiRepository katalogKaydiRepository,
                                             KatalogKaydiBusinessRules katalogKaydiBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiRepository = katalogKaydiRepository;
            _katalogKaydiBusinessRules = katalogKaydiBusinessRules;
        }

        public async Task<UpdatedKatalogKaydiResponse> Handle(UpdateKatalogKaydiCommand request, CancellationToken cancellationToken)
        {
            KatalogKaydi? katalogKaydi = await _katalogKaydiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKaydiBusinessRules.KatalogKaydiShouldExistWhenSelected(katalogKaydi);
            katalogKaydi = _mapper.Map(request, katalogKaydi);

            await _katalogKaydiRepository.UpdateAsync(katalogKaydi!);

            UpdatedKatalogKaydiResponse response = _mapper.Map<UpdatedKatalogKaydiResponse>(katalogKaydi);
            return response;
        }
    }
}
