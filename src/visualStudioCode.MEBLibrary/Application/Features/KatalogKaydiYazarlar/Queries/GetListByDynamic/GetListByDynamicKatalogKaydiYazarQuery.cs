using Application.Features.KatalogKaydiYazarlar.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKaydiYazarlar.Queries.GetListByDynamic;

public class GetListByDynamicKatalogKaydiYazarQuery : IRequest<GetListResponse<GetListKatalogKaydiYazarListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicKatalogKaydiYazarQueryHandler : IRequestHandler<GetListByDynamicKatalogKaydiYazarQuery, GetListResponse<GetListKatalogKaydiYazarListItemDto>>
    {
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicKatalogKaydiYazarQueryHandler(IKatalogKaydiYazarRepository katalogKaydiYazarRepository, IMapper mapper)
        {
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKatalogKaydiYazarListItemDto>> Handle(GetListByDynamicKatalogKaydiYazarQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KatalogKaydiYazar> katalogKaydiYazars = await _katalogKaydiYazarRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKatalogKaydiYazarListItemDto> response = _mapper.Map<GetListResponse<GetListKatalogKaydiYazarListItemDto>>(katalogKaydiYazars);
            return response;
        }
    }
}


