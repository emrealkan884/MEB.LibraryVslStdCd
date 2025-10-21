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
|10 | Ince ayarli yetkilendirme | Kismen | Pipeline hazir, policy attributeler eksik |
|11 | KOHA katalog kayitlarinin aktarimi | Eksik | CSV/JSON import pipeline hazir degil. |
|12 | Menü tabanlı güvenlik | Kismen | API seviyesinde güvenlik var, UI guardlar gelistirilecek. |
|13 | Arayuzde sorgulama destegi | Tamamlandi | Dinamik filtreleme altyapisi hazir. |
|14 | Veri tabani yedekleme | Eksik | Scheduled job veya otomasyon hazirlanacak. |
|15 | Mobil / tablet giris destegi | Kismen | Responsive taslak var, native destek planlanıyor. |
|16 | Kullanici karti üretimi | Eksik | PDF/sablon altyapisi oluşturulacak. |
|17 | Toplu materyal islemleri | Kismen | Komutlar hazir, UI ve dosya import bekleniyor. |
|18 | Barkod / RFID destegi | Eksik | Donanım entegrasyonu yapılmadı. |
|19 | Resim saklama | Tamamlandi | Cloudinary adapter aktif.

### Merkez Kütüphane Ek Gereksinimler
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | AACR2 uyumlu katalog | Kismen | KatalogKaydi varlığında alanlar mevcut, validasyon eksik. |
| 2 | MARC21 desteği | Kismen | Marc json alanı var, editör henüz yok. |
| 3 | RDA uyumluluğu | Eksik | RDA flag schema’da var, kurallar eklenmeli. |
| 4 | Z39.50 entegrasyonu | Eksik | Servis adapter taslak halinda. |
| 5 | ISBN ile otomatik veri çekme | Eksik | API bağlantısı yok. |
| 6 | Dijital içerik yükleme | Kismen | Materyal format detayları var, dosya yükleme pipeline eksik. |
| 7 | Çoklu materyal türü desteği | Tamamlandı | MateryalTuru enum kapsamlı. |
| 8 | Dijital erişim | Kismen | Dosya yolu alanı var, download servisi yok. |
| 9 | Barkod ile hızlı işlem | Eksik | Barcode service yazılacak. |
|10 | RFID desteği | Eksik | Donanım düzeyinde planlanacak. |

### Okul Kütüphaneleri Ek Gereksinimler
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Merkezden katalog devralma | Tamamlandı | Materyal oluştururken merkez katalog seçimi yapılabiliyor. |
| 2 | Hızlı ISBN araması | Kismen | Backend tarafında normalize ISBN desteği var; UI eksik. |
| 3 | Yeni katalog talebi modülü | Tamamlandı | Workflow servisleri aktif. |
| 4 | Öğrenci listelerini toplu yükleme | Eksik | Import komutları yazılmadı. |
| 5 | Ödünç/iade yönetimi | Tamamlandı | Komutlar + kurallar hazır. |
| 6 | Etkinlik modülü | Tamamlandı | Etkinlik entity + CRUD aktif. |
| 7 | Dewey sınıflama | Tamamlandı | DeweySiniflama entity + API mevcut. |
| 8 | ISBN ile künyeye ulaşma | Kismen | Backend normalize kontrol yapıyor; lookup servisi eksik. |
| 9 | Filtrelenebilir kaynak listesi | Tamamlandı | Dynamic filtering endpoints mevcut. |
|10 | TC ile öğrenci kaydı | Eksik | MEBBIS entegrasyonu bekliyor. |
|11 | Alfabetik sıralama | Tamamlandı | Dynamic sort destekleniyor. |

### Ödünç Alma ve İade Gereksinimleri
| # | Gereksinim | Durum | Notlar |
|---|------------|-------|--------|
| 1 | Otomatik e-posta hatırlatmaları | Eksik | IMailService var; cron job eklenecek. |
| 2 | İşlem sırasında bilgilendirme mesajları | Kismen | API response’ları var, UI geliştirmesi lazım. |
| 3 | Barkod/manuel/RFID ile işlem | Eksik | Barkod/RFID servisleri yazılacak. |
| 4 | Toplu ödünç verme | Kismen | Komut tek materyal destekli; batch sürümü planlanıyor. |
| 5 | Kurallara uymayan üyeyi askıya alma | Tamamlandı | Kullanıcı status alanı + iş kuralı var. |
| 6 | Kimlik kartı basma | Eksik | PDF şablon planlanıyor. |
| 7 | İşlem sonunda fiş | Eksik | Printer entegrasyonu yok. |

## 📚 Modül Bazlı Durum

