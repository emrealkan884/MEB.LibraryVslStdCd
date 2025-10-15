using Application.Features.OtoriteKayitlari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.OtoriteKayitlari.Commands.Create;

public class CreateOtoriteKaydiCommand : IRequest<CreatedOtoriteKaydiResponse>
{
    public required string YetkiliBaslik { get; set; }
    public OtoriteTuru OtoriteTuru { get; set; }
    public string? AlternatifBasliklar { get; set; }
    public string? BagliTerimler { get; set; }
    public string? Aciklama { get; set; }
    public string? HariciKayitNo { get; set; }

    public class CreateOtoriteKaydiCommandHandler : IRequestHandler<CreateOtoriteKaydiCommand, CreatedOtoriteKaydiResponse>
    {
        private readonly IMapper _mapper;
        private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
        private readonly OtoriteKaydiBusinessRules _otoriteKaydiBusinessRules;

        public CreateOtoriteKaydiCommandHandler(IMapper mapper, IOtoriteKaydiRepository otoriteKaydiRepository,
                                             OtoriteKaydiBusinessRules otoriteKaydiBusinessRules)
        {
            _mapper = mapper;
            _otoriteKaydiRepository = otoriteKaydiRepository;
            _otoriteKaydiBusinessRules = otoriteKaydiBusinessRules;
        }

        public async Task<CreatedOtoriteKaydiResponse> Handle(CreateOtoriteKaydiCommand request, CancellationToken cancellationToken)
        {
            OtoriteKaydi otoriteKaydi = _mapper.Map<OtoriteKaydi>(request);

            await _otoriteKaydiRepository.AddAsync(otoriteKaydi);

            CreatedOtoriteKaydiResponse response = _mapper.Map<CreatedOtoriteKaydiResponse>(otoriteKaydi);
            return response;
        }
    }
}
