# Domain Çalışma Durumu

## Yapılanlar
- Domain katmanındaki karmaşık İngilizce isimlendirmeleri kaldırarak Türkçe ve daha sade sınıflar tanımladım (`Kutuphane`, `KatalogKaydi`, `Materyal`, `Nusha`, `OduncIslemi`, `Rezervasyon`, `Etkinlik`).
- Temel iş kavramlarını yöneten enum’ları Türkçeleştirip sadeleştirdim (`KutuphaneTipi`, `MateryalFormati`, `NushaDurumu`, `OduncDurumu`, `RezervasyonDurumu`).
- Kütüphane içi hiyerarşiyi desteklemek için yeni domain sınıfları ekledim (`KutuphaneBolumu`, `Raf`, `MateryalEtiket`) ve `Materyal` ile `Nusha` sınıflarını bu yapıya bağladım.
- Eski aggregate/value object yapılarını temizleyerek ilk adımda anlaşılması kolay bir domain temelini oluşturdum.
- `dotnet build VisualStudioCode.MEBLibrary.sln` komutuyla çözümleri derleyerek yeni domain yapısının projede sorunsuz derlendiğini doğruladım.

## Kalan Adımlar
1. Persistence katmanındaki EF Core konfigürasyonlarını yeni sınıf ve enum isimleriyle eşleştir.
2. Domain sınıflarına temel doğrulama ve iş kurallarını (ör. zorunlu alan kontrolleri, süre limitleri) ekle.
3. Uygulama (Application) katmanındaki servisleri ve DTO’ları yeni domain yapısına göre güncelle.
4. Gerekli birim testlerini güncelleyerek yeni domain davranışlarını doğrula.
5. Katmanlar arası bağımlılıkları tekrar gözden geçirip yeni adlandırmalarla tutarlı hâle getir (örn. DI kayıtları, AutoMapper profilleri).
6. Önceki ayrıntılı domain tasarımında bulunan gelişmiş kavramları (authority kayıtları, MARC alanları, dijital varlıklar, edinme süreçleri, bildirim politikaları, entegrasyon planları, raporlama modelleri, RFID desteği, barkod yönetimi, kullanıcı kartı üretimi, etkinlik raporlama, Z39.50 entegrasyonu, otomatik yedekleme senaryoları) tekrar hangi seviyede ihtiyaç duyulacağına karar ver; gerekiyorsa bu bileşenleri Türkçe ve sadeleştirilmiş şekilde yeniden modelle.
7. Kataloglama, dolaşım ve etkinlik gibi alanların sınırlarını yeniden belirleyerek eksik kalan ilişkileri (ör. birden fazla kütüphane hiyerarşisi, kullanıcı rollerine göre erişim) netleştir ve domain nesnelerini buna göre genişlet.
