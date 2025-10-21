# Yol Haritası (Önceliklendirilmiş Sprint Planı)

## Sprint 1 – Güvenlik ve Yetkilendirme
- **Tamamlandı:** ASP.NET Core authorization policy’leri ile rol hiyerarşisi uygulandı. Controller’lar ilgili policy’lerle korundu, seed kullanıcılarına örnek rol claim’leri eklendi.
- MEBBİS / e-Okul SSO entegrasyonu için adapter tasarımı. _(Bekliyor)_
- HTTPS/SSL zorunluluğu, 2FA altyapısı (AuthenticatorService) aktivasyonu. _(Kısmen hazır: OTP eklentisi var, uygulama zorlaması yapılacak)_
- Otomatik yedekleme (cron tabanlı job veya SQL Agent scripti) için PoC. _(Planlandı)_

## Sprint 2 – Kataloglama ve Authority
- **Tamamlandı:** Katalog kaydı oluştururken yazar/konu bilgilerini payload ile alıp otomatik ilişkilendirme akışı.
- MARC21 & RDA doğrulama servisleri (`IMarcValidationService`, `IRdaValidationService`). 
- Dewey sınıflaması için iş kurallarının zorlayıcı hâle getirilmesi.
- Z39.50/WorldCat/KOHA import adapter’larının tasarımı ve temel entegrasyonu.

## Sprint 3 – Dolaşım & Barkod/RFID
- Barkod/RFID entegrasyonu için servisler, çoklu barkodla toplu ödünç akışı.
- Ödünç/iade hatırlatma e-postaları için background job (Hangfire, Quartz vb.).
- İşlem sonrası fiş yazdırma ve kullanıcı kimlik kartı oluşturma.
- Kullanıcı ihlallerinde otomatik askıya alma iş kuralı.

## Sprint 4 – Raporlama ve OPAC
- Çok boyutlu raporlar (yaş, cinsiyet, il, ilçe vb.) ve PDF/Excel export.
- Rapor erişimi için rol bazlı yetki modeli.
- OPAC arayüzünde gelişmiş arama, kullanılabilirlik ve dijital içerik sunumu.
- Etiket/format detaylarının UI formlarıyla entegre edilmesi.

## Sürekli Yapılacaklar
- Unicode temizlik ve Türkçe/İngilizce lokalizasyonun tamamlanması.
- Mobil/tablet deneyimi için UI testleri.
- Her sprint sonunda entegrasyon testleri ve dokümantasyon güncellemeleri.
