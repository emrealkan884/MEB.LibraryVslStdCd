using Application.Features.DeweySiniflamalari.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.DeweySiniflamalari.Queries.GetListByDynamic;

public class GetListByDynamicDeweySiniflamaQuery : IRequest<GetListResponse<GetListDeweySiniflamaListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicDeweySiniflamaQueryHandler : IRequestHandler<GetListByDynamicDeweySiniflamaQuery, GetListResponse<GetListDeweySiniflamaListItemDto>>
    {
        private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicDeweySiniflamaQueryHandler(IDeweySiniflamaRepository deweySiniflamaRepository, IMapper mapper)
        {
            _deweySiniflamaRepository = deweySiniflamaRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListDeweySiniflamaListItemDto>> Handle(GetListByDynamicDeweySiniflamaQuery request, CancellationToken cancellationToken)
        {
            IPaginate<DeweySiniflama> deweySiniflamas = await _deweySiniflamaRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListDeweySiniflamaListItemDto> response = _mapper.Map<GetListResponse<GetListDeweySiniflamaListItemDto>>(deweySiniflamas);
            return response;
        }
    }
}


