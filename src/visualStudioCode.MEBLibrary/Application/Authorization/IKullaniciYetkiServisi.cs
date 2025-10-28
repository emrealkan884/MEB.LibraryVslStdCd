namespace Application.Authorization;

public interface IKullaniciYetkiServisi
{
    Task<KullaniciYetkiBilgisi> AktifKullaniciYetkisiAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Guid>?> YetkiliKutuphaneIdListesiAsync(CancellationToken cancellationToken);
}
