using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.Nushalar.Queries.GetList;

public class GetListNushaQuery : IRequest<GetListResponse<GetListNushaListItemDto>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListNushaQueryHandler : IRequestHandler<GetListNushaQuery, GetListResponse<GetListNushaListItemDto>>
    {
        private readonly INushaRepository _nushaRepository;
        private readonly IMapper _mapper;

        public GetListNushaQueryHandler(INushaRepository nushaRepository, IMapper mapper)
        {
            _nushaRepository = nushaRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListNushaListItemDto>> Handle(GetListNushaQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Nusha> nushas = await _nushaRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListNushaListItemDto> response = _mapper.Map<GetListResponse<GetListNushaListItemDto>>(nushas);
            return response;
        }
    }
}
