using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Pipelines.Caching;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Kutuphaneler.Queries.GetList;

public class GetListKutuphaneQuery : IRequest<GetListResponse<GetListKutuphaneListItemDto>>, ICachableRequest
{
    public required PageRequest PageRequest { get; set; }

    public bool BypassCache { get; set; }
    public string CacheKey => $"GetListKutuphaneler-{PageRequest.PageIndex}-{PageRequest.PageSize}";
    public string? CacheGroupKey => "GetKutuphaneler";
    public TimeSpan? SlidingExpiration { get; set; }

    public class GetListKutuphaneQueryHandler : IRequestHandler<GetListKutuphaneQuery, GetListResponse<GetListKutuphaneListItemDto>>
    {
        private readonly IKutuphaneRepository _kutuphaneRepository;
        private readonly IMapper _mapper;

        public GetListKutuphaneQueryHandler(IKutuphaneRepository kutuphaneRepository, IMapper mapper)
        {
            _kutuphaneRepository = kutuphaneRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKutuphaneListItemDto>> Handle(GetListKutuphaneQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Kutuphane> kutuphanes = await _kutuphaneRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKutuphaneListItemDto> response = _mapper.Map<GetListResponse<GetListKutuphaneListItemDto>>(kutuphanes);
            return response;
        }
    }
}
