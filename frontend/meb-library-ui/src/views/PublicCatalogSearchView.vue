<template>
  <div class="public-catalog">
    <header class="hero">
      <div class="hero__actions">
        <RouterLink class="cta-btn cta-btn--ghost" to="/login?mode=register">Kayıt ol</RouterLink>
        <RouterLink class="cta-btn cta-btn--solid" to="/login">Giriş yap</RouterLink>
      </div>

      <div class="hero__text">
        <span class="hero__eyebrow">MEB Dijital Kütüphane</span>
        <h1>Türkiye genelindeki kitap ve materyalleri tek noktadan keşfedin</h1>
        <p>
          Okulunuz ya da merkez kütüphane hesabınız olmasa bile koleksiyonu inceleyin,
          aradığınız kaydı bulun ve erişim seçeneklerini öğrenin.
        </p>
      </div>

      <form class="search-panel" @submit.prevent="submitSearch">
        <div class="search-panel__input">
          <span aria-hidden="true">🔍</span>
          <input
            v-model.trim="criteria.query"
            type="search"
            placeholder="Başlık, yazar, ISBN veya konu ara..."
            autocomplete="off"
          />
        </div>
        <button type="submit" class="search-panel__button">
          {{ state.loading ? 'Aranıyor...' : 'Ara' }}
        </button>
      </form>

      <div class="quick-filters" role="group" aria-label="Materyal türü filtreleri">
        <button
          v-for="option in materialOptions"
          :key="option.value"
          type="button"
          :class="['quick-filter', { active: criteria.materialType === option.value }]"
          @click="applyMaterialFilter(option.value)"
        >
          <span class="quick-filter__icon" aria-hidden="true">{{ option.icon }}</span>
          {{ option.label }}
        </button>
      </div>
    </header>

    <section class="filters">
      <div class="filters__group">
        <label for="language-filter">Dil</label>
        <select id="language-filter" v-model="criteria.language" @change="handleFilterChange(true)">
          <option value="all">Tüm diller</option>
          <option v-for="language in languageOptions" :key="language" :value="language">
            {{ language }}
          </option>
        </select>
      </div>

      <div class="filters__group">
        <label for="year-from">Yayın yılı</label>
        <div class="year-range">
          <input
            id="year-from"
            v-model.number="criteria.year.from"
            type="number"
            min="1800"
            max="2100"
            placeholder="Başlangıç"
            @change="handleFilterChange(true)"
          />
          <span aria-hidden="true">—</span>
          <input
            v-model.number="criteria.year.to"
            type="number"
            min="1800"
            max="2100"
            placeholder="Bitiş"
            @change="handleFilterChange(true)"
          />
        </div>
      </div>

      <div class="filters__group">
        <label for="sort-option">Sıralama</label>
        <select id="sort-option" v-model="criteria.sort" @change="handleFilterChange(true)">
          <option value="relevance">En alakalı sonuçlar</option>
          <option value="recent">En yeni kayıtlar</option>
          <option value="title">Başlığa göre A-Z</option>
        </select>
      </div>

      <div class="filters__group">
        <label for="page-size">Sayfa başına</label>
        <select id="page-size" v-model.number="criteria.pageSize" @change="handleFilterChange(true)">
          <option v-for="size in pageSizeOptions" :key="size" :value="size">
            {{ size }}
          </option>
        </select>
      </div>

      <button type="button" class="filters__reset" @click="resetFilters">
        Filtreleri sıfırla
      </button>
    </section>

    <section class="results">
      <header class="results__header">
        <div>
          <h2>Arama sonuçları</h2>
          <p v-if="state.searchPerformed">
            {{ state.total === 0 ? 'Sonuç bulunamadı' : `${state.total.toLocaleString('tr-TR')} kayıt listeleniyor` }}
          </p>
        </div>
        <small v-if="state.searchPerformed && state.total > 0">
          {{ showingRange }}
        </small>
      </header>

      <div v-if="state.loading" class="results__loading">
        <span class="spinner" aria-hidden="true"></span>
        <p>Katalog taranıyor, lütfen bekleyin...</p>
      </div>

      <div v-else-if="state.error && records.length === 0" class="results__alert">
        <strong>Şu anda canlı katalog cevap vermiyor.</strong>
        <p>{{ state.error }}</p>
        <p>Örnek kayıtlar gösteriliyor.</p>
      </div>

      <div v-if="records.length > 0" class="record-grid">
        <article v-for="record in records" :key="record.id" class="record-card">
          <div class="record-card__badge" :data-status="record.status">
            {{ availabilityLabel(record.status) }}
          </div>

          <h3>{{ record.title }}</h3>
          <p class="record-card__subtitle">
            {{ record.author }} • {{ formattedYear(record.publicationYear) }}
          </p>

          <p class="record-card__summary">
            {{ record.summary || 'Bu kayıt için özet bilgisi eklenmedi.' }}
          </p>

          <dl class="record-card__meta">
            <div>
              <dt>Materyal</dt>
              <dd>{{ record.materialType }}</dd>
            </div>
            <div>
              <dt>ISBN</dt>
              <dd>{{ record.isbn || 'Belirtilmemiş' }}</dd>
            </div>
            <div>
              <dt>Dil</dt>
              <dd>{{ record.language || 'Bilinmiyor' }}</dd>
            </div>
            <div>
              <dt>Kütüphane</dt>
              <dd>{{ record.location }}</dd>
            </div>
          </dl>

          <footer class="record-card__footer">
            <button type="button" class="record-card__action" @click="openDetail(record.id)">
              Erişim seçenekleri
            </button>
            <span class="record-card__updated">
              Güncellendi: {{ formatLastUpdated(record.lastUpdated) }}
            </span>
          </footer>
        </article>
      </div>

      <div v-else-if="state.searchPerformed && !state.loading" class="results__empty">
        <h3>Aradığınız kriterlere uygun sonuç bulunamadı.</h3>
        <p>Farklı anahtar kelimeler deneyebilir veya filtreleri temizleyebilirsiniz.</p>
        <button type="button" @click="resetFilters">Filtreleri temizle</button>
      </div>

      <nav
        v-if="state.total > criteria.pageSize"
        class="pagination"
        role="navigation"
        aria-label="Arama sonucu sayfalama"
      >
        <button type="button" :disabled="criteria.page === 1" @click="changePage('previous')">
          Önceki
        </button>
        <span>{{ criteria.page }} / {{ totalPages }}</span>
        <button type="button" :disabled="criteria.page === totalPages" @click="changePage('next')">
          Sonraki
        </button>
      </nav>
    </section>

    <footer class="landing-footer">
      <div>
        <h4>Kütüphaneler için</h4>
        <p>
          Kaynağı rezerve etmek veya ödünç almak isterseniz MEB kimliğinizle giriş yapın.
        </p>
      </div>
      <a class="footer-link tertiary" href="https://www.meb.gov.tr" target="_blank" rel="noopener">
        MEB Ana Sayfa
      </a>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { apiClient } from '@/stores/api'

