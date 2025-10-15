using Application.Features.OtoriteKayitlari.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.OtoriteKayitlari.Rules;

public class OtoriteKaydiBusinessRules : BaseBusinessRules
{
    private readonly IOtoriteKaydiRepository _otoriteKaydiRepository;
    private readonly ILocalizationService _localizationService;

    public OtoriteKaydiBusinessRules(IOtoriteKaydiRepository otoriteKaydiRepository, ILocalizationService localizationService)
    {
        _otoriteKaydiRepository = otoriteKaydiRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, OtoriteKaydisBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task OtoriteKaydiShouldExistWhenSelected(OtoriteKaydi? otoriteKaydi)
    {
        if (otoriteKaydi == null)
            await throwBusinessException(OtoriteKaydisBusinessMessages.OtoriteKaydiNotExists);
    }

    public async Task OtoriteKaydiIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        OtoriteKaydi? otoriteKaydi = await _otoriteKaydiRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await OtoriteKaydiShouldExistWhenSelected(otoriteKaydi);
    }

    public async Task OtoriteKaydiYetkiliBaslikShouldBeUnique(
        string yetkiliBaslik,
        OtoriteTuru otoriteTuru,
        CancellationToken cancellationToken,
        Guid? excludeId = null
    )
    {
        bool alreadyExists = await _otoriteKaydiRepository.AnyAsync(
            predicate: x =>
                x.OtoriteTuru == otoriteTuru
                && x.YetkiliBaslik.ToLower() == yetkiliBaslik.ToLower()
                && (!excludeId.HasValue || x.Id != excludeId),
            cancellationToken: cancellationToken
        );

        if (alreadyExists)
            await throwBusinessException(OtoriteKaydisBusinessMessages.OtoriteKaydiAlreadyExists);
    }
}
