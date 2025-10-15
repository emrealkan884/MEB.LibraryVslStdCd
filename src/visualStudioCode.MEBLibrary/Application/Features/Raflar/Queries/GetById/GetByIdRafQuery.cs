using Application.Features.Raflar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Raflar.Queries.GetById;

public class GetByIdRafQuery : IRequest<GetByIdRafResponse>
{
    public Guid Id { get; set; }

    public class GetByIdRafQueryHandler : IRequestHandler<GetByIdRafQuery, GetByIdRafResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRafRepository _rafRepository;
        private readonly RafBusinessRules _rafBusinessRules;

        public GetByIdRafQueryHandler(IMapper mapper, IRafRepository rafRepository, RafBusinessRules rafBusinessRules)
        {
            _mapper = mapper;
            _rafRepository = rafRepository;
            _rafBusinessRules = rafBusinessRules;
        }

        public async Task<GetByIdRafResponse> Handle(GetByIdRafQuery request, CancellationToken cancellationToken)
        {
            Raf? raf = await _rafRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _rafBusinessRules.RafShouldExistWhenSelected(raf);

            GetByIdRafResponse response = _mapper.Map<GetByIdRafResponse>(raf);
            return response;
        }
    }
}
