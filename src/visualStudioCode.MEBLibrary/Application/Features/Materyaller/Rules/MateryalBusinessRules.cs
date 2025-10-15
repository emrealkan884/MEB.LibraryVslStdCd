using Application.Features.Materyaller.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Materyaller.Rules;

public class MateryalBusinessRules : BaseBusinessRules
{
    private readonly IMateryalRepository _materyalRepository;
    private readonly ILocalizationService _localizationService;

    public MateryalBusinessRules(IMateryalRepository materyalRepository, ILocalizationService localizationService)
    {
        _materyalRepository = materyalRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, MateryalsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task MateryalShouldExistWhenSelected(Materyal? materyal)
    {
        if (materyal == null)
            await throwBusinessException(MateryalsBusinessMessages.MateryalNotExists);
    }

    public async Task MateryalIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Materyal? materyal = await _materyalRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await MateryalShouldExistWhenSelected(materyal);
    }
}
