using Application.Features.YeniKatalogTalepleri.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.YeniKatalogTalepleri.Rules;

public class YeniKatalogTalebiBusinessRules : BaseBusinessRules
{
    private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
    private readonly ILocalizationService _localizationService;

    public YeniKatalogTalebiBusinessRules(IYeniKatalogTalebiRepository yeniKatalogTalebiRepository, ILocalizationService localizationService)
    {
        _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, YeniKatalogTalebisBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task YeniKatalogTalebiShouldExistWhenSelected(YeniKatalogTalebi? yeniKatalogTalebi)
    {
        if (yeniKatalogTalebi == null)
            await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiNotExists);
    }

    public async Task YeniKatalogTalebiIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);
    }
}
