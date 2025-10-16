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
- Ayn� kutuphanenin ayni baslik/ISBN ile tekrar talep olusturmasi engellendi; CreateYeniKatalogTalebiCommand oncesinde benzersiz talep kontrolu yapiliyor.
- Katalog talep akisi icin `ApproveYeniKatalogTalebiCommand` ve `RejectYeniKatalogTalebiCommand` dahil olmak uzere tum komut/yanit siniflari ile `YeniKatalogTalebiWorkflowService` yazildi; onaylandiginda katalog kaydi otomatik aciliyor, reddedildiginde gerekce saklaniyor ve workflow adimlari is kurallariyla baglandi.
- YeniKatalogTalep listesi icin klasik `GET /api/YeniKatalogTalepleri` endpoint'i aktif tutuldu; ayrica ayrintili filtreleme saglayan `POST /api/YeniKatalogTalepleri/GetListByDynamic` endpoint'i eklendi.
- Ayni strateji tum temel feature'lara tasinarak `POST /api/<Feature>/GetListByDynamic` uc noktalarinda tutarli dinamik filtreleme destegi saglandi (Materyaller, KatalogKayitlari, Kutuphaneler vb.).
- Swagger'da dinamik filtreleme uc noktalarini test etmek icin `Request body` alanina asagidaki gibi ornek bir payload birakildi; `DynamicQuery` icerigini degistirerek farkli kombinasyonlari deneyebilirsin:
  ```json
  {
    "filter": {
      "logic": "and",
      "filters": [
        { "field": "Durum", "operator": "eq", "value": "Beklemede" },
        { "field": "TalepTarihi", "operator": "gte", "value": "2025-01-01T00:00:00Z" }
      ]
    },
    "sort": [
      { "field": "TalepTarihi", "dir": "desc" }
    ]
  }
  ```
- UI/istemci katmaninda filtre formu olustururken kullanicinin operator secimini (`eq`, `contains`, `gte` vb.) ve deger tipini (metin, enum, tarih) belirtmesine imkan tanina; olusan filtreler JSON'a cevrilerek ilgili endpoint'e gonderilir.
- Yeni workflow (`Beklemede -> Inceleniyor -> Onaylandi/Reddedildi`) icin istemci tarafinda kart/etiket renkleriyle durum gosterimi ve `review` komutunu tetikleyen bir aksiyon butonu eklenmesi planlanmali. Durum degistiginde liste uc noktalarindaki dinamik filtreler sayesinde ekran anlik olarak guncellenebilir.

### Yol Haritası / Planlanan İyileştirmeler
- **Kimlik ve Yetki**: MEBBİS / e-Okul entegrasyonu için merkezi kimlik sağlayıcı, rol hiyerarşisi ve policy tabanlı `[Authorize]` kontrolleri.
- **Raporlama**: Geciken materyal, kullanım sıklığı, ödünç/iade istatistiklerini PDF/Excel olarak üreten CQRS sorguları.
- **Entegrasyon**: KOHA veri aktarımı, Z39.50 & ISBN lookup, dijital içerik (Word/PDF) ve resim saklama servisleri.
- **Donanım destekleri**: Barkod/RFID adaptörleri, toplu materyal/yazar/öğrenci import akışı, kullanıcı kartı basımı.
- **Yedekleme & Bildirim**: Otomatik yedekleme job’ları, ödünç/iade hatırlatma e-postaları veya SMS bildirimleri.
- **UI/Swagger**: Dinamik filtre formunun ve `review` akışının ön yüz destekleri; Swagger’da tüm yeni endpoint’ler için örnekler.
- **Test & Audit**: Workflow ve dinamik filtre uç noktaları için birim/entegrasyon testleri, audit log’ların raporlanması.
- **Çoklu dil**: TR/EN localization dosyalarının tamamlanması ve istemcinin dil seçimi desteği.
- Talep inceleme adimi icin `POST /api/YeniKatalogTalepleri/{id}/review` endpoint'i eklendi; talepler `Beklemede -> Inceleniyor -> Onaylandi/Reddedildi` durum zincirini izler.
- Onay isleminde merkez kullanici `MateryalTuru` ve `MateryalAltTuru` degerlerini istekte acikca girmeye devam ediyor; workflow yalnizca katalog kaydini olusturuyor, materyal/nusha otomasyonu devrede degil.
- Tum degisiklikler `dotnet build VisualStudioCode.MEBLibrary.sln` komutuyla dogrulandi (0 uyari, 0 hata).

