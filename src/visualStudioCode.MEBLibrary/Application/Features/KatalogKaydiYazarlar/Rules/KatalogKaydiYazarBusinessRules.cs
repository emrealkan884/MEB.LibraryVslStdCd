using Application.Features.KatalogKaydiYazarlar.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.KatalogKaydiYazarlar.Rules;

public class KatalogKaydiYazarBusinessRules : BaseBusinessRules
{
    private readonly IKatalogKaydiYazarRepository _katalogKaydiYazarRepository;
    private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
    private readonly ILocalizationService _localizationService;

    public KatalogKaydiYazarBusinessRules(
        IKatalogKaydiYazarRepository katalogKaydiYazarRepository,
        IOtoriteKaydiRepository otoriteKaydiRepository,
        ILocalizationService localizationService
    )
    {
        _katalogKaydiYazarRepository = katalogKaydiYazarRepository;
        _otoriteKaydiRepository = otoriteKaydiRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, KatalogKaydiYazarsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task KatalogKaydiYazarShouldExistWhenSelected(KatalogKaydiYazar? katalogKaydiYazar)
    {
        if (katalogKaydiYazar == null)
            await throwBusinessException(KatalogKaydiYazarsBusinessMessages.KatalogKaydiYazarNotExists);
    }

    public async Task KatalogKaydiYazarIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        KatalogKaydiYazar? katalogKaydiYazar = await _katalogKaydiYazarRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await KatalogKaydiYazarShouldExistWhenSelected(katalogKaydiYazar);
    }

    public async Task KatalogKaydiYazarShouldReferenceExistingOtorite(
        Guid? otoriteKaydiId,
        CancellationToken cancellationToken
    )
    {
        if (!otoriteKaydiId.HasValue)
            await throwBusinessException(KatalogKaydiYazarsBusinessMessages.OtoriteKaydiRequired);

        bool exists = await _otoriteKaydiRepository.AnyAsync(
            predicate: x => x.Id == otoriteKaydiId.Value,
            cancellationToken: cancellationToken
        );

        if (!exists)
            await throwBusinessException(KatalogKaydiYazarsBusinessMessages.OtoriteKaydiNotExists);
    }
}
