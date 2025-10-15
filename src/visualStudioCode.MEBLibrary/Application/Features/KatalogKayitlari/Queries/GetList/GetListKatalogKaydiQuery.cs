using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KatalogKayitlari.Queries.GetList;

public class GetListKatalogKaydiQuery : IRequest<GetListResponse<GetListKatalogKaydiListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListKatalogKaydiQueryHandler : IRequestHandler<GetListKatalogKaydiQuery, GetListResponse<GetListKatalogKaydiListItemDto>>
    {
        private readonly IKatalogKaydiRepository _katalogKaydiRepository;
        private readonly IMapper _mapper;

        public GetListKatalogKaydiQueryHandler(IKatalogKaydiRepository katalogKaydiRepository, IMapper mapper)
        {
            _katalogKaydiRepository = katalogKaydiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKatalogKaydiListItemDto>> Handle(GetListKatalogKaydiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KatalogKaydi> katalogKaydis = await _katalogKaydiRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKatalogKaydiListItemDto> response = _mapper.Map<GetListResponse<GetListKatalogKaydiListItemDto>>(katalogKaydis);
            return response;
        }
    }
}
