## Raporlama ve Denetim Rehberi

Bu rehber, WebAPI'deki raporlama uç noktalarının kullanımını ve örnek veri setinin nasıl sonuç ürettiğini açıklar. Örnek çağrılar Postman, Swagger UI veya herhangi bir HTTP istemcisi ile tekrarlanabilir.

### 1. Geciken Ödünç İşlemleri

- **Endpoint:** `GET /api/Raporlama/odunc/overdue`
- **Amaç:** Son teslim tarihi geçmiş, hâlâ iade edilmemiş ödünç işlemlerini listeler.
- **Sorgu Parametreleri:**
  - `KutuphaneId` *(Guid, opsiyonel)* – Belirli bir kütüphane için filtreler.
  - `KullaniciId` *(Guid, opsiyonel)* – Belirli bir kullanıcıya ait geciken işlemleri getirir.

Örnek çağrı:

```http
GET /api/Raporlama/odunc/overdue?KutuphaneId=8c4e8f23-28c9-4a27-8cf7-208407828fa9 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

**Önemli:** `DataSeedingExtensions` artık geçmiş teslim tarihi olan bir örnek ödünç kaydı (`stemOverdueOduncId`) içerir. Uygulama ilk kez ayağa kalktığında bu uç noktaya istek attığınızda en az bir kayıt dönmelidir.

### 2. Ödünç Kullanım İstatistikleri

- **Endpoint:** `GET /api/Raporlama/odunc/usage`
- **Amaç:** Belirli tarih aralıklarında ödünç alma eğilimlerini raporlar.
- **Sorgu Parametreleri:**
  - `KutuphaneId` *(Guid, opsiyonel)* – Kütüphane bazlı filtre.
  - `BaslangicTarihi` ve `BitisTarihi` *(DateTime, opsiyonel)* – Tarih aralığı seçimi.

Örnek çağrı:

```http
GET /api/Raporlama/odunc/usage?KutuphaneId=8c4e8f23-28c9-4a27-8cf7-208407828fa9&BaslangicTarihi=2025-01-01T00:00:00Z HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

Endpoint boş liste dönerse seçilen tarihlerde ödünç kaydı yok demektir; sorguyu tarih filtresi olmadan deneyebilir veya veri setini genişletebilirsiniz.

### 3. CSV Dışa Aktarım

Her iki rapor da CSV formatında dışa aktarılabilir:

- `POST /api/Raporlama/odunc/overdue/export`
- `POST /api/Raporlama/odunc/usage/export`

Örnek istek gövdesi:

```http
POST /api/Raporlama/odunc/overdue/export HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "KutuphaneId": "8c4e8f23-28c9-4a27-8cf7-208407828fa9"
}
```

Sunucu `text/csv` içerik tipinde, zaman damgalı bir dosya adı (`overdue-loans_YYYYMMDDHHmmss.csv`) ve UTF-8 içeriği döner.

### 4. DynamicQuery ile Filtreleme

Export uç noktalarına `DynamicQuery` gövdesi ile daha karmaşık filtreler uygulanabilir. Örnek kullanım:

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

Bu yapı `POST /api/Raporlama/odunc/overdue/export` isteğinin gövdesine eklendiğinde, CSV çıktısı filtrelenmiş ve sıralanmış şekilde üretilecektir.

### 5. Denetim (Audit) Kayıtları

MediatR pipeline’ına eklenen `AuditLoggingBehavior` sayesinde her komut işlendiğinde `AuditLogs` tablosuna kayıt düşülür. Kayıt içeriği:

- Kullanıcı Id ve kullanıcı adı
- IP adresi ile User-Agent
- Komut adı
- Komutun JSON payload’ı
- İşlemin gerçekleştiği tarih

Varsayılan kurguda audit hataları ana iş akışını engellemez; log yazımı sırasında yaşanan sorunlar swallow edilerek komutun devam etmesine izin verilir. İhtiyaç halinde bu tablo üzerinden ayrı raporlar oluşturabilirsiniz.
