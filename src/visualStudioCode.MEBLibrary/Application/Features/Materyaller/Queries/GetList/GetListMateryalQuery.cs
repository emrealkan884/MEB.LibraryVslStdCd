using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Materyaller.Queries.GetList;

public class GetListMateryalQuery : IRequest<GetListResponse<GetListMateryalListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListMateryalQueryHandler : IRequestHandler<GetListMateryalQuery, GetListResponse<GetListMateryalListItemDto>>
    {
        private readonly IMateryalRepository _materyalRepository;
        private readonly IMapper _mapper;

        public GetListMateryalQueryHandler(IMateryalRepository materyalRepository, IMapper mapper)
        {
            _materyalRepository = materyalRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMateryalListItemDto>> Handle(GetListMateryalQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Materyal> materyals = await _materyalRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListMateryalListItemDto> response = _mapper.Map<GetListResponse<GetListMateryalListItemDto>>(materyals);
            return response;
        }
    }
}
