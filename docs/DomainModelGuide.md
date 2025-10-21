## 3. Katalog Kaydi � Materyal � Nusha Temel Iliski
- `KatalogKaydi`: Kaynagin bibliyografik kimligi. Baslik, ISBN, Dewey kodu, Marc21 verisi gibi bilgileri tutar. `KutuphaneId` hangi kurumun kaydi yonettigini, `KaynakTalepId` kaydin hangi talep uzerinden olustugunu gosterir. `MateryalTuru` (kitap, dergi, video) ve `MateryalAltTuru` (roman, belgesel, aylik bilim) kaynagin turunu belirtir.
- `Materyal`: Belirli bir kutuphanedeki hizmet parametrelerini tutar. `KutuphaneId` okul ya da merkez, `KutuphaneBolumuId` materyalin bolumu, `MaksimumOduncSuresiGun` ve `RezervasyonaAcik` gibi alanlar yerel politikayi belirtir. `MateryalEtiket` ile okul icin esnek etiketleme yapilabilir.
- `Nusha`: Materyalin tekil kopyasi (fiziksel ya da dijital). `Barkod`, `RafId`, `Durumu` alanlariyla takip edilir. Ayn� materyale ait birden fazla nusha olabilir.

## 4. Ayrintili Senaryo: Yahya Turan Fen Lisesi
1. **Yeni kaynak talebi** � Okul k�t�phanecisi Mehmet Demir �21. Yuzyilda STEM Egitimi� kitabi icin `YeniKatalogTalebi` olusturur (ISBN, yazar, Dewey kodu, aciklama).
2. **Merkez onayi** � Bakanlik yetkilisi Ayse Kara talebi `TalepDurumu.Onaylandi` yapar ve `KatalogKaydi` olusur; `KutuphaneId` merkez, `KaynakTalepId` talep kaydini gosterir.
3. **Authority ve konu baglantilari** � Yazar `Yazar` ve `KatalogKaydiYazar` uzerinden kayda baglanir, konu basliklari `KatalogKonu` ile `OtoriteKaydi` kayitlarina eslenir.
4. **Format detaylari** � `MateryalFormatDetay` (fiziksel tanim, dil, erisim) olusturulur, kapak resmi kaydedilir.
5. **Okul materyali** � Okul `Materyal` kaydi acar (bolum, odunc suresi, not) ve etiketler (`MateryalEtiket`).
6. **Nusha girisi** � Uc kopya icin `Nusha` kayitlari eklenir; barkodlar �STEM-2024-001� vb.
7. **Rezervasyon ve odunc** � Ogrenci Elif `Rezervasyon` olusturur, Mehmet `OduncIslemi` kaydi ile kitabi verir.
8. **Etkinlik** � �STEM Odakli Bahar Etkinligi� `Etkinlik` kaydi olarak eklenir; kitaplar proje icin kullanilir.
9. **Raporlama** � Merkez yetkilisi `KatalogKaydi -> Materyal -> Nusha -> OduncIslemi` baglantisi uzerinden kopya ve odunc raporu alir.

## 5. Neden `KatalogKaydi.KutuphaneId` Onemli?
- Kaydin kim tarafindan uretildigini ve kimlerin degistirebilecegini belirler.
- Talep uzerinden olusan kayitlar onaylandiginda merkeze devredildigini isaret eder.
- Toplu KOHA aktarmasi gibi talep disi kayitlarin da kaynaginin bilinmesini saglar.
- Audit, raporlama ve izin kontrolu icin temel metadir.

## 6. Otorite Kayitlari Neden Gerekli?
- Otorite kayitlari (`OtoriteKaydi`, `OtoriteTuru`) konu basliklari, yazar adlari ve kurum isimlerinin tutarli sekilde kullanilmasini saglar.
- `YetkiliBaslik` resmi basligi, `AlternatifBasliklar` varyantlari, `BagliTerimler` iliskili kavramlari tutar; `HariciKayitNo` dis sistemlerle eslesmeyi kolaylastirir.
- `KatalogKonu` ve `KatalogKaydiYazar` varliklari katalog kayitlarini authority kayitlarina baglar; aramalarda tutarlilik saglanir.

