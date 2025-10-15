# Domain Notlari

## 1. Genel Durum
- Domain katmanindaki temel varliklar Turkce ve sade isimlerle yeniden tanimlandi (`Kutuphane`, `KatalogKaydi`, `Materyal`, `Nusha`, `OduncIslemi`, `Rezervasyon`, `Etkinlik`).
- Kutuphane ici hiyerarsi icin `KutuphaneBolumu`, `Raf`, `MateryalEtiket` siniflari eklendi; okul bazli filtreleme ve yer bilgisi bu yapilar uzerinden yonetiliyor.
- Bibliyografik iliskileri yonetmek icin `Yazar`, `KatalogKaydiYazar`, `YazarRolu` enum u olusturuldu; tum property ler uc ornekle aciklandi.
- Merkez onayi gerektiren kataloglama sureci icin `YeniKatalogTalebi`, `TalepDurumu`, `KaynakTalepId` alanlari ve `Kutuphane` uzerindeki `YeniKatalogTalepleri` koleksiyonu eklendi.
- `KatalogKaydi` sinifi Dewey kodu, Marc21 verisi, RDA bayragi, sayfa sayisi, kapak yolu gibi alanlarla zenginlestirildi; `MateryalTuru` enum u ile baglandi.
- Materyal formatlarini detaylandirmak icin `MateryalFormatDetay` sinifi, Dewey siniflamasi icin `DeweySiniflama` varligi ve authority yapisi icin `OtoriteKaydi`, `OtoriteTuru`, `KatalogKonu` siniflari eklendi.
- Guvenlikten gelen hazir varliklar `Entities/Security` klasorune tasinarak domain yapisi sade tutuldu.
- `nArchGenerator` ile tum domain nesneleri icin CQRS komut/sorgu setleri, AutoMapper profilleri, business kurallari, validatorler, servis arayuzleri ve WebAPI controller lari olusturuldu. Persistence katmaninda ilgili repository ve konfigurasyon siniflari eklendi; `ApplicationServiceRegistration` ve `PersistenceServiceRegistration` uzerinden DI kayitlari guncellendi.
- Tum degisiklikler `dotnet build VisualStudioCode.MEBLibrary.sln` komutuyla dogrulandi (0 uyari, 0 hata).

## 2. Kalan Adimlar
1. Domain servisleri ve validation kurallarini zenginlestir: talep olusurken zorunlu alanlar, materyal turune gore ISBN/ISSN kontrolu, authority baglantisi dogrulama vb. (su anda olusan iskelet kurallar `NotEmpty` seviyesinde).
2. Application katmaninda talep olusturma/onay/ret akislari, katalog kaydina baglama, okulun materyal ve nusha eklemesi icin davranis odakli komut ve sorgulari gelistir.
3. Birim testlerini guncelleyerek yeni davranislari dogrula; talep onayi, otorite baglantisi, Dewey siniflama baglari icin senaryolar yaz.
4. Siradaki gelistirmeleri planla: kullanici/rol modeli, audit log, dijital icerik yonetimi, barkod/RFID, bildirim ve odunc kurallari, raporlama, entegrasyon ve yedekleme.

## 3. Katalog Kaydi – Materyal – Nusha Temel Iliski
- `KatalogKaydi`: Kaynagin bibliyografik kimligi. Baslik, ISBN, Dewey kodu, Marc21 verisi gibi bilgileri tutar. `KutuphaneId` hangi kurumun kaydi yonettigini, `KaynakTalepId` kaydin hangi talep uzerinden olustugunu gosterir. `MateryalTuru` (kitap, dergi, video) ve `MateryalAltTuru` (roman, belgesel, aylik bilim) kaynagin turunu belirtir.
- `Materyal`: Belirli bir kutuphanedeki hizmet parametrelerini tutar. `KutuphaneId` okul ya da merkez, `KutuphaneBolumuId` materyalin bolumu, `MaksimumOduncSuresiGun` ve `RezervasyonaAcik` gibi alanlar yerel politikayi belirtir. `MateryalEtiket` ile okul icin esnek etiketleme yapilabilir.
- `Nusha`: Materyalin tekil kopyasi (fiziksel ya da dijital). `Barkod`, `RafId`, `Durumu` alanlariyla takip edilir. Aynı materyale ait birden fazla nusha olabilir.

