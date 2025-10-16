using Application.Features.KutuphaneBolumleri.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KutuphaneBolumleri.Queries.GetListByDynamic;

public class GetListByDynamicKutuphaneBolumuQuery : IRequest<GetListResponse<GetListKutuphaneBolumuListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicKutuphaneBolumuQueryHandler : IRequestHandler<GetListByDynamicKutuphaneBolumuQuery, GetListResponse<GetListKutuphaneBolumuListItemDto>>
    {
        private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicKutuphaneBolumuQueryHandler(IKutuphaneBolumuRepository kutuphaneBolumuRepository, IMapper mapper)
        {
            _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKutuphaneBolumuListItemDto>> Handle(GetListByDynamicKutuphaneBolumuQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KutuphaneBolumu> kutuphaneBolumus = await _kutuphaneBolumuRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKutuphaneBolumuListItemDto> response = _mapper.Map<GetListResponse<GetListKutuphaneBolumuListItemDto>>(kutuphaneBolumus);
            return response;
        }
    }
}


