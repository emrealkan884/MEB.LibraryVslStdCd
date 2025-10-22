## Domain Model Rehberi

Bu doküman, kütüphane domainindeki temel varlıkların amaçlarını ve birbirleriyle ilişkilerini açıklar. Referans olması için önemli senaryolar ve tasarım gerekçeleri de eklenmiştir.

---

### 1. Katalog Kaydı → Materyal → Nusha

| Varlık | Açıklama |
| --- | --- |
| `KatalogKaydi` | Kaynağın bibliyografik kimliğini tutar (başlık, ISBN, Dewey kodu, MARC21 verisi, dil, yayın bilgileri, materyal türü). |
| `Materyal` | Belirli bir kütüphanedeki hizmet parametrelerini tutar (maksimum ödünç süresi, rezervasyon durumu, yer bilgisi, etiketler). |
| `Nusha` | Bir materyalin fiziksel veya dijital kopyasıdır (barkod, raf, durum, eklenme tarihi). |

**Akış:** Onaylanan her katalog kaydı, ihtiyaç duyan kütüphaneler için materyal kaydı oluşturur; materyaller üzerinden nusha girişleri yapılır. Aynı katalog kaydına bağlı birden çok materyal ve nusha tanımlanabilir.

---

### 2. Yeni Katalog Talebi Senaryosu

1. Okul kütüphanecisi `YeniKatalogTalebi` oluşturur (ISBN, yazar, Dewey kodu, açıklama).
2. Bakanlık yetkilisi talebi inceler ve onaylar (`TalepDurumu.Onaylandi`). Onay işlemi otomatik olarak bir `KatalogKaydi` oluşturur ve talep ile ilişkilendirir.
3. İlgili yazar, konu ve authority kayıtları `KatalogKaydiYazar` ve `KatalogKonu` üzerinden bağlanır.
4. Okul, katalog kaydını referans alarak `Materyal` ve `Nusha` kayıtlarını açar; barkodlar üretir.
5. Öğrenciler rezervasyon/ödünç süreçlerini başlatır; raporlar katalogdan materyale, materyalden nusha ve ödünç işlemine kadar zincir boyunca veri sağlar.

---

### 3. Otorite Kayıtları

- `OtoriteKaydi`: Yetkili başlık, alternatif başlıklar, bağlı terimler ve harici kayıt numarası içerir.
- `KatalogKaydiYazar`: `Yazar` varlığı ile katalog kaydını bağlar; `Rol` enum’u (Yazar, Editör, Çevirmen vb.) ve sıralama bilgisi taşır.
- `KatalogKonu`: Katalog kaydını konu otoritesine bağlar; konu başlıklarının tutarlı kullanılmasını sağlar.

**Neden önemli?** Standart authority kayıtları; arama sonuçlarının tutarlı olmasını, dış sistemlerle eşleşmeleri ve raporlamayı kolaylaştırır.

---

### 4. Materyal Format Detayları

`MateryalFormatDetay` varlığı, bir katalog kaydının fiziki/dijital formatına dair bilgileri saklar:

- `FizikselTanimi` (sayfa sayısı, boyut),
- `SureBilgisi` (DVD/Video için süre veya disk sayısı),
- `FormatBilgisi` (PDF, ciltli, sesli kitap),
- `Dil` ve `ErisimBilgisi`.

Bu alanlar, MARC21 alan 300/347/538 karşılıklarını modellemek için tasarlanmıştır.

---

### 5. Materyal Etiketleri ve Kütüphane Bölümleri

- `MateryalEtiket`: Okullara özel kategoriler (Ör: “STEM”, “Okuma Kulübü”). Bir materyale birden fazla etiket atanabilir.
- `KutuphaneBolumu`: Fiziksel yer bilgisi (örn. “STEM Laboratuvarı Rafı”). `Raf` varlığı bu bölümlere bağlıdır.

**Fark:** Konu (`KatalogKonu`) kaynağın entelektüel içeriğini, `KutuphaneBolumu` ise fiziksel konumunu tanımlar.

---

### 6. Roller ve Yetkilendirme

- Roller: `Role.BakanlikYetkilisi`, `Role.IlYetkilisi`, `Role.IlceYetkilisi`, `Role.OkulKutuphaneYoneticisi`.
- Politikalar: `RequireMinistry`, `RequireProvinceOrAbove`, `RequireDistrictOrAbove`, `RequireSchoolOrAbove`.

Controller’larda rol tabanlı erişim bu politikalar üzerinden yönetilir. Seed edilen kullanıcılar örnek rollerle gelir.

---

### 7. Ödünç İşlemi Varlığı

`OduncIslemi` entity’si şunları içerir:

- Kimlikler: `KutuphaneId`, `KullaniciId`, `NushaId`.
- Tarihler: `AlinmaTarihi`, `SonTeslimTarihi`, `IadeTarihi`, `UzatmaTarihi`.
- İş Kuralları: `UzatmaSayisi` (maks. 2), `Durumu` (`Aktif`, `IadeEdildi`, `Gecikmis`), gecikme cezası ve not alanları.
- Davranışlar: `OdunciIadeEt()`, `SureUzat()`, `GecikmeDurumunaGec()` gibi metotlar domain kurallarını uygular.

---

### 8. Önemli İş Akışları

- **Katalog Talebi Onayı:** `YeniKatalogTalebiWorkflowService.ApproveAsync()` → katalog kaydı açılır, talep durumu güncellenir.
- **Talep Reddi:** `RejectAsync()` → gerekçe zorunludur, talep final durumuna çekilir.
- **Ödünç İade:** `ReturnOduncIslemiCommand` → iade tarihi, gecikme, not güncellemesi.
- **Ödünç Uzatma:** `ExtendOduncIslemiCommand` → limit ve tarih kontrolleri, notlara uzatma nedeni ekleme.

---

### 9. Tasarım Kararları

- Domain davranışları entity/metot seviyesinde tutuldu; application katmanı yalnızca orchestration yapıyor.
- FluentValidation kuralları dışında iş kuralları `BusinessRules` sınıflarında toplanarak tekrar kullanılabilir hale getirildi.
- Seed verileri gerçekçi senaryolar içeriyor (gecikmiş ödünç, hazır rezervasyon, etkinlik vb.) ve rapor testlerini kolaylaştırıyor.

---

### 10. Geliştirme Önerileri

1. InMemory yerine ilişkisel veritabanı sağlayıcısına geçiş ve migration yönetimi.
2. `OduncIslemiBusinessRules` içerisindeki placeholder metotların repository destekli hale getirilmesi.
3. `CreateKatalogKaydi` komutuna toplu yazar/konu ekleme desteği.
4. Materyal format ve etiket bilgilerinin frontend’de yönetilebilir formlarla sunulması.
5. Authority kayıtlarının dış sistemlerle (Z39.50, KOHA) senkronizasyonu.

---

Bu rehber, domain modeline dair hızlı bir referans sağlar. Detaylı süreçlerin örnekleri için diğer dokümanlara bakabilirsiniz. Katkılarınızı PR veya issue açarak paylaşabilirsiniz.
