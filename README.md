<p align="center">
  <a href="https://github.com/kodlamaio-projects/nArchitecture/graphs/contributors"><img src="https://img.shields.io/github/contributors/kodlamaio-projects/nArchitecture.svg?style=for-the-badge"></a>
  <a href="https://github.com/kodlamaio-projects/nArchitecture/network/members"><img src="https://img.shields.io/github/forks/kodlamaio-projects/nArchitecture.svg?style=for-the-badge"></a>
  <a href="https://github.com/kodlamaio-projects/nArchitecture/stargazers"><img src="https://img.shields.io/github/stars/kodlamaio-projects/nArchitecture.svg?style=for-the-badge"></a>
  <a href="https://github.com/kodlamaio-projects/nArchitecture/issues"><img src="https://img.shields.io/github/issues/kodlamaio-projects/nArchitecture.svg?style=for-the-badge"></a>
  <a href="https://github.com/kodlamaio-projects/nArchitecture/blob/master/LICENSE"><img src="https://img.shields.io/github/license/kodlamaio-projects/nArchitecture.svg?style=for-the-badge"></a>
</p><br />

<p align="center">
  <a href="https://github.com/kodlamaio-projects/nArchitecture"><img src="https://user-images.githubusercontent.com/53148314/194872467-827dc967-acee-4bca-88a2-59ed5695bebf.png" height="125"></a>
  <h3 align="center">nArchitecture Project
</h3>
  <p align="center">
    <!-- PROJECT_DESCRIPTION -->
    <!-- <br />
    <a href="https://github.com/kodlamaio-projects/nArchitecture"><strong>Explore the docs »</strong></a>
    <br /> -->
    <!-- <br />
    <a href="https://github.com/kodlamaio-projects/nArchitecture">View Demo</a>
    · -->
    <a href="https://github.com/kodlamaio-projects/nArchitecture/issues">Report Bug</a>
    ·
    <a href="https://github.com/kodlamaio-projects/nArchitecture/discussions">Request Feature</a>
  </p>
</p>

## 💻 About The Project

# MEB Kütüphane Yönetim Sistemi

**MEB Kütüphane Yönetim Sistemi**, Milli Eğitim Bakanlığı için geliştirilmiş kapsamlı bir kütüphane yönetim çözümüdür. Clean Architecture prensipleriyle geliştirilmiş bu sistem, okul kütüphanelerinin ve merkez kütüphanelerin tüm ihtiyaçlarını karşılayacak şekilde tasarlanmıştır.

### 🎯 Temel Özellikler

- **📚 Katalog Yönetimi** - Kitap, dergi, multimedia materyallerin detaylı kataloglanması
- **👥 Kullanıcı Yönetimi** - Öğrenci, öğretmen ve yönetici rolleri
- **📖 Ödünç İşlemleri** - Kitap ödünç alma/iade süreçleri
- **🏛️ Çoklu Kütüphane Desteği** - Merkez ve okul kütüphaneleri
- **🔍 Gelişmiş Arama** - Dinamik filtreleme ve akıllı öneri sistemi
- **📊 Raporlama** - Ödünç istatistikleri ve kullanım raporları
- **🔐 Güvenlik** - JWT authentication ve rol tabanlı yetkilendirme
- **⚡ Cache Sistemi** - Yüksek performans için gelişmiş önbellekleme
- **📋 İş Akışları** - Yeni katalog talepleri ve onay süreçleri
- **🏷️ Otorite Kontrolü** - Bibliyografik standartlara uygun otorite yönetimi

### 🏗️ Teknik Altyapı

**Backend:**
- **.NET 8** - Modern ve performanslı backend
- **Clean Architecture** - Katmanlı mimari prensipleri
- **CQRS Pattern** - Komut ve sorgu sorumluluğu ayrımı
- **Entity Framework Core** - ORM ve veritabanı işlemleri
- **MediatR** - In-memory messaging bus
- **JWT Authentication** - Güvenli kimlik doğrulama
- **Distributed Cache** - Redis ve memory cache desteği
- **Serilog** - Yapılandırılmış loglama
- **AutoMapper** - Nesne mapping işlemleri
- **FluentValidation** - Model validasyonları

**Frontend:**
- **Vue.js 3** - Modern ve reactive frontend
- **TypeScript** - Tip güvenli geliştirme
- **Pinia** - State management
- **Vitest** - Unit testing
- **Responsive Design** - Mobil uyumlu arayüz

