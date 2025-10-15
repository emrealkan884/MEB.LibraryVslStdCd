# Domain Çalışma Durumu

## Yapılanlar
- Domain katmanındaki karmaşık İngilizce isimlendirmeleri kaldırarak Türkçe ve daha sade sınıflar tanımladım (`Kutuphane`, `KatalogKaydi`, `Materyal`, `Nusha`, `OduncIslemi`, `Rezervasyon`, `Etkinlik`).
- Temel iş kavramlarını yöneten enum’ları Türkçeleştirip sadeleştirdim (`KutuphaneTipi`, `MateryalFormati`, `NushaDurumu`, `OduncDurumu`, `RezervasyonDurumu`).
- Kütüphane içi hiyerarşiyi desteklemek için yeni domain sınıfları ekledim (`KutuphaneBolumu`, `Raf`, `MateryalEtiket`) ve `Materyal` ile `Nusha` sınıflarını bu yapıya bağladım.
- Bibliyografik ilişkileri yönetmek için `Yazar`, `KatalogKaydiYazar` varlıklarını ve `YazarRolu` enum’unu ekledim; `KatalogKaydi` üzerinde yazar ilişkileri için alanlar açtım.
- Merkez onayı gerektiren kataloglama sürecini desteklemek için `YeniKatalogTalebi` varlığını ve `TalepDurumu` enum’unu tanımladım; `KatalogKaydi` içine `KaynakTalepId` alanını ekleyerek taleple ilişki kurdum.
- `KatalogKaydi` sınıfını zenginleştirip `MateryalTuru` enum’una geçtim, Dewey, MARC ve kapak bilgisi gibi alanları ekledim; `Kutuphane` sınıfına `YeniKatalogTalepleri` koleksiyonunu ekledim.
- Dewey sınıflaması ve format detaylarını modellemek için `DeweySiniflama` ve `MateryalFormatDetay` varlıklarını ekledim, `KatalogKaydi` ile ilişkilendirdim.
- Konu başlıkları ve otorite kayıtlarını yönetmek için `KatalogKonu`, `OtoriteKaydi` varlıklarını ve `OtoriteTuru` enum’unu ekledim; `KatalogKaydi` ve `KatalogKaydiYazar` ile ilişkilerini tanımladım.
- Okul bazında esnek sınıflandırma ihtiyacını karşılamak için `MateryalEtiket` varlığını korudum; merkezden gelen katalog bilgisini bozmadan okul içi listeleme/filtreleme yapılabiliyor. (`MateryalEtiket` sayesinde her kütüphane, merkezi kayıt üzerinde değişiklik yapmadan “Önerilen Kitaplar”, “Sınav Hazırlık”, “Kulüp Rafı” gibi yerel etiketlerle materyallerini etiketleyebiliyor.)
- `dotnet build VisualStudioCode.MEBLibrary.sln` komutuyla çözümleri derleyerek yeni domain yapısının projede sorunsuz derlendiğini doğruladım.

## Kalan Adımlar
1. Persistence katmanındaki EF Core konfigürasyonlarını yeni sınıf ve enum isimleriyle eşleştir (özellikle `YeniKatalogTalebi`, `KatalogKaydi`-`KaynakTalepId` ilişkisi ve `TalepDurumu`).
2. Domain sınıflarına temel doğrulama ve iş kurallarını ekle: talep oluştururken zorunlu alan kontrolü, format-tabanlı kurallar (kitap için ISBN, dergi için ISSN vb.), talep durum geçişleri.
3. Application katmanında komut/sorgu akışlarını yaz: talep oluşturma, inceleme/onay/ret, talep üzerinden katalog kaydı oluşturma; okul tarafı için katalogdan materyal ekleme komutları.
4. Gerekli birim testlerini güncelleyerek yeni domain davranışlarını doğrula; talep onayı sonrası katalog kaydına bağlanma, reddetme senaryoları.
5. Katmanlar arası bağımlılıkları gözden geçirip yeni adlandırmalarla tutarlı hâle getir (örn. DI kayıtları, AutoMapper profilleri, DTO’lar).
6. İster listesine göre henüz modellenmemiş gelişmiş alanları planla: MARC/RDA detaylarının derinleştirilmesi (alan bazlı doğrulama), dijital içerik yönetimi, bildirim/barkod/kimlik kartı üretimi, Z39.50 entegrasyonu, yedekleme planları.
7. Kataloglama, dolaşım, etkinlik gibi alanlarda eksik ilişkileri netleştir; kullanıcı yetki seviyeleri (bakanlık/il/ilçe/okul), raporlama kapsamları, güvenlik politikaları için domain tarafında hazırlık yap.
8. Yeni domain adaylarını önceliklendirme listesine ekle: `Kullanici`/`Rol` (merkez, il, ilçe, okul yetkileri), `DijitalIcerik`, `BarkodSablonu`, `KimlikKartSablonu`, `EtkinlikMedya`, `EtkinlikKatilan`, `OduncKurali`, `BildirimSablonu`, `RaporTanimi`, `RaporFiltre`, `EntegrasyonBaglantisi`, `YedeklemePlani`, `RFIDTag`, `GorselDepolama`, `KullaniciHareketKaydi`.
