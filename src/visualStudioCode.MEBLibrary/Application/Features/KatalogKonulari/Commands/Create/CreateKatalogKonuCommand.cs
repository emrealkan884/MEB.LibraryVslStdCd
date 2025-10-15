using Application.Features.KatalogKonulari.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.KatalogKonulari.Commands.Create;

public class CreateKatalogKonuCommand : IRequest<CreatedKatalogKonuResponse>
{
    public Guid KatalogKaydiId { get; set; }
    public required string KonuBasligi { get; set; }
    public Guid? OtoriteKaydiId { get; set; }

    public class CreateKatalogKonuCommandHandler : IRequestHandler<CreateKatalogKonuCommand, CreatedKatalogKonuResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKonuRepository _katalogKonuRepository;
        private readonly KatalogKonuBusinessRules _katalogKonuBusinessRules;

        public CreateKatalogKonuCommandHandler(IMapper mapper, IKatalogKonuRepository katalogKonuRepository,
                                             KatalogKonuBusinessRules katalogKonuBusinessRules)
        {
            _mapper = mapper;
            _katalogKonuRepository = katalogKonuRepository;
            _katalogKonuBusinessRules = katalogKonuBusinessRules;
        }

        public async Task<CreatedKatalogKonuResponse> Handle(CreateKatalogKonuCommand request, CancellationToken cancellationToken)
        {
            KatalogKonu katalogKonu = _mapper.Map<KatalogKonu>(request);

            await _katalogKonuRepository.AddAsync(katalogKonu);

            CreatedKatalogKonuResponse response = _mapper.Map<CreatedKatalogKonuResponse>(katalogKonu);
            return response;
        }
    }
}
