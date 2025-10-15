using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.OduncIslemler.Queries.GetById;

public class GetByIdOduncIslemiQuery : IRequest<GetByIdOduncIslemiResponse>
{
    public Guid Id { get; set; }

    public class GetByIdOduncIslemiQueryHandler : IRequestHandler<GetByIdOduncIslemiQuery, GetByIdOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public GetByIdOduncIslemiQueryHandler(IMapper mapper, IOduncIslemiRepository oduncIslemiRepository, OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<GetByIdOduncIslemiResponse> Handle(GetByIdOduncIslemiQuery request, CancellationToken cancellationToken)
        {
            OduncIslemi? oduncIslemi = await _oduncIslemiRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _oduncIslemiBusinessRules.OduncIslemiShouldExistWhenSelected(oduncIslemi);

            GetByIdOduncIslemiResponse response = _mapper.Map<GetByIdOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}