type CatalogAvailability = 'available' | 'limited' | 'unavailable'

interface CatalogRecord {
  id: string
  title: string
  author: string
  materialType: string
  language?: string | null
  publicationYear?: number | null
  isbn?: string | null
  summary?: string | null
  status: CatalogAvailability
  location: string
  lastUpdated: string
}

interface ServerCatalogRecord {
  id: string
  title: string
  author: string
  materialType: string
  language?: string | null
  publicationYear?: number | null
  isbn?: string | null
  summary?: string | null
  status: string
  location: string
  lastUpdated?: string | null
}

interface SearchResponse {
  items: ServerCatalogRecord[]
  totalCount: number
  page: number
  pageSize: number
}

interface MaterialOption {
  value: string
  label: string
  icon: string
  keywords: string[]
}

const materialOptions: MaterialOption[] = [
  { value: 'all', label: 'Tüm materyaller', icon: '📚', keywords: [] },
  { value: 'book', label: 'Kitaplar', icon: '📖', keywords: ['book', 'kitap', 'roman'] },
  { value: 'magazine', label: 'Süreli yayınlar', icon: '🗞️', keywords: ['magazine', 'dergi', 'süreli'] },
  { value: 'media', label: 'Multimedya', icon: '🎧', keywords: ['media', 'multimedya', 'dvd', 'cd'] },
  { value: 'thesis', label: 'Tez & rapor', icon: '📑', keywords: ['tez', 'thesis', 'rapor', 'report'] }
]

