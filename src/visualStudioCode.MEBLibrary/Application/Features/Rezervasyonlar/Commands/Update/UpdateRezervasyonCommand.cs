using Application.Features.Rezervasyonlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Rezervasyonlar.Commands.Update;

public class UpdateRezervasyonCommand : IRequest<UpdatedRezervasyonResponse>
{
    public Guid Id { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid MateryalId { get; set; }
    public DateTime TalepTarihi { get; set; }
    public DateTime? HazirlanmaTarihi { get; set; }
    public DateTime? TamamlanmaTarihi { get; set; }
    public RezervasyonDurumu Durumu { get; set; }
    public string? Not { get; set; }

    public class UpdateRezervasyonCommandHandler : IRequestHandler<UpdateRezervasyonCommand, UpdatedRezervasyonResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRezervasyonRepository _rezervasyonRepository;
        private readonly RezervasyonBusinessRules _rezervasyonBusinessRules;

        public UpdateRezervasyonCommandHandler(IMapper mapper, IRezervasyonRepository rezervasyonRepository,
                                             RezervasyonBusinessRules rezervasyonBusinessRules)
        {
            _mapper = mapper;
            _rezervasyonRepository = rezervasyonRepository;
            _rezervasyonBusinessRules = rezervasyonBusinessRules;
        }

        public async Task<UpdatedRezervasyonResponse> Handle(UpdateRezervasyonCommand request, CancellationToken cancellationToken)
        {
            Rezervasyon? rezervasyon = await _rezervasyonRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _rezervasyonBusinessRules.RezervasyonShouldExistWhenSelected(rezervasyon);
            rezervasyon = _mapper.Map(request, rezervasyon);

            await _rezervasyonRepository.UpdateAsync(rezervasyon!);

            UpdatedRezervasyonResponse response = _mapper.Map<UpdatedRezervasyonResponse>(rezervasyon);
            return response;
        }
    }
}
