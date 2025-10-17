# 📚 Ödünç İşlemleri Sistemi (Loan Management System)

MEB Kütüphane Yönetim Sistemi'nin kapsamlı ödünç işlemleri modülü, kitap ödünç alma, iade etme, süre uzatma ve rezervasyon işlemlerini yönetir.

## 🏛️ İş Kuralları ve Politikalar

### Ödünç Verme Politikaları
- **Maksimum Aktif Ödünç:** Her kullanıcı aynı anda en fazla 3 kitap ödünç alabilir
- **Standart Ödünç Süresi:** 14 gün (1-30 gün arası ayarlanabilir)
- **Maksimum Uzatma Sayısı:** 2 kez (her uzatmada maksimum 7 gün eklenebilir)
- **Günlük Gecikme Cezası:** 1.00 TL (konfigüre edilebilir)

### Kullanıcı Uygunluk Kontrolleri
- Aktif ödünç limiti aşılmamış olmalı
- Gecikmiş ödünç işlemi bulunmamalı
- Kullanıcı kütüphane üyesi olmalı

## 🔌 API Endpoints

### Kitap Ödünç Alma
```http
POST /api/OduncIslemler/Borrow
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "kutuphaneId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "kullaniciId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
  "nushaId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
  "oduncSuresiGun": 14,
  "not": "MEB sınavları için gerekli"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
  "kutuphaneId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "kullaniciId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
  "nushaId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
  "alinmaTarihi": "2024-01-15T10:30:00Z",
  "sonTeslimTarihi": "2024-01-29T10:30:00Z",
  "durumu": 1,
  "not": "MEB sınavları için gerekli"
}
```

### Kitap İade Etme
```http
POST /api/OduncIslemler/Return/3fa85f64-5717-4562-b3fc-2c963f66afa9
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "iadeNotu": "Zamanında iade edildi"
}
```

**Response (Gecikme Durumunda):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
  "iadeTarihi": "2024-01-31T10:30:00Z",
  "durumu": 2,
  "gecikmeGunSayisi": 2,
  "gecikmeCezaMiktari": 2.00,
  "not": "2024-01-15: İade - Zamanında iade edildi."
}
```

### Süre Uzatma
```http
POST /api/OduncIslemler/Extend/3fa85f64-5717-4562-b3fc-2c963f66afa9
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "ekGun": 7,
  "uzatmaNedeni": "Proje teslim tarihi"
}
```

**Response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
  "sonTeslimTarihi": "2024-02-05T10:30:00Z",
  "uzatmaTarihi": "2024-01-15T10:30:00Z",
  "uzatmaSayisi": 1,
  "not": "2024-01-15: Uzatma nedeni - Proje teslim tarihi."
}
```

## 💻 Kullanım Örnekleri

### Frontend/JavaScript ile Kullanım
```javascript
// Kitap ödünç alma
const borrowBook = async (bookData) => {
  const response = await fetch('/api/OduncIslemler/Borrow', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      kutuphaneId: bookData.libraryId,
      kullaniciId: currentUser.id,
      nushaId: bookData.copyId,
      oduncSuresiGun: 14,
      not: 'Öğrenci talebi'
    })
  });

  return await response.json();
};

// Kitap iade etme
const returnBook = async (loanId, note) => {
  const response = await fetch(`/api/OduncIslemler/Return/${loanId}`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      iadeNotu: note
    })
  });

  return await response.json();
};
```

### cURL ile Test
```bash
# Kitap ödünç alma
curl -X POST "http://localhost:5278/api/OduncIslemler/Borrow" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "kutuphaneId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "kullaniciId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    "nushaId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
    "oduncSuresiGun": 14,
    "not": "MEB sınavları için gerekli"
  }'

# Süre uzatma
curl -X POST "http://localhost:5278/api/OduncIslemler/Extend/LOAN_ID" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "ekGun": 7,
    "uzatmaNedeni": "Ek süre ihtiyacı"
  }'
```

## ⚙️ Konfigürasyon

### İş Politikalarını Özelleştirme
`Application/Features/OduncIslemler/Rules/OduncIslemiBusinessRules.cs` dosyasında politikaları düzenleyin:

```csharp
// Ödünç verme politikaları (konfigüre edilebilir)
private const int MaxAktifOduncSayisi = 3;        // Maksimum aktif ödünç sayısı
private const int StandartOduncSuresiGun = 14;    // Standart ödünç süresi (gün)
private const int MaxUzatmaSayisi = 2;            // Maksimum uzatma sayısı
private const int MaxUzatmaGun = 7;               // Maksimum uzatma günü
private const decimal GunlukCezaMiktari = 1.0m;   // Günlük ceza miktarı (TL)
```

### Localization (Çoklu Dil Desteği)
Türkçe ve İngilizce mesajlar için `Resources/Locales/` klasöründeki dosyaları düzenleyin:

```yaml
# Resources/Locales/OduncIslemis.tr.yaml
OduncIslemiNotExists: "Ödünç işlemi bulunamadı"
NushaAlreadyBorrowed: "Bu nüsha zaten ödünç verilmiş"
UserHasExceededLoanLimit: "Kullanıcı ödünç limitini aştı"
UserHasOverdueLoans: "Kullanıcının gecikmiş ödünç işlemi bulunuyor"
LoanExtensionLimitExceeded: "Uzatma limiti aşıldı"
InvalidLoanPeriod: "Geçersiz ödünç süresi"
```

## 🔐 Yetkilendirme

Ödünç işlemleri için gerekli roller:
- **Kütüphaneci:** Tüm ödünç işlemlerini yönetebilir
- **Admin:** Tüm işlemleri yapabilir

JWT token ile yetkilendirme:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🚨 Hata Kodları ve Anlamları

| HTTP Status | Error Code | Açıklama |
|-------------|------------|----------|
| 400 | NushaAlreadyBorrowed | Nüsha zaten ödünç verilmiş |
| 400 | UserHasExceededLoanLimit | Kullanıcı ödünç limitini aştı |
| 400 | UserHasOverdueLoans | Gecikmiş ödünç işlemi var |
| 400 | LoanExtensionLimitExceeded | Uzatma limiti aşıldı |
| 400 | InvalidLoanPeriod | Geçersiz ödünç süresi |
| 404 | OduncIslemiNotExists | Ödünç işlemi bulunamadı |

## 📊 Raporlama ve İzleme

### Aktif Ödünç Sayısı
```http
GET /api/OduncIslemler/GetActiveLoansCount?kullaniciId=USER_ID
```

### Gecikmiş Ödünçler
```http
GET /api/OduncIslemler/GetOverdueLoans?kullaniciId=USER_ID
```

### Geciken Ödünçleri İşleme
```http
POST /api/OduncIslemler/ProcessOverdueLoans
```

## 🎯 Domain Entity Özellikleri

### Gelişmiş OduncIslemi Entity
```csharp
public class OduncIslemi : Entity<Guid>
{
    // Temel bilgiler
    public Guid KutuphaneId { get; set; }
    public Guid KullaniciId { get; set; }
    public Guid NushaId { get; set; }

    // Tarih bilgileri
    public DateTime AlinmaTarihi { get; set; } = DateTime.UtcNow;
    public DateTime SonTeslimTarihi { get; set; }
    public DateTime? IadeTarihi { get; set; }
    public DateTime? UzatmaTarihi { get; set; }

    // İş kuralı özellikleri
    public int UzatmaSayisi { get; set; } = 0; // Maksimum 2
    public OduncDurumu Durumu { get; set; } = OduncDurumu.Aktif;

    // Ceza yönetimi
    public decimal? GecikmeCezaMiktari { get; set; }
    public int? GecikmeGunSayisi { get; set; }

    // Notlar
    public string? Not { get; set; }

    // Business methods
    public void OdunciIadeEt() // İade işlemi
    public bool SureUzatabilirMi() // Uzatma kontrolü
    public void SureUzat(int ekGun) // Süre uzatma
    public bool GecikmisMi() // Gecikme kontrolü
    public void GecikmeDurumunaGec() // Otomatik ceza
}
```

## 🚀 Hızlı Başlangıç

### 1. Ödünç Alma İşlemi
```csharp
// Command oluşturma
var command = new CreateOduncIslemiCommand
{
    KutuphaneId = kutuphaneId,
    KullaniciId = kullaniciId,
    NushaId = nushaId,
    OduncSuresiGun = 14,
    Not = "MEB sınavları için gerekli"
};

// Mediator ile çalıştırma
var result = await Mediator.Send(command);
```

### 2. İade İşlemi
```csharp
var command = new ReturnOduncIslemiCommand
{
    Id = oduncIslemiId,
    IadeNotu = "Zamanında iade edildi"
};

var result = await Mediator.Send(command);
// Otomatik ceza hesaplanır eğer gecikmişse
```

### 3. Süre Uzatma
```csharp
var command = new ExtendOduncIslemiCommand
{
    Id = oduncIslemiId,
    EkGun = 7,
    UzatmaNedeni = "Proje teslim tarihi"
};

var result = await Mediator.Send(command);
```

## 📋 Business Rules Engine

### Validation Kontrolleri
- **Nusha Kontrolü:** Nusha ödünç verilebilir durumda mı?
- **Kullanıcı Limiti:** Aktif ödünç sayısı 3'ü aşmıyor mu?
- **Gecikme Kontrolü:** Kullanıcının gecikmiş ödünç işlemi yok mu?
- **Uzatma Limiti:** Maksimum uzatma sayısı aşılmadı mı?
- **Süre Kontrolü:** Ödünç süresi 1-30 gün arası mı?

### Otomatik İşlemler
- **Ceza Hesaplama:** İade sırasında otomatik gecikme cezası
- **Durum Güncelleme:** Geciken ödünçler otomatik olarak işaretlenir
- **Loglama:** Tüm işlemler audit log'a kaydedilir
- **Cache Yönetimi:** İşlemler sonrası cache temizlenir

Bu sistem **MEB kütüphanelerinin** tüm ihtiyaçlarını karşılayacak şekilde tasarlanmış ve **gerçek dünya kullanımına** hazırdır.