**DevOps & Tools:**
- **Docker** - Containerization
- **Swagger** - API documentation
- **xUnit** - Unit testing framework
- **GitHub Actions** - CI/CD pipelines

## Gereksinim Durum Ozeti

### Genel Gereksinimler
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | MEBBIS / e-Okul ile kimlik dogrulama | Eksik | Mevcut kimlik dogrulama yalnizca JWT + yerel kullanici deposu uzerinden calisiyor. |
| 2 | Bakanligin diger otomasyonlariyla entegrasyon | Eksik | e-Okul vb. sistemler icin adapter bulunmuyor. |
| 3 | Rol bazli erisim (kutuphaneci, yonetici, ogrenci, ogretmen) | Kismen | Roller tanimli; endpoint seviyesinde politika uygulanmiyor. |
| 4 | Kullanim takibi ve raporlama | Kismen | Audit pipeline olaylari kaydediyor, ancak analitik paneller yok. |
| 5 | Merkez icin tam, okullar icin ozellestirilmis moduller | Kismen | Moduller hazir; kutuphane tipine gore erisim kisiti tanimli degil. |
| 6 | Oracle / MSSQL / MySQL destekleme | Eksik | EF Core yalnizca SQL Server icin ayarlanmis. |
| 7 | Web tabanli ve gercek zamanli calisma | Tamamlandi | ASP.NET Core WebAPI + Vue SPA calisiyor. |
| 8 | Unicode (AL32UTF8) desteği | Kismen | .NET unicode destekli; tohum verilerinde bozulan karakterler var. |
| 9 | Turkce ve Ingilizce arayuz | Kismen | Lokalizasyon dosyalari mevcut, fakat uygulama genelinde tamamlanmadi. |
|10 | Ince ayarli yetkilendirme | Kismen | Pipeline hazir, policy attributelari kullanilmiyor. |
|11 | KOHA kataloglarini toplu aktarim | Eksik | Import modul veya script yok. |
|12 | Bilgi guvenligi ve menu tabanli is akisi | Kismen | JWT + exception middleware var; dogrudan veri tabani kalkanlari eksik. |
|13 | Gelismis sorgulama arabirimi | Kismen | `GetListByDynamic` var, fakat kullanici arayuzu sinirli. |
|14 | Otomatik yedekleme | Eksik | Yedekleme gorevi/belgesi hazirlanmadi. |
|15 | Mobil / tablet erisimi | Kismen | Vue arayuz responsive; senaryo testleri yapilmadi. |
|16 | Kullanici karti uretimi ve yazdirma | Eksik | Kart sablonu veya servis yok. |
|17 | Materyalleri toplu ekleme/silme/guncelleme | Kismen | Repository toplu islemi destekler, dedike API/arayuz eksik. |
|18 | Etiket / barkod olusturma ve yazdirma | Eksik | Back-end ve front-end tarafinda barkod ozelligi yok. |
|19 | Resim saklama | Kismen | ImageService altyapisi var; katalog kayitlariyla baglanti eksik. |

### Merkez Kutuphane Ek Gereksinimler
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | AACR2 bibliyografik künye | Eksik | AACR2 formatina gore dogrulama bulunmuyor. |
| 2 | MARC21 tabanli kataloglama | Eksik | MARC alanlari tutuluyor fakat arayuz/servis yok. |
| 3 | RDA uyumlu altyapi | Eksik | RDA spesifik veri modeli veya kontrolu bulunmuyor. |
| 4 | Z39.50 veri degisimi | Eksik | Z39.50 istemcisi uygulanmadi. |
| 5 | ISBN ile dis kataloglardan veri cekme | Eksik | ISBN zenginlestirme servisi yok. |
| 6 | Word/PDF vb. dosya yukleme | Kismen | Dosya baglama isi planlanmis; mevcutta isleyen servis yok. |
| 7 | Multimedya materyal desteği | Kismen | Domain alanlari var; UI/hizmetler kitap odakli. |
| 8 | Dijital iceriklere dogrudan erisim | Eksik | Depolama veya streaming ucu yok. |
| 9 | Barkod ile hizli dolaşim | Eksik | Manuel GUID girisi gerekiyor, barkod akisi yok. |
|10 | RFID destegi | Eksik | RFID ile ilgili alan veya servis yok. |

