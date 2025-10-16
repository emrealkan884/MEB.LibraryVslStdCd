using Application.Features.Yazarlar.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Yazarlar.Queries.GetListByDynamic;

public class GetListByDynamicYazarQuery : IRequest<GetListResponse<GetListYazarListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicYazarQueryHandler : IRequestHandler<GetListByDynamicYazarQuery, GetListResponse<GetListYazarListItemDto>>
    {
        private readonly IYazarRepository _yazarRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicYazarQueryHandler(IYazarRepository yazarRepository, IMapper mapper)
        {
            _yazarRepository = yazarRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListYazarListItemDto>> Handle(GetListByDynamicYazarQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Yazar> yazars = await _yazarRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListYazarListItemDto> response = _mapper.Map<GetListResponse<GetListYazarListItemDto>>(yazars);
            return response;
        }
    }
}