## 2. Kalan Adimlar
1. WebAPI katmaninda onay/ret komutlarini aciga cikaran endpoint leri ekle ve yetkilendirme politikalarini netlestir.
2. Yeni workflow ve is kurallari icin birim/integration testleri yazarak onay ve red senaryolarini dogrula.
3. Talep olusturma/guncelleme validatorlerini ISBN, ISSN ve materyal turu baglami ile zenginlestir.
4. Yerellestirme dosyalarini TR/EN olarak genisletip Swagger uzerinde yeni akisi belgele.

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

## 7. Yeni Katalog Talebi Akisi
1. Okul kutuphanesi uygun `KatalogKaydi` bulamazsa `YeniKatalogTalebi` olusturur ve kayit otomatik olarak `TalepDurumu = Beklemede` olur.
2. Merkez gorevlisi talebi isleme aldiginda `POST /api/YeniKatalogTalepleri/{id}/review` endpoint'i ile durum `Inceleniyor` yapilir; boylece kaydin aktif olarak incelendigini izlemek kolaylasir.
3. Inceleme tamamlandiginda `approve` veya `reject` komutlari kullanilir. Onayda `KatalogKaydi` olusur ve durum `Onaylandi`, reddedilirse gerekce saklanarak durum `Reddedildi` olur.
4. Onaylanan kayit uzerinden okul `Materyal` ve `Nusha` kayitlarini acar.
5. Reddedilen talepler gerekcesi ile saklandigindan okul gerekli duzeltmeleri yapip yeni talep olusturabilir.
## 8. nArchGenerator Ciktilari - Dosya Haritasi
- `src/visualStudioCode.MEBLibrary/Application/Features/*`: Her domain nesnesi icin Create/Update/Delete komutlari, GetById/GetList sorgulari, business kurallari, AutoMapper profilleri ve yerellestirme dosyalari.
- `src/visualStudioCode.MEBLibrary/Application/Services/*`: IoC kaydi yapilmis servis arayuzleri ve Manager siniflari; repository arayuzleri `Application/Services/Repositories` altinda.
- `src/visualStudioCode.MEBLibrary/Persistence/Repositories` ve `Persistence/EntityConfigurations`: EfRepositoryBase tabanli repository ler ile Fluent API konfigurasyonlari.
- `src/visualStudioCode.MEBLibrary/Persistence/Contexts/BaseDbContext.cs`: Tum yeni varliklar icin `DbSet<>` tanimlari.
- `src/visualStudioCode.MEBLibrary/WebAPI/Controllers/*Controller.cs`: CQRS endpoint lerini saglayan REST controller lari.

> Not: Olusan iskelet kodlar domain is kurallari, validasyonlar ve test kapsamiyla detaylandirilmaya devam edecek.

## 9. Otorite Senaryosunu Adim Adim Denemek
WebAPI uygulamasi calistiginda InMemory veritabani otomatik olarak ornek verilerle tohumlaniyor (`WebAPI/Extensions/DataSeedingExtensions.cs`). Swagger uzerinden ya da herhangi bir HTTP istemcisiyle asagidaki akisi izleyebilirsin:

1. **Otorite kayitlarini listele**  
   `GET /api/OtoriteKayitlari` � �Pamuk, Orhan� (Kisi) ve �T�rk romanlar�� (Konu) otoritelerini g�r�rs�n.
2. **Hazir katalog kaydini incele**  
   `GET /api/KatalogKayitlari/{id}`  
   �rnek id: `B89F8F4E-29C3-4F1C-A301-16D5B7BBC7DB`. D�nen DTO i�inde hem `KatalogKaydiYazarlar` hem `KatalogKonular` baglanti otorite kimliklerini gosterir.
3. **Yeni otorite ekle**  
   `POST /api/OtoriteKayitlari`  
   ```json
   {
     "yetkiliBaslik": "Pamuk, Orhan (TR �rne�i)",
     "otoriteTuru": 2,
     "alternatifBasliklar": "Orhan Pamuk TR; Pamuk, O. (TR)",
     "aciklama": "Deneme icin ikinci otorite"
   }
   ```  
   Ayn� yetkili ba�l�k ve t�rle tekrar denersen `OtoriteKaydiAlreadyExists` hatasi al�rs�n (business rule).
