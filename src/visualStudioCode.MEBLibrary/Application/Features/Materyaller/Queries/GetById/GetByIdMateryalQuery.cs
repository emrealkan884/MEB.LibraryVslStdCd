using Application.Features.Materyaller.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Materyaller.Queries.GetById;

public class GetByIdMateryalQuery : IRequest<GetByIdMateryalResponse>
{
    public Guid Id { get; set; }

    public class GetByIdMateryalQueryHandler : IRequestHandler<GetByIdMateryalQuery, GetByIdMateryalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalRepository _materyalRepository;
        private readonly MateryalBusinessRules _materyalBusinessRules;

        public GetByIdMateryalQueryHandler(IMapper mapper, IMateryalRepository materyalRepository, MateryalBusinessRules materyalBusinessRules)
        {
            _mapper = mapper;
            _materyalRepository = materyalRepository;
            _materyalBusinessRules = materyalBusinessRules;
        }

        public async Task<GetByIdMateryalResponse> Handle(GetByIdMateryalQuery request, CancellationToken cancellationToken)
        {
            Materyal? materyal = await _materyalRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalBusinessRules.MateryalShouldExistWhenSelected(materyal);

            GetByIdMateryalResponse response = _mapper.Map<GetByIdMateryalResponse>(materyal);
            return response;
        }
    }
}
