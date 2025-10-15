using Application.Features.DeweySiniflamalari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.DeweySiniflamalari.Commands.Update;

public class UpdateDeweySiniflamaCommand : IRequest<UpdatedDeweySiniflamaResponse>
{
    public Guid Id { get; set; }
    public required string Kod { get; set; }
    public required string Baslik { get; set; }
    public string? Aciklama { get; set; }
    public int? UstSinifId { get; set; }

    public class UpdateDeweySiniflamaCommandHandler : IRequestHandler<UpdateDeweySiniflamaCommand, UpdatedDeweySiniflamaResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
        private readonly DeweySiniflamaBusinessRules _deweySiniflamaBusinessRules;

        public UpdateDeweySiniflamaCommandHandler(IMapper mapper, IDeweySiniflamaRepository deweySiniflamaRepository,
                                             DeweySiniflamaBusinessRules deweySiniflamaBusinessRules)
        {
            _mapper = mapper;
            _deweySiniflamaRepository = deweySiniflamaRepository;
            _deweySiniflamaBusinessRules = deweySiniflamaBusinessRules;
        }

        public async Task<UpdatedDeweySiniflamaResponse> Handle(UpdateDeweySiniflamaCommand request, CancellationToken cancellationToken)
        {
            DeweySiniflama? deweySiniflama = await _deweySiniflamaRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _deweySiniflamaBusinessRules.DeweySiniflamaShouldExistWhenSelected(deweySiniflama);
            deweySiniflama = _mapper.Map(request, deweySiniflama);

            await _deweySiniflamaRepository.UpdateAsync(deweySiniflama!);

            UpdatedDeweySiniflamaResponse response = _mapper.Map<UpdatedDeweySiniflamaResponse>(deweySiniflama);
            return response;
        }
    }
}
