<template>
  <div class="cataloging-page">
    <section class="hero">
      <div class="hero__content">
        <div class="hero__status">Son senkronizasyon 18 sn önce</div>
        <h1 class="hero__title">Kataloglama Stüdyosu</h1>
        <p class="hero__subtitle">
          MARC21, RDA ve yerel standartları tek arayüzde yönetin. Ekip içi iş akışlarını hızlandıran modern kataloglama aracınız.
        </p>
        <ul class="hero__highlights">
          <li v-for="feature in heroHighlights" :key="feature.label" class="hero__highlight">
            <span class="hero__highlight-dot" :style="{ background: feature.color }"></span>
            {{ feature.label }}
          </li>
        </ul>
      </div>
      <button class="hero__cta" type="button">
        <span class="hero__cta-icon">+</span>
        Yeni MARC kaydı başlat
      </button>
    </section>

    <section class="quick-actions">
      <button
        v-for="action in quickActions"
        :key="action.id"
        type="button"
        @click="handleQuickAction(action.id)"
        class="quick-actions__item"
      >
        <div class="quick-actions__header">
          <span class="quick-actions__shortcut" :style="{ background: action.gradient }">{{ action.shortcut }}</span>
          <span class="quick-actions__badge">{{ action.meta }}</span>
        </div>
        <h3 class="quick-actions__title">{{ action.title }}</h3>
        <p class="quick-actions__description">{{ action.description }}</p>
      </button>
    </section>

    <section class="stats">
      <article v-for="metric in metrics" :key="metric.label" class="stats-card">
        <header class="stats-card__header">
          <span class="stats-card__category">{{ metric.category }}</span>
        </header>
        <div class="stats-card__value">{{ metric.value }}</div>
        <div class="stats-card__label">{{ metric.label }}</div>
        <div class="stats-card__trend" :class="'stats-card__trend--' + metric.tone">
          <span class="stats-card__trend-icon">{{ metric.direction === 'down' ? '▼' : metric.direction === 'steady' ? '■' : '▲' }}</span>
          <span>{{ metric.trendLabel }}</span>
          <span class="stats-card__trend-period">{{ metric.period }}</span>
        </div>
      </article>
    </section>

    <section class="main-grid">
      <div class="card form-card">
        <header class="card__header">
          <div>
            <h2 class="card__title">Yeni katalog kaydı</h2>
            <p class="card__subtitle">Alanları doldurdukça MARC21 önizlemesi otomatik güncellenir.</p>
          </div>
          <div class="card__meta">
            <span class="pill pill--neutral">Doğrulama aktif</span>
            <span class="pill pill--primary">%{{ completionProgress }} tamamlandı</span>
          </div>
        </header>

        <nav class="tabs">
          <button
            v-for="tab in tabs"
            :key="tab.id"
            type="button"
            @click="activeTab = tab.id"
            :class="['tabs__item', { 'tabs__item--active': activeTab === tab.id }]"
          >
            <span class="tabs__label">{{ tab.label }}</span>
            <span class="tabs__description">{{ tab.description }}</span>
          </button>
        </nav>

        <form class="form" @submit.prevent>
          <template v-if="activeTab === 'temel'">
            <div class="form__grid form__grid--two">
              <label class="form-field">
                <span class="form-field__label">ISBN</span>
                <input v-model="formModel.isbn" type="text" class="form-field__control" placeholder="9789754707083" />
              </label>
              <label class="form-field">
                <span class="form-field__label">Başlık</span>
                <input v-model="formModel.title" type="text" class="form-field__control" placeholder="Benim Adım Kırmızı" />
              </label>
            </div>
            <div class="form__grid form__grid--three">
              <label class="form-field">
                <span class="form-field__label">Yazar(lar)</span>
                <input v-model="formModel.author" type="text" class="form-field__control" placeholder="Orhan Pamuk" />
              </label>
              <label class="form-field">
                <span class="form-field__label">Yayınevi</span>
                <input v-model="formModel.publisher" type="text" class="form-field__control" placeholder="İletişim Yayınları" />
              </label>
              <label class="form-field">
                <span class="form-field__label">Yayın Yılı</span>
                <input v-model="formModel.publicationYear" type="number" class="form-field__control" placeholder="1998" />
              </label>
            </div>
            <label class="form-field">
              <span class="form-field__label">Özet Notu</span>
              <textarea v-model="formModel.summary" rows="3" class="form-field__control" placeholder="Eserle ilgili kısa içerik bilgisi girin."></textarea>
            </label>
          </template>

          <template v-else-if="activeTab === 'siniflama'">
            <div class="form__grid form__grid--two">
              <label class="form-field">
                <span class="form-field__label">Dewey Sınıfı</span>
                <select v-model="formModel.dewey" class="form-field__control">
                  <option disabled value="">Seçiniz</option>
                  <option v-for="option in deweyOptions" :key="option" :value="option">{{ option }}</option>
                </select>
              </label>
              <label class="form-field">
                <span class="form-field__label">Materyal Türü</span>
                <select v-model="formModel.materialType" class="form-field__control">
                  <option disabled value="">Seçiniz</option>
                  <option v-for="type in materialTypes" :key="type" :value="type">{{ type }}</option>
                </select>
              </label>
            </div>
            <div class="form__grid form__grid--two">
              <label class="form-field">
                <span class="form-field__label">Konu Başlıkları</span>
                <input v-model="formModel.subjects" type="text" class="form-field__control" placeholder="Türk romanı, minyatür sanatı..." />
              </label>
              <label class="form-field">
                <span class="form-field__label">Dil</span>
                <select v-model="formModel.language" class="form-field__control">
                  <option disabled value="">Seçiniz</option>
                  <option v-for="language in languages" :key="language" :value="language">{{ language }}</option>
                </select>
              </label>
            </div>
            <div class="form__grid form__grid--three">
              <label class="form-field">
                <span class="form-field__label">Hedef Kitle</span>
                <select v-model="formModel.audience" class="form-field__control">
                  <option disabled value="">Seçiniz</option>
                  <option v-for="audience in audienceLevels" :key="audience" :value="audience">{{ audience }}</option>
                </select>
              </label>
              <label class="form-field">
                <span class="form-field__label">Koleksiyon</span>
                <input v-model="formModel.collection" type="text" class="form-field__control" placeholder="Edebiyat Koleksiyonu" />
              </label>
              <label class="form-field">
                <span class="form-field__label">Erişim Durumu</span>
                <select v-model="formModel.availability" class="form-field__control">
                  <option disabled value="">Seçiniz</option>
                  <option v-for="availability in availabilityStatuses" :key="availability" :value="availability">{{ availability }}</option>
                </select>
              </label>
            </div>
          </template>

          <template v-else>
            <div class="form__grid form__grid--two">
              <label class="form-field">
                <span class="form-field__label">Dijital Varlık URL</span>
                <input v-model="formModel.digitalUrl" type="url" class="form-field__control" placeholder="https://kutuphane.meb.gov.tr/dijital/12345" />
              </label>
              <label class="form-field">
                <span class="form-field__label">Dosya Biçimi</span>
                <select v-model="formModel.digitalFormat" class="form-field__control">
                  <option disabled value="">Seçiniz</option>
                  <option v-for="format in digitalFormats" :key="format" :value="format">{{ format }}</option>
                </select>
              </label>
            </div>
            <label class="form-field">
              <span class="form-field__label">Lisans / Kullanım Koşulu</span>
              <input v-model="formModel.license" type="text" class="form-field__control" placeholder="CC BY-NC 4.0" />
            </label>
            <div class="dropzone">
              <div class="dropzone__icon">⤓</div>
              <div>
                Dosyaları sürükleyip bırakın ya da <span class="dropzone__link">göz atın</span>
              </div>
            </div>
          </template>

          <div class="form__footer">
            <div class="form__hint">Sistem MARC alanlarını kaydetmeden önce doğrulayacak.</div>
            <div class="form__actions">
              <button type="button" class="btn btn--ghost">Taslak Olarak Kaydet</button>
              <button type="submit" class="btn btn--primary">Yayına Al</button>
            </div>
          </div>
        </form>
      </div>

      <aside class="side-column">
        <div class="card preview-card">
          <header class="card__header">
            <h3 class="card__title">MARC21 önizlemesi</h3>
            <span class="pill pill--neutral">Canlı</span>
          </header>
          <pre class="preview-card__code">{{ marcPreview }}</pre>
          <p class="preview-card__footnote">RDA tanımlamalarına göre alan önerileri güncellendi.</p>
        </div>

        <div class="card workflow-card">
          <h3 class="card__title">İş akışı durumu</h3>
          <ul class="timeline">
            <li v-for="stage in workflowStages" :key="stage.title" class="timeline__item">
              <span class="timeline__icon" :style="{ background: stage.color }">{{ stage.icon }}</span>
              <div>
                <p class="timeline__title">{{ stage.title }}</p>
                <p class="timeline__subtitle">{{ stage.subtitle }}</p>
              </div>
              <span class="timeline__meta">{{ stage.meta }}</span>
            </li>
          </ul>
        </div>

        <div class="card activity-card">
          <header class="card__header">
            <div>
              <h3 class="card__title">Son aktiviteler</h3>
              <p class="card__subtitle">Ekibinizdeki kataloglama hareketleri</p>
            </div>
            <button type="button" class="card__link">Tümünü gör</button>
          </header>
          <ul class="activity-list">
            <li v-for="activity in recentActivity" :key="activity.id" class="activity-list__item">
              <div class="activity-list__header">
                <span class="activity-list__title">{{ activity.title }}</span>
                <span class="activity-list__time">{{ activity.timeAgo }}</span>
              </div>
              <p class="activity-list__description">{{ activity.detail }}</p>
            </li>
          </ul>
        </div>
      </aside>
    </section>

    <section class="card table-card">
      <header class="card__header">
        <div>
          <h3 class="card__title">Katalog koleksiyonunuz</h3>
          <p class="card__subtitle">Son eklenen kayıtlar, durum ve erişim bilgileri</p>
        </div>
        <div class="card__meta">
          <span class="pill pill--success">%{{ publishedPercentage }} yayınlandı</span>
          <span class="pill pill--warning">{{ draftCount }} taslak</span>
        </div>
      </header>
      <div class="table-card__wrapper">
        <table class="catalog-table">
          <thead>
            <tr>
              <th>ISBN</th>
              <th>Başlık</th>
              <th>Yazar</th>
              <th>Yayın Yılı</th>
              <th>Durum</th>
              <th class="catalog-table__actions">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="entry in catalogEntries" :key="entry.isbn">
              <td class="catalog-table__code">{{ entry.isbn }}</td>
              <td>{{ entry.title }}</td>
              <td>{{ entry.author }}</td>
              <td>{{ entry.year }}</td>
              <td>
                <span class="badge" :class="'badge--' + getStatusTone(entry.status)">{{ entry.status }}</span>
              </td>
              <td class="catalog-table__actions">
                <button type="button" class="table-action table-action--primary">Düzenle</button>
                <button type="button" class="table-action table-action--danger">Sil</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'

