using Application.Features.DeweySiniflamalari.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.DeweySiniflamalari.Rules;

public class DeweySiniflamaBusinessRules : BaseBusinessRules
{
    private readonly IDeweySiniflamaRepository _deweySiniflamaRepository;
    private readonly ILocalizationService _localizationService;

    public DeweySiniflamaBusinessRules(IDeweySiniflamaRepository deweySiniflamaRepository, ILocalizationService localizationService)
    {
        _deweySiniflamaRepository = deweySiniflamaRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, DeweySiniflamasBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task DeweySiniflamaShouldExistWhenSelected(DeweySiniflama? deweySiniflama)
    {
        if (deweySiniflama == null)
            await throwBusinessException(DeweySiniflamasBusinessMessages.DeweySiniflamaNotExists);
    }

    public async Task DeweySiniflamaIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        DeweySiniflama? deweySiniflama = await _deweySiniflamaRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await DeweySiniflamaShouldExistWhenSelected(deweySiniflama);
    }
}
