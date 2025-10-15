using Application.Features.Raflar.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Raflar.Rules;

public class RafBusinessRules : BaseBusinessRules
{
    private readonly IRafRepository _rafRepository;
    private readonly ILocalizationService _localizationService;

    public RafBusinessRules(IRafRepository rafRepository, ILocalizationService localizationService)
    {
        _rafRepository = rafRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, RafsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task RafShouldExistWhenSelected(Raf? raf)
    {
        if (raf == null)
            await throwBusinessException(RafsBusinessMessages.RafNotExists);
    }

    public async Task RafIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Raf? raf = await _rafRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await RafShouldExistWhenSelected(raf);
    }
}
