using Application.Features.YeniKatalogTalepleri.Rules;
using Application.Services.Repositories;
using Application.Services.YeniKatalogTalepleri;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.YeniKatalogTalepleri.Commands.Review;

public class StartReviewYeniKatalogTalebiCommand : IRequest<StartedReviewYeniKatalogTalebiResponse>
{
    public Guid Id { get; set; }

    public class StartReviewYeniKatalogTalebiCommandHandler
        : IRequestHandler<StartReviewYeniKatalogTalebiCommand, StartedReviewYeniKatalogTalebiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly IYeniKatalogTalebiWorkflowService _yeniKatalogTalebiWorkflowService;
        private readonly YeniKatalogTalebiBusinessRules _yeniKatalogTalebiBusinessRules;

        public StartReviewYeniKatalogTalebiCommandHandler(
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

        public async Task<StartedReviewYeniKatalogTalebiResponse> Handle(
            StartReviewYeniKatalogTalebiCommand request,
            CancellationToken cancellationToken
        )
        {
            YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                enableTracking: true,
                cancellationToken: cancellationToken
            );

            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);
            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldNotBeFinalized(yeniKatalogTalebi!);
            await _yeniKatalogTalebiBusinessRules.YeniKatalogTalebiShouldBePendingForReview(yeniKatalogTalebi!);

            YeniKatalogTalebi sonuc = await _yeniKatalogTalebiWorkflowService.StartReviewAsync(
                yeniKatalogTalebi!,
                cancellationToken
            );

            return _mapper.Map<StartedReviewYeniKatalogTalebiResponse>(sonuc);
        }
    }
}
