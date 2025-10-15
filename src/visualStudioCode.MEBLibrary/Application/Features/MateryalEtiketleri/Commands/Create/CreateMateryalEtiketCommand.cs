using Application.Features.MateryalEtiketleri.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.MateryalEtiketleri.Commands.Create;

public class CreateMateryalEtiketCommand : IRequest<CreatedMateryalEtiketResponse>
{
    public Guid MateryalId { get; set; }
    public required string Etiket { get; set; }

    public class CreateMateryalEtiketCommandHandler : IRequestHandler<CreateMateryalEtiketCommand, CreatedMateryalEtiketResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalEtiketRepository _materyalEtiketRepository;
        private readonly MateryalEtiketBusinessRules _materyalEtiketBusinessRules;

        public CreateMateryalEtiketCommandHandler(IMapper mapper, IMateryalEtiketRepository materyalEtiketRepository,
                                             MateryalEtiketBusinessRules materyalEtiketBusinessRules)
        {
            _mapper = mapper;
            _materyalEtiketRepository = materyalEtiketRepository;
            _materyalEtiketBusinessRules = materyalEtiketBusinessRules;
        }

        public async Task<CreatedMateryalEtiketResponse> Handle(CreateMateryalEtiketCommand request, CancellationToken cancellationToken)
        {
            MateryalEtiket materyalEtiket = _mapper.Map<MateryalEtiket>(request);

            await _materyalEtiketRepository.AddAsync(materyalEtiket);

            CreatedMateryalEtiketResponse response = _mapper.Map<CreatedMateryalEtiketResponse>(materyalEtiket);
            return response;
        }
    }
}
