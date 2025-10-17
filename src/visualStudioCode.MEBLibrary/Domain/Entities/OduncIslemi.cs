using Domain.Enums;
using NArchitecture.Core.Persistence.Repositories;

namespace Domain.Entities;

public class OduncIslemi : Entity<Guid>
{
    public Guid KutuphaneId { get; set; } // 205, 310, 512
    public Guid KullaniciId { get; set; } // 9001, 9050, 9205
    public Guid NushaId { get; set; } // 1, 75, 420
    public DateTime AlinmaTarihi { get; set; } = DateTime.UtcNow; // 2024-03-01 09:20, 2024-03-15 14:10, 2024-04-02 11:45
    public DateTime SonTeslimTarihi { get; set; } // 2024-03-15, 2024-03-22, 2024-04-16
    public DateTime? IadeTarihi { get; set; } // 2024-03-14, 2024-03-25, null
    public DateTime? UzatmaTarihi { get; set; } // Süre uzatıldığında set edilir
    public int UzatmaSayisi { get; set; } = 0; // Maksimum 2 uzatma
    public OduncDurumu Durumu { get; set; } = OduncDurumu.Aktif; // OduncDurumu.Aktif, OduncDurumu.IadeEdildi, OduncDurumu.Gecikmis
    public decimal? GecikmeCezaMiktari { get; set; } // Gecikme durumunda hesaplanan ceza
    public int? GecikmeGunSayisi { get; set; } // Kaç gün gecikti
    public string? Not { get; set; } // "Öğretmen talebi", "Gecikme hatırlatması gönderildi", null

    //Navigation properties
    public Kutuphane? Kutuphane { get; set; }
    public Nusha? Nusha { get; set; }

    // Business methods
    public void OdunciIadeEt()
    {
        IadeTarihi = DateTime.UtcNow;
        Durumu = OduncDurumu.IadeEdildi;

        // Gecikme kontrolü ve ceza hesaplama
        if (IadeTarihi > SonTeslimTarihi)
        {
            GecikmeGunSayisi = (int)Math.Ceiling((IadeTarihi.Value - SonTeslimTarihi).TotalDays);
            GecikmeCezaMiktari = GecikmeGunSayisi * 1.0m; // Günlük 1 TL ceza (konfigüre edilebilir)
        }
    }

    public bool SureUzatabilirMi()
    {
        return UzatmaSayisi < 2 && Durumu == OduncDurumu.Aktif;
    }

    public void SureUzat(int ekGun)
    {
        if (!SureUzatabilirMi())
            throw new InvalidOperationException("Bu ödünç işlemi için süre uzatılamaz.");

        SonTeslimTarihi = SonTeslimTarihi.AddDays(ekGun);
        UzatmaTarihi = DateTime.UtcNow;
        UzatmaSayisi++;

        Not += $"{DateTime.UtcNow:yyyy-MM-dd}: Süre {ekGun} gün uzatıldı. ";
    }

    public bool GecikmisMi()
    {
        return Durumu == OduncDurumu.Aktif && DateTime.UtcNow > SonTeslimTarihi;
    }

    public void GecikmeDurumunaGec()
    {
        if (GecikmisMi())
        {
            Durumu = OduncDurumu.Gecikmis;
            GecikmeGunSayisi = (int)Math.Ceiling((DateTime.UtcNow - SonTeslimTarihi).TotalDays);
            GecikmeCezaMiktari = GecikmeGunSayisi * 1.0m; // Günlük 1 TL ceza
        }
    }
}
