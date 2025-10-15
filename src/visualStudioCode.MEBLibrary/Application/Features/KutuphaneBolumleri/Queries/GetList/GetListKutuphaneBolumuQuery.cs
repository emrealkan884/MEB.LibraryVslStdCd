using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.KutuphaneBolumleri.Queries.GetList;

public class GetListKutuphaneBolumuQuery : IRequest<GetListResponse<GetListKutuphaneBolumuListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListKutuphaneBolumuQueryHandler : IRequestHandler<GetListKutuphaneBolumuQuery, GetListResponse<GetListKutuphaneBolumuListItemDto>>
    {
        private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
        private readonly IMapper _mapper;

        public GetListKutuphaneBolumuQueryHandler(IKutuphaneBolumuRepository kutuphaneBolumuRepository, IMapper mapper)
        {
            _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListKutuphaneBolumuListItemDto>> Handle(GetListKutuphaneBolumuQuery request, CancellationToken cancellationToken)
        {
            IPaginate<KutuphaneBolumu> kutuphaneBolumus = await _kutuphaneBolumuRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListKutuphaneBolumuListItemDto> response = _mapper.Map<GetListResponse<GetListKutuphaneBolumuListItemDto>>(kutuphaneBolumus);
            return response;
        }
    }
}