type TabId = 'temel' | 'siniflama' | 'dijital'

type MetricTone = 'positive' | 'warning' | 'info'
type TrendDirection = 'up' | 'down' | 'steady'

interface CatalogEntry {
  isbn: string
  title: string
  author: string
  year: string
  status: 'Aktif' | 'Taslak' | 'Gözden geçirme'
}

const heroHighlights = [
  { label: 'Otomatik alan doğrulama', color: 'linear-gradient(135deg, #34d399, #10b981)' },
  { label: 'RDA önerileri', color: 'linear-gradient(135deg, #60a5fa, #3b82f6)' },
  { label: 'Toplu içe aktarma', color: 'linear-gradient(135deg, #fbbf24, #f59e0b)' }
] as const

const quickActions = [
  {
    id: 'marc-template',
    title: 'MARC şablonu seç',
    description: 'Edebiyat, süreli yayın veya akademik şablonlardan birini kullanın.',
    shortcut: 'M1',
    meta: '3 adım',
    gradient: 'linear-gradient(135deg, #6366f1, #8b5cf6)'
  },
  {
    id: 'bulk-import',
    title: 'Toplu içe aktarma',
    description: 'CSV, Excel veya Z39.50 kaynaklarından yeni kayıtlar yükleyin.',
    shortcut: 'TI',
    meta: 'Yeni',
    gradient: 'linear-gradient(135deg, #22d3ee, #0ea5e9)'
  },
  {
    id: 'authority-link',
    title: 'Otorite bağlantısı',
    description: 'Yazar, konu ve seri otoritelerini ilişkilendirin.',
    shortcut: 'OB',
    meta: 'Otomatik',
    gradient: 'linear-gradient(135deg, #f97316, #ef4444)'
  },
  {
    id: 'publish-ready',
    title: 'Yayın haznesi',
    description: 'Taslakları yayınlamak için son kontrol adımını başlatın.',
    shortcut: 'YH',
    meta: '2 taslak',
    gradient: 'linear-gradient(135deg, #ec4899, #d946ef)'
  }
] as const

