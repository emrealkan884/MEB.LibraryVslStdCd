# Ödünç İşlemleri Rehberi

Bu rehber, ödünç alma/iade/uzatma süreçlerini ve ilgili API uç noktalarını açıklar. Örnek istekler Swagger, Postman veya herhangi bir HTTP istemcisiyle tekrar edilebilir.

---

## 1. Politika ve Kurallar

| Kural | Varsayılan Değer | Açıklama |
| --- | --- | --- |
| Maksimum aktif ödünç | 3 | Kullanıcı başına aynı anda alınabilecek ödünç sayısı. |
| Standart ödünç süresi | 14 gün | Komutta süre belirtilmezse kullanılır. |
| Maksimum uzatma sayısı | 2 | Her uzatma en fazla 7 gün ekler. |
| Gecikme cezası | 1 ₺ / gün | İade tarihinin geç kalması durumunda uygulanır. |

> Bu değerler ileride kütüphane bazlı konfigürasyona taşınabilir.

---

## 2. Ödünç Alma

**Endpoint**

```
POST /api/OduncIslemleri
Authorization: Bearer <JWT>
Content-Type: application/json
```

**Örnek Gövde**

```json
{
  "kutuphaneId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "kullaniciId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
  "nushaId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
  "oduncSuresiGun": 14,
  "not": "STEM projesi için ödünç alındı"
}
```

**Ön Koşullar**

- Nusha `Rafta` veya `Ayirtildi` durumda olmalı.
- Kullanıcının aktif ödünç limiti aşılmamış olmalı.
- Kullanıcı gecikmiş ödünçe sahip olmamalı.

---

## 3. İade İşlemi

**Endpoint**

```
PUT /api/OduncIslemleri/{id}/iade
Authorization: Bearer <JWT>
Content-Type: application/json
```

**Örnek Gövde**

```json
{
  "iadeNotu": "Zamanında iade edildi."
}
```

**Davranış**

- `OdunciIadeEt()` çağrılır; iade tarihi ve durum güncellenir.
- Gecikme varsa gün sayısı ve ceza otomatik hesaplanır.
- `Not` alanına iade notu eklenir.
- Uygulamada nusha durumu `Rafta` olacak şekilde güncellenmelidir (geliştirme yapılması önerilir).

---

## 4. Süre Uzatma

**Endpoint**

```
PUT /api/OduncIslemleri/{id}/extend
Authorization: Bearer <JWT>
Content-Type: application/json
```

**Örnek Gövde**

```json
{
  "ekGun": 7,
  "uzatmaNedeni": "Proje teslim süresi uzadı"
}
```

**Kurallar**

- Durum `Aktif` olmalı.
- `UzatmaSayisi` 2’den az olmalı.
- Ek gün 1-7 aralığında olmalı.
- Son teslim tarihi geçmişse uzatılamaz.

---

## 5. Raporlama ve İzleme

| Uç Nokta | Açıklama |
| --- | --- |
| `GET /api/Raporlama/odunc/gecikmis` | Gecikmiş ödünç işlemlerini listeler. |
| `GET /api/Raporlama/odunc/kullanim` | Belirli aralıklarda kullanım istatistikleri. |
| `GET /api/Raporlama/odunc/toplamlar` | Kitap, yazar veya kütüphane bazında agregasyon. |
| `/export` varyantları | Sonuçları CSV / Excel / PDF formatında dışa aktarır. |

Dinamik filtreler `DynamicQuery` şemasını takip eder:

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

---

## 6. Tipik Senaryolar

### 6.1 Ödünç Alma Akışı

```csharp
var command = new CreateOduncIslemiCommand
{
    KutuphaneId = kutuphaneId,
    KullaniciId = kullaniciId,
    NushaId = nushaId,
    OduncSuresiGun = 14,
    Not = "STEM projesi"
};

var result = await Mediator.Send(command);
```

### 6.2 İade Akışı

```csharp
var command = new ReturnOduncIslemiCommand
{
    Id = oduncIslemiId,
    IadeNotu = "Zamanında iade edildi"
};

var result = await Mediator.Send(command);
```

### 6.3 Süre Uzatma Akışı

```csharp
var command = new ExtendOduncIslemiCommand
{
    Id = oduncIslemiId,
    EkGun = 7,
    UzatmaNedeni = "Proje teslim süresi"
};

var result = await Mediator.Send(command);
```

---

## 7. Hata Kodları

| HTTP Kodu | Hata | Açıklama |
| --- | --- | --- |
| 400 | `NushaAlreadyBorrowed` | Nusha başka bir ödünçte aktif. |
| 400 | `UserHasExceededLoanLimit` | Kullanıcı maksimum ödünç limitini aştı. |
| 400 | `UserHasOverdueLoans` | Kullanıcının gecikmiş ödünçü var. |
| 400 | `LoanExtensionLimitExceeded` | Maksimum uzatma sayısına ulaşıldı. |
| 400 | `InvalidLoanPeriod` | Geçersiz ödünç süresi girildi. |
| 404 | `OduncIslemiNotExists` | Ödünç işlemi bulunamadı. |

---

## 8. Geliştirme Önerileri

1. Nusha durumunu ödünç/iadelerde otomatik güncellemek.
2. Rezervasyon önceliği ve sıra yönetimi eklemek.
3. Gecikme işleme (`ProcessOverdueLoans`) metodunu gerçek repository sorguları ile tamamlamak.
4. Ödünç limitlerini kütüphane bazlı konfigürasyona taşımak.
5. Bildirim sistemi (e-posta/SMS) ile gecikme hatırlatmaları göndermek.

---

Bu doküman, ödünç yönetimi modülünü genişletirken referans almanız için hazırlanmıştır. İhtiyaç duyduğunuz ek senaryoları issue veya PR üzerinden paylaşabilirsiniz.
