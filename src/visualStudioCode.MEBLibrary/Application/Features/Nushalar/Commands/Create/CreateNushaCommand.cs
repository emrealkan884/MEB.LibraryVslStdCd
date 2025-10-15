using Application.Features.Nushalar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Nushalar.Commands.Create;

public class CreateNushaCommand : IRequest<CreatedNushaResponse>
{
    public Guid MateryalId { get; set; }
    public Guid? RafId { get; set; }
    public required string Barkod { get; set; }
    public NushaDurumu Durumu { get; set; }
    public DateTime EklenmeTarihi { get; set; }

    public class CreateNushaCommandHandler : IRequestHandler<CreateNushaCommand, CreatedNushaResponse>
    {
        private readonly IMapper _mapper;
        private readonly INushaRepository _nushaRepository;
        private readonly NushaBusinessRules _nushaBusinessRules;

        public CreateNushaCommandHandler(IMapper mapper, INushaRepository nushaRepository,
                                             NushaBusinessRules nushaBusinessRules)
        {
            _mapper = mapper;
            _nushaRepository = nushaRepository;
            _nushaBusinessRules = nushaBusinessRules;
        }

        public async Task<CreatedNushaResponse> Handle(CreateNushaCommand request, CancellationToken cancellationToken)
        {
            Nusha nusha = _mapper.Map<Nusha>(request);

            await _nushaRepository.AddAsync(nusha);

            CreatedNushaResponse response = _mapper.Map<CreatedNushaResponse>(nusha);
            return response;
        }
    }
}