const metrics: Array<{
  category: string
  label: string
  value: string
  trendLabel: string
  tone: MetricTone
  direction: TrendDirection
  period: string
}> = [
  {
    category: 'Bu ay',
    label: 'Tamamlanan katalog kaydı',
    value: '128',
    trendLabel: '+18%',
    tone: 'positive',
    direction: 'up',
    period: 'Geçen aya göre'
  },
  {
    category: 'Bekleyen',
    label: 'RDA önerisi bekleyen kayıt',
    value: '12',
    trendLabel: '-3',
    tone: 'warning',
    direction: 'down',
    period: 'Kalan adım'
  },
  {
    category: 'Otorite',
    label: 'Başlık eşleşmesi',
    value: '92%',
    trendLabel: '+6%',
    tone: 'positive',
    direction: 'up',
    period: 'Bu hafta'
  },
  {
    category: 'Kalite',
    label: 'Alan doğrulama puanı',
    value: '97',
    trendLabel: '+2',
    tone: 'info',
    direction: 'up',
    period: 'Ortalama'
  }
]

const tabs: Array<{ id: TabId; label: string; description: string }> = [
  { id: 'temel', label: 'Temel Bilgiler', description: 'ISBN, başlık, yazar bilgileri' },
  { id: 'siniflama', label: 'Sınıflama', description: 'Dewey, konu başlıkları, koleksiyon' },
  { id: 'dijital', label: 'Dijital Varlıklar', description: 'URL, lisans ve dosya kontrolleri' }
]

