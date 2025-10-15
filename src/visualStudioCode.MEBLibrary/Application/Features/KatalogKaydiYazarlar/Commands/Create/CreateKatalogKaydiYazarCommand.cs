using Application.Features.KatalogKaydiYazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Create;

public class CreateKatalogKaydiYazarCommand : IRequest<CreatedKatalogKaydiYazarResponse>
{
    public Guid KatalogKaydiId { get; set; }
    public Guid YazarId { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
    public YazarRolu Rol { get; set; }
    public int Sira { get; set; }

    public class CreateKatalogKaydiYazarCommandHandler : IRequestHandler<CreateKatalogKaydiYazarCommand, CreatedKatalogKaydiYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly KatalogKaydiYazarBusinessRules _katalogKaydiYazarBusinessRules;

        public CreateKatalogKaydiYazarCommandHandler(IMapper mapper, IKatalogKaydiYazarRepository katalogKaydiYazarRepository,
                                             KatalogKaydiYazarBusinessRules katalogKaydiYazarBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _katalogKaydiYazarBusinessRules = katalogKaydiYazarBusinessRules;
        }

        public async Task<CreatedKatalogKaydiYazarResponse> Handle(CreateKatalogKaydiYazarCommand request, CancellationToken cancellationToken)
        {
            await _katalogKaydiYazarBusinessRules.KatalogKaydiYazarShouldReferenceExistingOtorite(
                request.OtoriteKaydiId,
                cancellationToken
            );

            KatalogKaydiYazar katalogKaydiYazar = _mapper.Map<KatalogKaydiYazar>(request);

            await _katalogKaydiYazarRepository.AddAsync(katalogKaydiYazar);

            CreatedKatalogKaydiYazarResponse response = _mapper.Map<CreatedKatalogKaydiYazarResponse>(katalogKaydiYazar);
            return response;
        }
    }
}
