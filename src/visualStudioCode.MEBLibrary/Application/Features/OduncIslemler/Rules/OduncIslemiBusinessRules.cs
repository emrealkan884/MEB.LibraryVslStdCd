using Application.Features.OduncIslemler.Constants;
using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using NArchitecture.Core.Application.Rules;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;

namespace Application.Features.OduncIslemler.Rules;

public class OduncIslemiBusinessRules : BaseBusinessRules
{
    private readonly IOduncIslemiRepository _oduncIslemiRepository;
    private readonly INushaRepository _nushaRepository;
    private readonly IRezervasyonRepository _rezervasyonRepository;
    private readonly ILocalizationService _localizationService;

    // Ödünç verme politikaları (konfigüre edilebilir)
    private const int MaxAktifOduncSayisi = 3;
    private const int StandartOduncSuresiGun = 14;
    private const int MaxUzatmaSayisi = 2;
    private const int MaxUzatmaGun = 7;
    private const decimal GunlukCezaMiktari = 1.0m;

    public OduncIslemiBusinessRules(
        IOduncIslemiRepository oduncIslemiRepository,
        INushaRepository nushaRepository,
        IRezervasyonRepository rezervasyonRepository,
        ILocalizationService localizationService)
    {
        _oduncIslemiRepository = oduncIslemiRepository;
        _nushaRepository = nushaRepository;
        _rezervasyonRepository = rezervasyonRepository;
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

    public async Task NushaShouldBeAvailableForLoan(Guid nushaId, CancellationToken cancellationToken)
    {
        // Nusha mevcut olmalı
        var nusha = await _nushaRepository.GetAsync(
            predicate: x => x.Id == nushaId,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        if (nusha == null)
            await throwBusinessException(OduncIslemisBusinessMessages.NushaNotAvailableForLoan);

        // Nusha ödünç verilebilir durumda olmalı (Rafta veya Ayirtildi durumunda olabilir)
        if (nusha.Durumu != NushaDurumu.Rafta && nusha.Durumu != NushaDurumu.Ayirtildi)
            await throwBusinessException(OduncIslemisBusinessMessages.NushaNotAvailableForLoan);

        // Aktif ödünç işlemi olmamalı
        bool hasActiveLoan = await _oduncIslemiRepository.AnyAsync(
            predicate: x => x.NushaId == nushaId && x.Durumu == OduncDurumu.Aktif,
            cancellationToken: cancellationToken
        );

        if (hasActiveLoan)
            await throwBusinessException(OduncIslemisBusinessMessages.NushaAlreadyBorrowed);
    }

    public async Task UserShouldBeEligibleForLoan(Guid kullaniciId, Guid kutuphaneId, CancellationToken cancellationToken)
    {
        // Her kütüphanenin kendi ödünç limiti olabilir - dinamik kontrol
        int maxLoanLimit = await GetLibraryLoanLimitAsync(kutuphaneId, cancellationToken);

        // Kullanıcının bu kütüphaneden aktif ödünç sayısı kontrolü
        int activeLoanCount = await GetUserActiveLoanCountForLibraryAsync(kullaniciId, kutuphaneId, cancellationToken);

        if (activeLoanCount >= maxLoanLimit)
        {
            await throwBusinessException(OduncIslemisBusinessMessages.UserHasExceededLoanLimit);
        }

        // Kullanıcının gecikmiş ödünç işlemi olmamalı
        bool hasOverdueLoans = await _oduncIslemiRepository.AnyAsync(
            predicate: x =>
                x.KullaniciId == kullaniciId &&
                x.Durumu == OduncDurumu.Gecikmis,
            cancellationToken: cancellationToken
        );

        if (hasOverdueLoans)
            await throwBusinessException(OduncIslemisBusinessMessages.UserHasOverdueLoans);
    }

    private async Task<int> GetLibraryLoanLimitAsync(Guid kutuphaneId, CancellationToken cancellationToken)
    {
        // Şimdilik varsayılan limit, sonra kütüphane tablosuna eklenebilir
        // TODO: Kutuphane entity'ine MaxOduncLimit alanı eklenebilir
        return 3; // Varsayılan 3 kitap limiti
    }

    private async Task<int> GetUserActiveLoanCountForLibraryAsync(Guid kullaniciId, Guid kutuphaneId, CancellationToken cancellationToken)
    {
        // Şimdilik basit kontrol - kullanıcının tüm aktif ödünçleri
        // TODO: Daha detaylı kontrol için repository metodunu genişletebilirsiniz
        var userLoans = await _oduncIslemiRepository.GetListAsync(
            predicate: x => x.KullaniciId == kullaniciId && x.Durumu == OduncDurumu.Aktif,
            cancellationToken: cancellationToken
        );

        return userLoans.Items.Count;
    }

    public async Task LoanShouldBeEligibleForExtension(OduncIslemi oduncIslemi)
    {
        if (oduncIslemi.Durumu != OduncDurumu.Aktif)
            await throwBusinessException(OduncIslemisBusinessMessages.LoanNotActive);

        if (oduncIslemi.UzatmaSayisi >= MaxUzatmaSayisi)
            await throwBusinessException(OduncIslemisBusinessMessages.LoanExtensionLimitExceeded);

        // Teslim tarihi geçmişse uzatma yapılamaz
        if (DateTime.UtcNow > oduncIslemi.SonTeslimTarihi)
            await throwBusinessException(OduncIslemisBusinessMessages.LoanNotActive);
    }

    public async Task ValidateLoanPeriod(int oduncSuresiGun)
    {
        if (oduncSuresiGun <= 0 || oduncSuresiGun > 30) // Maksimum 30 gün
            await throwBusinessException(OduncIslemisBusinessMessages.InvalidLoanPeriod);
    }

    public async Task<decimal> CalculateLateFee(OduncIslemi oduncIslemi)
    {
        if (oduncIslemi.IadeTarihi.HasValue && oduncIslemi.IadeTarihi > oduncIslemi.SonTeslimTarihi)
        {
            int gecikmeGunSayisi = (int)Math.Ceiling((oduncIslemi.IadeTarihi.Value - oduncIslemi.SonTeslimTarihi).TotalDays);
            return gecikmeGunSayisi * GunlukCezaMiktari;
        }

        return 0;
    }

    public async Task ProcessOverdueLoans(CancellationToken cancellationToken)
    {
        // Aktif ödünçleri getir ve teslim tarihi geçmiş olanları bul
        var activeLoans = await _oduncIslemiRepository.GetListAsync(
            predicate: x => x.Durumu == OduncDurumu.Aktif && x.SonTeslimTarihi < DateTime.UtcNow,
            enableTracking: true, // Update için tracking gerekli
            cancellationToken: cancellationToken
        );

        foreach (var loan in activeLoans.Items)
        {
            loan.GecikmeDurumunaGec();
            await _oduncIslemiRepository.UpdateAsync(loan);
        }
    }

    public async Task<int> GetUserActiveLoanCount(Guid kullaniciId, CancellationToken cancellationToken)
    {
        var userLoans = await _oduncIslemiRepository.GetListAsync(
            predicate: x => x.KullaniciId == kullaniciId && x.Durumu == OduncDurumu.Aktif,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        return userLoans.Items.Count;
    }

    public async Task<IEnumerable<OduncIslemi>> GetUserOverdueLoans(Guid kullaniciId, CancellationToken cancellationToken)
    {
        var userLoans = await _oduncIslemiRepository.GetListAsync(
            predicate: x => x.KullaniciId == kullaniciId && x.Durumu == OduncDurumu.Gecikmis,
            enableTracking: false,
            cancellationToken: cancellationToken
        );

        return userLoans.Items;
    }
}
