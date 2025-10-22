# Raporlama ve Denetim Rehberi

Bu rehber, WebAPI üzerindeki raporlama uç noktalarını, desteklenen parametreleri ve dışa aktarma seçeneklerini açıklar.

---

## 1. Özet Raporları

- **Katalog özeti:** `GET /api/Raporlama/katalog/ozet`  
  Toplam katalog kaydı, materyal ve nusha sayıları ile nusha durum dağılımlarını döndürür.
- **Kullanıcı özeti:** `GET /api/Raporlama/kullanici/ozet`  
  Aktif/pasif kullanıcı sayılarını ve `Role.*` claim'lerine göre kullanıcı dağılımını getirir.
- **Rezervasyon özeti:** `GET /api/Raporlama/rezervasyon/ozet`  
  Rezervasyonların durum bazında (Beklemede, Hazır, Tamamlandı, İptal, Süre Doldu) sayısal dağılımını sağlar.

Örnek çıktı:

```json
{
  "metrics": [
    { "key": "totalCatalogRecords", "label": "Toplam Katalog Kaydı", "value": 2 },
    { "key": "availableCopies", "label": "Raftaki Nüsha", "value": 2 }
  ]
}
```

Özet uç noktaları `metrics` isimli opsiyonel sorgu parametresi alır. İhtiyaç duyduğunuz metrikleri virgül ile ayırarak çağırabilirsiniz:

```
GET /api/Raporlama/katalog/ozet?metrics=totalCatalogRecords,availableCopies
```

Mümkün metrik anahtarları ilgili endpoint’lerin XML dokümantasyonunda belirtilmiştir (örn. katalog için `totalCatalogRecords`, `totalMaterials`, `lostCopies` vb.).
Kullanıcı özetinde rol dağılımları ayrıca `Role.*` claim’lerine göre Türkçe etiketlerle (`RoleLabel`) gelir.

> Bu uç noktalar yalnızca GET çağrısı kabul eder ve dinamik sorgu göndermez; yönetim ekranlarında hızlı göstergeler için kullanın.

---

## 2. Geciken Ödünç İşlemleri

- **Endpoint:** `GET /api/Raporlama/odunc/gecikmis`
- **Amaç:** Son teslim tarihi geçmiş ve hâlâ iade edilmemiş ödünçleri listeler.
- **Parametreler:**
  - `KutuphaneId` (Guid, opsiyonel)
  - `KullaniciId` (Guid, opsiyonel)

**Örnek:**

```
GET /api/Raporlama/odunc/gecikmis?KutuphaneId=8c4e8f23-28c9-4a27-8cf7-208407828fa9
Authorization: Bearer <token>
```

Seed verilerinde hazır bir gecikmiş işlem (`stemOverdueOduncId`) bulunduğundan ilk çalıştırmada örnek veri döner.

---

## 3. Ödünç Kullanım İstatistikleri

- **Endpoint:** `GET /api/Raporlama/odunc/kullanim`
- **Amaç:** Belirli tarih aralıklarındaki ödünç alma eğilimleri.
- **Parametreler:**
  - `KutuphaneId` (Guid, opsiyonel)
  - `BaslangicTarihi`, `BitisTarihi` (ISO-8601, opsiyonel)

**Örnek:**

```
GET /api/Raporlama/odunc/kullanim?BaslangicTarihi=2025-01-01T00:00:00Z
Authorization: Bearer <token>
```

---

## 4. Ödünç Agregasyonları

- **Endpoint:** `GET /api/Raporlama/odunc/toplamlar`
- **Amaç:** Ödünç işlemlerini kitap, yazar veya kütüphane bazında toplamak.
- **Parametreler:**
  - `Dimension` (`Book`, `Author`, `Library`) – zorunlu.
  - `Top` – döndürülecek kayıt sayısı (varsayılan 10).
  - `KutuphaneId`, `BaslangicTarihi`, `BitisTarihi` – opsiyonel filtreler.

**Örnek:**

```
GET /api/Raporlama/odunc/toplamlar?Dimension=Book&Top=20
Authorization: Bearer <token>
```

---

## 5. Dışa Aktarma Uç Noktaları

Her rapor için JSON çıktısına ek olarak CSV, Excel veya PDF formatında dosya üretilebilir.

| Rapor | Endpoint | Not |
| --- | --- | --- |
| Geciken ödünçler | `POST /api/Raporlama/odunc/gecikmis/export?format=Csv|Excel|Pdf` | Gövdede filtre parametreleri gönderilebilir. |
| Kullanım istatistikleri | `POST /api/Raporlama/odunc/kullanim/export?format=...` | Tarih/kütüphane filtreleri desteklenir. |
| Agregasyon raporu | `POST /api/Raporlama/odunc/toplamlar/export?format=...` | Gövdede `dimension` ve `top` yer almalı. |

**Örnek (Excel dışa aktarma):**

```http
POST /api/Raporlama/odunc/toplamlar/export?format=Excel
Content-Type: application/json
Authorization: Bearer <token>

{
  "dimension": "Book",
  "top": 15
}
```

**Çıktı Özeti**

| Format | Content-Type | Açıklama |
| --- | --- | --- |
| CSV | `text/csv` | UTF-8, virgülle ayrılmış. |
| Excel | `application/vnd.ms-excel` | HTML tablo, Excel tarafından açılır. |
| PDF | `application/pdf` | Basit tablo içeren PDF. |

Dosya adları `rapor_YYYYMMDDHHmmss.*` şeklindedir.

---

## 6. Dinamik Filtre Yapısı

Tüm export uç noktaları `DynamicQuery` gövdesini destekler:

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

Bu JSON’u export isteğinin gövdesine ekleyerek filtrelenmiş dosya çıktıları alabilirsiniz.

---

## 7. Denetim (Audit) Kayıtları

MediatR pipeline’ındaki `AuditLoggingBehavior`, tüm komut çağrılarını `AuditLogs` tablosuna kaydeder:

- Kullanıcı Id / adı
- IP adresi ve User-Agent
- Komut adı
- Komut gövdesi (JSON)
- Oluşma zamanı (UTC)

Audit log yazımı ana iş akışını bloke etmez; hata durumunda da komut çalışmaya devam eder.

---

## 8. Geliştirme Önerileri

1. Raporlara rol bazlı erişim kısıtları eklemek (örn. sadece merkez yetkilisi görebilsin).
2. Dashboard ve grafiksel görselleştirmeler (ör. aylık ödünç trend grafikleri).
3. Daha detaylı filtreler (yaş, sınıf, cinsiyet, materyal türü bazlı kırılımlar).
4. Audit log verilerini raporlayan yönetim ekranı.

---

Raporlama modülüne katkıda bulunurken bu rehberi referans alın. Yeni rapor ihtiyaçlarını issue veya PR olarak paylaşabilirsiniz.
