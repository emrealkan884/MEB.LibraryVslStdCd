using Application.Features.Rezervasyonlar.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Rezervasyonlar.Queries.GetListByDynamic;

public class GetListByDynamicRezervasyonQuery : IRequest<GetListResponse<GetListRezervasyonListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicRezervasyonQueryHandler : IRequestHandler<GetListByDynamicRezervasyonQuery, GetListResponse<GetListRezervasyonListItemDto>>
    {
        private readonly IRezervasyonRepository _rezervasyonRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicRezervasyonQueryHandler(IRezervasyonRepository rezervasyonRepository, IMapper mapper)
        {
            _rezervasyonRepository = rezervasyonRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRezervasyonListItemDto>> Handle(GetListByDynamicRezervasyonQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Rezervasyon> rezervasyons = await _rezervasyonRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRezervasyonListItemDto> response = _mapper.Map<GetListResponse<GetListRezervasyonListItemDto>>(rezervasyons);
            return response;
        }
    }
}


