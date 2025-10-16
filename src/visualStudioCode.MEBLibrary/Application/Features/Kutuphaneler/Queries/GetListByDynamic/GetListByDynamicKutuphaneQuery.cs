using Application.Features.Kutuphaneler.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Kutuphaneler.Queries.GetListByDynamic;

public class GetListByDynamicKutuphaneQuery : IRequest<GetListResponse<GetListKutuphaneListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicKutuphaneQueryHandler : IRequestHandler<GetListByDynamicKutuphaneQuery, GetListResponse<GetListKutuphaneListItemDto>>
    {
        private readonly IKutuphaneRepository _kutuphaneRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicKutuphaneQueryHandler(IKutuphaneRepository kutuphaneRepository, IMapper mapper)
        {
            _kutuphaneRepository = kutuphaneRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKutuphaneListItemDto>> Handle(GetListByDynamicKutuphaneQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Kutuphane> kutuphanes = await _kutuphaneRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKutuphaneListItemDto> response = _mapper.Map<GetListResponse<GetListKutuphaneListItemDto>>(kutuphanes);
            return response;
        }
    }
}


