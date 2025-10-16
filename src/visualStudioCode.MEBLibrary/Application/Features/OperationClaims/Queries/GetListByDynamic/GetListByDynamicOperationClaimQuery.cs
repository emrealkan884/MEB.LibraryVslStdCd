using Application.Features.OperationClaims.Queries.GetList;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Security;
using MediatR;
using NArchitecture.Core.Application.Requests;
using NArchitecture.Core.Application.Responses;
using NArchitecture.Core.Persistence.Dynamic;
using NArchitecture.Core.Persistence.Paging;

namespace Application.Features.OperationClaims.Queries.GetListByDynamic;

public class GetListByDynamicOperationClaimQuery : IRequest<GetListResponse<GetListOperationClaimListItemDto>>
{
    public required DynamicQuery DynamicQuery { get; set; }
    public required PageRequest PageRequest { get; set; }

    public class GetListByDynamicOperationClaimQueryHandler : IRequestHandler<GetListByDynamicOperationClaimQuery, GetListResponse<GetListOperationClaimListItemDto>>
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IMapper _mapper;

        public GetListByDynamicOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository, IMapper mapper)
        {
            _operationClaimRepository = operationClaimRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListOperationClaimListItemDto>> Handle(GetListByDynamicOperationClaimQuery request, CancellationToken cancellationToken)
        {
            IPaginate<OperationClaim> operationClaims = await _operationClaimRepository.GetListByDynamicAsync(
                request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListOperationClaimListItemDto> response = _mapper.Map<GetListResponse<GetListOperationClaimListItemDto>>(operationClaims);
            return response;
        }
    }
}


