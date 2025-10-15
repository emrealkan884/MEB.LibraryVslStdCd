using Application.Features.OduncIslemler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.OduncIslemler.Commands.Create;

public class CreateOduncIslemiCommand : IRequest<CreatedOduncIslemiResponse>
{
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }
    public DateTime AlinmaTarihi { get; set; }
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
    public OduncDurumu Durumu { get; set; }
    public string? Not { get; set; }

    public class CreateOduncIslemiCommandHandler : IRequestHandler<CreateOduncIslemiCommand, CreatedOduncIslemiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOduncIslemiRepository _oduncIslemiRepository;
        private readonly OduncIslemiBusinessRules _oduncIslemiBusinessRules;

        public CreateOduncIslemiCommandHandler(IMapper mapper, IOduncIslemiRepository oduncIslemiRepository,
                                             OduncIslemiBusinessRules oduncIslemiBusinessRules)
        {
            _mapper = mapper;
            _oduncIslemiRepository = oduncIslemiRepository;
            _oduncIslemiBusinessRules = oduncIslemiBusinessRules;
        }

        public async Task<CreatedOduncIslemiResponse> Handle(CreateOduncIslemiCommand request, CancellationToken cancellationToken)
        {
            OduncIslemi oduncIslemi = _mapper.Map<OduncIslemi>(request);

            await _oduncIslemiRepository.AddAsync(oduncIslemi);

            CreatedOduncIslemiResponse response = _mapper.Map<CreatedOduncIslemiResponse>(oduncIslemi);
            return response;
        }
    }
}
