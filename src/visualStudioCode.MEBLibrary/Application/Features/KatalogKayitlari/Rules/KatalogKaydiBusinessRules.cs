using Application.Features.KatalogKayitlari.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.KatalogKayitlari.Rules;

public class KatalogKaydiBusinessRules : BaseBusinessRules
{
    private readonly IKatalogKaydiRepository _katalogKaydiRepository;
    private readonly ILocalizationService _localizationService;

    public KatalogKaydiBusinessRules(IKatalogKaydiRepository katalogKaydiRepository, ILocalizationService localizationService)
    {
        _katalogKaydiRepository = katalogKaydiRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, KatalogKaydisBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task KatalogKaydiShouldExistWhenSelected(KatalogKaydi? katalogKaydi)
    {
        if (katalogKaydi == null)
            await throwBusinessException(KatalogKaydisBusinessMessages.KatalogKaydiNotExists);
    }

    public async Task KatalogKaydiIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        KatalogKaydi? katalogKaydi = await _katalogKaydiRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await KatalogKaydiShouldExistWhenSelected(katalogKaydi);
    }
}
