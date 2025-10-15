# Katalog Kaydı – Materyal – Nüsha İlişkisi

Bu doküman üç temel domain sınıfı arasındaki ilişkiyi sade bir dille açıklar.

## 1. KatalogKaydi
- Kaynağın bibliyografik bilgisini temsil eder.
- Örnek alanlar: `Baslik`, `Isbn`, `Yayinevi`, `YayinYeri`, `YayinYili`, `Dil`, `Ozet`.
- Tek bir eser için bir kez oluşturulur.
- Birden fazla kütüphane aynı katalog kaydını paylaşabilir.

## 2. Materyal
- Bir katalog kaydının kütüphane özelindeki hizmete alınmış halidir.
- Örnek alanlar: `KutuphaneId`, `MaksimumOduncSuresiGun`, `RezervasyonaAcik`.
- Aynı katalog kaydına bağlı farklı kütüphaneler veya kullanım koşulları için ayrı materyaller oluşturulabilir.
- Her materyalin sahip olduğu fiziksel/dijital kopyalar `Nusha` üzerinden takip edilir.

## 3. Nusha
- Materyalin tekil kopyasını temsil eder (fiziksel veya dijital).
- Örnek alanlar: `Barkod`, `RafId`, `Durumu`.
- Aynı materyale ait birden fazla nüsha olabilir; her biri ayrı barkod ile izlenir.

Bu yapı sayesinde bibliyografik veri (KatalogKaydi), kütüphane bazlı hizmet parametreleri (Materyal) ve kopya takip bilgisi (Nusha) net biçimde ayrılır.