const languageOptions = ['Türkçe', 'İngilizce', 'Arapça', 'Kürtçe', 'Almanca']
const pageSizeOptions = [9, 12, 18, 24]

const criteria = reactive({
  query: '',
  materialType: 'all',
  language: 'all',
  year: {
    from: undefined as number | undefined,
    to: undefined as number | undefined
  },
  sort: 'relevance',
  page: 1,
  pageSize: 12
})

const state = reactive({
  loading: false,
  searchPerformed: false,
  total: 0,
  error: ''
})

const router = useRouter()
const records = ref<CatalogRecord[]>([])
const hasUserSearched = ref(false)

const totalPages = computed(() =>
  state.total === 0 ? 1 : Math.max(1, Math.ceil(state.total / criteria.pageSize))
)

const showingRange = computed(() => {
  if (state.total === 0) {
    return '0 kayıt'
  }

  const start = (criteria.page - 1) * criteria.pageSize + 1
  const end = Math.min(criteria.page * criteria.pageSize, state.total)
  return `${start} - ${end}`
})

const availabilityLabel = (status: CatalogAvailability) => {
  switch (status) {
    case 'available':
      return 'Ödünç verilebilir'
    case 'limited':
      return 'Sınırlı erişim'
    default:
      return 'Kütüphane içinde'
  }
}

const formattedYear = (year: number | null | undefined) => year ?? 'Bilinmiyor'

const formatLastUpdated = (value: string) => {
  if (!value) {
    return 'Güncelleme bilgisi yok'
  }

  const parsed = new Date(value)
  if (Number.isNaN(parsed.getTime())) {
    return value
  }

  return new Intl.DateTimeFormat('tr-TR', { dateStyle: 'medium' }).format(parsed)
}

const mapServerRecord = (record: ServerCatalogRecord, index: number): CatalogRecord => {
  const availability = (['available', 'limited', 'unavailable'] as CatalogAvailability[]).includes(
    record.status as CatalogAvailability
  )
    ? (record.status as CatalogAvailability)
    : 'unavailable'

  const isoLastUpdated = record.lastUpdated ? new Date(record.lastUpdated).toISOString() : ''

  return {
    id: record.id ?? `record-${index}`,
    title: record.title?.trim() || 'Başlıksız kayıt',
    author: record.author?.trim() || 'Yazar bilgisi bulunmuyor',
    materialType: record.materialType?.trim() || 'Materyal',
    language: record.language?.trim(),
    publicationYear: record.publicationYear ?? null,
    isbn: record.isbn?.trim() || null,
    summary: record.summary?.trim() || null,
    status: availability,
    location: record.location?.trim() || 'Katalog deposu',
    lastUpdated: isoLastUpdated
  }
}

const applyMaterialFilter = (value: string) => {
  if (criteria.materialType === value) {
    return
  }

  criteria.materialType = value
  handleFilterChange(true)
}

const openDetail = (id: string) => {
  if (!id) {
    return
  }

  router.push({ name: 'public-catalog-detail', params: { id } })
}

const buildQueryParams = () => {
  const params: Record<string, unknown> = {
    query: criteria.query || undefined,
    page: criteria.page,
    pageSize: criteria.pageSize,
    sort: criteria.sort
  }

  if (criteria.materialType !== 'all') {
    params.materialType = criteria.materialType
  }

  if (criteria.language !== 'all') {
    params.language = criteria.language
  }

  if (criteria.year.from) {
    params.yearFrom = criteria.year.from
  }

  if (criteria.year.to) {
    params.yearTo = criteria.year.to
  }

  return params
}

