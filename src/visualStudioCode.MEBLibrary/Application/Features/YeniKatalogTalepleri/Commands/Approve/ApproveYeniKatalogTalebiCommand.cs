using Application.Features.YeniKatalogTalepleri.Events;
using Application.Features.YeniKatalogTalepleri.Rules;
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
    public MateryalTuru MateryalTuru { get; set; }
    public string? MateryalAltTuru { get; set; }
    public Guid? DeweySiniflamaId { get; set; }
    public string? Marc21Verisi { get; set; }
    public bool RdaUyumlu { get; set; }
    public int? SayfaSayisi { get; set; }
    public string? Notlar { get; set; }

    public class ApproveYeniKatalogTalebiCommandHandler : IRequestHandler<ApproveYeniKatalogTalebiCommand, ApprovedYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly IYeniKatalogTalebiWorkflowService _yeniKatalogTalebiWorkflowService;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public ApproveYeniKatalogTalebiCommandHandler(
            IMapper mapper,
            IMediator mediator,
            IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
            IYeniKatalogTalebiWorkflowService yeniKatalogTalebiWorkflowService,
            YeniKatalogTalebiBusinessRules yeniKatalogTalebiBusinessRules
        )
        {
            _mapper = mapper;
            _mediator = mediator;
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

            Guid resolvedKutuphaneId = await _yeniKatalogTalebiBusinessRules.ResolveOnaylayanKutuphaneIdAsync(
                null,
                cancellationToken
            );

            YeniKatalogTalebiApprovalResult sonuc = await _yeniKatalogTalebiWorkflowService.ApproveAsync(
                yeniKatalogTalebi!,
                resolvedKutuphaneId,
                request.MateryalTuru,
                request.MateryalAltTuru,
                request.DeweySiniflamaId,
                request.Marc21Verisi,
                request.RdaUyumlu,
                request.SayfaSayisi,
                request.Notlar,
                cancellationToken
            );

            ApprovedYeniKatalogTalebiResponse response = _mapper.Map<ApprovedYeniKatalogTalebiResponse>(sonuc.Talep);
            response.KatalogKaydiId = sonuc.KatalogKaydi.Id;
            response.MateryalId = sonuc.Materyal.Id;
            response.OnaylayanKutuphaneId = resolvedKutuphaneId;

            await _mediator.Publish(
                new CatalogApprovedNotification(
                    sonuc.Talep.Id,
                    sonuc.Talep.TalepEdenKutuphaneId,
                    sonuc.KatalogKaydi.Id,
                    sonuc.Materyal.Id
                ),
                cancellationToken
            );

            return response;
        }
    }
}
