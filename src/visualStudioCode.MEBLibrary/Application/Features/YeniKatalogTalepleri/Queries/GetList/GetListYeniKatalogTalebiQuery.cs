using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.YeniKatalogTalepleri.Queries.GetList;

public class GetListYeniKatalogTalebiQuery : IRequest<GetListResponse<GetListYeniKatalogTalebiListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListYeniKatalogTalebiQueryHandler : IRequestHandler<GetListYeniKatalogTalebiQuery, GetListResponse<GetListYeniKatalogTalebiListItemDto>>
    {
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly IMapper _mapper;

        public GetListYeniKatalogTalebiQueryHandler(IYeniKatalogTalebiRepository yeniKatalogTalebiRepository, IMapper mapper)
        {
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListYeniKatalogTalebiListItemDto>> Handle(GetListYeniKatalogTalebiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<YeniKatalogTalebi> yeniKatalogTalebis = await _yeniKatalogTalebiRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListYeniKatalogTalebiListItemDto> response = _mapper.Map<GetListResponse<GetListYeniKatalogTalebiListItemDto>>(yeniKatalogTalebis);
            return response;
        }
    }
}