4. **Katalog yazar baglantisi olustur**  
   `POST /api/KatalogKaydiYazarlar`  
   ```json
   {
     "katalogKaydiId": "B89F8F4E-29C3-4F1C-A301-16D5B7BBC7DB",
     "yazarId": "D6D6DF64-9F3F-4C40-9BA7-9B8177F9C0E7",
     "otoriteKaydiId": "E2E6A5C8-AD7B-4B07-A1A2-7AE8281F34D3",
     "rol": 1,
     "sira": 1
   }
   ```  
   `otoriteKaydiId` bos veya hataliysa is kurali devreye girer ve istek reddedilir.
5. **Katalog konu baglantisi olustur**  
   `POST /api/KatalogKonulari`  
   ```json
   {
     "katalogKaydiId": "B89F8F4E-29C3-4F1C-A301-16D5B7BBC7DB",
     "konuBasligi": "T�rk romanlar�",
     "otoriteKaydiId": "6F22A3D3-AD77-4F47-B5D0-7D1A0DE7C2A4"
   }
   ```  
   Girdi otorite kayd�na ba�lanamad���nda `OtoriteKaydiNotExistsForSubject` hatas�n� g�r�rs�n.

Bu ak��, otorite kayd�n�n katalog nesneleriyle nas�l zorunlu ve do�rulanm�� �ekilde ili�kilendirildi�ini g�zlemlemeyi sa�lar.

> Not: Yeni katalog talebini onaylamak icin POST /api/YeniKatalogTalepleri/{id}/approve, reddetmek icin POST /api/YeniKatalogTalepleri/{id}/reject endpointlerini kullanabilirsin.

## 10. Yeni Katalog Talepleri Dinamik Filtreleme
- Varsayilan listeleme icin `GET /api/YeniKatalogTalepleri?pageIndex=0&pageSize=10` kullanilmaya devam edilir.
- Nitelikli aramalar icin `POST /api/YeniKatalogTalepleri/GetListByDynamic?pageIndex=0&pageSize=20` endpoint'i eklendi. Govdede `DynamicQuery` modeli beklenir; operator degerleri `eq`, `neq`, `contains` gibi anahtar kelimelerle calisir.
- Enum alanlarinda (`Durum`) deger olarak enum ismi (`Beklemede`, `Inceleniyor`, `Onaylandi`, `Reddedildi`) gonderilmelidir. Ornek istek:
  ```json
  {
    "filter": {
      "field": "Durum",
      "operator": "eq",
      "value": "Reddedildi"
    },
    "sort": [
      { "field": "TalepTarihi", "dir": "desc" }
    ]
  }
  ```
- Birden fazla kosul icin `logic` ve `filters` alanlari kullanilabilir:
  ```json
  {
    "filter": {
      "logic": "and",
      "filters": [
        { "field": "Durum", "operator": "eq", "value": "Inceleniyor" },
        { "field": "TalepEdenKutuphaneId", "operator": "eq", "value": "5c87d9f1-6f9b-46e0-90f0-7f205874a9f8" }
      ]
    }
  }
  ```
- Endpoint, gelen filtreyi `YeniKatalogTalebi` repository'sindeki `GetListByDynamicAsync` ile calistirarak sayfalama bilgisiyle birlikte DTO listesini dondurur.

## 11. Dewey Onlu Siniflama Tablolari Neden Ayrı Saklaniyor?
- Dewey Decimal Classification (DDC) kitaplari tum dunyada ayni sayisal kodlarla konuya gore siralar; 000 Genel Konular, 100 Felsefe, 500 Bilim, 510 Matematik gibi kodlar standarttir ve OCLC tarafindan guncellenir.
- Kodlar degismediginden, sistemler arasi veri aktariminda (KOHA'dan gecis, milli kutuphane entegrasyonu vb.) DeweyId'nin korunmasi tutarlilik saglar.
- Veritabaninda `DeweySiniflama` tablosunu ayri tutarak hiyerarsik yapiyi (ust kategori, aciklama, yerel notlar) saklayabilir, UI'da agac veya filtre olarak sunabilir ve yeni kayitlarda dogrulama yapabiliriz.
- Materyal ve katalog kayitlari sadece kodu referans eder (`DeweySiniflamaId`), boylece kod degisimleri merkezi tablodan yonetilir ve raporlar (ornegin tum 510 Matematik kaynaklari) kolayca alinir.







