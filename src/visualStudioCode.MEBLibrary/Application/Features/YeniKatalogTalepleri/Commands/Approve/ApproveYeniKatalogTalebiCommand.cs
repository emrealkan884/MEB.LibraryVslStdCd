using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Features.YeniKatalogTalepleri.Utilities;
using Application.Services.Repositories;
using Application.Services.YeniKatalogTalepleri;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Commands.Approve;

public class ApproveYeniKatalogTalebiCommand : IRequest<ApprovedYeniKatalogTalebiResponse>
{
    public Guid Id { get; set; }
    public Guid OnaylayanKutuphaneId { get; set; }
    public MateryalTuru? MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public Guid? DeweySiniflamaId { get; set; }
    public string? Marc21Verisi { get; set; }
    public bool RdaUyumlu { get; set; }
    public int? SayfaSayisi { get; set; }
    public string? Notlar { get; set; }

    public class ApproveYeniKatalogTalebiCommandHandler : IRequestHandler<ApproveYeniKatalogTalebiCommand, ApprovedYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly IYeniKatalogTalebiWorkflowService _yeniKatalogTalebiWorkflowService;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public ApproveYeniKatalogTalebiCommandHandler(
            IMapper mapper,
            IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
            IYeniKatalogTalebiWorkflowService yeniKatalogTalebiWorkflowService,
            YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules
        )
        {
            _mapper = mapper;
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _yeniKatalogTalebiWorkflowService = yeniKatalogTalebiWorkflowService;
            _yeniKatalogTalebiBusinessRules = yeniKatalogTalebiBusinessRules;
        }

        public async Task<ApprovedYeniKatalogTalebiResponse> Handle(ApproveYeniKatalogTalebiCommand request, CancellationToken cancellationToken)
        {
            YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);

            MateryalTuru materyalTuru = DetermineMateryalTuru(request.MateryalTuru, yeniKatalogTalebi);
            string? materyalAltTuru = DetermineMateryalAltTuru(request.MateryalAltTuru, yeniKatalogTalebi);

            YeniKatalogTalebi sonuc = await _yeniKatalogTalebiWorkflowService.ApproveAsync(
                yeniKatalogTalebi!,
                request.OnaylayanKutuphaneId,
                materyalTuru,
                materyalAltTuru,
                request.DeweySiniflamaId,
                request.Marc21Verisi,
                request.RdaUyumlu,
                request.SayfaSayisi,
                request.Notlar,
                cancellationToken
            );

            ApprovedYeniKatalogTalebiResponse response = _mapper.Map<ApprovedYeniKatalogTalebiResponse>(sonuc);
            response.KatalogKaydiId = sonuc.KatalogKaydiId!.Value;
            response.OnaylayanKutuphaneId = request.OnaylayanKutuphaneId;

            return response;
        }

        private static MateryalTuru DetermineMateryalTuru(MateryalTuru? requestedValue, YeniKatalogTalebi? talep) =>
            requestedValue ?? MateryalTuruMapper.MapFromTalep(talep);

        private static string? DetermineMateryalAltTuru(string? requestedValue, YeniKatalogTalebi? talep) =>
            !string.IsNullOrWhiteSpace(requestedValue)
                ? requestedValue
                : talep?.MateryalAltTuru;
    }
}