## 4. Ayrintili Senaryo: Yahya Turan Fen Lisesi
1. **Yeni kaynak talebi** – Okul kütüphanecisi Mehmet Demir “21. Yuzyilda STEM Egitimi” kitabi icin `YeniKatalogTalebi` olusturur (ISBN, yazar, Dewey kodu, aciklama).
2. **Merkez onayi** – Bakanlik yetkilisi Ayse Kara talebi `TalepDurumu.Onaylandi` yapar ve `KatalogKaydi` olusur; `KutuphaneId` merkez, `KaynakTalepId` talep kaydini gosterir.
3. **Authority ve konu baglantilari** – Yazar `Yazar` ve `KatalogKaydiYazar` uzerinden kayda baglanir, konu basliklari `KatalogKonu` ile `OtoriteKaydi` kayitlarina eslenir.
4. **Format detaylari** – `MateryalFormatDetay` (fiziksel tanim, dil, erisim) olusturulur, kapak resmi kaydedilir.
5. **Okul materyali** – Okul `Materyal` kaydi acar (bolum, odunc suresi, not) ve etiketler (`MateryalEtiket`).
6. **Nusha girisi** – Uc kopya icin `Nusha` kayitlari eklenir; barkodlar “STEM-2024-001” vb.
7. **Rezervasyon ve odunc** – Ogrenci Elif `Rezervasyon` olusturur, Mehmet `OduncIslemi` kaydi ile kitabi verir.
8. **Etkinlik** – “STEM Odakli Bahar Etkinligi” `Etkinlik` kaydi olarak eklenir; kitaplar proje icin kullanilir.
9. **Raporlama** – Merkez yetkilisi `KatalogKaydi -> Materyal -> Nusha -> OduncIslemi` baglantisi uzerinden kopya ve odunc raporu alir.

## 5. Neden `KatalogKaydi.KutuphaneId` Onemli?
- Kaydin kim tarafindan uretildigini ve kimlerin degistirebilecegini belirler.
- Talep uzerinden olusan kayitlar onaylandiginda merkeze devredildigini isaret eder.
- Toplu KOHA aktarmasi gibi talep disi kayitlarin da kaynaginin bilinmesini saglar.
- Audit, raporlama ve izin kontrolu icin temel metadir.

## 6. Otorite Kayitlari Neden Gerekli?
- Otorite kayitlari (`OtoriteKaydi`, `OtoriteTuru`) konu basliklari, yazar adlari ve kurum isimlerinin tutarli sekilde kullanilmasini saglar.
- `YetkiliBaslik` resmi basligi, `AlternatifBasliklar` varyantlari, `BagliTerimler` iliskili kavramlari tutar; `HariciKayitNo` dis sistemlerle eslesmeyi kolaylastirir.
- `KatalogKonu` ve `KatalogKaydiYazar` varliklari katalog kayitlarini authority kayitlarina baglar; aramalarda tutarlilik saglanir.

## 7. Yeni Katalog Talebi Akisi
1. Okul kütüphanesi uygun `KatalogKaydi` bulamazsa `YeniKatalogTalebi` olusturur.
2. Talep kaydinda materyal turu, bibliyografik alanlar, aciklama ve talep eden kutuphane bilgileri bulunur.
3. Merkez talebi inceleyip onaylarsa `KatalogKaydi` olusur, `TalepDurumu` `Onaylandi` olur ve `KaynakTalepId` baglanir.
4. Okul, onaylanan kaydi kullanarak `Materyal` ve `Nusha` kayitlarini acar.
5. Reddedilen talepler gerekce ile saklanir; okul duzeltip yeniden gönderebilir.


## 8. nArchGenerator Ciktilari - Dosya Haritasi
- `src/visualStudioCode.MEBLibrary/Application/Features/*`: Her domain nesnesi icin Create/Update/Delete komutlari, GetById/GetList sorgulari, business kurallari, AutoMapper profilleri ve yerellestirme dosyalari.
- `src/visualStudioCode.MEBLibrary/Application/Services/*`: IoC kaydi yapilmis servis arayuzleri ve Manager siniflari; repository arayuzleri `Application/Services/Repositories` altinda.
- `src/visualStudioCode.MEBLibrary/Persistence/Repositories` ve `Persistence/EntityConfigurations`: EfRepositoryBase tabanli repository ler ile Fluent API konfigurasyonlari.
- `src/visualStudioCode.MEBLibrary/Persistence/Contexts/BaseDbContext.cs`: Tum yeni varliklar icin `DbSet<>` tanimlari.
- `src/visualStudioCode.MEBLibrary/WebAPI/Controllers/*Controller.cs`: CQRS endpoint lerini saglayan REST controller lari.

> Not: Olusan iskelet kodlar domain is kurallari, validasyonlar ve test kapsamiyla detaylandirilmaya devam edecek.
