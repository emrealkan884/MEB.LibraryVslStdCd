using Application.Features.KatalogKonulari.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKonulari.Queries.GetListByDynamic;

public class GetListByDynamicKatalogKonuQuery : IRequest<GetListResponse<GetListKatalogKonuListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicKatalogKonuQueryHandler : IRequestHandler<GetListByDynamicKatalogKonuQuery, GetListResponse<GetListKatalogKonuListItemDto>>
    {
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicKatalogKonuQueryHandler(IKatalogKonuRepository katalogKonuRepository, IMapper mapper)
        {
            _katalogKonuRepository = katalogKonuRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKatalogKonuListItemDto>> Handle(GetListByDynamicKatalogKonuQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KatalogKonu> katalogKonus = await _katalogKonuRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKatalogKonuListItemDto> response = _mapper.Map<GetListResponse<GetListKatalogKonuListItemDto>>(katalogKonus);
            return response;
        }
    }
}


