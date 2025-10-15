using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OduncIslemler.Queries.GetList;

public class GetListOduncIslemiQuery : IRequest<GetListResponse<GetListOduncIslemiListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListOduncIslemiQueryHandler : IRequestHandler<GetListOduncIslemiQuery, GetListResponse<GetListOduncIslemiListItemDto>>
    {
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly IMapper _mapper;

        public GetListOduncIslemiQueryHandler(IOduncIslemiRepository oduncIslemiRepository, IMapper mapper)
        {
            _oduncIslemiRepository = oduncIslemiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListOduncIslemiListItemDto>> Handle(GetListOduncIslemiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OduncIslemi> oduncIslemis = await _oduncIslemiRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListOduncIslemiListItemDto> response = _mapper.Map<GetListResponse<GetListOduncIslemiListItemDto>>(oduncIslemis);
            return response;
        }
    }
}