const deweyOptions = [
  '000 - Genel Konular',
  '100 - Felsefe ve Psikoloji',
  '200 - Din',
  '300 - Sosyal Bilimler',
  '400 - Dil',
  '500 - Bilim',
  '600 - Teknoloji',
  '700 - Sanat',
  '800 - Edebiyat',
  '900 - Tarih ve Coğrafya'
] as const

const materialTypes = ['Kitap', 'Dergi', 'Referans Kaydı', 'Dijital Koleksiyon', 'Harita', 'Ses Kaydı'] as const
const languages = ['Türkçe', 'İngilizce', 'Almanca', 'Fransızca', 'Arapça', 'Rusça'] as const
const audienceLevels = ['Genel Okur', 'Lise', 'Üniversite', 'Akademisyen', 'Çocuk'] as const
const availabilityStatuses = ['Raflarda', 'Sınırlı erişim', 'Sadece dijital', 'Arşiv', 'Ön sipariş'] as const
const digitalFormats = ['PDF', 'EPUB', 'MOBI', 'MP3', 'MP4', 'JPEG', 'Diğer'] as const

const catalogEntries: CatalogEntry[] = [
  {
    isbn: '9789754707083',
    title: 'Benim Adım Kırmızı',
    author: 'Orhan Pamuk',
    year: '1998',
    status: 'Aktif'
  },
  {
    isbn: '9789750842665',
    title: 'Masumiyet Müzesi',
    author: 'Orhan Pamuk',
    year: '2008',
    status: 'Taslak'
  },
  {
    isbn: '9789750740183',
    title: 'Kör Baykuş',
    author: 'Sâdık Hidâyet',
    year: '2022',
    status: 'Gözden geçirme'
  },
  {
    isbn: '9786053323560',
    title: 'Tutunamayanlar',
    author: 'Oğuz Atay',
    year: '2014',
    status: 'Aktif'
  }
]

