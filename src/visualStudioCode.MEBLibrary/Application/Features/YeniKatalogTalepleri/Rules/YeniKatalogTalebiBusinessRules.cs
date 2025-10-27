using Application.Features.YeniKatalogTalepleri.Utilities;
using Application.Features.YeniKatalogTalepleri.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.YeniKatalogTalepleri.Rules;

public class YeniKatalogTalebiBusinessRules : BaseBusinessRules
{
    private readonly IYeniKatalogTalebiRepository _yeniKatalogTalebiRepository;
    private readonly IKutuphaneRepository _kutuphaneRepository;
    private readonly ILocalizationService _localizationService;

    public YeniKatalogTalebiBusinessRules(
        IYeniKatalogTalebiRepository yeniKatalogTalebiRepository,
        IKutuphaneRepository kutuphaneRepository,
        ILocalizationService localizationService
    )
    {
        _yeniKatalogTalebiRepository = yeniKatalogTalebiRepository;
        _kutuphaneRepository = kutuphaneRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, YeniKatalogTalebisBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task YeniKatalogTalebiShouldExistWhenSelected(YeniKatalogTalebi? yeniKatalogTalebi)
    {
        if (yeniKatalogTalebi == null)
            await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiNotExists);
    }

    public async Task YeniKatalogTalebiIdShouldExistWhenSelected(Guid id, CancellationToken cancellationToken)
    {
        YeniKatalogTalebi? yeniKatalogTalebi = await _yeniKatalogTalebiRepository.GetAsync(
            predicate: x => x.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);
    }

    public Task YeniKatalogTalebiShouldNotBeFinalized(YeniKatalogTalebi yeniKatalogTalebi)
    {
        if (yeniKatalogTalebi.Durum is TalepDurumu.Onaylandi or TalepDurumu.Reddedildi)
            return throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiAlreadyFinalized);

        return Task.CompletedTask;
    }

    public Task YeniKatalogTalebiShouldAllowUpdate(YeniKatalogTalebi yeniKatalogTalebi)
    {
        if (yeniKatalogTalebi.Durum is TalepDurumu.Onaylandi or TalepDurumu.Reddedildi)
            return throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiInvalidStatusForUpdate);

        return Task.CompletedTask;
    }

    public Task YeniKatalogTalebiShouldNotHaveCatalogRecord(YeniKatalogTalebi yeniKatalogTalebi)
    {
        if (yeniKatalogTalebi.KatalogKaydiId.HasValue)
            return throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiCatalogAlreadyCreated);

        return Task.CompletedTask;
    }

    public async Task YeniKatalogTalebiShouldBeApprovable(YeniKatalogTalebi yeniKatalogTalebi)
    {
        await YeniKatalogTalebiShouldExistWhenSelected(yeniKatalogTalebi);
        await YeniKatalogTalebiShouldNotHaveCatalogRecord(yeniKatalogTalebi!);

        if (yeniKatalogTalebi!.Durum is not (TalepDurumu.Beklemede or TalepDurumu.Inceleniyor))
            await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiInvalidStatusForApproval);
    }

    public async Task YeniKatalogTalebiShouldBeUnique(Guid talepEdenKutuphaneId, string baslik, string? isbn, CancellationToken cancellationToken)
    {
        string sanitizedBaslikLower = baslik.Trim().ToLowerInvariant();
        string? normalizedIsbn = YeniKatalogTalebiSanitizer.NormalizeIsbn(isbn);

        bool exists = await _yeniKatalogTalebiRepository.AnyAsync(
            predicate: x =>
                x.TalepEdenKutuphaneId == talepEdenKutuphaneId
                && x.Baslik.ToLower() == sanitizedBaslikLower
                && (normalizedIsbn == null || (x.Isbn != null && x.Isbn == normalizedIsbn))
                && x.Durum != TalepDurumu.Reddedildi,
            cancellationToken: cancellationToken
        );

        if (exists)
            await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiAlreadyExists);
    }

    public Task YeniKatalogTalebiRejectReasonShouldBeProvided(string? gerekce)
    {
        if (string.IsNullOrWhiteSpace(gerekce))
            return throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiRejectReasonRequired);

        return Task.CompletedTask;
    }

    public Task YeniKatalogTalebiShouldBePendingForReview(YeniKatalogTalebi yeniKatalogTalebi)
    {
        if (yeniKatalogTalebi.Durum != TalepDurumu.Beklemede)
            return throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiInvalidStatusForReview);

        return Task.CompletedTask;
    }

    public async Task<Guid> ResolveOnaylayanKutuphaneIdAsync(Guid? onaylayanKutuphaneId, CancellationToken cancellationToken)
    {
        if (onaylayanKutuphaneId.HasValue && onaylayanKutuphaneId.Value != Guid.Empty)
        {
            Kutuphane? kutuphane = await _kutuphaneRepository.GetAsync(
                predicate: k => k.Id == onaylayanKutuphaneId.Value,
                enableTracking: false,
                cancellationToken: cancellationToken
            );

            if (kutuphane is null)
                await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiApproverNotFound);

            if (kutuphane.Tip != KutuphaneTipi.Merkez)
                await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiApproverMustBeCentral);

            return kutuphane.Id;
        }

        Kutuphane? merkezKutuphane = await _kutuphaneRepository.GetAsync(
            predicate: k => k.Tip == KutuphaneTipi.Merkez,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        if (merkezKutuphane is null)
            await throwBusinessException(YeniKatalogTalebisBusinessMessages.YeniKatalogTalebiCentralLibraryNotConfigured);

        return merkezKutuphane.Id;
    }

}