const mockDataset: CatalogRecord[] = [
  {
    id: 'mock-1',
    title: 'Benim Adım Kırmızı',
    author: 'Orhan Pamuk',
    materialType: 'Kitap • Roman',
    language: 'Türkçe',
    publicationYear: 1998,
    isbn: '9754707080',
    summary: '16. yüzyıl İstanbulunda geçen polisiye roman.',
    status: 'available',
    location: 'Merkez Kütüphane • İstanbul',
    lastUpdated: new Date().toISOString()
  },
  {
    id: 'mock-2',
    title: 'İnce Memed',
    author: 'Yaşar Kemal',
    materialType: 'Kitap • Roman',
    language: 'Türkçe',
    publicationYear: 1955,
    isbn: '9789754707083',
    summary: 'Çukurova topraklarında destansı bir hikâye.',
    status: 'available',
    location: 'Merkez Kütüphane • Adana',
    lastUpdated: new Date().toISOString()
  },
  {
    id: 'mock-3',
    title: 'Beyaz Diş',
    author: 'Jack London',
    materialType: 'Kitap • Macera',
    language: 'Türkçe',
    publicationYear: 1906,
    isbn: '9786059852564',
    summary: 'Vahşi doğada geçen insan ve hayvan ilişkisinin hikâyesi.',
    status: 'limited',
    location: 'Ankara İl Kütüphanesi',
    lastUpdated: new Date().toISOString()
  },
  {
    id: 'mock-4',
    title: 'STEM Eğitimi',
    author: 'Milli Eğitim Bakanlığı',
    materialType: 'Kitap • Eğitim',
    language: 'Türkçe',
    publicationYear: 2023,
    isbn: '9789751100000',
    summary: 'Sınıf içi STEM etkinlikleri için uygulamalı rehber.',
    status: 'available',
    location: 'Merkez Kütüphane • Ankara',
    lastUpdated: new Date().toISOString()
  },
  {
    id: 'mock-5',
    title: 'Bilim Dergisi - Özel Sayı',
    author: 'TÜBİTAK',
    materialType: 'Süreli Yayın',
    language: 'Türkçe',
    publicationYear: 2024,
    isbn: null,
    summary: 'Sürdürülebilir enerji dosyası.',
    status: 'unavailable',
    location: 'İzmir Bölge Kütüphanesi',
    lastUpdated: new Date().toISOString()
  },
  {
    id: 'mock-6',
    title: 'Digital Literacy for Educators',
    author: 'UNESCO',
    materialType: 'Rapor',
    language: 'İngilizce',
    publicationYear: 2021,
    isbn: null,
    summary: 'Eğitimciler için dijital dönüşüm rehberi.',
    status: 'limited',
    location: 'Merkez Kütüphane • Ankara',
    lastUpdated: new Date().toISOString()
  }
]

const useMockResults = (reason?: string) => {
  state.error = reason ?? ''

  const selectedMaterial = materialOptions.find(option => option.value === criteria.materialType)
  const queryLower = criteria.query.trim().toLowerCase()

  const matches = mockDataset.filter(record => {
    const recordMaterial = record.materialType.toLowerCase()
    const matchesQuery =
      !queryLower ||
      record.title.toLowerCase().includes(queryLower) ||
      record.author.toLowerCase().includes(queryLower) ||
      (record.isbn && record.isbn.includes(criteria.query))

    const matchesMaterial =
      criteria.materialType === 'all' ||
      !!selectedMaterial?.keywords.some(keyword => recordMaterial.includes(keyword))

    const matchesLanguage =
      criteria.language === 'all' ||
      record.language?.toLowerCase() === criteria.language.toLowerCase()

    const matchesYearFrom = !criteria.year.from || (record.publicationYear ?? 0) >= criteria.year.from
    const matchesYearTo = !criteria.year.to || (record.publicationYear ?? 9999) <= criteria.year.to

    return matchesQuery && matchesMaterial && matchesLanguage && matchesYearFrom && matchesYearTo
  })

  state.total = matches.length
  state.searchPerformed = true
  records.value = matches.slice(0, criteria.pageSize)
}