const recentActivity = [
  {
    id: 1,
    title: 'Yeni katalog kaydı yayınlandı',
    detail: '“Bilgi Çağında Kütüphanecilik” kaydı M. Keskin tarafından yayınlandı.',
    timeAgo: '2 dk'
  },
  {
    id: 2,
    title: 'RDA önerisi uygulandı',
    detail: '“Modern Eğitim Yaklaşımları” kaydında alan 245 alt alanlar güncellendi.',
    timeAgo: '14 dk'
  },
  {
    id: 3,
    title: 'Toplu içe aktarma tamamlandı',
    detail: 'STEM Koleksiyonu için 26 yeni kayıt başarıyla eklendi.',
    timeAgo: '32 dk'
  }
]

const workflowStages = [
  {
    title: 'Taslak oluşturuldu',
    subtitle: 'ISBN ve temel alanlar tamamlandı.',
    meta: '08:42',
    icon: '1',
    color: 'linear-gradient(135deg, #6366f1, #8b5cf6)'
  },
  {
    title: 'Otorite eşleştirme',
    subtitle: 'Yazar ve konu başlığı doğrulamaları yapılıyor.',
    meta: 'Devam',
    icon: '2',
    color: 'linear-gradient(135deg, #22d3ee, #0ea5e9)'
  },
  {
    title: 'Kurul onayı',
    subtitle: 'Yayın Komitesi incelemesinde.',
    meta: 'Yarın',
    icon: '3',
    color: 'linear-gradient(135deg, #ec4899, #d946ef)'
  }
]

const activeTab = ref<TabId>('temel')

const formModel = reactive({
  isbn: '9789754707083',
  title: 'Benim Adım Kırmızı',
  author: 'Orhan Pamuk',
  publisher: 'İletişim Yayınları',
  publicationYear: '1998',
  summary: "16. yüzyıl İstanbul'unda minyatür ustalarının dünyasını anlatan ödüllü roman.",
  dewey: '800 - Edebiyat',
  materialType: 'Kitap',
  subjects: 'Türk romanı, Tarihî kurgu, Minyatür',
  language: 'Türkçe',
  audience: 'Genel Okur',
  collection: 'Edebiyat Koleksiyonu',
  availability: 'Raflarda',
  digitalUrl: '',
  digitalFormat: '',
  license: 'Kütüphane içi kullanım'
})

const completionProgress = computed(() => {
  const fields: Array<keyof typeof formModel> = ['isbn', 'title', 'author', 'publisher', 'publicationYear', 'dewey', 'materialType', 'language']
  const filled = fields.filter((field) => String(formModel[field]).trim().length > 0).length
  return Math.round((filled / fields.length) * 100)
})

const marcPreview = computed(() => {
  const currentYear = String(new Date().getFullYear())
  const publicationYear = String(formModel.publicationYear || '').padStart(4, '0')
  const controlField = (currentYear + publicationYear + 'tu a          ').slice(0, 18)

  const marcObject = {
    '001': formModel.isbn || '---',
    '003': 'TR-MEB',
    '008': controlField,
    '020': { a: formModel.isbn || 'Belirtilmedi' },
    '100': { a: formModel.author || 'Yazar tanımsız' },
    '245': { a: formModel.title || 'Başlık tanımsız', b: formModel.summary?.slice(0, 60) || ' ' },
    '260': {
      a: 'İstanbul',
      b: formModel.publisher || 'Yayınevi tanımsız',
      c: formModel.publicationYear || 'Yayın yılı?'
    },
    '520': { a: formModel.summary || 'Özet eklenmedi.' },
    '650': { a: formModel.subjects || 'Konu başlığı eklenmedi.' },
    '856': { u: formModel.digitalUrl || 'Dijital kaynak eklenmedi.' }
  }

  return JSON.stringify(marcObject, null, 2)
})

const draftCount = computed(() => catalogEntries.filter((entry) => entry.status === 'Taslak').length)

const publishedPercentage = computed(() => {
  const published = catalogEntries.filter((entry) => entry.status === 'Aktif').length
  return Math.round((published / catalogEntries.length) * 100)
})

const getStatusTone = (status: CatalogEntry['status']) => {
  const map: Record<CatalogEntry['status'], 'success' | 'warning' | 'info'> = {
    Aktif: 'success',
    Taslak: 'warning',
    'Gözden geçirme': 'info'
  }
  return map[status]
}

const handleQuickAction = (id: string) => {
  console.info('Hızlı işlem tetiklendi: ' + id)
}
</script>

