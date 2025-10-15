using Application.Features.DeweySiniflamalari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.DeweySiniflamalari.Queries.GetById;

public class GetByIdDeweySiniflamaQuery : IRequest<GetByIdDeweySiniflamaResponse>
{
    public Guid Id { get; set; }

    public class GetByIdDeweySiniflamaQueryHandler : IRequestHandler<GetByIdDeweySiniflamaQuery, GetByIdDeweySiniflamaResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
        private readonly DeweySiniflamaBusinessRules _deweySiniflamaBusinessRules;

        public GetByIdDeweySiniflamaQueryHandler(IMapper mapper, IDeweySiniflamaRepository deweySiniflamaRepository, DeweySiniflamaBusinessRules deweySiniflamaBusinessRules)
        {
            _mapper = mapper;
            _deweySiniflamaRepository = deweySiniflamaRepository;
            _deweySiniflamaBusinessRules = deweySiniflamaBusinessRules;
        }

        public async Task<GetByIdDeweySiniflamaResponse> Handle(GetByIdDeweySiniflamaQuery request, CancellationToken cancellationToken)
        {
            DeweySiniflama? deweySiniflama = await _deweySiniflamaRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _deweySiniflamaBusinessRules.DeweySiniflamaShouldExistWhenSelected(deweySiniflama);

            GetByIdDeweySiniflamaResponse response = _mapper.Map<GetByIdDeweySiniflamaResponse>(deweySiniflama);
            return response;
        }
    }
}