const performSearch = async (resetPage = false, options?: { userInitiated?: boolean }) => {
  if (resetPage) {
    criteria.page = 1
  }

  if (options?.userInitiated) {
    hasUserSearched.value = true
  } else if (!hasUserSearched.value) {
    return
  }

  state.loading = true
  state.searchPerformed = true
  state.error = ''

  try {
    const params = buildQueryParams()
    const { data } = await apiClient.get<SearchResponse>('/public-catalog/search', { params })

    if (!data || !Array.isArray(data.items)) {
      useMockResults('Canlı katalogdan beklenmeyen bir yanıt alındı.')
      return
    }

    state.total = data.totalCount ?? data.items.length
    criteria.page = data.page ?? criteria.page
    criteria.pageSize = data.pageSize ?? criteria.pageSize

    records.value = data.items.map(mapServerRecord)
  } catch (error: any) {
    const message =
      error?.response?.data?.message ??
      error?.message ??
      'Katalog servisine şu anda ulaşılamıyor.'
    console.warn('Public catalog search failed, falling back to mock data.', error)
    useMockResults(message)
  } finally {
    state.loading = false
  }
}

const submitSearch = () => {
  if (!criteria.query.trim()) {
    state.error = 'Lütfen aramak için anahtar kelime girin.'
    state.searchPerformed = false
    state.total = 0
    records.value = []
    return
  }

  performSearch(true, { userInitiated: true })
}

const handleFilterChange = (resetPage = false) => {
  performSearch(resetPage)
}

const changePage = (direction: 'previous' | 'next') => {
  if (direction === 'previous' && criteria.page > 1) {
    criteria.page -= 1
    performSearch()
  } else if (direction === 'next' && criteria.page < totalPages.value) {
    criteria.page += 1
    performSearch()
  }
}

const resetFilters = () => {
  criteria.query = ''
  criteria.materialType = 'all'
  criteria.language = 'all'
  criteria.year.from = undefined
  criteria.year.to = undefined
  criteria.sort = 'relevance'
  criteria.page = 1
  criteria.pageSize = 12

  if (hasUserSearched.value) {
    performSearch(true)
  } else {
    records.value = []
    state.total = 0
    state.error = ''
    state.searchPerformed = false
  }
}
</script>

