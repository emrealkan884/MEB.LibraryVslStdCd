# Yol Haritasi (Onceliklendirilmis Sprint Plani)

## Sprint 1 – Guvenlik ve Yetkilendirme
- **Tamamlandi:** ASP.NET Core authorization policy'leri ile rol hiyerarsisi uygulandi. Controller'lar ilgili policy'lerle korundu, seed kullanicilarina ornek rol claim'leri eklendi.
- MEBBIS / e-Okul SSO entegrasyonu icin adapter tasarimi. _(Bekliyor)_
- HTTPS/SSL zorunlulugu, 2FA altyapisi (AuthenticatorService) aktivasyonu. _(Kismen hazir: OTP altyapisi var, uygulama zorlamasi yapilacak)_
- Otomatik yedekleme (cron tabanli job veya SQL Agent scripti) icin PoC. _(Planlandi)_

## Sprint 2 – Kataloglama ve Authority
- **Tamamlandi:** Katalog kaydi olustururken yazar/konu bilgilerini payload ile alip otomatik iliskilendirme akisi.
- MARC21 & RDA dogrulama servisleri (`IMarcValidationService`, `IRdaValidationService`). _(Bekliyor)_
- Dewey siniflamasi icin is kurallarinin zorlayici hale getirilmesi.
- Z39.50 / WorldCat / KOHA import adapter'larinin tasarimi ve temel entegrasyonu.

## Sprint 3 – Dolasim & Barkod/RFID
- Barkod/RFID entegrasyonu icin servisler, coklu barkodla toplu odunc akisi. _(Planlandi)_
- Odunc/iade hatirlatma e-postalari icin background job (Hangfire, Quartz vb.). _(Planlandi)_
- Islemler sonunda fis yazdirma ve kullanici kimlik karti olusturma. _(Planlandi)_
- Kural ihlallerinde kullaniciyi otomatik askiya alma is kurali. _(Planlandi)_

## Sprint 4 – Raporlama ve OPAC
- **Tamamlandi:** Raporlama controller'i CSV/Excel/PDF ciktilari destekleyecek sekilde guncellendi, `ReportExportService` altyapisi eklendi, `ReportingGuide.md` guncellendi.
- Cok boyutlu raporlar (yas, cinsiyet, il, ilce vb.) icin gelismis filtre setleri. _(Bekliyor)_
- Rapor erisimi icin rol bazli yetki modeli. _(Bekliyor)_
- OPAC arayuzunde gelismis arama, kullanilabilirlik ve dijital icerik sunumu. _(Bekliyor)_
- Etiket / format detaylarinin UI formlariyla entegre edilmesi. _(Bekliyor)_

## Surekli Yapilacaklar
- Unicode temizligi ve TR/EN lokalizasyonunun tamamlanmasi.
- Mobil/tablet deneyimi icin UI testleri.
- Her sprint sonunda entegrasyon testleri ve dokumantasyon guncellemeleri.
