using Application.Features.KatalogKonulari.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.KatalogKonulari.Rules;

public class KatalogKonuBusinessRules : BaseBusinessRules
{
    private readonly IKatalogKonuRepository _katalogKonuRepository;
    private readonly ILocalizationService _localizationService;

    public KatalogKonuBusinessRules(IKatalogKonuRepository katalogKonuRepository, ILocalizationService localizationService)
    {
        _katalogKonuRepository = katalogKonuRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, KatalogKonusBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task KatalogKonuShouldExistWhenSelected(KatalogKonu? katalogKonu)
    {
        if (katalogKonu == null)
            await throwBusinessException(KatalogKonusBusinessMessages.KatalogKonuNotExists);
    }

    public async Task KatalogKonuIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        KatalogKonu? katalogKonu = await _katalogKonuRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await KatalogKonuShouldExistWhenSelected(katalogKonu);
    }
}
