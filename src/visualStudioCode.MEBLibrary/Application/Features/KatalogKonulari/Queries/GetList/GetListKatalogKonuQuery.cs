using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKonulari.Queries.GetList;

public class GetListKatalogKonuQuery : IRequest<GetListResponse<GetListKatalogKonuListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListKatalogKonuQueryHandler : IRequestHandler<GetListKatalogKonuQuery, GetListResponse<GetListKatalogKonuListItemDto>>
    {
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly IMapper _mapper;

        public GetListKatalogKonuQueryHandler(IKatalogKonuRepository katalogKonuRepository, IMapper mapper)
        {
            _katalogKonuRepository = katalogKonuRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKatalogKonuListItemDto>> Handle(GetListKatalogKonuQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KatalogKonu> katalogKonus = await _katalogKonuRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKatalogKonuListItemDto> response = _mapper.Map<GetListResponse<GetListKatalogKonuListItemDto>>(katalogKonus);
            return response;
        }
    }
}
