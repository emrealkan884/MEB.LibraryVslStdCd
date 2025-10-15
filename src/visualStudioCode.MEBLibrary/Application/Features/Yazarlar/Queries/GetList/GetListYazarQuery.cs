using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Yazarlar.Queries.GetList;

public class GetListYazarQuery : IRequest<GetListResponse<GetListYazarListItemDto>>
{
    public PageRequest PageRequest { get; set; }

    public class GetListYazarQueryHandler : IRequestHandler<GetListYazarQuery, GetListResponse<GetListYazarListItemDto>>
    {
        private readonly IYazarRepository _yazarRepository;
        private readonly IMapper _mapper;

        public GetListYazarQueryHandler(IYazarRepository yazarRepository, IMapper mapper)
        {
            _yazarRepository = yazarRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListYazarListItemDto>> Handle(GetListYazarQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Yazar> yazars = await _yazarRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListYazarListItemDto> response = _mapper.Map<GetListResponse<GetListYazarListItemDto>>(yazars);
            return response;
        }
    }
}