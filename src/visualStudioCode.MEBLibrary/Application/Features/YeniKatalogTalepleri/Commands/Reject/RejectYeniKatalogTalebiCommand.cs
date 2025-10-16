using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using Application.Services.YeniKatalogTalepleri;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Commands.Reject;

public class RejectYeniKatalogTalebiCommand : IRequest<RejectedYeniKatalogTalebiResponse>
{
    public Guid Id { get; set; }
    public required string Gerekce { get; set; }

    public class RejectYeniKatalogTalebiCommandHandler : IRequestHandler<RejectYeniKatalogTalebiCommand, RejectedYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly IYeniKatalogTalebiWorkflowService _yeniKatalogTalebiWorkflowService;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public RejectYeniKatalogTalebiCommandHandler(
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

        public async Task<RejectedYeniKatalogTalebiResponse> Handle(RejectYeniKatalogTalebiCommand request, CancellationToken cancellationToken)
        {
            YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);

            YeniKatalogTalebi sonuc = await _yeniKatalogTalebiWorkflowService.RejectAsync(
                yeniKatalogTalebi!,
                request.Gerekce,
                cancellationToken
            );

            return _mapper.Map<RejectedYeniKatalogTalebiResponse>(sonuc);
        }
    }
}
