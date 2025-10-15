using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Raflar.Queries.GetList;

public class GetListRafQuery : IRequest<GetListResponse<GetListRafListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListRafQueryHandler : IRequestHandler<GetListRafQuery, GetListResponse<GetListRafListItemDto>>
    {
        private readonly IRafRepository _rafRepository;
        private readonly IMapper _mapper;

        public GetListRafQueryHandler(IRafRepository rafRepository, IMapper mapper)
        {
            _rafRepository = rafRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRafListItemDto>> Handle(GetListRafQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Raf> rafs = await _rafRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRafListItemDto> response = _mapper.Map<GetListResponse<GetListRafListItemDto>>(rafs);
            return response;
        }
    }
}
