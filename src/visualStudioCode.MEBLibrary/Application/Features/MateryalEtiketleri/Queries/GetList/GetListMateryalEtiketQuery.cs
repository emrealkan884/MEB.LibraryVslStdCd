using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.MateryalEtiketleri.Queries.GetList;

public class GetListMateryalEtiketQuery : IRequest<GetListResponse<GetListMateryalEtiketListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListMateryalEtiketQueryHandler : IRequestHandler<GetListMateryalEtiketQuery, GetListResponse<GetListMateryalEtiketListItemDto>>
    {
        private readonly IMateryalEtiketRepository _materyalEtiketRepository;
        private readonly IMapper _mapper;

        public GetListMateryalEtiketQueryHandler(IMateryalEtiketRepository materyalEtiketRepository, IMapper mapper)
        {
            _materyalEtiketRepository = materyalEtiketRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMateryalEtiketListItemDto>> Handle(GetListMateryalEtiketQuery request, CancellationToken cancellationToken)
        {
            IPaginate<MateryalEtiket> materyalEtikets = await _materyalEtiketRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListMateryalEtiketListItemDto> response = _mapper.Map<GetListResponse<GetListMateryalEtiketListItemDto>>(materyalEtikets);
            return response;
        }
    }
}
