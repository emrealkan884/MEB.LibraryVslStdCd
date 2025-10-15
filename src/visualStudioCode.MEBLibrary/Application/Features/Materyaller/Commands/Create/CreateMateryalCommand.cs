using Application.Features.Materyaller.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Materyaller.Commands.Create;

public class CreateMateryalCommand : IRequest<CreatedMateryalResponse>
{
    public Guid KatalogKaydiId { get; set; }
    public Guid KutuphaneId { get; set; }
    public Guid? KutuphaneBolumuId { get; set; }
    public MateryalFormati Formati { get; set; }
    public bool RezervasyonaAcik { get; set; }
    public int MaksimumOduncSuresiGun { get; set; }
    public string? Not { get; set; }

    public class CreateMateryalCommandHandler : IRequestHandler<CreateMateryalCommand, CreatedMateryalResponse>
    {
        private readonly IMapper _mapper;
        private readonly IMateryalRepository _materyalRepository;
        private readonly MateryalBusinessRules _materyalBusinessRules;

        public CreateMateryalCommandHandler(IMapper mapper, IMateryalRepository materyalRepository,
                                             MateryalBusinessRules materyalBusinessRules)
        {
            _mapper = mapper;
            _materyalRepository = materyalRepository;
            _materyalBusinessRules = materyalBusinessRules;
        }

        public async Task<CreatedMateryalResponse> Handle(CreateMateryalCommand request, CancellationToken cancellationToken)
        {
            Materyal materyal = _mapper.Map<Materyal>(request);

            await _materyalRepository.AddAsync(materyal);

            CreatedMateryalResponse response = _mapper.Map<CreatedMateryalResponse>(materyal);
            return response;
        }
    }
}
