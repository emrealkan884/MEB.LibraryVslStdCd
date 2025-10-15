using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Commands.Create;

public class CreateYeniKatalogTalebiCommand : IRequest<CreatedYeniKatalogTalebiResponse>
{
    public Guid TalepEdenKutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? AltBaslik { get; set; }
    public string? YazarMetni { get; set; }
    public string? Isbn { get; set; }
    public string? Issn { get; set; }
    public string? MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public string? Dil { get; set; }
    public string? Yayinevi { get; set; }
    public string? YayinYeri { get; set; }
    public int? YayinYili { get; set; }
    public string? Aciklama { get; set; }
    public TalepDurumu Durum { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? SonGuncellemeTarihi { get; set; }
    public Guid? KatalogKaydiId { get; set; }

    public class CreateYeniKatalogTalebiCommandHandler : IRequestHandler<CreateYeniKatalogTalebiCommand, CreatedYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public CreateYeniKatalogTalebiCommandHandler(IMapper mapper, IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
                                             YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules)
        {
            _mapper = mapper;
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
        }

        public async Task<CreatedYeniKatalogTalebiResponse> Handle(CreateYeniKatalogTalebiCommand request, CancellationToken cancellationToken)
        {
            YeniKatalogTalebi yeniKatalogTalebi = _mapper.Map<YeniKatalogTalebi>(request);

            await _yeniKatalogTalebiRepository.AddAsync(yeniKatalogTalebi);

            CreatedYeniKatalogTalebiResponse response = _mapper.Map<CreatedYeniKatalogTalebiResponse>(yeniKatalogTalebi);
            return response;
        }
    }
}
