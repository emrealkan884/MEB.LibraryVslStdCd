using Application.Features.YeniKatalogTalepleri.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.YeniKatalogTalepleri.Queries.GetListByDynamic;

public class GetListByDynamicYeniKatalogTalebiQuery : IRequest<GetListResponse<GetListYeniKatalogTalebiListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicYeniKatalogTalebiQueryHandler
        : IRequestHandler<GetListByDynamicYeniKatalogTalebiQuery, GetListResponse<GetListYeniKatalogTalebiListItemDto>>
    {
        private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicYeniKatalogTalebiQueryHandler(IYeniKatalogTalebiRepository yeniKatalogTalebiRepository, IMapper mapper)
        {
            _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListYeniKatalogTalebiListItemDto>> Handle(
            GetListByDynamicYeniKatalogTalebiQuery request,
            CancellationToken cancellationToken
        )
        {
            IPaginate<YeniKatalogTalebi> yeniKatalogTalebis = await _yeniKatalogTalebiRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListYeniKatalogTalebiListItemDto> response =
                _mapper.Map<GetListResponse<GetListYeniKatalogTalebiListItemDto>>(yeniKatalogTalebis);
            return response;
        }
    }
}

