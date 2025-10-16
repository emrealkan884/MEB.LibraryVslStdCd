# Raporlama ve Denetim Dokümantasyonu

Bu belge, sistemde eklenen raporlama uç noktalarının nasıl kullanılacağını, çıktılarının nasıl dışa aktarılacağını ve denetim (audit) kayıtlarının nasıl tutulduğunu anlatır. Örnekler REST çağrıları üzerinden verilmiştir; aynı istekler Swagger veya Postman gibi araçlarla da gönderilebilir.

## 1. Geciken Ödünç İşlemleri

### Endpoint
- `GET /api/Raporlama/odunc/overdue`

### Query Parametreleri
| Parametre       | Tip    | Açıklama                                                     |
|-----------------|--------|--------------------------------------------------------------|
| `KutuphaneId`   | `Guid` | İsteğe bağlı. Belirli bir kütüphane için filtreler.          |
| `KullaniciId`   | `Guid` | İsteğe bağlı. Belirli bir kullanıcıya ait geciken kayıtları döndürür. |

### Örnek Çağrı
```http
GET /api/Raporlama/odunc/overdue?KutuphaneId=5c87d9f1-6f9b-46e0-90f0-7f205874a9f8 HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

### Örnek Yanıt
```json
[
  {
    "oduncIslemiId": "3f8e5baf-8ad8-4585-9414-0c912e8fdf48",
    "kutuphaneId": "5c87d9f1-6f9b-46e0-90f0-7f205874a9f8",
    "kullaniciId": "6f0fb04a-2a72-4f5e-b334-7a09dc7c046a",
    "nushaId": "28454a8b-7601-4d54-af0d-6fa1f1259fb3",
    "materyalId": "b6831c85-894e-4aa5-a27e-a35c7f0cdbf3",
    "materyalBaslik": "Benim Adım Kırmızı",
    "alinmaTarihi": "2025-01-05T09:00:00Z",
    "sonTeslimTarihi": "2025-01-20T09:00:00Z",
    "gecikenGun": 12
  }
]
```

## 2. Ödünç Kullanım İstatistikleri

### Endpoint
- `GET /api/Raporlama/odunc/usage`

### Query Parametreleri
| Parametre          | Tip       | Açıklama                                                   |
|--------------------|-----------|------------------------------------------------------------|
| `KutuphaneId`      | `Guid`    | İsteğe bağlı. Belirli kütüphane için rapor üretir.         |
| `BaslangicTarihi`  | `DateTime`| İsteğe bağlı. Bu tarihten sonraki ödünçleri dikkate alır.  |
| `BitisTarihi`      | `DateTime`| İsteğe bağlı. Bu tarihe kadar olan ödünçleri dahil eder.   |

### Örnek Çağrı
```http
GET /api/Raporlama/odunc/usage?KutuphaneId=5c87d9f1-6f9b-46e0-90f0-7f205874a9f8&BaslangicTarihi=2025-01-01T00:00:00Z&BitisTarihi=2025-02-01T00:00:00Z HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
```

### Örnek Yanıt
```json
[
  {
    "kutuphaneId": "5c87d9f1-6f9b-46e0-90f0-7f205874a9f8",
    "materyalId": "b6831c85-894e-4aa5-a27e-a35c7f0cdbf3",
    "materyalBaslik": "Benim Adım Kırmızı",
    "toplamOdunc": 18,
    "aktifOdunc": 4,
    "gecikenOdunc": 2,
    "iadeEdilenOdunc": 12
  }
]
```

## 3. CSV Dışa Aktarım (Export)

Her iki rapor için CSV çıktısı alınabilir:
- `POST /api/Raporlama/odunc/overdue/export`
- `POST /api/Raporlama/odunc/usage/export`

### Örnek İstek
```http
POST /api/Raporlama/odunc/overdue/export HTTP/1.1
Host: localhost:5000
Authorization: Bearer <token>
Content-Type: application/json

{
  "KutuphaneId": "5c87d9f1-6f9b-46e0-90f0-7f205874a9f8"
}
```

### Dönüş
Sunucu, `text/csv` içerik tipinde ve adında zaman damgası bulunan (`overdue-loans_YYYYMMDDHHMMSS.csv`) bir dosya döndürür.

## 4. DynamicQuery ile Filtreleme

Rapor uç noktalarında gövde alabilen çağrılarda `DynamicQuery` yapısından da yararlanabilirsiniz. Örneğin:

```json
{
  "filter": {
    "logic": "and",
    "filters": [
      { "field": "Durum", "operator": "in", "value": ["Aktif","Gecikmis"] },
      { "field": "SonTeslimTarihi", "operator": "lte", "value": "2025-02-01T00:00:00Z" }
    ]
  },
  "sort": [
    { "field": "SonTeslimTarihi", "dir": "asc" }
  ]
}
```

Bu JSON, `POST /api/Raporlama/odunc/overdue/export` gibi uç noktalara gönderildiğinde filtre/sıralama kriterleri uygulanarak CSV üretilir.

## 5. Denetim (Audit) Kayıtları

- Sistem MediatR pipeline üzerinden her `*Command` isteğini `AuditLogs` tablosuna kaydeder.
- Kaydedilen bilgiler: kullanıcı Id/adı, IP, User-Agent, komut adı, JSON formatında payload ve gerçekleşme zamanı.
- Audit altyapısı başarısız olsa bile ana iş akışı etkilenmez; komut normal seyrinde çalışır.

Audit kayıtları için ileride raporlama/izleme ihtiyaçlarınıza göre ayrı sorgular veya dashboard’lar oluşturabilirsiniz.
