using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKaydiYazarlar.Queries.GetList;

public class GetListKatalogKaydiYazarQuery : IRequest<GetListResponse<GetListKatalogKaydiYazarListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListKatalogKaydiYazarQueryHandler : IRequestHandler<GetListKatalogKaydiYazarQuery, GetListResponse<GetListKatalogKaydiYazarListItemDto>>
    {
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly IMapper _mapper;

        public GetListKatalogKaydiYazarQueryHandler(IKatalogKaydiYazarRepository katalogKaydiYazarRepository, IMapper mapper)
        {
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKatalogKaydiYazarListItemDto>> Handle(GetListKatalogKaydiYazarQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KatalogKaydiYazar> katalogKaydiYazars = await _katalogKaydiYazarRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKatalogKaydiYazarListItemDto> response = _mapper.Map<GetListResponse<GetListKatalogKaydiYazarListItemDto>>(katalogKaydiYazars);
            return response;
        }
    }
}
