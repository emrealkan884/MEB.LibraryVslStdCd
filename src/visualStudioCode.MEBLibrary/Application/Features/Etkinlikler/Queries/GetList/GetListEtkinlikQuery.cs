using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Etkinlikler.Queries.GetList;

public class GetListEtkinlikQuery : IRequest<GetListResponse<GetListEtkinlikListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListEtkinlikQueryHandler : IRequestHandler<GetListEtkinlikQuery, GetListResponse<GetListEtkinlikListItemDto>>
    {
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly IMapper _mapper;

        public GetListEtkinlikQueryHandler(IEtkinlikRepository etkinlikRepository, IMapper mapper)
        {
            _etkinlikRepository = etkinlikRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListEtkinlikListItemDto>> Handle(GetListEtkinlikQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Etkinlik> etkinliks = await _etkinlikRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListEtkinlikListItemDto> response = _mapper.Map<GetListResponse<GetListEtkinlikListItemDto>>(etkinliks);
            return response;
        }
    }
}
