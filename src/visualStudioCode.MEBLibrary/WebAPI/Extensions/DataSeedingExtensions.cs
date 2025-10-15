using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace WebAPI.Extensions;

public static class DataSeedingExtensions
{
    public static WebApplication SeedSampleData(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        BaseDbContext context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        if (context.OtoriteKayitlari.Any())
            return app;

        DateTime now = DateTime.UtcNow;

        Guid merkezKutuphaneId = Guid.Parse("5C87D9F1-6F9B-46E0-90F0-7F205874A9F8");
        Guid orhanPamukOtoriteId = Guid.Parse("E2E6A5C8-AD7B-4B07-A1A2-7AE8281F34D3");
        Guid turkRomanlariOtoriteId = Guid.Parse("6F22A3D3-AD77-4F47-B5D0-7D1A0DE7C2A4");
        Guid orhanPamukYazarId = Guid.Parse("D6D6DF64-9F3F-4C40-9BA7-9B8177F9C0E7");
        Guid katalogKaydiId = Guid.Parse("B89F8F4E-29C3-4F1C-A301-16D5B7BBC7DB");
        Guid katalogKaydiYazarId = Guid.Parse("F6F71988-3C63-4BD5-B83A-41A58427A7A6");
        Guid katalogKonuId = Guid.Parse("9F25B2DF-4EFA-4F8F-8071-919F9FCEFA21");

        Kutuphane merkezKutuphane = new()
        {
            Id = merkezKutuphaneId,
            Kod = "MERKEZ-01",
            Ad = "Milli Eğitim Merkez Kütüphanesi",
            Tip = KutuphaneTipi.Merkez,
            Adres = "Bakanlıklar / Ankara",
            Telefon = "0312 123 45 67",
            EPosta = "merkez@meb.gov.tr",
            Aktif = true,
            CreatedDate = now
        };

        OtoriteKaydi orhanPamukAuthority = new()
        {
            Id = orhanPamukOtoriteId,
            YetkiliBaslik = "Pamuk, Orhan",
            OtoriteTuru = OtoriteTuru.Kisi,
            AlternatifBasliklar = "Orhan Pamuk; Pamuk, O.",
            HariciKayitNo = "TR-PER-000245",
            CreatedDate = now
        };

        OtoriteKaydi turkRomanlariAuthority = new()
        {
            Id = turkRomanlariOtoriteId,
            YetkiliBaslik = "Türk romanları",
            OtoriteTuru = OtoriteTuru.KonuBasligi,
            AlternatifBasliklar = "Türk edebiyatı -- Roman",
            CreatedDate = now
        };

        Yazar orhanPamuk = new()
        {
            Id = orhanPamukYazarId,
            AdSoyad = "Orhan Pamuk",
            DogumTarihi = new DateTime(1952, 6, 7),
            Uyruk = "Türkiye",
            Aciklama = "Nobel Edebiyat Ödülü 2006 sahibi yazar",
            CreatedDate = now
        };

        KatalogKaydi katalogKaydi = new()
        {
            Id = katalogKaydiId,
            KutuphaneId = merkezKutuphaneId,
            Baslik = "Benim Adım Kırmızı",
            AltBaslik = "Osmanlı resim sanatı üzerine bir roman",
            Isbn = "9789754707083",
            Yayinevi = "İletişim Yayınları",
            YayinYeri = "İstanbul",
            YayinYili = 1998,
            SayfaSayisi = 445,
            Dil = "Türkçe",
            MateryalTuru = MateryalTuru.Kitap,
            MateryalAltTuru = "Roman",
            Marc21Verisi = "{ \"245\": { \"a\": \"Benim Adım Kırmızı\" } }",
            RdaUyumlu = true,
            KayitTarihi = now,
            CreatedDate = now
        };

        KatalogKaydiYazar katalogKaydiYazar = new()
        {
            Id = katalogKaydiYazarId,
            KatalogKaydiId = katalogKaydiId,
            YazarId = orhanPamukYazarId,
            OtoriteKaydiId = orhanPamukOtoriteId,
            Rol = YazarRolu.Yazar,
            Sira = 1,
            CreatedDate = now
        };

        KatalogKonu katalogKonu = new()
        {
            Id = katalogKonuId,
            KatalogKaydiId = katalogKaydiId,
            KonuBasligi = "Türk romanları",
            OtoriteKaydiId = turkRomanlariOtoriteId,
            CreatedDate = now
        };

        context.Kutuphaneler.Add(merkezKutuphane);
        context.OtoriteKayitlari.AddRange(orhanPamukAuthority, turkRomanlariAuthority);
        context.Yazarlar.Add(orhanPamuk);
        context.KatalogKayitlari.Add(katalogKaydi);
        context.KatalogKaydiYazarlar.Add(katalogKaydiYazar);
        context.KatalogKonulari.Add(katalogKonu);

        context.SaveChanges();

        return app;
    }
}
