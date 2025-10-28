using System.Diagnostics.CodeAnalysis;

namespace Application.Authorization;

public sealed record KullaniciYetkiBilgisi(
    bool SistemYoneticisi,
    bool BakanlikYetkilisi,
    bool IlYetkilisi,
    bool IlceYetkilisi,
    bool OkulKutuphaneYoneticisi,
    string? IlKodu,
    string? IlceKodu,
    Guid? KutuphaneId
)
{
    public bool TumUlkeGenelYetki => SistemYoneticisi || BakanlikYetkilisi;

    public bool TryGetIlKodu([NotNullWhen(true)] out string? kod)
    {
        kod = IlKodu;
        return !string.IsNullOrWhiteSpace(IlKodu);
    }

    public bool TryGetIlceKodu([NotNullWhen(true)] out string? kod)
    {
        kod = IlceKodu;
        return !string.IsNullOrWhiteSpace(IlceKodu);
    }

    public bool TryGetKutuphaneId([NotNullWhen(true)] out Guid? id)
    {
        id = KutuphaneId;
        return KutuphaneId.HasValue && KutuphaneId != Guid.Empty;
    }
}
