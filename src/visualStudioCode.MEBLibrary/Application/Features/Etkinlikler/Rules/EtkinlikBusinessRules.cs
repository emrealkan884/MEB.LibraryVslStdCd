using Application.Features.Etkinlikler.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Etkinlikler.Rules;

public class EtkinlikBusinessRules : BaseBusinessRules
{
    private readonly IEtkinlikRepository _etkinlikRepository;
    private readonly ILocalizationService _localizationService;

    public EtkinlikBusinessRules(IEtkinlikRepository etkinlikRepository, ILocalizationService localizationService)
    {
        _etkinlikRepository = etkinlikRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, EtkinliksBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task EtkinlikShouldExistWhenSelected(Etkinlik? etkinlik)
    {
        if (etkinlik == null)
            await throwBusinessException(EtkinliksBusinessMessages.EtkinlikNotExists);
    }

    public async Task EtkinlikIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Etkinlik? etkinlik = await _etkinlikRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await EtkinlikShouldExistWhenSelected(etkinlik);
    }
}