<style scoped>
.cataloging-page {
  display: flex;
  flex-direction: column;
  gap: 2.5rem;
  padding: 2.5rem 3rem 4rem;
  background: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
  color: #111827;
}

.hero {
  position: relative;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 2rem;
  padding: 2.75rem 3rem;
  border-radius: 28px;
  background: linear-gradient(135deg, #5b67ff 0%, #8c3fff 100%);
  overflow: hidden;
  color: #fff;
  box-shadow: 0 24px 60px -40px rgba(76, 29, 149, 0.65);
}

.hero::after {
  content: '';
  position: absolute;
  inset: 0;
  background: radial-gradient(circle at top right, rgba(255, 255, 255, 0.28), transparent 45%);
  pointer-events: none;
}

.hero__content {
  position: relative;
  z-index: 1;
  max-width: 620px;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.hero__status {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.35rem 1rem;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.14);
  font-size: 0.85rem;
  letter-spacing: 0.01em;
  backdrop-filter: blur(6px);
}

.hero__title {
  margin: 0;
  font-size: 2.4rem;
  font-weight: 600;
  letter-spacing: -0.01em;
}

.hero__subtitle {
  margin: 0;
  font-size: 1rem;
  line-height: 1.6;
  color: rgba(255, 255, 255, 0.82);
}

.hero__highlights {
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  margin: 0;
  padding: 0;
}

.hero__highlight {
  display: inline-flex;
  align-items: center;
  gap: 0.55rem;
  padding: 0.4rem 0.9rem;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.12);
  font-size: 0.9rem;
  backdrop-filter: blur(6px);
}

.hero__highlight-dot {
  width: 0.65rem;
  height: 0.65rem;
  border-radius: 50%;
  display: inline-block;
}

.hero__cta {
  position: relative;
  z-index: 1;
  display: inline-flex;
  align-items: center;
  gap: 0.8rem;
  padding: 0.85rem 1.6rem;
  border: none;
  border-radius: 16px;
  background: rgba(255, 255, 255, 0.18);
  color: #fff;
  font-weight: 600;
  cursor: pointer;
  box-shadow: 0 14px 24px -16px rgba(15, 23, 42, 0.5);
  transition: transform 0.2s ease, background 0.2s ease, box-shadow 0.2s ease;
}

.hero__cta:hover {
  transform: translateY(-2px);
  background: rgba(255, 255, 255, 0.26);
  box-shadow: 0 28px 40px -26px rgba(15, 23, 42, 0.45);
}

.hero__cta-icon {
  width: 2rem;
  height: 2rem;
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.25);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2rem;
}

.quick-actions {
  display: grid;
  gap: 1.25rem;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
}

.quick-actions__item {
  border: none;
  border-radius: 20px;
  background: #fff;
  padding: 1.4rem 1.5rem;
  text-align: left;
  display: flex;
  flex-direction: column;
  gap: 0.9rem;
  box-shadow: 0 18px 40px -30px rgba(15, 23, 42, 0.45);
  cursor: pointer;
  transition: transform 0.18s ease, box-shadow 0.18s ease;
}

.quick-actions__item:hover {
  transform: translateY(-4px);
  box-shadow: 0 26px 50px -28px rgba(15, 23, 42, 0.42);
}

.quick-actions__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.quick-actions__shortcut {
  color: #fff;
  font-weight: 600;
  border-radius: 14px;
  padding: 0.35rem 0.75rem;
  font-size: 0.85rem;
  box-shadow: inset 0 0 0 1px rgba(255, 255, 255, 0.16);
}

.quick-actions__badge {
  padding: 0.25rem 0.6rem;
  border-radius: 999px;
  background: rgba(15, 23, 42, 0.08);
  font-size: 0.75rem;
  font-weight: 500;
  color: #475569;
}

.quick-actions__title {
  margin: 0;
  font-size: 1rem;
  font-weight: 600;
  color: #0f172a;
}

.quick-actions__description {
  margin: 0;
  font-size: 0.9rem;
  color: #475569;
  line-height: 1.5;
}

.stats {
  display: grid;
  gap: 1.25rem;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
}

