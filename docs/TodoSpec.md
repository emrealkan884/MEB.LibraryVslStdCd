# Millî Eğitim Bakanlığı Kütüphane Otomasyon Sistemi – Çalışma Planı

> Bu dosya, projenin gereksinimlerine göre güncellenen çalışma listesi ve ilerleme kaydını içerir. Her güncellemede tarih ve sorumlu kayıt altına alınmalıdır.

Son güncelleme: 2025-10-23  
Hazırlayan: Codex (GPT-5)

---

## 1. Kataloglama ve Metadata
- ✅ Katalog kaydı oluşturma komutu, yazar/konu ilişkileri ve AutoMapper profilleri tanımlandı (`CreateKatalogKaydiCommand`, `MappingProfiles`).
- ✅ Listeleme DTO’su temel alanları döndürüyor (`GetListKatalogKaydiListItemDto`).
- ✅ Liste yanıtında kullanıcıya dönük alanların zenginleştirilmesi (yazar dizisi, Dewey metni, kopya sayıları, lokasyon).
    - Kütüphane adı, il/ilçe, bölüm, raf ve müsait nüsha bilgisi tek satır etiketlerde sunuluyor.
- ☐ Z39.50 / ISBN tabanlı dış katalogdan veri çekme servislerinin uygulanması.
- ☐ MARC21 doğrulama, RDA tam uyumluluk kontrolleri ve RFID entegrasyonu için arka plan hizmetleri.

## 2. Materyal Yönetimi ve Dolaşım
- ✅ Ödünç verme/iadeye ilişkin temel komutlar, iş kuralları ve rehber dokümantasyon hazır (`Create/Return/ExtendOduncIslemiCommand`, `LoanManagementGuide.md`).
- ☐ Çoklu barkod okutma, RFID, fiş yazdırma ve otomatik e-posta bildirimleri gibi saha gereksinimleri.
- ☐ Gecikme cezaları ve üyelik askıya alma işlemlerinin otomasyonu.
- ☐ Okul yöneticisinin materyali katalog sonrasında stoklayıp demirbaş numarası üretmesi için iş akışı.

## 3. Yeni Katalog Talebi ve Okul İş Akışları
- ✅ Yeni katalog talebi oluşturma, onay/red/inceleme komutları ve iş kuralları tamamlandı.
- ✅ Talep onaylandığında okul kütüphanesine otomatik materyal oluşturma ve bildirim akışı. (Merkez onayında katalog kaydı + okul materyali otomatik oluşturuluyor, log tabanlı bildirim tetikleniyor.)
- ☐ Etkinlik modülü (takvim, görsel yükleme, raporlar) geliştirilmesi.
- ☐ Öğrenci listelerinin toplu içe/dışa aktarımı ve sınıf yükseltme otomasyonları.

## 4. Raporlama ve Analitik
- [x] Katalog ozeti, kullanici ozeti, gecikenler vb. temel rapor sorgulari mevcut (`GetCatalogSummaryQuery`, `GetLoanAggregatesQuery`, `GetOverdueLoansReportQuery`).
- [x] Raporlama rehberi ve model semalari olusturuldu (`ReportingGuide.md`, `SqlReportTemplateDefinition`).
- [x] Il/ilce/kutuphane bazli yetki filtreleriyle rapor erisimi sinirlandirildi (`KullaniciYetkiServisi`, rapor sorgulari guncellendi).
- [ ] PDF/Excel disa aktarim servisleri ve zamanlanmis rapor dagitimi.
- [ ] Ozel rapor sihirbazi ve parametrik SQL sablon yonetimi.

## 5. Kullanici Yonetimi ve Guvenlik
- [x] Roller, politika sabitleri ve temel kimlik dogrulama akisleri hazir (`AuthorizationPolicies`, `LoginCommand`, Vue yonlendirme korumalari).
- [ ] MEBBIS / e-Okul SSO entegrasyonu ve iki faktorlu dogrulama.
- [x] Yetki seviyelerini alan bazinda kisitlayan politikalar ve kapsam kontrolleri guncellendi (KullaniciYetkiServisi, yeni roller).
- [ ] Veri yedekleme, arsivleme, izleme ve uyari mekanizmalarinin devreye alinmasi.

## 6.  Ön Yüz ve Kullanıcı Deneyimi
- ✅ Vue tabanlı çoklu kütüphane yerleşimleri ve yönlendirme korumaları yapılandırıldı (`MerkezLayout.vue`, `OkulLayout.vue`, `router/index.ts`).
- ☐ Türkçe/İngilizce tam lokalizasyon ve dil bazlı içerik yönetimi.
- ☐ Mobil/tablet uyarlamaları ve erişilebilirlik (WCAG) testleri.
- ☐ Kütüphane OPAC deneyiminde kapak görselleri, müsaitlik durumu ve filtrelerin zenginleştirilmesi.

## 7. Operasyonel Hazırlık
- ☐ KOHA’dan toplu veri migrasyonu için script ve doğrulama süreci.
- ☐ Yedekleme stratejisi (dakika/saat/gün/hafta) ve otomatik testleri.
- ☐ Performans, yük ve güvenlik testlerinin planlanması.

---

### Notlar
- Yeni gereksinimler veya tamamlanan işler bu dosyada güncellenmelidir.
- Güncelleme yapılırken “Son güncelleme” ve sorumlu bilgisi değiştirilmelidir.


