using Application.Features.MateryalEtiketleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MateryalEtiketleri.Queries.GetById;

public class GetByIdMateryalEtiketQuery : IRequest<GetByIdMateryalEtiketResponse>
{
    public Guid Id { get; set; }

    public class GetByIdMateryalEtiketQueryHandler : IRequestHandler<GetByIdMateryalEtiketQuery, GetByIdMateryalEtiketResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalEtiketRepository _materyalEtiketRepository;
        private readonly MateryalEtiketBusinessRules _materyalEtiketBusinessRules;

        public GetByIdMateryalEtiketQueryHandler(IMapper mapper, IMateryalEtiketRepository materyalEtiketRepository, MateryalEtiketBusinessRules materyalEtiketBusinessRules)
        {
            _mapper = mapper;
            _materyalEtiketRepository = materyalEtiketRepository;
            _materyalEtiketBusinessRules = materyalEtiketBusinessRules;
        }

        public async Task<GetByIdMateryalEtiketResponse> Handle(GetByIdMateryalEtiketQuery request, CancellationToken cancellationToken)
        {
            MateryalEtiket? materyalEtiket = await _materyalEtiketRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _materyalEtiketBusinessRules.MateryalEtiketShouldExistWhenSelected(materyalEtiket);

            GetByIdMateryalEtiketResponse response = _mapper.Map<GetByIdMateryalEtiketResponse>(materyalEtiket);
            return response;
        }
    }
}
