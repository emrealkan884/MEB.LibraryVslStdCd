# Raporlama ve Denetim Rehberi

Bu rehber, WebAPI üzerindeki raporlama uç noktalarını ve çıktı seçeneklerini açıklar. Örnek istekler Swagger UI, Postman veya herhangi bir HTTP istemcisi ile tekrarlanabilir.

## 1. Geciken Ödünç İşlemleri

- **Endpoint:** `GET /api/Raporlama/odunc/gecikmis`
- **Amaç:** Son teslim tarihi geçmiş ve hâlâ iade edilmemiş ödünç işlemlerini listeler.
- **Sorgu Parametreleri:**
  - `KutuphaneId` *(Guid, opsiyonel)* – Belirli bir kütüphane için filtre.
  - `KullaniciId` *(Guid, opsiyonel)* – Belirli bir kullanıcıya ait geciken işlemleri getirir.

Örnek çağrı:

```http
GET /api/Raporlama/odunc/gecikmis?KutuphaneId=8c4e8f23-28c9-4a27-8cf7-208407828fa9 HTTP/1.1
Host: localhost:5278
Authorization: Bearer <token>
```

**Not:** `DataSeedingExtensions` dosyası gecikmiş bir ödünç kaydı (`stemOverdueOduncId`) içerir; uygulamayı ilk kez ayağa kaldırdığınızda bu uç nokta örnek veri döndürür.

## 2. Ödünç Kullanım İstatistikleri

- **Endpoint:** `GET /api/Raporlama/odunc/kullanim`
- **Amaç:** Belirli tarih aralıklarındaki ödünç alma eğilimlerini raporlar.
- **Sorgu Parametreleri:**
  - `KutuphaneId` *(Guid, opsiyonel)* – Kütüphane bazlı filtre.
  - `BaslangicTarihi`, `BitisTarihi` *(DateTime, opsiyonel)* – Tarih aralığı (UTC, ISO-8601 formatı).

Örnek çağrı:

```http
GET /api/Raporlama/odunc/kullanim?KutuphaneId=8c4e8f23-28c9-4a27-8cf7-208407828fa9&BaslangicTarihi=2025-01-01T00:00:00Z HTTP/1.1
Host: localhost:5278
Authorization: Bearer <token>
```

Boş liste dönmesi seçtiğiniz aralıkta ödünç kaydı olmadığını gösterir; filtreyi genişletebilir veya veri setini artırabilirsiniz.

## 3. Ödünç Aggregasyonu (En Çok Ödünç Alınanlar)

- **Endpoint:** `GET /api/Raporlama/odunc/toplamlar`
- **Amaç:** Ödünç işlemlerini kitap, yazar veya kütüphane boyutunda toplayarak sıralar.
- **Sorgu Parametreleri:**
  - `Dimension` *(Book | Author | Library, zorunlu)* – Toplama boyutu.
  - `Top` *(int, opsiyonel)* – Döndürülecek kayıt sayısı (varsayılan 10).
  - `KutuphaneId`, `BaslangicTarihi`, `BitisTarihi` *(opsiyonel)* – Ek filtreler.

Örnek çağrı (en çok ödünç alınan kitaplar için ilk 20):

```http
GET /api/Raporlama/odunc/toplamlar?Dimension=Book&Top=20 HTTP/1.1
Host: localhost:5278
Authorization: Bearer <token>
```

Yazar veya kütüphane bazlı toplamlara erişmek için `Dimension=Author` ya da `Dimension=Library` değerleri kullanılabilir.

## 4. Raporları Dosya Olarak Dışa Aktarma

Her rapor için JSON çıktısının yanı sıra CSV, Excel veya PDF formatında dosya üretmek mümkündür. İlgili `POST` uç noktaları:

| Rapor | Endpoint | Not |
|-------|----------|-----|
| Geciken ödünçler | `POST /api/Raporlama/odunc/gecikmis/export?format=Csv|Excel|Pdf` | Gövdede `KutuphaneId`, `KullaniciId` vb. filtreler gönderilebilir |
| Ödünç kullanım istatistiği | `POST /api/Raporlama/odunc/kullanim/export?format=Csv|Excel|Pdf` | Tarih aralığı ve kütüphane filtreleri geçerli |
| Ödünç agregasyonları | `POST /api/Raporlama/odunc/toplamlar/export?format=Csv|Excel|Pdf` | Gövdede `Dimension`, `Top` ve diğer filtreler yer almalı |

Örnek istek (en çok ödünç alınan ilk 15 kitabı Excel olarak dışa aktarma):

```http
POST /api/Raporlama/odunc/toplamlar/export?format=Excel HTTP/1.1
Host: localhost:5278
Authorization: Bearer <token>
Content-Type: application/json

{
  "dimension": "Book",
  "top": 15
}
```

Çıktı özetleri:

| Format | İçerik Tipi | Uzantı | Açıklama |
|--------|-------------|--------|----------|
| CSV | `text/csv` | `.csv` | UTF-8, virgül ile ayrılmış |
| Excel | `application/vnd.ms-excel` | `.xls` | HTML tablo, Excel tarafından doğrudan açılır |
| PDF | `application/pdf` | `.pdf` | Basit tablo içeren PDF |

Tüm dosya adları `rapor_YYYYMMDDHHmmss.*` biçiminde zaman damgası içerir.

## 5. Dinamik Filtre ve Sıralama

Export uç noktalarına dinamik filtre (DynamicQuery) yapıları da gönderebilirsiniz. Örneğin geciken ödünç raporunu sadeleştirmek için:

```json
{
  "filter": {
    "logic": "and",
    "filters": [
      { "field": "Durumu", "operator": "neq", "value": "IadeEdildi" },
      { "field": "SonTeslimTarihi", "operator": "lte", "value": "2025-02-01T00:00:00Z" }
    ]
  },
  "sort": [
    { "field": "SonTeslimTarihi", "dir": "asc" }
  ]
}
```

Bu JSON'u ilgili `POST /export` isteğinin gövdesine eklediğinizde, çıktı filtrelenmiş ve sıralanmış şekilde üretilir.

## 6. Denetim (Audit) Kayıtları

MediatR pipeline’ındaki `AuditLoggingBehavior` her komut çalıştığında `AuditLogs` tablosuna kayıt düşer. Kaydedilen alanlar:

- Kullanıcı Id ve kullanıcı adı
- IP adresi ve User-Agent bilgisi
- Komut adı
- Komutun JSON olarak serialized payload’ı
- UTC zaman damgası

Audit log yazımı sırasında oluşan hatalar ana iş akışını engellemez; yine de üretim ortamında izleme yapılması önerilir.