.stats-card {
  background: #fff;
  border-radius: 20px;
  padding: 1.5rem;
  box-shadow: 0 18px 40px -32px rgba(15, 23, 42, 0.4);
  display: flex;
  flex-direction: column;
  gap: 0.7rem;
  border: 1px solid rgba(148, 163, 184, 0.2);
}

.stats-card__category {
  font-size: 0.85rem;
  font-weight: 600;
  color: #6366f1;
  letter-spacing: 0.03em;
}

.stats-card__value {
  font-size: 2rem;
  font-weight: 600;
  color: #0f172a;
}

.stats-card__label {
  font-size: 0.95rem;
  color: #475569;
}

.stats-card__trend {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.85rem;
  border-radius: 12px;
  padding: 0.25rem 0.55rem;
  width: fit-content;
}

.stats-card__trend-icon {
  font-size: 0.75rem;
}

.stats-card__trend-period {
  color: #64748b;
}

.stats-card__trend--positive {
  background: rgba(16, 185, 129, 0.12);
  color: #0f766e;
}

.stats-card__trend--warning {
  background: rgba(251, 191, 36, 0.14);
  color: #b45309;
}

.stats-card__trend--info {
  background: rgba(59, 130, 246, 0.14);
  color: #1d4ed8;
}

.main-grid {
  display: grid;
  grid-template-columns: minmax(0, 7fr) minmax(0, 4fr);
  gap: 1.75rem;
}

.card {
  background: #fff;
  border-radius: 24px;
  padding: 1.8rem;
  box-shadow: 0 18px 50px -36px rgba(15, 23, 42, 0.45);
  border: 1px solid rgba(148, 163, 184, 0.18);
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.card__header {
  display: flex;
  justify-content: space-between;
  gap: 1.5rem;
  align-items: flex-start;
}

.card__title {
  margin: 0;
  font-size: 1.25rem;
  font-weight: 600;
  color: #0f172a;
}

.card__subtitle {
  margin: 0.2rem 0 0;
  font-size: 0.9rem;
  color: #64748b;
}

.card__meta {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
  align-items: center;
}

.pill {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.35rem 0.75rem;
  border-radius: 999px;
  font-size: 0.8rem;
  font-weight: 600;
}

.pill--neutral {
  background: rgba(148, 163, 184, 0.16);
  color: #475569;
}

.pill--primary {
  background: rgba(99, 102, 241, 0.12);
  color: #4338ca;
}

.pill--success {
  background: rgba(34, 197, 94, 0.14);
  color: #15803d;
}

.pill--warning {
  background: rgba(250, 204, 21, 0.16);
  color: #b45309;
}

.tabs {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.tabs__item {
  flex: 1 1 180px;
  padding: 0.85rem 1rem;
  border-radius: 16px;
  background: rgba(15, 23, 42, 0.04);
  border: 1px solid transparent;
  cursor: pointer;
  text-align: left;
  transition: all 0.18s ease;
}

.tabs__item--active {
  background: rgba(99, 102, 241, 0.14);
  border-color: rgba(79, 70, 229, 0.45);
}

.tabs__label {
  display: block;
  font-weight: 600;
  color: #0f172a;
}

.tabs__description {
  font-size: 0.82rem;
  color: #64748b;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form__grid {
  display: grid;
  gap: 1rem;
}

.form__grid--two {
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
}

.form__grid--three {
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
}

.form-field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-field__label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #475569;
}

.form-field__control {
  border-radius: 12px;
  border: 1px solid rgba(148, 163, 184, 0.5);
  padding: 0.7rem 0.85rem;
  font-size: 0.95rem;
  transition: border 0.2s ease, box-shadow 0.2s ease;
  font-family: inherit;
}

.form-field__control:focus {
  outline: none;
  border-color: rgba(79, 70, 229, 0.5);
  box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.18);
}

textarea.form-field__control {
  resize: vertical;
}

.dropzone {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.2rem;
  border-radius: 16px;
  border: 1px dashed rgba(148, 163, 184, 0.6);
  background: rgba(248, 250, 252, 0.8);
  color: #475569;
  font-size: 0.9rem;
}

.dropzone__icon {
  width: 2.6rem;
  height: 2.6rem;
  border-radius: 14px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  background: rgba(99, 102, 241, 0.12);
  color: #4f46e5;
  font-size: 1.2rem;
}

.dropzone__link {
  color: #4f46e5;
  font-weight: 600;
}

