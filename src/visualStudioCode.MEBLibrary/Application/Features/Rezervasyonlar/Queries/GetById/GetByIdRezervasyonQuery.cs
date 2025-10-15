using Application.Features.Rezervasyonlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Rezervasyonlar.Queries.GetById;

public class GetByIdRezervasyonQuery : IRequest<GetByIdRezervasyonResponse>
{
    public Guid Id { get; set; }

    public class GetByIdRezervasyonQueryHandler : IRequestHandler<GetByIdRezervasyonQuery, GetByIdRezervasyonResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRezervasyonRepository _rezervasyonRepository;
        private readonly RezervasyonBusinessRules _rezervasyonBusinessRules;

        public GetByIdRezervasyonQueryHandler(IMapper mapper, IRezervasyonRepository rezervasyonRepository, RezervasyonBusinessRules rezervasyonBusinessRules)
        {
            _mapper = mapper;
            _rezervasyonRepository = rezervasyonRepository;
            _rezervasyonBusinessRules = rezervasyonBusinessRules;
        }

        public async Task<GetByIdRezervasyonResponse> Handle(GetByIdRezervasyonQuery request, CancellationToken cancellationToken)
        {
            Rezervasyon? rezervasyon = await _rezervasyonRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _rezervasyonBusinessRules.RezervasyonShouldExistWhenSelected(rezervasyon);

            GetByIdRezervasyonResponse response = _mapper.Map<GetByIdRezervasyonResponse>(rezervasyon);
            return response;
        }
    }
}
