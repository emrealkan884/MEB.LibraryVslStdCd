using Application.Features.Etkinlikler.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Etkinlikler.Queries.GetListByDynamic;

public class GetListByDynamicEtkinlikQuery : IRequest<GetListResponse<GetListEtkinlikListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicEtkinlikQueryHandler : IRequestHandler<GetListByDynamicEtkinlikQuery, GetListResponse<GetListEtkinlikListItemDto>>
    {
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicEtkinlikQueryHandler(IEtkinlikRepository etkinlikRepository, IMapper mapper)
        {
            _etkinlikRepository = etkinlikRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListEtkinlikListItemDto>> Handle(GetListByDynamicEtkinlikQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Etkinlik> etkinliks = await _etkinlikRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListEtkinlikListItemDto> response = _mapper.Map<GetListResponse<GetListEtkinlikListItemDto>>(etkinliks);
            return response;
        }
    }
}


