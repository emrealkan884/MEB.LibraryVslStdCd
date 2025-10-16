using Application.Features.OduncIslemler.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OduncIslemler.Queries.GetListByDynamic;

public class GetListByDynamicOduncIslemiQuery : IRequest<GetListResponse<GetListOduncIslemiListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicOduncIslemiQueryHandler : IRequestHandler<GetListByDynamicOduncIslemiQuery, GetListResponse<GetListOduncIslemiListItemDto>>
    {
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicOduncIslemiQueryHandler(IOduncIslemiRepository oduncIslemiRepository, IMapper mapper)
        {
            _oduncIslemiRepository = oduncIslemiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListOduncIslemiListItemDto>> Handle(GetListByDynamicOduncIslemiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OduncIslemi> oduncIslemis = await _oduncIslemiRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListOduncIslemiListItemDto> response = _mapper.Map<GetListResponse<GetListOduncIslemiListItemDto>>(oduncIslemis);
            return response;
        }
    }
}


