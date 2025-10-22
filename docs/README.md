# Domain ve Mimari Notları

Bu belge, MEB Kütüphane Yönetim Sistemi’nin ana bileşenlerini ve mimari kararlarını özetler. Ayrıntılı rehberler için alt dosyalara (`DomainModelGuide`, `LoanManagementGuide`, `ReportingGuide`, `Roadmap`) bakabilirsiniz.

---

## 1. Domain Genel Durumu

- **Varlıklar:** `Kutuphane`, `KatalogKaydi`, `Materyal`, `Nusha`, `Rezervasyon`, `OduncIslemi`, `Etkinlik`, `OtoriteKaydi` vb. Türkçe ve anlaşılır isimler ile yeniden tanımlandı.
- **Kütüphane Hiyerarşisi:** `KutuphaneBolumu`, `Raf`, `MateryalEtiket` yapıları sayesinde okul bazlı lokasyon ve esnek etiketleme yapılabiliyor.
- **Authority Yönetimi:** `Yazar`, `KatalogKaydiYazar`, `KatalogKonu` ve `OtoriteKaydi` varlıkları bibliyografik standartlara uygunluk için entegre edildi.
- **Yeni Katalog Talebi Workflow’u:** `YeniKatalogTalebi` varlığı, `TalepDurumu` enum’u ve `YeniKatalogTalebiWorkflowService` ile talep → inceleme → onay/red akışı sağlandı. Onaylandığında ilgili `KatalogKaydi` otomatik açılır.
- **Dinamik Sorgular:** Tüm ana modüller için `POST /GetListByDynamic` uç noktaları oluşturuldu; `DynamicQuery` yapısı ile kompleks filtreleme destekleniyor.
- **Audit Logging:** MediatR pipeline’ına eklenen `AuditLoggingBehavior` tüm komut çağrılarını `AuditLogs` tablosuna kaydeder (kullanıcı, IP, user-agent, payload, zaman damgası).
- **Seed Verisi:** `WebAPI/Extensions/DataSeedingExtensions.cs` örnek kütüphane, kullanıcı, nusha, ödünç, rezervasyon kayıtlarını sağlar; sistem ilk çalıştırmada gerçekçi senaryolar sunar.

---

## 2. Mimari İlkeler

- **Clean Architecture:** Domain bağımsız, Application katmanı davranış odaklı, Infrastructure ve Persistence detaylardan sorumlu.
- **CQRS:** Tüm iş akışları komut (write) ve sorgu (read) olarak ayrıldı; MediatR üzerinden işleniyor.
- **Validasyon:** FluentValidation tabanlı validator’lar `RequestValidationBehavior` pipeline’ı ile otomatik tetikleniyor.
- **Yetkilendirme:** `AuthorizationPolicies` ve `ApplicationRoles` sabitleri ile rol tabanlı erişim; controller seviyesinde `[Authorize(Policy = ...)]` kullanımı.
- **Localization:** YAML tabanlı çok dilli mesajlar `NArchitecture.Core.Localization` desteği ile yükleniyor.

---

## 3. Çekirdek İş Kuralları

- **Katalog Kaydı:** Aynı kütüphane içinde başlık + ISBN kombinasyonu benzersiz olmalı. Dewey sınıflaması, materyal türü ve format detayları zorunlu alanlar olarak kurgulandı.
- **Yeni Katalog Talebi:** Katalog kaydı oluşturulmuş talepler tekrar onaylanamaz; reddedilen talepler gerekçe saklar. İsbn bilgisi normalize edilerek saklanır.
- **Ödünç İşlemleri:** Her kullanıcı için aktif ödünç limiti, gecikme kontrolleri, uzatma sınırları (maks. 2 kez, 7 gün) ve ceza hesaplaması `OduncIslemiBusinessRules` içinde yönetilir.
- **Otorite Kayıtları:** Authority kayıtları olmadan yazar/konu eşleşmesi yapılmaması hedeflenmiştir. `KatalogKaydiYazar` ve `KatalogKonu` varlıkları authority referanslarını taşır.

---

## 4. Servis ve Adaptörler

- **ImageServiceBase / CloudinaryImageServiceAdapter:** Etkinlik afişleri ve materyal görselleri için örnek adaptör.
- **ReportExportService:** JSON veri kümelerini CSV, HTML tabanlı Excel ve basit PDF formatlarına dönüştürür. `IReportExportService` ile DI üzerinden kullanılır.
- **YeniKatalogTalebiWorkflowService:** Katalog talebi onay/red/inceleme akışını yöneten domain servisi.

---

## 5. Kullanılan Paket ve Çatılar

- **Backend:** MediatR, FluentValidation, AutoMapper, Serilog, MailKit, ElasticSearch client.
- **Frontend:** Vue 3 + Vite, PrimeVue, Pinia, Vue Router, Axios, Tailwind, vue-i18n.
- **Test:** xUnit, FluentAssertions, Moq.

---

## 6. Bilinen Açık Noktalar

- InMemory veritabanı kullanılmaktadır; çoklu sağlayıcı (SQL Server, PostgreSQL, Oracle) desteği henüz eklenmedi.
- Ödünç iş kurallarındaki bazı yardımcı metotlar örnek olarak bırakılmıştır (kuvvetlendirilmesi planlanıyor).
- Frontend bileşenleri prototip düzeyindedir; gerçek API ile entegrasyon ve guard mekanizmaları tamamlanmalıdır.
- Dokümantasyonda belirtilen entegrasyon adaptörleri (MEBBİS, e-Okul, KOHA, Z39.50) henüz implement edilmedi.

---

## 7. Yol Haritası Özeti

`docs/Roadmap.md` dosyasında ayrıntılı sprint hedefleri bulunur. Kısa vade öncelikleri:

1. MEBBİS / e-Okul entegrasyonu ve merkezi kimlik doğrulama.
2. Barkod/RFID servisleri ve ödünç iade süreçlerine entegrasyon.
3. Frontend tarafında dinamik filtre builder ve gerçek API bağlantısı.
4. Gelişmiş raporlar ve dashboard görselleştirmeleri.
5. Test kapsamının genişletilmesi (katalog, materyal, ödünç modülleri).

---

## 8. Ek Belgeler

- [`DomainModelGuide.md`](./DomainModelGuide.md) – Domain varlıkları ve ilişkileri.
- [`LoanManagementGuide.md`](./LoanManagementGuide.md) – Ödünç yönetimi iş kuralları ve API örnekleri.
- [`ReportingGuide.md`](./ReportingGuide.md) – Rapor uç noktaları ve dışa aktarma formatları.
- [`Roadmap.md`](./Roadmap.md) – Sprint planı ve planlanan geliştirmeler.
- [`Semantic Commit Messages.md`](./Semantic Commit Messages.md) – Commit mesajı standartları.

---

Bu doküman, projeyi yeni tanıyan ekip üyelerinin mimariyi hızlıca kavraması için hazırlanmıştır. Eksik gördüğünüz bölümleri geliştirerek katkı sağlayabilirsiniz.