### Okul Kutuphaneleri Ek Gereksinimler
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Merkezde kataloglanan kayitlari ekleyebilme | Kismen | Iliskiler mevcut; okula kopyalama akisi hazir degil. |
| 2 | ISBN/yazar/isim ile hizli ekleme | Eksik | Otomatik doldurma arayuzu yok. |
| 3 | Yeni Katalog Talebi modülü | Tamamlandi | Workflow onay/red/inceleme adimlari calisiyor. |
| 4 | Ogrencileri toplu yukle/guncelle/sil | Eksik | CSV/Excel aktarimi yapilmiyor. |
| 5 | Odunc alma/iade yonetimi | Kismen | Komutlar var; okul odakli UI eksik. |
| 6 | Etkinlik modulu (takvim, gorsel, rapor) | Kismen | Varliklar var; CRUD/rapor servisleri eksik. |
| 7 | Dewey Onlu Siniflama | Kismen | Enum mevcut, uygulamada zorunlu degil. |
| 8 | ISBN ile bilgiye erisim | Eksik | ISBN bilgisi icin dis servis yok. |
| 9 | ISBN/yazar/kategori filtreleme | Kismen | Dinamik sorgu altyapisi var; UI sinirli. |
|10 | TC kimlik ile toplu ogrenci kaydi | Eksik | Islev icin servis yok. |
|11 | Alfabetik siralama | Kismen | Listeleme altyapisi var; acik secenek sunulmuyor. |

### Odunc Alma ve Iade
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Son tarih icin e-posta hatirlatmasi | Eksik | Zamanlayici veya e-posta sablonu yok. |
| 2 | Odunc esnasinda bilgilendirme mesajlari | Kismen | BusinessRules hata uretir; arayuz uyarisi yok. |
| 3 | Barkod/RFID/manuel islem | Eksik | Sadece manuel veri girisi yapiliyor. |
| 4 | Coklu barkod ile tek seferde odunc | Eksik | Toplu odunc endpointi bulunmuyor. |
| 5 | Kurala uymayanlari askiya alma | Eksik | Kullanici durumu otomatik degistirilmiyor. |
| 6 | Kutuphane kimlik karti basimi | Eksik | Ozellik uygulanmadi. |
| 7 | Islemler sonunda fis yazdirma | Eksik | Servis veya UI bulunmuyor. |

### Kataloglama Modulu
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Merkez odakli kataloglama | Kismen | CQRS katmani var; okul sinirlari net degil. |
| 2 | MARC alanlarini kopyala/ekle/duzelt | Eksik | MARC editoru yok. |
| 3 | Bosluk/buyuk-kucuk/aksan duyarsiz arama | Kismen | Collation iyilestirmesi yapildi; UI uyarlanmadı. |
| 4 | Tur/alt tur/hizli secim | Kismen | Domain alanlari hazir; arayuzde yok. |
| 5 | Otomatik MARC dogrulamasi | Eksik | DOGRULAMA servisi bulunmuyor. |
| 6 | UTF-8 cok dilli veri girisi | Kismen | Platform uygun; bozuk tohum verileri duzeltilmedi. |
| 7 | Dewey kullanimi | Kismen | Enum mevcut; is kurali zorunlu degil. |
| 8 | Karakter siniri olmamasi | Tamamlandi | Girdi alanlari sinirsiz string. |
| 9 | Z39.50 ile kayit al/ver | Eksik | Protokol istemcisi yok. |
|10 | Fiziksel konum (depo/raf) takibi | Kismen | Alanlar var; UI/ak��� eksik. |
|11 | Cok alanli arama ve Word/Excel cikti | Eksik | Yalnizca temel liste uclari var. |
|12 | Materiyal bazinda odunc suresi | Kismen | Alanlar mevcut; cevap modellerine yansimiyor. |
|13 | Kaydi olusturan/guncelleyen ve istatistik | Kismen | Audit log var; raporlama bulunmuyor. |
|14 | Silme kaydi ve geri yukleme | Eksik | Soft delete/geri alma mekanizmasi yok. |

### Raporlama
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Yetkisiz erisim engeli | Kismen | Kimlik dogrulama var; ayrintili roller yok. |
| 2 | Yas, cinsiyet, il vb. cok boyutlu raporlar | Eksik | Sadece geciken odunc sorgusu mevcut. |
| 3 | PDF/Excel disari aktarma | Eksik | `IReportExportService` gerceklenmemis. |
| 4 | Gunluk gecikme kontrol raporu | Kismen | Sorgu var; zamanlama ve cikti eksik. |

