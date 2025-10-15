using Application.Features.Yazarlar.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Yazarlar.Rules;

public class YazarBusinessRules : BaseBusinessRules
{
    private readonly IYazarRepository _yazarRepository;
    private readonly ILocalizationService _localizationService;

    public YazarBusinessRules(IYazarRepository yazarRepository, ILocalizationService localizationService)
    {
        _yazarRepository = yazarRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, YazarsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task YazarShouldExistWhenSelected(Yazar? yazar)
    {
        if (yazar == null)
            await throwBusinessException(YazarsBusinessMessages.YazarNotExists);
    }

    public async Task YazarIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Yazar? yazar = await _yazarRepository.GetAsync(
            predicate: y => y.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await YazarShouldExistWhenSelected(yazar);
    }
}