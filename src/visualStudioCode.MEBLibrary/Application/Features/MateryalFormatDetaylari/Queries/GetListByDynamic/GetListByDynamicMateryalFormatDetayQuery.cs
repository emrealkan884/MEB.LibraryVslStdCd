using Application.Features.MateryalFormatDetaylari.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.MateryalFormatDetaylari.Queries.GetListByDynamic;

public class GetListByDynamicMateryalFormatDetayQuery : IRequest<GetListResponse<GetListMateryalFormatDetayListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicMateryalFormatDetayQueryHandler : IRequestHandler<GetListByDynamicMateryalFormatDetayQuery, GetListResponse<GetListMateryalFormatDetayListItemDto>>
    {
        private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicMateryalFormatDetayQueryHandler(IMateryalFormatDetayRepository materyalFormatDetayRepository, IMapper mapper)
        {
            _materyalFormatDetayRepository = materyalFormatDetayRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListMateryalFormatDetayListItemDto>> Handle(GetListByDynamicMateryalFormatDetayQuery request, CancellationToken cancellationToken)
        {
            IPaginate<MateryalFormatDetay> materyalFormatDetays = await _materyalFormatDetayRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListMateryalFormatDetayListItemDto> response = _mapper.Map<GetListResponse<GetListMateryalFormatDetayListItemDto>>(materyalFormatDetays);
            return response;
        }
    }
}


