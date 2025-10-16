using Application.Features.KatalogKayitlari.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKayitlari.Queries.GetListByDynamic;

public class GetListByDynamicKatalogKaydiQuery : IRequest<GetListResponse<GetListKatalogKaydiListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicKatalogKaydiQueryHandler : IRequestHandler<GetListByDynamicKatalogKaydiQuery, GetListResponse<GetListKatalogKaydiListItemDto>>
    {
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicKatalogKaydiQueryHandler(IKatalogKaydiRepository katalogKaydiRepository, IMapper mapper)
        {
            _katalogKaydiRepository = katalogKaydiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKatalogKaydiListItemDto>> Handle(GetListByDynamicKatalogKaydiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KatalogKaydi> katalogKaydis = await _katalogKaydiRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKatalogKaydiListItemDto> response = _mapper.Map<GetListResponse<GetListKatalogKaydiListItemDto>>(katalogKaydis);
            return response;
        }
    }
}