| Modül | Backend | Frontend | Açıklama |
|-------|---------|----------|----------|
| Authentication | ✅ | 🟡 | JWT + Refresh token hazır, MEBBIS entegrasyonu planlanıyor. |
| Kullanıcı Yönetimi | ✅ | 🟡 | CRUD + roller tamam, UI mock durumda. |
| Kataloglama | ✅ | 🟡 | Komutlar, workflow, authority yönetimi hazır; UI prototip. |
| Materyal Yönetimi | ✅ | 🟡 | Materyal, nusha, raf yönetimi aktif; UI listeleri mock. |
| Ödünç İşlemleri | ✅ | 🟡 | Loan/Return/Extend komutları mevcut; UI formu hazırlanacak. |
| Raporlama | ✅ | 🟡 | JSON ve CSV çıktı destekli; PDF/Excel kurgulanıyor. |
| Etkinlikler | ✅ | 🟡 | CRUD + afiş alanı mevcut, UI entegrasyonu yapılacak. |
| Arama & Filtreleme | ✅ | 🟡 | DynamicQuery alt yapısı var, frontend bileşenleri oluşturulacak. |
| Güvenlik | ✅ | 🟡 | Audit logging, policies hazır; UI guardlar eksik. |

✅ Tamamlandı | 🟡 Devam ediyor | ❌ Başlanmadı

## 🔌 Entegrasyon Planı

| Sistem | Beklenen İşlev | Durum |
|--------|----------------|-------|
| MEBBIS | Kullanıcı senkronizasyonu ve kimlik doğrulama | Analiz aşamasında |
| e-Okul | Öğrenci listelerinin toplu aktarımı | Analiz aşamasında |
| KOHA | Mevcut katalog verisinin migrasyonu | Analiz aşamasında |
| Z39.50 | Bibliyografik veri çekme | Analiz aşamasında |
| ISBN API | ISBN ile otomatik künyeleme | Araştırma aşamasında |
| Cloudinary | Etkinlik afişleri / materyal medyası | ✅ Entegre |
| SMTP | E-posta hatırlatmaları | Konfigürasyon bekliyor |

## 🚀 Mevcut Sprint Notları

- Yeni katalog talebi workflow’una `review` adımı eklendi.
- Dewey sınıflama API’si için validasyon kuralları yazıldı.
- Audit log pipeline’ı tüm command’lar için aktif hale getirildi.
- Seed datada merkez ve okul kütüphaneleri için örnek kayıtlar oluşturuldu.
- Raporlama modülü için gecikmiş ödünç raporu yayınlandı.
- Vue arayüzünde merkez/okul layout’ları prototip olarak çizildi.
- i18n store ile TR/EN mesaj setleri hazırlandı (komponent entegrasyonu bekleniyor).
- PrimeVue component altyapısı kuruldu; tema olarak Aura seçildi.
- Login ekranı için MEBBIS/e-Okul giriş seçenekleri placeholder olarak eklendi.

## 🧭 Sonraki Adımlar

1. Authentication servisini MEBBIS/e-Okul entegrasyonu ile genişletmek.
2. Barcode/RFID servis arayüzlerini yazıp ödünç iade süreçlerine entegre etmek.
3. Vue tarafında dynamic filter builder bileşeni geliştirmek.
4. KOHA veri migrasyonu için import scriptlerini hazırlamak.
5. PDF/Excel rapor export yeteneklerini uygulamak.
6. Kullanıcı kartı basımı ve işlem fişi çıktıları için PDF şablonları oluşturmak.
7. Swagger dökümanlarını TR/EN açıklamalarla güncellemek.
8. Raporlama modülüne dashboard ve grafikler eklemek.
9. Mobil uyum için responsive layout’ları üretim senaryolarına göre test etmek.
10. Audit log verilerini raporlamak için admin paneli tasarlamak.

## 🧪 Test Durumu

- Unit testler: Kullanıcı oluşturma, login, refresh token senaryoları kapsanıyor.
- Workflow testleri: Yeni katalog talebi approve/reject senaryoları için yazıldı.
- Eksik testler: Katalog kaydı CRUD, Materyal yönetimi, Ödünç işlemleri.

## 🗂️ Dizin Yapısı (Özet)

```bash
src/
  visualStudioCode.MEBLibrary/
    Application/        # CQRS komutları, kurallar, validatorler
    Domain/             # Varlıklar ve enum’lar
    Infrastructure/     # Adaptörler (Cloudinary, Cache, vs.)
    Persistence/        # DbContext, repository implementasyonları
    WebAPI/             # Controllerlar, DI kayıtları, program giriş noktası
frontend/
  meb-library-ui/       # Vue 3 + TypeScript arayüz
tests/
  VisualStudioCode.MEBLibrary.Application.Tests/  # xUnit testleri
docs/
  README.md             # Detaylı domain ve roadmap notları
```

## 📦 Kurulum

### Backend
```bash
dotnet restore
dotnet run --project src/visualStudioCode.MEBLibrary/WebAPI/WebAPI.csproj
```
Swagger: `https://localhost:5278/swagger`

### Frontend
```bash
cd frontend/meb-library-ui
npm install
npm run dev
```
Uygulama: `http://localhost:5173`

### Giriş Bilgileri
- Merkez yetkilisi: `kutuphane.yonetici@example.com / Library123!`
- Öğrenci: `ogrenci@example.com / Library123!`

## 📄 Lisans

Distributed under the MIT License. See `LICENSE` for more information.

## 📬 İletişim

Projeye dair sorular için repository üzerinden issue açabilirsiniz.
