<h1 align="center">MEB Kütüphane Yönetim Sistemi</h1>

Bu depo, Milli Eğitim Bakanlığı için geliştirilen **MEB Kütüphane Yönetim Sistemi**’nin kaynak kodlarını içerir. Çözüm; merkez ve okul kütüphanelerinin kataloglama, otorite yönetimi, ödünç süreçleri, raporlama ve güvenlik gereksinimlerini Clean Architecture prensipleriyle ele alır.

---

## Genel Bakış

- **Backend:** .NET 8, CQRS + MediatR, Entity Framework Core, FluentValidation, AutoMapper, Serilog, JWT tabanlı kimlik doğrulama.
- **Frontend:** Vue 3, TypeScript, Pinia, PrimeVue, TailwindCSS, çok dilli destek.
- **Altyapı:** Dinamik sorgular, CSV/Excel/PDF rapor dışa aktarımı, audit logging, servis tabanlı adaptör yapısı.
- **Testler:** Uygulama katmanı için xUnit senaryoları; domain odaklı ek testler planlanmaktadır.

> Detaylı domain notları, ödünç yönetimi rehberi ve yol haritası için `docs/` klasörüne göz atın.

---

## Mimari Katmanlar

| Katman | Açıklama |
| --- | --- |
| `Domain` | Entity, enum ve davranışların tanımı. |
| `Application` | CQRS komut/sorgular, iş kuralları, validatörler, servis sözleşmeleri. |
| `Persistence` | EF Core DbContext, repository implementasyonları, veri tohumu. |
| `Infrastructure` | Harici servis adaptörleri (rapor dışa aktarım, medya, vb.). |
| `WebAPI` | ASP.NET Core API, middleware, DI kayıtları, Swagger konfigürasyonu. |
| `frontend/meb-library-ui` | Vue tabanlı istemci uygulaması. |
| `tests` | Uygulama katmanı test projeleri. |

---

## Başlıca Özellikler

- **Katalog Yönetimi:** Dewey sınıflaması, MARC21/RDA alanları, materyal format detayları.
- **Yeni Katalog Talebi Workflow’u:** Talep → İnceleme → Onay/Reddet akışı; onay sonrası otomatik katalog kaydı oluşturma.
- **Otorite Kayıtları:** Yazar, konu, kurum otoriteleri ile standartlaştırılmış bibliyografik veri.
- **Ödünç İşlemleri:** Ödünç alma, iade, süre uzatma, gecikme cezası hesaplama ve ilgili raporlar.
- **Raporlama:** Geciken ödünçler, kullanım istatistikleri, agregasyon raporları ve çok formatlı dışa aktarma.
- **Güvenlik:** JWT, rol tabanlı politikalar (`RequireMinistry`, `RequireSchoolOrAbove`, vb.), audit logleyici pipeline.
- **Çok Dilli Destek:** YAML tabanlı kaynak dosyaları ve frontend’de dinamik dil seçimi.

---

## Kurulum

### Gereksinimler

- .NET SDK 8.0+
- Node.js 20+
- NPM veya pnpm
- PostgreSQL 14+ (varsayılan bağlantı: `Host=localhost;Port=5432;Database=MEBLibraryDb;Username=postgres;Password=1234;`)

### Backend

```bash
dotnet restore
dotnet run --project src/visualStudioCode.MEBLibrary/WebAPI/WebAPI.csproj
```

> **Not:** Uygulamayı başlatmadan önce PostgreSQL sunucunuzda `MEBLibraryDb` adlı bir veritabanı oluşturun (veya `appsettings*.json` altında kendi bağlantınızı tanımlayın). Gerekli tablolar istemci ayağa kalkarken `AddDbMigrationApplier` ile otomatik uygulanır.

Swagger arayüzü: `https://localhost:5278/swagger`

### Frontend

```bash
cd frontend/meb-library-ui
npm install
npm run dev
```

Varsayılan adres: `http://localhost:5173`

### Örnek Giriş Bilgileri

| Rol | E-posta | Şifre |
| --- | --- | --- |
| Merkez yetkilisi | `kutuphane.yonetici@example.com` | `Library123!` |
| Öğrenci | `ogrenci@example.com` | `Library123!` |

---

## Yapılandırma

- Backend ayarları `src/visualStudioCode.MEBLibrary/WebAPI/appsettings*.json` dosyalarında tutulur (`TokenOptions`, `WebAPIConfiguration`, `MailSettings`, `ElasticSearchConfig`, vb.).
- Frontend API adresini `VITE_API_BASE_URL` çevresel değişkeni ile güncelleyebilirsiniz.

---

## Komutlar

| Komut | Açıklama |
| --- | --- |
| `dotnet test` | Uygulama katmanı testlerini çalıştırır. |
| `npm run lint` | Frontend lint denetimini çalıştırır. |
| `npm run build` | Vue üretim derlemesini oluşturur. |

---

## Dokümantasyon

| Dosya | İçerik |
| --- | --- |
| `docs/README.md` | Mimari kararlar ve domain panoraması. |
| `docs/DomainModelGuide.md` | Katalog, materyal, otorite ilişkileri. |
| `docs/LoanManagementGuide.md` | Ödünç süreçleri ve örnek API kullanımları. |
| `docs/ReportingGuide.md` | Rapor uç noktaları ve dışa aktarma seçenekleri. |
| `docs/Roadmap.md` | Sprint planı ve bekleyen çalışmalar. |

---

## Katkı ve Lisans

- Commit mesajları için `docs/Semantic Commit Messages.md` yönergelerini izleyin.
- Proje MIT lisansı ile dağıtılmaktadır. Ayrıntılar için `LICENSE` dosyasını inceleyin.
- Hata ve geliştirme taleplerinizi issue açarak paylaşabilirsiniz.

---

İyi çalışmalar! 🚀
