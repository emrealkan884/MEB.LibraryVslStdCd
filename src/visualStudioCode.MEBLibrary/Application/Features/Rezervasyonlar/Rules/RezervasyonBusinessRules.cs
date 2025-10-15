using Application.Features.Rezervasyonlar.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Rezervasyonlar.Rules;

public class RezervasyonBusinessRules : BaseBusinessRules
{
    private readonly IRezervasyonRepository _rezervasyonRepository;
    private readonly ILocalizationService _localizationService;

    public RezervasyonBusinessRules(IRezervasyonRepository rezervasyonRepository, ILocalizationService localizationService)
    {
        _rezervasyonRepository = rezervasyonRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, RezervasyonsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task RezervasyonShouldExistWhenSelected(Rezervasyon? rezervasyon)
    {
        if (rezervasyon == null)
            await throwBusinessException(RezervasyonsBusinessMessages.RezervasyonNotExists);
    }

    public async Task RezervasyonIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Rezervasyon? rezervasyon = await _rezervasyonRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await RezervasyonShouldExistWhenSelected(rezervasyon);
    }
}
