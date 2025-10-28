using System.Security.Claims;
using Application.Features.Auth.Constants;
using Application.Services.Repositories;
using Domain.Entities.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.CrossCuttingConcerns.Exception.Types;
using NArchitecture.Core.Localization.Abstraction;
using NArchitecture.Core.Security.Constants;

namespace Application.Authorization;

public class KullaniciYetkiServisi : IKullaniciYetkiServisi
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IKutuphaneRepository _kutuphaneRepository;
    private readonly ILocalizationService _localizationService;

    private KullaniciYetkiBilgisi? _yetkiBilgisi;
    private IReadOnlyList<Guid>? _kutuphaneIdListesi;
    private bool _kutuphaneIdListesiHazirlandi;

    public KullaniciYetkiServisi(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IKutuphaneRepository kutuphaneRepository,
        ILocalizationService localizationService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _kutuphaneRepository = kutuphaneRepository;
        _localizationService = localizationService;
    }

    public async Task<KullaniciYetkiBilgisi> AktifKullaniciYetkisiAsync(CancellationToken cancellationToken)
    {
        if (_yetkiBilgisi is not null)
            return _yetkiBilgisi;

        ClaimsPrincipal? principal = _httpContextAccessor.HttpContext?.User;
        if (principal?.Identity?.IsAuthenticated != true)
            await throwYetkiHatasi();

        string? idValue = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(idValue))
            await throwYetkiHatasi();
        if (!Guid.TryParse(idValue, out Guid userId))
            await throwYetkiHatasi();

        User? user = await _userRepository.GetAsync(
            predicate: u => u.Id == userId,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        if (user is null)
            await throwYetkiHatasi();

        bool sistemYoneticisi = principal.IsInRole(ApplicationRoles.SistemYoneticisi)
            || principal.IsInRole(GeneralOperationClaims.Admin);
        bool bakanlik = principal.IsInRole(ApplicationRoles.BakanlikYetkilisi);
        bool il = principal.IsInRole(ApplicationRoles.IlYetkilisi);
        bool ilce = principal.IsInRole(ApplicationRoles.IlceYetkilisi);
        bool okul = principal.IsInRole(ApplicationRoles.OkulKutuphaneYoneticisi);

        KullaniciYetkiBilgisi yetki = new(
            SistemYoneticisi: sistemYoneticisi,
            BakanlikYetkilisi: bakanlik,
            IlYetkilisi: il,
            IlceYetkilisi: ilce,
            OkulKutuphaneYoneticisi: okul,
            IlKodu: user.SorumluIlKodu,
            IlceKodu: user.SorumluIlceKodu,
            KutuphaneId: user.SorumluKutuphaneId
        );

        await validateScopeAsync(yetki);

        _yetkiBilgisi = yetki;
        return yetki;
    }

    public async Task<IReadOnlyList<Guid>?> YetkiliKutuphaneIdListesiAsync(CancellationToken cancellationToken)
    {
        if (_kutuphaneIdListesiHazirlandi)
            return _kutuphaneIdListesi;

        KullaniciYetkiBilgisi yetki = await AktifKullaniciYetkisiAsync(cancellationToken);

        if (yetki.TumUlkeGenelYetki)
        {
            _kutuphaneIdListesiHazirlandi = true;
            _kutuphaneIdListesi = null;
            return _kutuphaneIdListesi;
        }

        if (yetki.OkulKutuphaneYoneticisi && yetki.TryGetKutuphaneId(out Guid? kutuphaneId))
        {
            _kutuphaneIdListesi = new[] { kutuphaneId!.Value };
            _kutuphaneIdListesiHazirlandi = true;
            return _kutuphaneIdListesi;
        }

        IQueryable<Domain.Entities.Kutuphane> kutuphaneSorgusu = _kutuphaneRepository.Query();

        if (yetki.TryGetIlKodu(out string? ilKodu))
            kutuphaneSorgusu = kutuphaneSorgusu.Where(k => k.Il == ilKodu);

        if (yetki.TryGetIlceKodu(out string? ilceKodu))
            kutuphaneSorgusu = kutuphaneSorgusu.Where(k => k.Ilce == ilceKodu);

        List<Guid> kutuphaneIdleri = await kutuphaneSorgusu
            .Select(k => k.Id)
            .ToListAsync(cancellationToken);

        _kutuphaneIdListesi = kutuphaneIdleri;
        _kutuphaneIdListesiHazirlandi = true;
        return _kutuphaneIdListesi;
    }

    private async Task validateScopeAsync(KullaniciYetkiBilgisi yetki)
    {
        if (yetki.TumUlkeGenelYetki)
            return;

        if (yetki.IlYetkilisi && string.IsNullOrWhiteSpace(yetki.IlKodu))
        {
            string mesaj = await _localizationService.GetLocalizedAsync(AuthMessages.KullaniciKapsamiEksik, AuthMessages.SectionName);
            throw new BusinessException(mesaj);
        }

        if (yetki.IlceYetkilisi && (string.IsNullOrWhiteSpace(yetki.IlKodu) || string.IsNullOrWhiteSpace(yetki.IlceKodu)))
        {
            string mesaj = await _localizationService.GetLocalizedAsync(AuthMessages.KullaniciKapsamiEksik, AuthMessages.SectionName);
            throw new BusinessException(mesaj);
        }

        if (yetki.OkulKutuphaneYoneticisi && (!yetki.KutuphaneId.HasValue || yetki.KutuphaneId == Guid.Empty))
        {
            string mesaj = await _localizationService.GetLocalizedAsync(AuthMessages.KullaniciKapsamiEksik, AuthMessages.SectionName);
            throw new BusinessException(mesaj);
        }
    }

    private async Task throwYetkiHatasi() => throw new BusinessException(await _localizationService.GetLocalizedAsync(AuthMessages.YetkiGerekli, AuthMessages.SectionName));
}
