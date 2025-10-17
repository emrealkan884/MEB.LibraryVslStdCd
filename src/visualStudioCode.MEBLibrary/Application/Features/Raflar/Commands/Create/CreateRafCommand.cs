using Application.Features.Raflar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Raflar.Commands.Create;

public class CreateRafCommand : IRequest<CreatedRafResponse>
{
    public Guid KutuphaneBolumuId { get; set; }
    public required string Kod { get; set; }
    public string? Aciklama { get; set; }

    public class CreateRafCommandHandler : IRequestHandler<CreateRafCommand, CreatedRafResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRafRepository _rafRepository;
        private readonly RafBusinessRules _rafBusinessRules;

        public CreateRafCommandHandler(IMapper mapper, IRafRepository rafRepository,
                                             RafBusinessRules rafBusinessRules)
        {
            _mapper = mapper;
            _rafRepository = rafRepository;
            _rafBusinessRules = rafBusinessRules;
        }

        public async Task<CreatedRafResponse> Handle(CreateRafCommand request, CancellationToken cancellationToken)
        {
            Raf raf = _mapper.Map<Raf>(request);

            await _rafRepository.AddAsync(raf);

            CreatedRafResponse response = _mapper.Map<CreatedRafResponse>(raf);
            return response;
        }
    }
}
