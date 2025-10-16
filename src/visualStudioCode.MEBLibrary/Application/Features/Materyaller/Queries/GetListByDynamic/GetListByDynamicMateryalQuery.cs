using Application.Features.Materyaller.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Materyaller.Queries.GetListByDynamic;

public class GetListByDynamicMateryalQuery : IRequest<GetListResponse<GetListMateryalListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicMateryalQueryHandler : IRequestHandler<GetListByDynamicMateryalQuery, GetListResponse<GetListMateryalListItemDto>>
    {
        private readonly IMateryalRepository _materyalRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicMateryalQueryHandler(IMateryalRepository materyalRepository, IMapper mapper)
        {
            _materyalRepository = materyalRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMateryalListItemDto>> Handle(GetListByDynamicMateryalQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Materyal> materyals = await _materyalRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListMateryalListItemDto> response = _mapper.Map<GetListResponse<GetListMateryalListItemDto>>(materyals);
            return response;
        }
    }
}