.form__footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
}

.form__hint {
  font-size: 0.85rem;
  color: #475569;
}

.form__actions {
  display: flex;
  gap: 0.75rem;
}

.btn {
  border: none;
  border-radius: 12px;
  padding: 0.65rem 1.4rem;
  font-size: 0.92rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.16s ease, box-shadow 0.16s ease, background 0.16s ease;
}

.btn--ghost {
  background: rgba(15, 23, 42, 0.05);
  color: #1e293b;
}

.btn--ghost:hover {
  background: rgba(15, 23, 42, 0.08);
}

.btn--primary {
  background: linear-gradient(135deg, #6366f1, #8b5cf6);
  color: #fff;
  box-shadow: 0 18px 30px -24px rgba(99, 102, 241, 0.75);
}

.btn--primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 26px 38px -26px rgba(99, 102, 241, 0.7);
}

.side-column {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.preview-card__code {
  background: #0f172a;
  color: #e2e8f0;
  padding: 1.2rem;
  border-radius: 16px;
  font-size: 0.8rem;
  line-height: 1.5;
  max-height: 280px;
  overflow: auto;
}

.preview-card__footnote {
  margin: 0;
  font-size: 0.8rem;
  color: #64748b;
}

.timeline {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
}

.timeline__item {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.timeline__icon {
  width: 2.4rem;
  height: 2.4rem;
  border-radius: 18px;
  color: #fff;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
}

.timeline__title {
  margin: 0;
  font-weight: 600;
  color: #0f172a;
}

.timeline__subtitle {
  margin: 0;
  font-size: 0.85rem;
  color: #64748b;
}

.timeline__meta {
  margin-left: auto;
  font-size: 0.75rem;
  color: #1f2937;
  padding: 0.25rem 0.55rem;
  border-radius: 999px;
  background: rgba(148, 163, 184, 0.18);
}

.card__link {
  border: none;
  background: none;
  color: #4f46e5;
  font-weight: 600;
  cursor: pointer;
}

.activity-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.activity-list__item {
  padding-bottom: 1rem;
  border-bottom: 1px solid rgba(226, 232, 240, 0.8);
}

.activity-list__item:last-child {
  border-bottom: none;
  padding-bottom: 0;
}

.activity-list__header {
  display: flex;
  justify-content: space-between;
  font-size: 0.9rem;
  font-weight: 600;
  color: #0f172a;
}

.activity-list__description {
  margin: 0.3rem 0 0;
  font-size: 0.85rem;
  color: #64748b;
}

.activity-list__time {
  font-size: 0.75rem;
  color: #94a3b8;
}

.table-card__wrapper {
  overflow-x: auto;
}

.catalog-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.92rem;
}

.catalog-table thead {
  background: rgba(99, 102, 241, 0.08);
}

.catalog-table th,
.catalog-table td {
  padding: 0.85rem 1rem;
  text-align: left;
  border-bottom: 1px solid rgba(226, 232, 240, 0.9);
}

.catalog-table__code {
  font-family: 'JetBrains Mono', 'Fira Code', monospace;
  font-size: 0.85rem;
  color: #475569;
}

.catalog-table__actions {
  white-space: nowrap;
}

.table-action {
  border: none;
  background: none;
  font-size: 0.85rem;
  font-weight: 600;
  cursor: pointer;
  margin-right: 0.75rem;
}

.table-action:last-child {
  margin-right: 0;
}

.table-action--primary {
  color: #4f46e5;
}

.table-action--danger {
  color: #ef4444;
}

.badge {
  display: inline-flex;
  align-items: center;
  padding: 0.25rem 0.7rem;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 600;
}

.badge--success {
  background: rgba(34, 197, 94, 0.16);
  color: #15803d;
}

.badge--warning {
  background: rgba(250, 204, 21, 0.18);
  color: #b45309;
}

.badge--info {
  background: rgba(59, 130, 246, 0.18);
  color: #1d4ed8;
}

@media (max-width: 1200px) {
  .main-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .cataloging-page {
    padding: 1.5rem;
  }

  .hero {
    flex-direction: column;
    align-items: flex-start;
  }

  .quick-actions,
  .stats {
    grid-template-columns: 1fr;
  }
}
</style>
