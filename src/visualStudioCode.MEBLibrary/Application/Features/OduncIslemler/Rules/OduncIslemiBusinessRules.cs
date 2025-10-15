using Application.Features.OduncIslemler.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.OduncIslemler.Rules;

public class OduncIslemiBusinessRules : BaseBusinessRules
{
    private readonly IOduncIslemiRepository _oduncIslemiRepository;
    private readonly ILocalizationService _localizationService;

    public OduncIslemiBusinessRules(IOduncIslemiRepository oduncIslemiRepository, ILocalizationService localizationService)
    {
        _oduncIslemiRepository = oduncIslemiRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, OduncIslemisBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task OduncIslemiShouldExistWhenSelected(OduncIslemi? oduncIslemi)
    {
        if (oduncIslemi == null)
            await throwBusinessException(OduncIslemisBusinessMessages.OduncIslemiNotExists);
    }

    public async Task OduncIslemiIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        OduncIslemi? oduncIslemi = await _oduncIslemiRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await OduncIslemiShouldExistWhenSelected(oduncIslemi);
    }
}
