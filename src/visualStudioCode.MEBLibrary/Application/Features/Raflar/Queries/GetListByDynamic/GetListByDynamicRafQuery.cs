using Application.Features.Raflar.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Raflar.Queries.GetListByDynamic;

public class GetListByDynamicRafQuery : IRequest<GetListResponse<GetListRafListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicRafQueryHandler : IRequestHandler<GetListByDynamicRafQuery, GetListResponse<GetListRafListItemDto>>
    {
        private readonly IRafRepository _rafRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicRafQueryHandler(IRafRepository rafRepository, IMapper mapper)
        {
            _rafRepository = rafRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRafListItemDto>> Handle(GetListByDynamicRafQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Raf> rafs = await _rafRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRafListItemDto> response = _mapper.Map<GetListResponse<GetListRafListItemDto>>(rafs);
            return response;
        }
    }
}