### OPAC (Merkez Kullanicisi Arayuzu)
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Kayıt no, tur, yil, kopya, dil ile arama | Kismen | Arka plan destekli, UI tamamlanmadi. |
| 2 | Her kutuphaneyi ayri sorgulayabilme | Kismen | Filtre alanlari var; akış tamamlanmadı. |
| 3 | Tum kutuphaneleri bir arada sorgulama | Kismen | Global sorgu teknik olarak mumkun, UI yok. |
| 4 | Materyalin yeri, kopya sayisi, durumu | Kismen | Veriler var; ekranlar eksik. |
| 5 | PDF/dijital kaynaklara erisim | Eksik | Dijital varlik servisi yok. |
| 6 | Kullanilabilirlik bilgisini gostermek | Kismen | Data mevcut; UI'da gosterim yok. |

### Sistem Modulleri
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Hiyerarsik rol yapisina sahip kullanici modulu | Kismen | Roller var; hiyerarsi islemez halde. |
| 2 | Kataloglama modulu (merkez) | Kismen | CQRS yapisi hazir; gelismis araclar yok. |
| 3 | Materyal yonetimi (odunc/iade takibi) | Kismen | Temel akişlar var; barkod/RFID yok. |
| 4 | Arama ve filtreleme modulu | Kismen | Back-end saglam, UI gelistirilmeli. |
| 5 | Yetki seviyesine gore raporlama | Eksik | Bakanlik/il/ilce bazli raporlar yok. |
| 6 | Etkinlik modulu (takvim, medya, rapor) | Kismen | Varliklar mevcut; kullanici akişi eksik. |
| 7 | Guvenlik modulu (SSL, 2FA, yedekleme) | Kismen | HTTPS destekli; 2FA ve otomatik yedekleme yok. |

### 🛠️ Built With

