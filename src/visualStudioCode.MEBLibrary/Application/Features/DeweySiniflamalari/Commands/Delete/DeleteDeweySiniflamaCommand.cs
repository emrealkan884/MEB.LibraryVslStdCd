using Application.Features.DeweySiniflamalari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.DeweySiniflamalari.Commands.Delete;

public class DeleteDeweySiniflamaCommand : IRequest<DeletedDeweySiniflamaResponse>
{
    public Guid Id { get; set; }

    public class DeleteDeweySiniflamaCommandHandler : IRequestHandler<DeleteDeweySiniflamaCommand, DeletedDeweySiniflamaResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
        private readonly DeweySiniflamaBusinessRules _deweySiniflamaBusinessRules;

        public DeleteDeweySiniflamaCommandHandler(IMapper mapper, IDeweySiniflamaRepository deweySiniflamaRepository,
                                             DeweySiniflamaBusinessRules deweySiniflamaBusinessRules)
        {
            _mapper = mapper;
            _deweySiniflamaRepository = deweySiniflamaRepository;
            _deweySiniflamaBusinessRules = deweySiniflamaBusinessRules;
        }

        public async Task<DeletedDeweySiniflamaResponse> Handle(DeleteDeweySiniflamaCommand request, CancellationToken cancellationToken)
        {
            DeweySiniflama? deweySiniflama = await _deweySiniflamaRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _deweySiniflamaBusinessRules.DeweySiniflamaShouldExistWhenSelected(deweySiniflama);

            await _deweySiniflamaRepository.DeleteAsync(deweySiniflama!);

            DeletedDeweySiniflamaResponse response = _mapper.Map<DeletedDeweySiniflamaResponse>(deweySiniflama);
            return response;
        }
    }
}
