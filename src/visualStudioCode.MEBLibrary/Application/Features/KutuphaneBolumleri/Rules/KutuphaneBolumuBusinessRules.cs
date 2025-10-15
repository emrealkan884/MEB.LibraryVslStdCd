using Application.Features.KutuphaneBolumleri.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.KutuphaneBolumleri.Rules;

public class KutuphaneBolumuBusinessRules : BaseBusinessRules
{
    private readonly IKutuphaneBolumuRepository _kutuphaneBolumuRepository;
    private readonly ILocalizationService _localizationService;

    public KutuphaneBolumuBusinessRules(IKutuphaneBolumuRepository kutuphaneBolumuRepository, ILocalizationService localizationService)
    {
        _kutuphaneBolumuRepository = kutuphaneBolumuRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, KutuphaneBolumusBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task KutuphaneBolumuShouldExistWhenSelected(KutuphaneBolumu? kutuphaneBolumu)
    {
        if (kutuphaneBolumu == null)
            await throwBusinessException(KutuphaneBolumusBusinessMessages.KutuphaneBolumuNotExists);
    }

    public async Task KutuphaneBolumuIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        KutuphaneBolumu? kutuphaneBolumu = await _kutuphaneBolumuRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await KutuphaneBolumuShouldExistWhenSelected(kutuphaneBolumu);
    }
}
