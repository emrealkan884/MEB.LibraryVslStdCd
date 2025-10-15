using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OtoriteKayitlari.Queries.GetList;

public class GetListOtoriteKaydiQuery : IRequest<GetListResponse<GetListOtoriteKaydiListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListOtoriteKaydiQueryHandler : IRequestHandler<GetListOtoriteKaydiQuery, GetListResponse<GetListOtoriteKaydiListItemDto>>
    {
        private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
        private readonly IMapper _mapper;

        public GetListOtoriteKaydiQueryHandler(IOtoriteKaydiRepository otoriteKaydiRepository, IMapper mapper)
        {
            _otoriteKaydiRepository = otoriteKaydiRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListOtoriteKaydiListItemDto>> Handle(GetListOtoriteKaydiQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OtoriteKaydi> otoriteKaydis = await _otoriteKaydiRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListOtoriteKaydiListItemDto> response = _mapper.Map<GetListResponse<GetListOtoriteKaydiListItemDto>>(otoriteKaydis);
            return response;
        }
    }
}