**Backend:**
[![](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/tr-tr/dotnet/welcome)
[![](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/tr-tr/dotnet/csharp/)
[![](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=entity-framework&logoColor=white)](https://docs.microsoft.com/tr-tr/ef/)

**Architecture & Patterns:**
[![](https://img.shields.io/badge/Clean%20Architecture-000?style=for-the-badge&logo=architectural-patterns&logoColor=white)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![](https://img.shields.io/badge/CQRS-000?style=for-the-badge&logo=architectural-patterns&logoColor=white)](https://martinfowler.com/bliki/CQRS.html)
[![](https://img.shields.io/badge/MediatR-512BD4?style=for-the-badge&logo=mediatr&logoColor=white)](https://github.com/jbogard/MediatR)

**Security & Auth:**
[![](https://img.shields.io/badge/JWT-000?style=for-the-badge&logo=json-web-tokens&logoColor=white)](https://jwt.io/)
[![](https://img.shields.io/badge/ASP.NET%20Core%20Identity-512BD4?style=for-the-badge&logo=asp.net&logoColor=white)](https://docs.microsoft.com/tr-tr/aspnet/core/security/)

**Development Tools:**
[![](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)](https://swagger.io/)
[![](https://img.shields.io/badge/xUnit-512BD4?style=for-the-badge&logo=xunit&logoColor=white)](https://xunit.net/)
[![](https://img.shields.io/badge/Serilog-512BD4?style=for-the-badge&logo=serilog&logoColor=white)](https://serilog.net/)

### 📊 System Features

| Module | Status | Description |
|--------|--------|-------------|
| **📚 Katalog Yönetimi** | ✅ Aktif | Kitap ve materyal kataloglama |
| **👥 Kullanıcı Yönetimi** | ✅ Aktif | Öğrenci ve yönetici rolleri |
| **📖 Ödünç İşlemleri** | ✅ Aktif | Kitap ödünç alma/iade |
| **🏛️ Kütüphane Yönetimi** | ✅ Aktif | Çoklu kütüphane desteği |
| **🔍 Arama ve Filtreleme** | ✅ Aktif | Dinamik sorgu desteği |
| **📊 Raporlama** | ✅ Aktif | İstatistik ve raporlar |
| **🔐 Güvenlik** | ✅ Aktif | JWT ve rol tabanlı yetkilendirme |
| **⚡ Cache Sistemi** | ✅ Aktif | Yüksek performans önbellekleme |
| **🏷️ Otorite Kontrolü** | ✅ Aktif | Bibliyografik standartlar |
| **📋 İş Akışları** | ✅ Aktif | Onay ve talep süreçleri |
| **🌐 Localization** | 🟡 Geliştiriliyor | Çoklu dil desteği |
| **📱 Mobile API** | ⏳ Planlanıyor | Mobil uygulama desteği |

## ⚙️ Getting Started

MEB Kütüphane Yönetim Sistemi'ni çalıştırmak için aşağıdaki adımları takip edin.

### 📋 Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** veya **VS Code** - Geliştirme ortamı
- **SQL Server** veya **PostgreSQL** - Veritabanı (opsiyonel, in-memory database de kullanılabilir)

### 🚀 Installation

1. **Projeyi klonlayın:**
    ```sh
    git clone <repository-url>
    cd VisualStudioCode.MEBLibrary
    ```

2. **Bağımlılıkları yükleyin:**
    ```sh
    dotnet restore
    ```

3. **Veritabanını oluşturun:**
    ```sh
    cd src/visualStudioCode.MEBLibrary/WebAPI
    dotnet ef database update
    ```

4. **Sistemi çalıştırın:**
    ```sh
    dotnet run --project src/visualStudioCode.MEBLibrary/WebAPI
    ```

5. **Swagger UI'ye erişin:**
    ```
    http://localhost:5278/swagger
    ```

### 🔧 Configuration

`appsettings.json` dosyasında gerekli konfigürasyonları yapın:

```json
{
  "ConnectionStrings": {
    "BaseDb": "Server=localhost;Database=MEBLibraryDb;Trusted_Connection=True;"
  },
  "TokenOptions": {
    "SecurityKey": "YourStrongSecretKeyHere..."
  }
}
```

## 🚀 Usage

### **🏃‍♂️ API'yi Çalıştırın:**
```sh
cd src/visualStudioCode.MEBLibrary/WebAPI
dotnet run
```

API şu adreste erişilebilir: `http://localhost:5278`

### **📖 Swagger Documentation:**
API documentation ve test arayüzü için: `http://localhost:5278/swagger`

### **🧪 Test Kullanıcıları:**
```json
// Admin Girişi
POST /api/Auth/Login
{
  "email": "kutuphane.yonetici@example.com",
  "password": "Library123!"
}

// Öğrenci Girişi
POST /api/Auth/Login
{
  "email": "ogrenci@example.com",
  "password": "Library123!"
}
```

### **🔍 Örnek API Çağrıları:**

#### **Kütüphaneleri Listele:**
```sh
GET /api/Kutuphaneler
Authorization: Bearer YOUR_TOKEN
```

#### **Ödünç İşlemlerini Filtrele:**
```sh
POST /api/OduncIslemleri/GetListByDynamic
{
  "filter": {
    "field": "Durumu",
    "operator": "eq",
    "value": "Aktif"
  },
  "sort": [
    { "field": "AlinmaTarihi", "dir": "desc" }
  ]
}
```

### **🛠️ Development Tools:**

#### **Code Analysis:**
```sh
dotnet tool restore
dotnet roslynator analyze
```

#### **Code Formatting:**
```sh
dotnet tool restore
dotnet csharpier .
```

## 📚 Documentation

### **📖 Kullanıcı Kılavuzları:**

- **[📖 Ödünç İşlemleri Sistemi](./docs/LoanManagementGuide.md)** - Kitap ödünç alma/iade süreçleri
- **[📋 Domain Model Guide](./docs/DomainModelGuide.md)** - Sistem domain modeli açıklamaları
- **[📊 Raporlama Rehberi](./docs/ReportingGuide.md)** - Kullanım istatistikleri ve raporlar
- **[🏷️ Otorite Kontrolü](./docs/README.md)** - Bibliyografik otorite yönetimi

### **🔧 API Documentation:**

- **[🚀 Swagger UI](http://localhost:5278/swagger)** - İnteraktif API dokümantasyonu
- **[📋 Postman Collection](./docs/MEBLibraryAPI.postman_collection.json)** - API test collection'ı

### **🏗️ Architecture Documentation:**

- **[Clean Architecture](./docs/CleanArchitecture.md)** - Mimari prensipler
- **[CQRS Pattern](./docs/CQRSPattern.md)** - Komut ve sorgu ayrımı
- **[Caching Strategy](./docs/CachingStrategy.md)** - Önbellekleme stratejisi

## 🚧 Roadmap

### **✅ Tamamlanan Özellikler:**
- [x] **Clean Architecture** - Katmanlı mimari
- [x] **CQRS Pattern** - Komut ve sorgu ayrımı
- [x] **Authentication & Authorization** - JWT ve rol tabanlı güvenlik
- [x] **Cache Sistemi** - Distributed memory cache
- [x] **Dinamik Filtreleme** - GetListByDynamic endpoint'leri
- [x] **İş Akışları** - Yeni katalog talepleri ve onay süreçleri
- [x] **Otorite Kontrolü** - Bibliyografik standartlar
- [x] **Raporlama** - Ödünç istatistikleri
- [x] **Audit Logging** - Tüm işlemlerin loglanması

### **🚀 Planlanan Geliştirmeler:**
- [ ] **Vue.js Frontend** - Modern kullanıcı arayüzü
- [ ] **Mobile API** - Mobil uygulama desteği
- [ ] **Barcode/RFID** - Barkod sistemi entegrasyonu
- [ ] **Email Notifications** - Ödünç/iade hatırlatmaları
- [ ] **Advanced Reporting** - PDF/Excel rapor export
- [ ] **Multi-language** - Çoklu dil desteği
- [ ] **External API Integration** - KOHA, Z39.50 entegrasyonu
- [ ] **AI Recommendations** - Kitap öneri sistemi

### **🔧 Teknik İyileştirmeler:**
- [ ] **Redis Cache** - Distributed cache
- [ ] **Elasticsearch** - Gelişmiş arama
- [ ] **Docker** - Containerization
- [ ] **CI/CD Pipeline** - Otomatik deployment
- [ ] **Performance Monitoring** - Uygulama gözlemleme

## 🤝 Contributing

MEB Kütüphane Yönetim Sistemi'ne katkıda bulunmak için aşağıdaki adımları takip edin:

### 🚀 Nasıl Katkıda Bulunabilirsiniz?

1. **Projeyi Fork Edin:**
   ```sh
   git clone <your-fork-url>
   cd VisualStudioCode.MEBLibrary
   ```

2. **Feature Branch Oluşturun:**
   ```sh
   git checkout -b feature/amazing-feature
   ```

3. **Geliştirmelerinizi Yapın:**
   - Yeni özellikler ekleyin
   - Mevcut kodları iyileştirin
   - Testler yazın
   - Dokümantasyon güncelleyin

4. **Değişikliklerinizi Commit Edin:**
   ```sh
   git add .
   git commit -m 'feat: yeni ödünç verme özelliği eklendi'
   ```

5. **Push Yapın:**
   ```sh
   git push origin feature/amazing-feature
   ```

6. **Pull Request Açın**

### 📝 Commit Message Formatı:

```
type(scope): description

[optional body]

[optional footer]
```

**Types:**
- `feat:` - Yeni özellik
- `fix:` - Bug fix
- `docs:` - Dokümantasyon
- `style:` - Kod formatı
- `refactor:` - Kod iyileştirmesi
- `test:` - Test ekleme/düzenleme
- `chore:` - Genel bakım

**Examples:**
- `feat: kitap iade endpoint'i eklendi`
- `fix: ödünç limiti kontrolü düzeltildi`
- `docs: README güncellendi`

Contributing on Core Packages With This Repo:

1. Fork the [nArchitecture.Core](https://github.com/kodlamaio-projects/nArchitecture.Core) project
2. Locate to `src/corePackages` path (`cd .\src\corePackages\`)
3. Add your forked nArchitecture.Core repository remote address (`git remote add <YourUserName> https://github.com/<YourUserName>/nArchitecture.Core.git`)
4. Create your Feature Branch (`git checkout -b <Feature>/<AmazingFeature>'`)
5. Develop
6. Commit your changes (`git add . && git commit -m '<SemanticCommitType>(<Scope>): <AmazingFeature>'`)
   💡 Check [Semantic Commit Messages](./docs/Semantic%20Commit%20Messages.md)
7. Push to the branch (`git push <YourUserName> --set-upstream HEAD:refs/heads/<Feature>/<AmazingFeature>`)
8. Open a Pull Request

If your pull request is accepted and merged:

9. Locate to `src/corePackages` path (`cd .\src\corePackages\`)
10. Switch to main branch `git checkout main`
11. Locate root path `/` path (`cd ..\..\`)
12. Pull repo and submodule `git submodule update --remote`
13. Commit your changes (`git add . && git commit -m 'build(corePackages): update submodule'`)
14. Push to the Branch (`git push origin <Feature>/<AmazingFeature>`)
15. Open a Pull Request

## ⚖️ License

Distributed under the MIT License. See `LICENSE` for more information.

## 📧 Contact

**Project Link:** [https://github.com/kodlamaio-projects/nArchitecture](https://github.com/kodlamaio-projects/nArchitecture)

<!-- ## 🙏 Acknowledgements
- []() -->
