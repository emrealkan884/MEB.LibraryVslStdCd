using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Security;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NArchitecture.Core.Security.Constants;
using NArchitecture.Core.Security.Hashing;
using Persistence.Contexts;

namespace WebAPI.Extensions;

public static class DataSeedingExtensions
{
    public static WebApplication SeedSampleData(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        BaseDbContext context = scope.ServiceProvider.GetRequiredService<BaseDbContext>();

        if (context.Kutuphaneler.Any())
            return app;

        DateTime now = DateTime.UtcNow;
        DateTime lastWeek = now.AddDays(-7);
        DateTime lastMonth = now.AddMonths(-1);

        Guid merkezKutuphaneId = Guid.Parse("5C87D9F1-6F9B-46E0-90F0-7F205874A9F8");
        Guid fenKutuphaneId = Guid.Parse("8C4E8F23-28C9-4A27-8CF7-208407828FA9");

        Guid merkezEdebiyatBolumId = Guid.Parse("A5A15DF1-782C-4E9A-8DD2-C24D3EA302AD");
        Guid merkezBilimBolumId = Guid.Parse("91BB46A2-6574-4BF4-BCC7-27F3B4BF02D2");
        Guid fenStemBolumId = Guid.Parse("4D3C7F57-4F72-4E56-8084-B8190E3325A7");

        Guid merkezEdebiyatRafId = Guid.Parse("F8C777DF-1824-40D2-8056-9ABBB1F2B3DA");
        Guid merkezBilimRafId = Guid.Parse("48E8EC1E-11B4-4D04-A0B3-CE64E2750B9A");
        Guid fenStemRafId = Guid.Parse("3F1EC7D7-CDEF-4E0C-8A0B-8ACBF54D8BA6");

        Guid deweyEdebiyatId = Guid.Parse("3A54CBCF-75DE-4A66-9FC0-7300AB9A2F90");
        Guid deweyTurkRomanId = Guid.Parse("4A9B9061-24B0-4F78-8AF2-F0B1D01BC457");
        Guid deweyEgitimId = Guid.Parse("235BA1AC-A1B6-45C7-8200-39B5B9EA3AE4");

        Guid orhanPamukOtoriteId = Guid.Parse("E2E6A5C8-AD7B-4B07-A1A2-7AE8281F34D3");
        Guid stemOtoriteId = Guid.Parse("7BB21D1F-7F00-4D3F-9E4A-AB40F2910BF4");
        Guid turkRomanOtoriteId = Guid.Parse("6F22A3D3-AD77-4F47-B5D0-7D1A0DE7C2A4");

        Guid orhanPamukYazarId = Guid.Parse("D6D6DF64-9F3F-4C40-9BA7-9B8177F9C0E7");
        Guid aylinYilmazYazarId = Guid.Parse("92E3A7ED-4F6C-4973-9B63-9D89F5A3E6B0");

        Guid romanKatalogId = Guid.Parse("B89F8F4E-29C3-4F1C-A301-16D5B7BBC7DB");
        Guid stemKatalogId = Guid.Parse("3B533C06-F033-4E6E-A4C0-2F1F53F10F5F");

        Guid romanFormatId = Guid.Parse("9C1AFA29-46F3-45F0-BE68-523C7A71C6A1");
        Guid stemFormatId = Guid.Parse("D43B4D73-42EC-4D96-8F5E-155B3E4AB9B6");

        Guid romanMateryalId = Guid.Parse("487B0B9F-94AE-4A7A-9C83-A91B60F0558C");
        Guid stemMateryalId = Guid.Parse("43452C1A-3CB8-4E57-9416-BD5176568A3A");

        Guid romanEtiketId = Guid.Parse("D038F95F-EDDF-4CF0-A8D9-DF2AA6A2660D");
        Guid stemEtiketId = Guid.Parse("39C6FBBF-57E2-42B0-9797-0A1FC7061710");

        Guid romanNushaId = Guid.Parse("0A18F4FA-0E2A-41DE-9C77-53F1E4491A3D");
        Guid stemNushaId = Guid.Parse("827D9E70-99B8-4E0D-B198-3E3F7E589243");
        Guid stemNusha2Id = Guid.Parse("6B9C4D32-79C8-4829-9150-8F24C5A70A6E");
        Guid stemNusha3Id = Guid.Parse("99E2BC17-62E2-47F1-A4E2-0D56E3EB81E7");

        Guid romanYazarBagId = Guid.Parse("F6F71988-3C63-4BD5-B83A-41A58427A7A6");
        Guid stemYazarBagId = Guid.Parse("5AA8C527-5F07-420B-8A2D-3C6B620B95C2");

        Guid romanKonuId = Guid.Parse("9F25B2DF-4EFA-4F8F-8071-919F9FCEFA21");
        Guid stemKonuId = Guid.Parse("B3242865-E364-4E95-896D-79C36FA38269");

        Guid stemTalepId = Guid.Parse("96CB0C7C-B9FB-41B1-9BBA-5CF6F729B47F");
        Guid stemRezervasyonId = Guid.Parse("6F756107-1E43-4C03-A047-3A8AAC0E762E");
        Guid stemOduncId = Guid.Parse("5446C4F5-A970-41A9-AEA9-BA97FE38E558");
        Guid stemOverdueOduncId = Guid.Parse("1E1F9F0B-6323-48E9-89A8-3E6A787BC279");

        Guid merkezEtkinlikId = Guid.Parse("2CD1FA16-7564-40A7-B828-138950137C71");
        Guid auditLogId = Guid.Parse("8F063057-5AAB-4F4E-B6D5-27A3C7C01A2D");

        Guid ogrenciUserId = Guid.Parse("1C77CE41-5A1C-4C07-BD81-9B88C3F3A807");
        Guid personelUserId = Guid.Parse("8FE171E5-3C60-441E-9F8B-64B3EABFA86C");
        Guid personelClaimLinkId = Guid.Parse("5D7460EE-0F95-483C-96CF-6F7934F7F96E");

        HashingHelper.CreatePasswordHash("Library123!", out byte[] studentHash, out byte[] studentSalt);
        HashingHelper.CreatePasswordHash("Library123!", out byte[] staffHash, out byte[] staffSalt);

        var users = new List<User>
        {
            new()
            {
                Id = ogrenciUserId,
                Email = "ogrenci@example.com",
                FirstName = "Aylin",
                LastName = "Tas",
                PasswordHash = studentHash,
                PasswordSalt = studentSalt,
                Status = true,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = personelUserId,
                Email = "kutuphane.yonetici@example.com",
                FirstName = "Esra",
                LastName = "Balcik",
                PasswordHash = staffHash,
                PasswordSalt = staffSalt,
                Status = true,
                CreatedDate = lastMonth
            }
        };
        context.Users.AddRange(users);

        int? adminClaimId = context.OperationClaims.AsNoTracking()
            .FirstOrDefault(claim => claim.Name == GeneralOperationClaims.Admin)?.Id;

        if (adminClaimId.HasValue)
        {
            context.UserOperationClaims.Add(
                new UserOperationClaim
                {
                    Id = personelClaimLinkId,
                    UserId = personelUserId,
                    OperationClaimId = adminClaimId.Value,
                    CreatedDate = lastMonth
                });
        }

        var kutuphaneler = new List<Kutuphane>
        {
            new()
            {
                Id = merkezKutuphaneId,
                Kod = "MERK-01",
                Ad = "Milli Egitim Merkez Kutuphanesi",
                Tip = KutuphaneTipi.Merkez,
                Adres = "Bakanliklar Cad. No:1 Ankara",
                Telefon = "0312 111 22 33",
                EPosta = "merkez@meb.gov.tr",
                Aktif = true,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = fenKutuphaneId,
                Kod = "FEN-01",
                Ad = "Yahya Turan Fen Lisesi Kutuphanesi",
                Tip = KutuphaneTipi.Okul,
                Adres = "Fen Lisesi Kampusu Izmir",
                Telefon = "0232 765 43 21",
                EPosta = "kutuphane@fenlisesi.edu.tr",
                Aktif = true,
                CreatedDate = lastMonth
            }
        };
        context.Kutuphaneler.AddRange(kutuphaneler);

        var bolumler = new List<KutuphaneBolumu>
        {
            new()
            {
                Id = merkezEdebiyatBolumId,
                KutuphaneId = merkezKutuphaneId,
                Ad = "Edebiyat",
                Aciklama = "Roman ve siir koleksiyonu",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = merkezBilimBolumId,
                KutuphaneId = merkezKutuphaneId,
                Ad = "Bilim",
                Aciklama = "Fen bilimleri kaynaklari",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = fenStemBolumId,
                KutuphaneId = fenKutuphaneId,
                Ad = "STEM",
                Aciklama = "STEM projeleri icin kaynaklar",
                CreatedDate = lastMonth
            }
        };
        context.KutuphaneBolumleri.AddRange(bolumler);

        var raflar = new List<Raf>
        {
            new()
            {
                Id = merkezEdebiyatRafId,
                KutuphaneBolumuId = merkezEdebiyatBolumId,
                Kod = "EDB-A1",
                Aciklama = "Turk edebiyati romanlari",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = merkezBilimRafId,
                KutuphaneBolumuId = merkezBilimBolumId,
                Kod = "BLM-B2",
                Aciklama = "Fen bilimleri referanslari",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = fenStemRafId,
                KutuphaneBolumuId = fenStemBolumId,
                Kod = "STEM-01",
                Aciklama = "Laboratuvar setleri",
                CreatedDate = lastMonth
            }
        };
        context.Raflar.AddRange(raflar);

        var deweySiniflamalar = new List<DeweySiniflama>
        {
            new()
            {
                Id = deweyEdebiyatId,
                Kod = "800",
                Baslik = "Edebiyat",
                Aciklama = "Edebiyat koleksiyonu",
                UstSinifId = null,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = deweyTurkRomanId,
                Kod = "894.353",
                Baslik = "Turk Romanlari",
                Aciklama = "Modern Turk edebiyati romanlari",
                UstSinifId = deweyEdebiyatId,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = deweyEgitimId,
                Kod = "370",
                Baslik = "Egitim",
                Aciklama = "Egitim bilimleri kaynaklari",
                UstSinifId = null,
                CreatedDate = lastMonth
            }
        };
        context.DeweySiniflamalar.AddRange(deweySiniflamalar);

        var otoriteKayitlari = new List<OtoriteKaydi>
        {
            new()
            {
                Id = orhanPamukOtoriteId,
                YetkiliBaslik = "Pamuk, Orhan",
                OtoriteTuru = OtoriteTuru.Kisi,
                AlternatifBasliklar = "Orhan Pamuk",
                Aciklama = "Nobel odullu Turk yazari",
                HariciKayitNo = "VIAF:34456789",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemOtoriteId,
                YetkiliBaslik = "STEM egitimi",
                OtoriteTuru = OtoriteTuru.KonuBasligi,
                AlternatifBasliklar = "Fen teknoloji muhendislik matematik",
                Aciklama = "Disiplinler arasi egitim modeli",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = turkRomanOtoriteId,
                YetkiliBaslik = "Turk romanlari",
                OtoriteTuru = OtoriteTuru.KonuBasligi,
                AlternatifBasliklar = "Turk edebiyati -- roman",
                Aciklama = "Turk roman koleksiyonu",
                CreatedDate = lastMonth
            }
        };
        context.OtoriteKayitlari.AddRange(otoriteKayitlari);

        var yazarlar = new List<Yazar>
        {
            new()
            {
                Id = orhanPamukYazarId,
                AdSoyad = "Orhan Pamuk",
                DogumTarihi = new DateTime(1952, 6, 7),
                Uyruk = "Turkiye",
                Aciklama = "Nobel odullu yazar",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = aylinYilmazYazarId,
                AdSoyad = "Aylin Yilmaz",
                Uyruk = "Turkiye",
                Aciklama = "STEM egitimi danismani",
                CreatedDate = lastMonth
            }
        };
        context.Yazarlar.AddRange(yazarlar);

        var yeniTalepler = new List<YeniKatalogTalebi>
        {
            new()
            {
                Id = stemTalepId,
                TalepEdenKutuphaneId = fenKutuphaneId,
                Baslik = "STEM Proje Rehberi",
                AltBaslik = "Atolye etkinlikleri icin uygulama rehberi",
                YazarMetni = "Aylin Yilmaz",
                Isbn = "9786051234567",
                MateryalTuru = "Kitap",
                MateryalAltTuru = "Egitim",
                Dil = "Turkce",
                Yayinevi = "MEB Yayinlari",
                YayinYeri = "Ankara",
                YayinYili = 2024,
                Aciklama = "STEM kulubu ihtiyaci",
                Durum = TalepDurumu.Onaylandi,
                TalepTarihi = lastMonth,
                SonGuncellemeTarihi = lastWeek,
                KatalogKaydiId = stemKatalogId,
                CreatedDate = lastMonth
            }
        };
        context.YeniKatalogTalepleri.AddRange(yeniTalepler);

        var katalogKayitlari = new List<KatalogKaydi>
        {
            new()
            {
                Id = romanKatalogId,
                KutuphaneId = merkezKutuphaneId,
                Baslik = "Benim Adim Kirmizi",
                AltBaslik = "Osmanli resim sanati uzerine roman",
                Isbn = "9789754707083",
                Yayinevi = "Iletisim Yayinlari",
                YayinYeri = "Istanbul",
                YayinYili = 1998,
                SayfaSayisi = 448,
                Dil = "Turkce",
                MateryalTuru = MateryalTuru.Kitap,
                MateryalAltTuru = "Roman",
                DeweySiniflamaId = deweyTurkRomanId,
                Marc21Verisi = "{ \"245\": { \"a\": \"Benim Adim Kirmizi\" } }",
                RdaUyumlu = true,
                KayitTarihi = lastMonth,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemKatalogId,
                KutuphaneId = fenKutuphaneId,
                Baslik = "STEM Proje Rehberi",
                AltBaslik = "Pratik etkinlikler ve planlar",
                Isbn = "9786051234567",
                Yayinevi = "MEB Yayinlari",
                YayinYeri = "Ankara",
                YayinYili = 2024,
                SayfaSayisi = 256,
                Dil = "Turkce",
                MateryalTuru = MateryalTuru.Kitap,
                MateryalAltTuru = "Egitim",
                DeweySiniflamaId = deweyEgitimId,
                Marc21Verisi = "{ \"245\": { \"a\": \"STEM Proje Rehberi\" } }",
                RdaUyumlu = true,
                YeniKatalogTalebiId = stemTalepId,
                KayitTarihi = lastWeek,
                CreatedDate = lastWeek
            }
        };
        context.KatalogKayitlari.AddRange(katalogKayitlari);

        var katalogYazarBaglari = new List<KatalogKaydiYazar>
        {
            new()
            {
                Id = romanYazarBagId,
                KatalogKaydiId = romanKatalogId,
                YazarId = orhanPamukYazarId,
                OtoriteKaydiId = orhanPamukOtoriteId,
                Rol = YazarRolu.Yazar,
                Sira = 1,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemYazarBagId,
                KatalogKaydiId = stemKatalogId,
                YazarId = aylinYilmazYazarId,
                OtoriteKaydiId = stemOtoriteId,
                Rol = YazarRolu.Yazar,
                Sira = 1,
                CreatedDate = lastWeek
            }
        };
        context.KatalogKaydiYazarlar.AddRange(katalogYazarBaglari);

        var katalogKonular = new List<KatalogKonu>
        {
            new()
            {
                Id = romanKonuId,
                KatalogKaydiId = romanKatalogId,
                KonuBasligi = "Turk romanlari",
                OtoriteKaydiId = turkRomanOtoriteId,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemKonuId,
                KatalogKaydiId = stemKatalogId,
                KonuBasligi = "STEM egitimi",
                OtoriteKaydiId = stemOtoriteId,
                CreatedDate = lastWeek
            }
        };
        context.KatalogKonulari.AddRange(katalogKonular);

        var formatDetaylari = new List<MateryalFormatDetay>
        {
            new()
            {
                Id = romanFormatId,
                KatalogKaydiId = romanKatalogId,
                MateryalTuru = MateryalTuru.Kitap,
                FizikselTanimi = "448 s. ; 24 cm",
                FormatBilgisi = "Ciltli",
                Dil = "Turkce",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemFormatId,
                KatalogKaydiId = stemKatalogId,
                MateryalTuru = MateryalTuru.Kitap,
                FizikselTanimi = "256 s. ; 27 cm",
                FormatBilgisi = "Spiral baglama",
                Dil = "Turkce",
                CreatedDate = lastWeek
            }
        };
        context.MateryalFormatDetaylar.AddRange(formatDetaylari);

        var materyaller = new List<Materyal>
        {
            new()
            {
                Id = romanMateryalId,
                KatalogKaydiId = romanKatalogId,
                KutuphaneId = merkezKutuphaneId,
                KutuphaneBolumuId = merkezEdebiyatBolumId,
                Formati = MateryalFormati.Kitap,
                RezervasyonaAcik = true,
                MaksimumOduncSuresiGun = 21,
                Not = "Okuma kulubu listesinde",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemMateryalId,
                KatalogKaydiId = stemKatalogId,
                KutuphaneId = fenKutuphaneId,
                KutuphaneBolumuId = fenStemBolumId,
                Formati = MateryalFormati.Kitap,
                RezervasyonaAcik = true,
                MaksimumOduncSuresiGun = 14,
                Not = "STEM kulubu izleme listesi",
                CreatedDate = lastWeek
            }
        };
        context.Materyalsler.AddRange(materyaller);

        var materyalEtiketler = new List<MateryalEtiket>
        {
            new()
            {
                Id = romanEtiketId,
                MateryalId = romanMateryalId,
                Etiket = "OkumaKulubu",
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemEtiketId,
                MateryalId = stemMateryalId,
                Etiket = "STEM",
                CreatedDate = lastWeek
            }
        };
        context.MateryalEtiketler.AddRange(materyalEtiketler);

        var nushalar = new List<Nusha>
        {
            new()
            {
                Id = romanNushaId,
                MateryalId = romanMateryalId,
                RafId = merkezEdebiyatRafId,
                Barkod = "9789754707083-001",
                Durumu = NushaDurumu.Rafta,
                EklenmeTarihi = lastMonth,
                CreatedDate = lastMonth
            },
            new()
            {
                Id = stemNushaId,
                MateryalId = stemMateryalId,
                RafId = fenStemRafId,
                Barkod = "STEM-2024-001",
                Durumu = NushaDurumu.Rafta,
                EklenmeTarihi = lastWeek,
                CreatedDate = lastWeek
            },
            new()
            {
                Id = stemNusha2Id,
                MateryalId = stemMateryalId,
                RafId = fenStemRafId,
                Barkod = "STEM-2024-002",
                Durumu = NushaDurumu.Oduncta,
                EklenmeTarihi = lastWeek,
                CreatedDate = lastWeek
            },
            new()
            {
                Id = stemNusha3Id,
                MateryalId = stemMateryalId,
                RafId = fenStemRafId,
                Barkod = "STEM-2024-003",
                Durumu = NushaDurumu.Oduncta,
                EklenmeTarihi = lastMonth,
                CreatedDate = lastMonth.AddDays(-10)
            }
        };
        context.Nushas.AddRange(nushalar);

        var rezervasyonlar = new List<Rezervasyon>
        {
            new()
            {
                Id = stemRezervasyonId,
                KutuphaneId = fenKutuphaneId,
                KullaniciId = ogrenciUserId,
                MateryalId = stemMateryalId,
                TalepTarihi = lastWeek,
                HazirlanmaTarihi = lastWeek.AddDays(1),
                Durumu = RezervasyonDurumu.Hazir,
                Not = "STEM kulubu grup calismasi",
                CreatedDate = lastWeek
            }
        };
        context.Rezervasyonlar.AddRange(rezervasyonlar);

        var oduncIslemleri = new List<OduncIslemi>
        {
            new()
            {
                Id = stemOduncId,
                KutuphaneId = fenKutuphaneId,
                KullaniciId = ogrenciUserId,
                NushaId = stemNusha2Id,
                AlinmaTarihi = lastWeek,
                SonTeslimTarihi = now.AddDays(7),
                Durumu = OduncDurumu.Aktif,
                UzatmaSayisi = 0,
                Not = "Atolye calismasi icin ayrildi",
                CreatedDate = lastWeek
            },
            new()
            {
                Id = stemOverdueOduncId,
                KutuphaneId = fenKutuphaneId,
                KullaniciId = ogrenciUserId,
                NushaId = stemNusha3Id,
                AlinmaTarihi = lastWeek.AddDays(-10),
                SonTeslimTarihi = lastWeek.AddDays(-2),
                Durumu = OduncDurumu.Aktif,
                UzatmaSayisi = 1,
                Not = "Robotik proje arastirmasi icin verildi",
                CreatedDate = lastWeek.AddDays(-10)
            }
        };
        context.OduncIslemleri.AddRange(oduncIslemleri);

        var etkinlikler = new List<Etkinlik>
        {
            new()
            {
                Id = merkezEtkinlikId,
                KutuphaneId = merkezKutuphaneId,
                Baslik = "Okuma Kulubu Tartismasi",
                Aciklama = "Benim Adim Kirmizi uzerine sohbet",
                BaslangicTarihi = now.AddDays(3).Date.AddHours(18),
                BitisTarihi = now.AddDays(3).Date.AddHours(20),
                Konum = "Merkez konferans salonu",
                AfisDosyasi = "/events/okuma-kulubu.jpg",
                CreatedDate = lastWeek
            }
        };
        context.Etkinlikler.AddRange(etkinlikler);

        var auditLogs = new List<AuditLog>
        {
            new()
            {
                Id = auditLogId,
                UserId = personelUserId,
                UserName = "kutuphane.yonetici@example.com",
                ActionName = "YeniKatalogTalebi.Onayla",
                Payload = "{ \"talepId\": \"" + stemTalepId + "\" }",
                ClientIp = "10.10.0.5",
                UserAgent = "SeedData/1.0",
                OccurredOn = lastWeek,
                CreatedDate = lastWeek
            }
        };
        context.AuditLogs.AddRange(auditLogs);

        context.SaveChanges();

        return app;
    }
}