<style scoped>
.public-catalog {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  padding: 2.5rem min(5vw, 4rem) 3.5rem;
  background: radial-gradient(circle at top, #fef2f2 0%, #fee2e2 45%, #fef9c3 100%);
  min-height: 100vh;
}

.hero {
  position: relative;
  display: grid;
  gap: 1.5rem;
  background: rgba(255, 255, 255, 0.75);
  border-radius: 24px;
  padding: 2.5rem;
  box-shadow: 0 24px 40px rgba(252, 165, 165, 0.25);
}

.hero__actions {
  display: flex;
  gap: 0.75rem;
  justify-self: end;
}

.cta-btn {
  border-radius: 999px;
  padding: 0.55rem 1.4rem;
  font-weight: 600;
  font-size: 0.95rem;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  border: 1px solid transparent;
  text-decoration: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.cta-btn--solid {
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: #fff;
  box-shadow: 0 14px 24px rgba(239, 68, 68, 0.25);
}

.cta-btn--ghost {
  background: rgba(255, 255, 255, 0.85);
  color: #b91c1c;
  border-color: rgba(248, 113, 113, 0.4);
}

.cta-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 18px 30px rgba(239, 68, 68, 0.25);
}

.cta-btn--ghost:hover {
  background: rgba(255, 255, 255, 1);
}

.hero__text {
  display: grid;
  gap: 0.75rem;
}

.hero__eyebrow {
  text-transform: uppercase;
  font-size: 0.85rem;
  letter-spacing: 0.16em;
  font-weight: 600;
  color: #b91c1c;
}

.hero h1 {
  font-size: clamp(2rem, 4vw, 3rem);
  color: #7f1d1d;
  margin: 0;
}

.hero p {
  max-width: 620px;
  color: rgba(127, 29, 29, 0.7);
  font-size: 1.05rem;
  line-height: 1.6;
}

.search-panel {
  display: grid;
  grid-template-columns: 1fr auto;
  gap: 0.75rem;
  align-items: center;
  background: white;
  border-radius: 16px;
  padding: 0.75rem;
  border: 1px solid rgba(252, 165, 165, 0.35);
}

.search-panel__input {
  display: flex;
  align-items: center;
  gap: 0.65rem;
  padding: 0.75rem 1rem;
  background: rgba(248, 250, 252, 0.8);
  border-radius: 12px;
  font-size: 1rem;
}

.search-panel__input input {
  border: none;
  flex: 1;
  background: transparent;
  font-size: 1rem;
  color: #1f2937;
}

.search-panel__input input:focus-visible {
  outline: none;
}

.search-panel__button {
  border: none;
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: white;
  padding: 0.9rem 2.2rem;
  border-radius: 12px;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
  box-shadow: 0 16px 24px rgba(220, 38, 38, 0.25);
}

.search-panel__button:disabled {
  opacity: 0.75;
  cursor: progress;
  box-shadow: none;
}

.quick-filters {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
}

.quick-filter {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.6rem 1rem;
  border-radius: 999px;
  border: 1px solid rgba(248, 113, 113, 0.35);
  background: rgba(255, 255, 255, 0.85);
  color: #7f1d1d;
  font-weight: 500;
  cursor: pointer;
  transition: transform 0.16s ease, background 0.16s ease;
}

.quick-filter.active {
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: #fff;
  border-color: transparent;
}

.quick-filter:hover {
  transform: translateY(-1px);
}

.filters {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  background: rgba(255, 255, 255, 0.9);
  border-radius: 18px;
  padding: 1.25rem 1.5rem;
  border: 1px solid rgba(248, 250, 252, 0.8);
  align-items: flex-end;
}

.filters__group {
  display: grid;
  gap: 0.35rem;
}

.filters__group label {
  font-size: 0.82rem;
  font-weight: 600;
  letter-spacing: 0.02em;
  color: rgba(76, 29, 149, 0.8);
}

.filters select,
.filters input {
  border: 1px solid rgba(226, 232, 240, 0.9);
  border-radius: 10px;
  padding: 0.55rem 0.75rem;
  font-size: 0.95rem;
  background: white;
  color: #1f2937;
}

.filters__reset {
  margin-left: auto;
  border: none;
  background: rgba(248, 113, 113, 0.12);
  color: #b91c1c;
  padding: 0.65rem 0.95rem;
  border-radius: 12px;
  font-weight: 600;
  cursor: pointer;
}

.year-range {
  display: flex;
  align-items: center;
  gap: 0.35rem;
}

.results {
  display: grid;
  gap: 1.75rem;
}

.results__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  color: #1f2937;
}

.results__header h2 {
  margin: 0;
  font-size: 1.5rem;
}

.results__header p {
  margin: 0.2rem 0 0;
  color: rgba(55, 65, 81, 0.7);
}

.results__loading,
.results__alert,
.results__empty {
  background: rgba(255, 255, 255, 0.9);
  border-radius: 18px;
  padding: 2rem;
  display: grid;
  gap: 0.75rem;
  place-items: center;
  text-align: center;
  color: rgba(55, 65, 81, 0.8);
  border: 1px solid rgba(226, 232, 240, 0.9);
}

.results__alert strong {
  color: #b91c1c;
}

