using Application.Features.Nushalar.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Nushalar.Queries.GetListByDynamic;

public class GetListByDynamicNushaQuery : IRequest<GetListResponse<GetListNushaListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicNushaQueryHandler : IRequestHandler<GetListByDynamicNushaQuery, GetListResponse<GetListNushaListItemDto>>
    {
        private readonly INushaRepository _nushaRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicNushaQueryHandler(INushaRepository nushaRepository, IMapper mapper)
        {
            _nushaRepository = nushaRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListNushaListItemDto>> Handle(GetListByDynamicNushaQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Nusha> nushas = await _nushaRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListNushaListItemDto> response = _mapper.Map<GetListResponse<GetListNushaListItemDto>>(nushas);
            return response;
        }
    }
}


