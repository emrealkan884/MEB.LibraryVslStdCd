using Application.Features.MateryalFormatDetaylari.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.MateryalFormatDetaylari.Rules;

public class MateryalFormatDetayBusinessRules : BaseBusinessRules
{
    private readonly IMateryalFormatDetayRepository _materyalFormatDetayRepository;
    private readonly ILocalizationService _localizationService;

    public MateryalFormatDetayBusinessRules(IMateryalFormatDetayRepository materyalFormatDetayRepository, ILocalizationService localizationService)
    {
        _materyalFormatDetayRepository = materyalFormatDetayRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, MateryalFormatDetaysBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task MateryalFormatDetayShouldExistWhenSelected(MateryalFormatDetay? materyalFormatDetay)
    {
        if (materyalFormatDetay == null)
            await throwBusinessException(MateryalFormatDetaysBusinessMessages.MateryalFormatDetayNotExists);
    }

    public async Task MateryalFormatDetayIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        MateryalFormatDetay? materyalFormatDetay = await _materyalFormatDetayRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await MateryalFormatDetayShouldExistWhenSelected(materyalFormatDetay);
    }
}
