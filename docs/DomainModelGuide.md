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


