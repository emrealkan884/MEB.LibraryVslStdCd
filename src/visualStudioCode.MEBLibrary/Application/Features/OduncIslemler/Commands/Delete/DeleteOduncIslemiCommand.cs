using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.OduncIslemler.Commands.Delete;

public class DeleteOduncIslemiCommand : IRequest<DeletedOduncIslemiResponse>
{
    public Guid Id { get; set; }

    public class DeleteOduncIslemiCommandHandler : IRequestHandler<DeleteOduncIslemiCommand, DeletedOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public DeleteOduncIslemiCommandHandler(IMapper mapper, IOduncIslemiRepository oduncIslemiRepository,
                                             OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<DeletedOduncIslemiResponse> Handle(DeleteOduncIslemiCommand request, CancellationToken cancellationToken)
        {
            OduncIslemi? oduncIslemi = await _oduncIslemiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _oduncIslemiBusinessRules.OduncIslemiShouldExistWhenSelected(oduncIslemi);

            await _oduncIslemiRepository.DeleteAsync(oduncIslemi!);

            DeletedOduncIslemiResponse response = _mapper.Map<DeletedOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}
