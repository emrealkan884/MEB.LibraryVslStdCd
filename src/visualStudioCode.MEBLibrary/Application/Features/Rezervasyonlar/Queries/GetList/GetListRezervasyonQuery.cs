using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Rezervasyonlar.Queries.GetList;

public class GetListRezervasyonQuery : IRequest<GetListResponse<GetListRezervasyonListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListRezervasyonQueryHandler : IRequestHandler<GetListRezervasyonQuery, GetListResponse<GetListRezervasyonListItemDto>>
    {
        private readonly IRezervasyonRepository _rezervasyonRepository;
        private readonly IMapper _mapper;

        public GetListRezervasyonQueryHandler(IRezervasyonRepository rezervasyonRepository, IMapper mapper)
        {
            _rezervasyonRepository = rezervasyonRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRezervasyonListItemDto>> Handle(GetListRezervasyonQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Rezervasyon> rezervasyons = await _rezervasyonRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRezervasyonListItemDto> response = _mapper.Map<GetListResponse<GetListRezervasyonListItemDto>>(rezervasyons);
            return response;
        }
    }
}
