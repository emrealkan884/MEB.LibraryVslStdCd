# Yol Haritası (Önceliklendirilmiş Sprint Planı)

Bu belge, proje için planlanan sprintleri ve sürekli yapılacak çalışmaları özetler. Tarihler örnek olup, sprint uzunluğu 2 hafta varsayılmıştır.

---

## Sprint 1 – Güvenlik ve Yetkilendirme

- ASP.NET Core authorization policy’leri (`BakanlikYetkisi`, `OkulYetkisiVeyaUstu` vb.) tamamlandı.
- Seed kullanıcılarına örnek rol claim’leri atandı.
- Yapılacaklar:
  - MEBBİS / e-Okul SSO entegrasyon adaptörünün tasarlanması.
  - HTTPS/SSL zorunluluğu ve 2FA altyapısının etkinleştirilmesi.
  - Otomatik yedekleme için planlama (cron job veya SQL Agent).

---

## Sprint 2 – Kataloglama ve Authority

- Katalog kaydı oluştururken yazar/konu ilişkilerini payload üzerinden alma desteği hazırlandı.
- Yapılacaklar:
  - MARC21 & RDA doğrulama servisleri (`IMarcValidationService`, `IRdaValidationService`).
  - Dewey sınıflama kurallarının zorlayıcı hale getirilmesi.
  - Z39.50 / WorldCat / KOHA import adaptörleri.

---

## Sprint 3 – Dolaşım & Barkod/RFID

- Planlanan çalışmalar:
  - Barkod okuyucu ve RFID servis adaptörleri, toplu ödünç akışları.
  - Ödünç/iade hatırlatma e-postaları için background job (Hangfire/Quartz).
  - İşlem fişi ve kullanıcı kartı çıktıları için PDF şablonları.
  - Kural ihlallerinde kullanıcının otomatik askıya alınması.

---

## Sprint 4 – Raporlama ve OPAC

- `RaporlamaController` CSV/Excel/PDF destekleyecek şekilde güncellendi.
- Yapılacaklar:
  - Çok boyutlu raporlar (yaş, sınıf, il/ilçe bazlı).
  - Raporlara rol bazlı yetki modeli.
  - OPAC arayüzünde gelişmiş arama, kullanılabilirlik ve dijital içerik sunumu.
  - Etiket/format detaylarının UI formlarıyla entegrasyonu.

---

## Sürekli Yapılacaklar

- Unicode temizliği ve TR/EN lokalizasyonlarının tamamlanması.
- Mobil/tablet deneyimi için UI testleri ve optimizasyon.
- Her sprint sonunda entegrasyon testleri ile dokümantasyonun güncel tutulması.
- Test kapsamının genişletilmesi (katalog, materyal, ödünç modülleri).

---

## Gelecek Sprint Önerileri

1. **Kimlik Doğrulama:** MEBBİS/e-Okul entegrasyonu sonrası rol hiyerarşisinin derinleştirilmesi.
2. **Entegrasyon:** KOHA veri migrasyonu, ISBN lookup, Cloudinary medya yönetimi otomasyonu.
3. **Donanım:** Barkod/RFID yanında kiosk destekleri.
4. **Bildirim:** E-posta/SMS hatırlatmaları, otomatik uyarı sistemleri.
5. **Analitik:** Dashboard, grafikler, yönetim paneli.

---

Yeni sprint planlarını ve güncellemeleri bu dosya üzerinden takip edebilirsiniz. Değişiklik önerilerinizi PR veya issue olarak iletebilirsiniz.
