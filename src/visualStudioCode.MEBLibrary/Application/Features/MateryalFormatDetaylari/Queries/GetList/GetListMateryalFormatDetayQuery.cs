using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.MateryalFormatDetaylari.Queries.GetList;

public class GetListMateryalFormatDetayQuery : IRequest<GetListResponse<GetListMateryalFormatDetayListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListMateryalFormatDetayQueryHandler : IRequestHandler<GetListMateryalFormatDetayQuery, GetListResponse<GetListMateryalFormatDetayListItemDto>>
    {
        private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
        private readonly IMapper _mapper;

        public GetListMateryalFormatDetayQueryHandler(IMateryalFormatDetayRepository materyalFormatDetayRepository, IMapper mapper)
        {
            _materyalFormatDetayRepository = materyalFormatDetayRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMateryalFormatDetayListItemDto>> Handle(GetListMateryalFormatDetayQuery request, CancellationToken cancellationToken)
        {
            IPaginate<MateryalFormatDetay> materyalFormatDetays = await _materyalFormatDetayRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListMateryalFormatDetayListItemDto> response = _mapper.Map<GetListResponse<GetListMateryalFormatDetayListItemDto>>(materyalFormatDetays);
            return response;
        }
    }
}