.spinner {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  border: 4px solid rgba(248, 113, 113, 0.3);
  border-top-color: #dc2626;
  animation: spin 0.85s linear infinite;
}

.record-grid {
  display: grid;
  gap: 1.5rem;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
}

.record-card {
  background: white;
  border-radius: 18px;
  padding: 1.5rem;
  box-shadow: 0 14px 35px rgba(15, 23, 42, 0.12);
  border: 1px solid rgba(226, 232, 240, 0.9);
  display: grid;
  gap: 0.75rem;
}

.record-card__badge {
  align-self: flex-start;
  font-size: 0.78rem;
  font-weight: 600;
  padding: 0.35rem 0.8rem;
  border-radius: 999px;
  letter-spacing: 0.04em;
  text-transform: uppercase;
}

.record-card__badge[data-status='available'] {
  background: rgba(34, 197, 94, 0.16);
  color: #166534;
}

.record-card__badge[data-status='limited'] {
  background: rgba(251, 191, 36, 0.22);
  color: #92400e;
}

.record-card__badge[data-status='unavailable'] {
  background: rgba(248, 113, 113, 0.18);
  color: #b91c1c;
}

.record-card h3 {
  margin: 0;
  font-size: 1.25rem;
  color: #111827;
}

.record-card__subtitle {
  margin: 0;
  color: rgba(55, 65, 81, 0.75);
  font-size: 0.95rem;
}

.record-card__summary {
  margin: 0;
  color: rgba(55, 65, 81, 0.82);
  line-height: 1.5;
  font-size: 0.95rem;
}

.record-card__meta {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 0.75rem;
  margin: 0;
}

.record-card__meta div {
  display: grid;
  gap: 0.25rem;
}

.record-card__meta dt {
  font-size: 0.75rem;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(148, 163, 184, 0.9);
}

.record-card__meta dd {
  margin: 0;
  font-weight: 600;
  color: #1f2937;
}

.record-card__footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  font-size: 0.85rem;
  color: rgba(75, 85, 99, 0.8);
}

.record-card__action {
  border: none;
  background: rgba(248, 113, 113, 0.15);
  color: #b91c1c;
  padding: 0.5rem 1.2rem;
  border-radius: 10px;
  font-weight: 600;
  cursor: pointer;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
}

.pagination button {
  border: none;
  background: rgba(248, 113, 113, 0.12);
  color: #b91c1c;
  padding: 0.6rem 1.1rem;
  border-radius: 10px;
  font-weight: 600;
  cursor: pointer;
}

.pagination button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.landing-footer {
  background: rgba(15, 23, 42, 0.86);
  color: rgba(226, 232, 240, 0.9);
  border-radius: 18px;
  padding: 2rem 2.5rem;
  display: flex;
  justify-content: space-between;
  gap: 2rem;
  flex-wrap: wrap;
}

.landing-footer h4 {
  margin: 0 0 0.5rem;
  font-size: 1.2rem;
  color: #fff;
}

.footer-link {
  color: #fff;
  text-decoration: none;
  padding: 0.65rem 1.25rem;
  border-radius: 10px;
  font-weight: 600;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.25);
}

.footer-link.tertiary {
  align-self: center;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

@media (max-width: 960px) {
  .hero__actions {
    justify-self: stretch;
  }

  .search-panel {
    grid-template-columns: 1fr;
    padding: 1rem;
  }

  .search-panel__button {
    width: 100%;
  }
}

@media (max-width: 640px) {
  .public-catalog {
    padding: 1.8rem 1.25rem 2.8rem;
  }

  .hero__actions {
    flex-direction: column;
    align-items: stretch;
    gap: 0.5rem;
  }

  .cta-btn {
    width: 100%;
  }

  .filters {
    flex-direction: column;
    align-items: stretch;
  }

  .filters__reset {
    margin-left: 0;
  }

  .record-card__meta {
    grid-template-columns: 1fr;
  }

  .landing-footer {
    padding: 1.5rem;
  }
}
</style>
