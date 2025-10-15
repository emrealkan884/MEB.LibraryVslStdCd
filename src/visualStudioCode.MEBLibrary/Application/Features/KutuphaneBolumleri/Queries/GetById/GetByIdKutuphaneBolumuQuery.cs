using Application.Features.KutuphaneBolumleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KutuphaneBolumleri.Queries.GetById;

public class GetByIdKutuphaneBolumuQuery : IRequest<GetByIdKutuphaneBolumuResponse>
{
    public Guid Id { get; set; }

    public class GetByIdKutuphaneBolumuQueryHandler : IRequestHandler<GetByIdKutuphaneBolumuQuery, GetByIdKutuphaneBolumuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
        private readonly KutuphaneBolumuBusinessRules _kutuphaneBolumuBusinessRules;

        public GetByIdKutuphaneBolumuQueryHandler(IMapper mapper, IKutuphaneBolumuRepository kutuphaneBolumuRepository, KutuphaneBolumuBusinessRules kutuphaneBolumuBusinessRules)
        {
            _mapper = mapper;
            _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
            _kutuphaneBolumuBusinessRules = kutuphaneBolumuBusinessRules;
        }

        public async Task<GetByIdKutuphaneBolumuResponse> Handle(GetByIdKutuphaneBolumuQuery request, CancellationToken cancellationToken)
        {
            KutuphaneBolumu? kutuphaneBolumu = await _kutuphaneBolumuRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _kutuphaneBolumuBusinessRules.KutuphaneBolumuShouldExistWhenSelected(kutuphaneBolumu);

            GetByIdKutuphaneBolumuResponse response = _mapper.Map<GetByIdKutuphaneBolumuResponse>(kutuphaneBolumu);
            return response;
        }
    }
}
