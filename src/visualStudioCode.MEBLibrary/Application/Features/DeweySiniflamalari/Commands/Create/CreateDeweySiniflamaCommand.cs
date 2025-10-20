using Application.Features.DeweySiniflamalari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.DeweySiniflamalari.Commands.Create;

public class CreateDeweySiniflamaCommand : IRequest<CreatedDeweySiniflamaResponse>
{
    public required string Kod { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public Guid? UstSinifId { get; set; }

    public class CreateDeweySiniflamaCommandHandler : IRequestHandler<CreateDeweySiniflamaCommand, CreatedDeweySiniflamaResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
        private readonly DeweySiniflamaBusinessRules _deweySiniflamaBusinessRules;

        public CreateDeweySiniflamaCommandHandler(IMapper mapper, IDeweySiniflamaRepository deweySiniflamaRepository,
                                             DeweySiniflamaBusinessRules deweySiniflamaBusinessRules)
        {
            _mapper = mapper;
            _deweySiniflamaRepository = deweySiniflamaRepository;
            _deweySiniflamaBusinessRules = deweySiniflamaBusinessRules;
        }

        public async Task<CreatedDeweySiniflamaResponse> Handle(CreateDeweySiniflamaCommand request, CancellationToken cancellationToken)
        {
            DeweySiniflama deweySiniflama = _mapper.Map<DeweySiniflama>(request);

            await _deweySiniflamaRepository.AddAsync(deweySiniflama);

            CreatedDeweySiniflamaResponse response = _mapper.Map<CreatedDeweySiniflamaResponse>(deweySiniflama);
            return response;
        }
    }
}
