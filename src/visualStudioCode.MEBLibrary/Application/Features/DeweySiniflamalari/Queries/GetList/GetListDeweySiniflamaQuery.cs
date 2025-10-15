using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.DeweySiniflamalari.Queries.GetList;

public class GetListDeweySiniflamaQuery : IRequest<GetListResponse<GetListDeweySiniflamaListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListDeweySiniflamaQueryHandler : IRequestHandler<GetListDeweySiniflamaQuery, GetListResponse<GetListDeweySiniflamaListItemDto>>
    {
        private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
        private readonly IMapper _mapper;

        public GetListDeweySiniflamaQueryHandler(IDeweySiniflamaRepository deweySiniflamaRepository, IMapper mapper)
        {
            _deweySiniflamaRepository = deweySiniflamaRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListDeweySiniflamaListItemDto>> Handle(GetListDeweySiniflamaQuery request, CancellationToken cancellationToken)
        {
            IPaginate<DeweySiniflama> deweySiniflamas = await _deweySiniflamaRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListDeweySiniflamaListItemDto> response = _mapper.Map<GetListResponse<GetListDeweySiniflamaListItemDto>>(deweySiniflamas);
            return response;
        }
    }
}