## 7. Otorite Kaydi, Yazar ve Konu Baglantisi
1. Gerekli kisi/kurum/konu icin `POST /api/OtoriteKayitlari` ile authority kaydi olusturun.
2. Yazar kaydini (`POST /api/Yazarlar`) ekleyin.
3. Katalog kaydini (`POST /api/KatalogKayitlari`) olusturun.
4. Yazar-katalog eslesmesini `POST /api/KatalogKaydiYazarlar` ile olustururken `OtoriteKaydiId` alanini doldurun.
5. Konu baglantisi icin `POST /api/KatalogKonular` uzerinden `KatalogKaydiId` ve `OtoriteKaydiId` bilgilerini gonderin.

Bu adimlar authority kontrolu ile yazar ve konu basliklarinin standart kalmasini saglar.

## 8. MateryalFormatDetay Kullanimi
- `MateryalFormatDetay` bir katalog kaydinin fiziki veya dijital formatini aciklar: MARC 300/347/538 alanlarinin karsiligidir.
- `FizikselTanimi`: sayfa/boyut bilgisi.
- `SureBilgisi`: video, ses, DVD gibi materyallerde sure veya disk sayisi.
- `FormatBilgisi`: PDF, MP4, ciltli vb.
- `Dil`: formatin dili, bibliyografik kayit ile farkli olabilir (ceviri baski).
- `ErisimBilgisi`: URL, intranet notu ya da fiziksel erisim uyarisi.
- Akis: Ilk olarak katalog kaydi olusturulur, ardindan `POST /api/MateryalFormatDetaylar` ile farkli formatlar eklenir. Gerektiginde GET/PUT/DELETE ile guncellenebilir.

## 9. MateryalEtiket Neden Var?
- Okul veya merkez kutuphaneleri icin ozgurluk saglayan esnek etiket yapisidir.
- Sorgulamalarda populer listeler (\"OkumaKulubu\", \"STEM\" gibi) veya kampanya bazli filtreleme yapmak icin kullanilir.
- Etiketler `POST /api/MateryalEtiketler` ile eklenir, ayni materyale birden fazla etiket baglanabilir.

## 10. KatalogKonu ve KutuphaneBolumu Arasindaki Fark
- `KatalogKonu`: Kaynagin entelektuel icerigini (konu basligi) tanimlar ve authority kaydi ile baglanabilir.
- `KutuphaneBolumu`: Kaynagin fiziksel olarak bulundugu lokasyonu belirtir (bolum, salon).
- Bir kitap \"STEM egitimi\" konusuna sahip olurken fiziksel olarak okulun \"STEM\" bolumundeki raflarda bulunabilir; bu nedenle iki farkli varlikla takip edilir.

## 11. Katalog Kaydina Yazar Baglama Akisi
- `KatalogKaydi` olusturulduktan sonra her yazar icin `KatalogKaydiYazar` kaydi eklenir.
- Zorunlu alanlar: `KatalogKaydiId`, `YazarId`, `Rol`, `Sira`. Opsiyonel: `OtoriteKaydiId`.
- Boylece ayni kayit birden fazla yazar, cevirmen veya editor ile sirali sekilde eslenebilir.
- Ileride otomatiklestirme icin `CreateKatalogKaydi` komutuna yazar listesi eklenebilir; mevcut mimari, SOLID prensibi geregi bu sorumlulugu bagli servis uzerinde tutar.

## 12. Rol Hiyerarsisi ve Policy Kullanimı
- Tanımlı roller: `Role.BakanlikYetkilisi`, `Role.IlYetkilisi`, `Role.IlceYetkilisi`, `Role.OkulKutuphaneYoneticisi`.
- Politika isimleri `AuthorizationPolicies` altında tutulur ve şu eşleşmeleri kullanır:
  - `RequireMinistry`: yalnızca `Role.BakanlikYetkilisi`.
  - `RequireProvinceOrAbove`: Bakanlık veya İl yetkilileri.
  - `RequireDistrictOrAbove`: Bakanlık, İl, İlçe.
  - `RequireSchoolOrAbove`: tüm roller.
- `Program.cs` içinde `AddLibraryAuthorization()` çağrısı ile politikalar kaydedilir; controller'larda `[Authorize(Policy = ...)]` ile uygulanır.
- Seed edilen `kutuphane.yonetici@example.com` kullanıcısı hem `Role.BakanlikYetkilisi` hem de `Role.OkulKutuphaneYoneticisi` claim'lerine sahiptir; gereksinime göre ek kullanıcı-rol eşleştirmeleri yapılabilir.


