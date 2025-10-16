using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Commands.Update;

public class UpdateYeniKatalogTalebiCommand : IRequest<UpdatedYeniKatalogTalebiResponse>
{
    public Guid Id { get; set; }
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

    public class UpdateYeniKatalogTalebiCommandHandler : IRequestHandler<UpdateYeniKatalogTalebiCommand, UpdatedYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public UpdateYeniKatalogTalebiCommandHandler(
            IMapper mapper,
            IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
            YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules
        )
        {
            _mapper = mapper;
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
        }

        public async Task<UpdatedYeniKatalogTalebiResponse> Handle(UpdateYeniKatalogTalebiCommand request, CancellationToken cancellationToken)
        {
            YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);
            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldAllowUpdate(yeniKatalogTalebi!);

            yeniKatalogTalebi!.TalepEdenKutuphaneId = request.TalepEdenKutuphaneId;
            yeniKatalogTalebi.TalepBilgileriniGuncelle(
                request.Baslik,
                request.AltBaslik,
                request.YazarMetni,
                request.Isbn,
                request.Issn,
                request.MateryalTuru,
                request.MateryalAltTuru,
                request.Dil,
                request.Yayinevi,
                request.YayinYeri,
                request.YayinYili,
                request.Aciklama
            );

            await _yeniKatalogTalebiRepository.UpdateAsync(yeniKatalogTalebi);

            UpdatedYeniKatalogTalebiResponse response = _mapper.Map<UpdatedYeniKatalogTalebiResponse>(yeniKatalogTalebi);
            return response;
        }
    }
}
