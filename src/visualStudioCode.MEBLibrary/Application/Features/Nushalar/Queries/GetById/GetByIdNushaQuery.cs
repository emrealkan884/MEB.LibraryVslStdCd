using Application.Features.Nushalar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Nushalar.Queries.GetById;

public class GetByIdNushaQuery : IRequest<GetByIdNushaResponse>
{
    public Guid Id { get; set; }

    public class GetByIdNushaQueryHandler : IRequestHandler<GetByIdNushaQuery, GetByIdNushaResponse>
    {
        private readonly IMapper _mapper;
        private readonly INushaRepository _nushaRepository;
        private readonly NushaBusinessRules _nushaBusinessRules;

        public GetByIdNushaQueryHandler(IMapper mapper, INushaRepository nushaRepository, NushaBusinessRules nushaBusinessRules)
        {
            _mapper = mapper;
            _nushaRepository = nushaRepository;
            _nushaBusinessRules = nushaBusinessRules;
        }

        public async Task<GetByIdNushaResponse> Handle(GetByIdNushaQuery request, CancellationToken cancellationToken)
        {
            Nusha? nusha = await _nushaRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _nushaBusinessRules.NushaShouldExistWhenSelected(nusha);

            GetByIdNushaResponse response = _mapper.Map<GetByIdNushaResponse>(nusha);
            return response;
        }
    }
}
