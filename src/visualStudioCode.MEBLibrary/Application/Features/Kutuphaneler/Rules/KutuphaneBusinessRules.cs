using Application.Features.Kutuphaneler.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Kutuphaneler.Rules;

public class KutuphaneBusinessRules : BaseBusinessRules
{
    private readonly IKutuphaneRepository _kutuphaneRepository;
    private readonly ILocalizationService _localizationService;

    public KutuphaneBusinessRules(IKutuphaneRepository kutuphaneRepository, ILocalizationService localizationService)
    {
        _kutuphaneRepository = kutuphaneRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, KutuphanesBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task KutuphaneShouldExistWhenSelected(Kutuphane? kutuphane)
    {
        if (kutuphane == null)
            await throwBusinessException(KutuphanesBusinessMessages.KutuphaneNotExists);
    }

    public async Task KutuphaneIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Kutuphane? kutuphane = await _kutuphaneRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await KutuphaneShouldExistWhenSelected(kutuphane);
    }
}
