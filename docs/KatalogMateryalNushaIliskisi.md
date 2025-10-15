# Katalog Kaydı – Materyal – Nüsha İlişkisi

Bu doküman üç temel domain sınıfı arasındaki ilişkiyi sade bir dille açıklar.

## 1. KatalogKaydi
- Kaynağın bibliyografik bilgisini temsil eder.
- Örnek alanlar: `Baslik`, `Yazar`, `Isbn`, `Yayinevi`, `YayinYili`, `Dil`.
- Tek bir eser için bir kez oluşturulur.
- Birden fazla kütüphane aynı katalog kaydını paylaşabilir.

## 2. Materyal
- Bir katalog kaydının kütüphane özelindeki hizmete alınmış halidir.
- Örnek alanlar: `KutuphaneId`, `MaksimumOduncSuresiGun`, `RezervasyonaAcik`.
- Aynı katalog kaydına bağlı farklı kütüphaneler veya kullanım koşulları için ayrı materyaller oluşturulabilir.
- Her materyalin sahip olduğu fiziksel veya dijital kopyalar `Nusha` üzerinden takip edilir.

## 3. Nusha
- Materyalin tekil kopyasını temsil eder (fiziksel veya dijital).
- Örnek alanlar: `Barkod`, `RafKonumu`, `Durumu`.
- Aynı materyale ait birden fazla nüsha olabilir; her biri ayrı barkod ile izlenir.

## Örnek Senaryo
1. “Benim Adım Kırmızı” isimli kitap için tek bir `KatalogKaydi` oluşturulur.
2. Merkez kütüphane bu kayda bağlı bir `Materyal` açar (ödünç süresi 15 gün, rezervasyon açık).
3. Okul kütüphanesi aynı kaydı kendi `Materyal`iyle kullanır (ödünç süresi 7 gün, rezervasyon kapalı).
4. Okul kütüphanesinde rafta üç adet fiziksel kopya varsa her biri için ayrı `Nusha` kaydı tutulur.

Bu yapı sayesinde bibliyografik veri (KatalogKaydi), kütüphane bazlı hizmet parametreleri (Materyal) ve kopya takip bilgisi (Nusha) net biçimde ayrılır.
