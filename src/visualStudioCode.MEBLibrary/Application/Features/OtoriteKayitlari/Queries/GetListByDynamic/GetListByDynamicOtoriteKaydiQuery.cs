using Application.Features.OtoriteKayitlari.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OtoriteKayitlari.Queries.GetListByDynamic;

public class GetListByDynamicOtoriteKaydiQuery : IRequest<GetListResponse<GetListOtoriteKaydiListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicOtoriteKaydiQueryHandler : IRequestHandler<GetListByDynamicOtoriteKaydiQuery, GetListResponse<GetListOtoriteKaydiListItemDto>>
    {
        private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicOtoriteKaydiQueryHandler(IOtoriteKaydiRepository otoriteKaydiRepository, IMapper mapper)
        {
            _otoriteKaydiRepository = otoriteKaydiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListOtoriteKaydiListItemDto>> Handle(GetListByDynamicOtoriteKaydiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OtoriteKaydi> otoriteKaydis = await _otoriteKaydiRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListOtoriteKaydiListItemDto> response = _mapper.Map<GetListResponse<GetListOtoriteKaydiListItemDto>>(otoriteKaydis);
            return response;
        }
    }
}


