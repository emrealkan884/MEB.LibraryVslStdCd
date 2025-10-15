using Application.Features.MateryalEtiketleri.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.MateryalEtiketleri.Rules;

public class MateryalEtiketBusinessRules : BaseBusinessRules
{
    private readonly IMateryalEtiketRepository _materyalEtiketRepository;
    private readonly ILocalizationService _localizationService;

    public MateryalEtiketBusinessRules(IMateryalEtiketRepository materyalEtiketRepository, ILocalizationService localizationService)
    {
        _materyalEtiketRepository = materyalEtiketRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, MateryalEtiketsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task MateryalEtiketShouldExistWhenSelected(MateryalEtiket? materyalEtiket)
    {
        if (materyalEtiket == null)
            await throwBusinessException(MateryalEtiketsBusinessMessages.MateryalEtiketNotExists);
    }

    public async Task MateryalEtiketIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        MateryalEtiket? materyalEtiket = await _materyalEtiketRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await MateryalEtiketShouldExistWhenSelected(materyalEtiket);
    }
}
