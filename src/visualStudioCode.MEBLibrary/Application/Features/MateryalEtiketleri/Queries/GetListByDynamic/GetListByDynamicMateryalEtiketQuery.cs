using Application.Features.MateryalEtiketleri.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.MateryalEtiketleri.Queries.GetListByDynamic;

public class GetListByDynamicMateryalEtiketQuery : IRequest<GetListResponse<GetListMateryalEtiketListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicMateryalEtiketQueryHandler : IRequestHandler<GetListByDynamicMateryalEtiketQuery, GetListResponse<GetListMateryalEtiketListItemDto>>
    {
        private readonly IMateryalEtiketRepository _materyalEtiketRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicMateryalEtiketQueryHandler(IMateryalEtiketRepository materyalEtiketRepository, IMapper mapper)
        {
            _materyalEtiketRepository = materyalEtiketRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMateryalEtiketListItemDto>> Handle(GetListByDynamicMateryalEtiketQuery request, CancellationToken cancellationToken)
        {
            IPaginate<MateryalEtiket> materyalEtikets = await _materyalEtiketRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListMateryalEtiketListItemDto> response = _mapper.Map<GetListResponse<GetListMateryalEtiketListItemDto>>(materyalEtikets);
            return response;
        }
    }
}


