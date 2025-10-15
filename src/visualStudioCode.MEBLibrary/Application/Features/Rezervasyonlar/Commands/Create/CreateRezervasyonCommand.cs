using Application.Features.Rezervasyonlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Rezervasyonlar.Commands.Create;

public class CreateRezervasyonCommand : IRequest<CreatedRezervasyonResponse>
{
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid MateryalId { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? HazirlanmaTarihi { get; set; }
    public DateTime? TamamlanmaTarihi { get; set; }
    public RezervasyonDurumu Durumu { get; set; }
    public string? Not { get; set; }

    public class CreateRezervasyonCommandHandler : IRequestHandler<CreateRezervasyonCommand, CreatedRezervasyonResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRezervasyonRepository _rezervasyonRepository;
        private readonly RezervasyonBusinessRules _rezervasyonBusinessRules;

        public CreateRezervasyonCommandHandler(IMapper mapper, IRezervasyonRepository rezervasyonRepository,
                                             RezervasyonBusinessRules rezervasyonBusinessRules)
        {
            _mapper = mapper;
            _rezervasyonRepository = rezervasyonRepository;
            _rezervasyonBusinessRules = rezervasyonBusinessRules;
        }

        public async Task<CreatedRezervasyonResponse> Handle(CreateRezervasyonCommand request, CancellationToken cancellationToken)
        {
            Rezervasyon rezervasyon = _mapper.Map<Rezervasyon>(request);

            await _rezervasyonRepository.AddAsync(rezervasyon);

            CreatedRezervasyonResponse response = _mapper.Map<CreatedRezervasyonResponse>(rezervasyon);
            return response;
        }
    }
}
