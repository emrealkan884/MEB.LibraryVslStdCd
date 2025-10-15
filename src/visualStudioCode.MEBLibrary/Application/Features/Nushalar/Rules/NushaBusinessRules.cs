using Application.Features.Nushalar.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.Nushalar.Rules;

public class NushaBusinessRules : BaseBusinessRules
{
    private readonly INushaRepository _nushaRepository;
    private readonly ILocalizationService _localizationService;

    public NushaBusinessRules(INushaRepository nushaRepository, ILocalizationService localizationService)
    {
        _nushaRepository = nushaRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, NushasBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task NushaShouldExistWhenSelected(Nusha? nusha)
    {
        if (nusha == null)
            await throwBusinessException(NushasBusinessMessages.NushaNotExists);
    }

    public async Task NushaIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        Nusha? nusha = await _nushaRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await NushaShouldExistWhenSelected(nusha);
    }
}
