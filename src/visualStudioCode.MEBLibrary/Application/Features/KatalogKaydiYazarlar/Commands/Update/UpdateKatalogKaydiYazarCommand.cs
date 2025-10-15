using Application.Features.KatalogKaydiYazarlar.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.KatalogKaydiYazarlar.Commands.Update;

public class UpdateKatalogKaydiYazarCommand : IRequest<UpdatedKatalogKaydiYazarResponse>
{
    public Guid Id { get; set; }
    public Guid KatalogKaydiId { get; set; }
    public Guid YazarId { get; set; }
    public Guid? OtoriteKaydiId { get; set; }
    public YazarRolu Rol { get; set; }
    public int Sira { get; set; }

    public class UpdateKatalogKaydiYazarCommandHandler : IRequestHandler<UpdateKatalogKaydiYazarCommand, UpdatedKatalogKaydiYazarResponse>
    {
        private readonly IMapper _mapper;
        private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
        private readonly KatalogKaydiYazarBusinessRules _katalogKaydiYazarBusinessRules;

        public UpdateKatalogKaydiYazarCommandHandler(IMapper mapper, IKatalogKaydiYazarRepository katalogKaydiYazarRepository,
                                             KatalogKaydiYazarBusinessRules katalogKaydiYazarBusinessRules)
        {
            _mapper = mapper;
            _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
            _katalogKaydiYazarBusinessRules = katalogKaydiYazarBusinessRules;
        }

        public async Task<UpdatedKatalogKaydiYazarResponse> Handle(UpdateKatalogKaydiYazarCommand request, CancellationToken cancellationToken)
        {
            KatalogKaydiYazar? katalogKaydiYazar = await _katalogKaydiYazarRepository.GetAsync(predicate: x => x.Id == request.Id, cancellationToken: cancellationToken);
            await _katalogKaydiYazarBusinessRules.KatalogKaydiYazarShouldExistWhenSelected(katalogKaydiYazar);
            await _katalogKaydiYazarBusinessRules.KatalogKaydiYazarShouldReferenceExistingOtorite(
                request.OtoriteKaydiId,
                cancellationToken
            );
            katalogKaydiYazar = _mapper.Map(request, katalogKaydiYazar);

            await _katalogKaydiYazarRepository.UpdateAsync(katalogKaydiYazar!);

            UpdatedKatalogKaydiYazarResponse response = _mapper.Map<UpdatedKatalogKaydiYazarResponse>(katalogKaydiYazar);
            return response;
        }
    }
}
