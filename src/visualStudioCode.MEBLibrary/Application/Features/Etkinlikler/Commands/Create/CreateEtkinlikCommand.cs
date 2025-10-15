using Application.Features.Etkinlikler.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Etkinlikler.Commands.Create;

public class CreateEtkinlikCommand : IRequest<CreatedEtkinlikResponse>
{
    public Guid KutuphaneId { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public string? Konum { get; set; }
    public string? AfisDosyasi { get; set; }

    public class CreateEtkinlikCommandHandler : IRequestHandler<CreateEtkinlikCommand, CreatedEtkinlikResponse>
    {
        private readonly IMapper _mapper;
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly EtkinlikBusinessRules _etkinlikBusinessRules;

        public CreateEtkinlikCommandHandler(IMapper mapper, IEtkinlikRepository etkinlikRepository,
                                             EtkinlikBusinessRules etkinlikBusinessRules)
        {
            _mapper = mapper;
            _etkinlikRepository = etkinlikRepository;
            _etkinlikBusinessRules = etkinlikBusinessRules;
        }

        public async Task<CreatedEtkinlikResponse> Handle(CreateEtkinlikCommand request, CancellationToken cancellationToken)
        {
            Etkinlik etkinlik = _mapper.Map<Etkinlik>(request);

            await _etkinlikRepository.AddAsync(etkinlik);

            CreatedEtkinlikResponse response = _mapper.Map<CreatedEtkinlikResponse>(etkinlik);
            return response;
        }
    }
}
