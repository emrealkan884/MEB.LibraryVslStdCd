<template>
  <div class="catalog-search-page">
    <section class="hero-card">
      <div class="hero-content">
        <h1>Katalog Arama</h1>
        <p>
          Merkez kütüphane koleksiyonunda hızlıca arama yapın, filtreleyin ve materyal detaylarına ulaşın.
        </p>
      </div>
      <div class="hero-actions">
        <div class="search-input">
          <span class="search-icon">🔍</span>
          <input
            v-model="searchTerm"
            type="text"
            placeholder="Başlık, alt başlık veya ISBN ara..."
          />
          <button type="button" @click="triggerImmediateSearch">Ara</button>
        </div>
        <div class="hero-stats">
          <div class="stat-card">
            <span class="stat-label">Toplam kayıt</span>
            <strong>{{ totalCount.toLocaleString('tr-TR') }}</strong>
            <small>Son güncelleme: {{ formatDateTime(lastUpdated) }}</small>
          </div>
          <div class="stat-card">
            <span class="stat-label">Aktif filtre</span>
            <strong>{{ activeFilterLabel }}</strong>
            <small>{{ filteredSummary }}</small>
          </div>
          <div class="stat-card">
            <span class="stat-label">Sayfa</span>
            <strong>{{ page + 1 }} / {{ totalPages }}</strong>
            <small>{{ showingRange }}</small>
          </div>
        </div>
      </div>
    </section>

    <section class="filters-bar">
      <div class="filter-group">
        <span class="filter-label">Materyal türü</span>
        <div class="chip-group">
          <button
            v-for="option in materialTypeOptions"
            :key="option.value"
            type="button"
            :class="['chip', { active: selectedMaterialType === option.value }]"
            @click="selectMaterialType(option.value)"
          >
            <span class="chip-icon">{{ option.icon }}</span>
            {{ option.label }}
          </button>
        </div>
      </div>

      <div class="filter-group compact">
        <label class="filter-label" for="language-filter">Dil</label>
        <select
          id="language-filter"
          v-model="selectedLanguage"
        >
          <option value="all">Tüm diller</option>
          <option
            v-for="language in availableLanguages"
            :key="language"
            :value="language"
          >
            {{ language }}
          </option>
        </select>
      </div>

      <div class="filter-group compact year-range">
        <label class="filter-label">Yayın yılı</label>
        <div class="year-inputs">
          <input
            v-model="yearRange.from"
            type="number"
            min="1800"
            max="2100"
            placeholder="Başlangıç"
          />
          <span class="year-separator">—</span>
          <input
            v-model="yearRange.to"
            type="number"
            min="1800"
            max="2100"
            placeholder="Bitiş"
          />
        </div>
      </div>

      <div class="filter-group compact">
        <label class="filter-label" for="sort-option">Sırala</label>
        <select id="sort-option" v-model="sortOption">
          <option value="recent">En yeni kayıt</option>
          <option value="title">Başlığa göre A-Z</option>
          <option value="year-desc">Yayın yılı (Yeni)</option>
          <option value="year-asc">Yayın yılı (Eski)</option>
        </select>
      </div>

      <div class="filter-group compact clear-button">
        <button type="button" @click="resetFilters">Filtreleri sıfırla</button>
      </div>
    </section>

    <section class="results-section">
      <header class="results-header">
        <div class="results-meta">
          <h2>Arama Sonuçları</h2>
          <p>
            {{ totalCount === 0 ? 'Sonuç bulunamadı' : `${totalCount.toLocaleString('tr-TR')} kayıt bulundu` }}
          </p>
        </div>

        <div class="results-controls">
          <label>
            Sayfa boyutu
            <select v-model.number="pageSize" @change="handlePageSizeChange">
              <option v-for="size in pageSizeOptions" :key="size" :value="size">{{ size }}</option>
            </select>
          </label>
        </div>
      </header>

      <div v-if="loading" class="skeleton-grid">
        <div v-for="n in 6" :key="`skeleton-${n}`" class="skeleton-card">
          <div class="skeleton-line title"></div>
          <div class="skeleton-line"></div>
          <div class="skeleton-line short"></div>
          <div class="skeleton-line"></div>
        </div>
      </div>

      <div v-else-if="error" class="error-state">
        <div class="error-icon">⚠️</div>
        <h3>Veriler alınırken bir sorun oluştu</h3>
        <p>{{ error }}</p>
        <button type="button" @click="fetchCatalogRecords">Tekrar dene</button>
      </div>

      <div v-else>
        <div v-if="results.length === 0" class="empty-state">
          <div class="empty-icon">📭</div>
          <h3>Aradığınız kriterlere uygun kayıt bulunamadı</h3>
          <p>Farklı anahtar kelimeleri deneyebilir veya filtreleri sıfırlayabilirsiniz.</p>
          <button type="button" @click="resetFilters">Filtreleri sıfırla</button>
        </div>

        <div v-else class="results-grid">
          <article
            v-for="record in results"
            :key="record.id"
            class="record-card"
            @click="openRecord(record)"
          >
            <header>
              <div class="record-title">
                <h3>{{ record.baslik }}</h3>
                <span v-if="record.rdaUyumlu" class="badge badge--rda">RDA</span>
              </div>
              <p v-if="record.altBaslik" class="subtitle">{{ record.altBaslik }}</p>
            </header>

            <div class="record-metadata">
              <span class="meta-chip">
                {{ materialTypeLabel(record.materyalTuru) }}
              </span>
              <span v-if="record.yayinYili" class="meta-chip">Yayın: {{ record.yayinYili }}</span>
              <span v-if="record.dil" class="meta-chip">{{ record.dil }}</span>
              <span v-if="record.isbn" class="meta-chip">ISBN {{ record.isbn }}</span>
            </div>

            <p v-if="record.ozet" class="record-summary">
              {{ truncate(record.ozet, 190) }}
            </p>

            <footer>
              <div class="meta-info">
                <span v-if="record.yayinevi">
                  <strong>Yayınevi:</strong> {{ record.yayinevi }}
                </span>
                <span>
                  <strong>Kayıt:</strong> {{ formatDate(record.kayitTarihi) }}
                </span>
              </div>
              <button type="button" class="detail-button">
                Detayları gör →
              </button>
            </footer>
          </article>
        </div>
      </div>

      <footer v-if="totalPages > 0" class="pagination-bar">
        <div class="pagination-info">
          {{ showingRange }} arası görüntüleniyor
        </div>
        <div class="pagination-controls">
          <button type="button" :disabled="page === 0" @click="goToPage(page - 1)">Önceki</button>
          <div class="page-indicators">
            <button
              v-for="pageIndex in visiblePageButtons"
              :key="`page-${pageIndex}`"
              type="button"
              :class="['page-button', { active: pageIndex === page }]"
              @click="goToPage(pageIndex)"
            >
              {{ pageIndex + 1 }}
            </button>
          </div>
          <button type="button" :disabled="page >= totalPages - 1" @click="goToPage(page + 1)">Sonraki</button>
        </div>
      </footer>
    </section>

    <transition name="fade">
      <div v-if="selectedRecord" class="detail-overlay" @click.self="closeDetail">
        <aside class="detail-panel">
          <header>
            <h3>{{ selectedRecord.baslik }}</h3>
            <p v-if="selectedRecord.altBaslik">{{ selectedRecord.altBaslik }}</p>
            <div class="detail-tags">
              <span class="meta-chip">{{ materialTypeLabel(selectedRecord.materyalTuru) }}</span>
              <span v-if="selectedRecord.yayinYili" class="meta-chip">Yayın: {{ selectedRecord.yayinYili }}</span>
              <span v-if="selectedRecord.dil" class="meta-chip">{{ selectedRecord.dil }}</span>
            </div>
            <button type="button" class="close-button" @click="closeDetail">✕</button>
          </header>

          <section class="detail-body">
            <div v-if="selectedRecord.ozet" class="detail-block">
              <h4>Özet</h4>
              <p>{{ selectedRecord.ozet }}</p>
            </div>

            <div class="detail-grid">
              <div>
                <h5>Materyal Bilgisi</h5>
                <ul>
                  <li><strong>Materyal türü:</strong> {{ materialTypeLabel(selectedRecord.materyalTuru) }}</li>
                  <li v-if="selectedRecord.materyalAltTuru">
                    <strong>Alt tür:</strong> {{ selectedRecord.materyalAltTuru }}
                  </li>
                  <li v-if="selectedRecord.isbn"><strong>ISBN:</strong> {{ selectedRecord.isbn }}</li>
                  <li v-if="selectedRecord.sayfaSayisi">
                    <strong>Sayfa sayısı:</strong> {{ selectedRecord.sayfaSayisi }}
                  </li>
                  <li>
                    <strong>RDA uyumluluğu:</strong> {{ selectedRecord.rdaUyumlu ? 'Evet' : 'Hayır' }}
                  </li>
                </ul>
              </div>
              <div>
                <h5>Yayın Bilgisi</h5>
                <ul>
                  <li v-if="selectedRecord.yayinevi"><strong>Yayınevi:</strong> {{ selectedRecord.yayinevi }}</li>
                  <li v-if="selectedRecord.yayinYeri"><strong>Yayın yeri:</strong> {{ selectedRecord.yayinYeri }}</li>
                  <li v-if="selectedRecord.yayinYili"><strong>Yayın yılı:</strong> {{ selectedRecord.yayinYili }}</li>
                  <li v-if="selectedRecord.baski"><strong>Baskı:</strong> {{ selectedRecord.baski }}</li>
                </ul>
              </div>
            </div>

            <div v-if="selectedRecord.notlar" class="detail-block">
              <h4>Notlar</h4>
              <p>{{ selectedRecord.notlar }}</p>
            </div>
          </section>

          <footer class="detail-footer">
            <span>Kayıt tarihi: {{ formatDate(selectedRecord.kayitTarihi) }}</span>
            <button type="button" class="primary-action">
              Kaydı aç
            </button>
          </footer>
        </aside>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import { useDebounceFn } from '@vueuse/core'
import apiClient from '@/stores/api'
import { getErrorMessage } from '@/utils/error'

type MaterialFilter = 'all' | number
type SortOption = 'recent' | 'title' | 'year-desc' | 'year-asc'

interface CatalogRecord {
  id: string
  baslik: string
  altBaslik?: string | null
  isbn?: string | null
  yayinevi?: string | null
  yayinYeri?: string | null
  yayinYili?: number | null
  sayfaSayisi?: number | null
  dil?: string | null
  dizi?: string | null
  baski?: string | null
  ozet?: string | null
  notlar?: string | null
  kapakResmiYolu?: string | null
  materyalTuru: number
  materyalAltTuru?: string | null
  marc21Verisi?: string | null
  rdaUyumlu: boolean
  kayitTarihi: string
}

interface PagedResponse<T> {
  items: T[]
  index: number
  size: number
  count: number
  pages: number
  hasNext: boolean
  hasPrevious: boolean
}

const materialTypeLabels: Record<number, string> = {
  1: 'Kitap',
  2: 'Süreli Yayın',
  3: 'Görsel Materyal',
  4: 'Multimedya',
  5: 'E-Kitap',
  6: 'Harita',
  7: 'El Yazması',
  99: 'Diğer'
}

const materialTypeOptions = [
  { value: 'all' as MaterialFilter, label: 'Tüm materyaller', icon: '🔎' },
  { value: 1 as MaterialFilter, label: 'Kitap', icon: '📘' },
  { value: 2 as MaterialFilter, label: 'Süreli yayın', icon: '📰' },
  { value: 3 as MaterialFilter, label: 'Görsel materyal', icon: '🎞️' },
  { value: 4 as MaterialFilter, label: 'Multimedya', icon: '💿' },
  { value: 5 as MaterialFilter, label: 'E-Kitap', icon: '💻' },
  { value: 6 as MaterialFilter, label: 'Harita', icon: '🗺️' },
  { value: 7 as MaterialFilter, label: 'El yazması', icon: '✍️' },
  { value: 99 as MaterialFilter, label: 'Diğer', icon: '📦' }
]

const pageSizeOptions = [12, 24, 36] as const
const defaultPageSize = pageSizeOptions[0]

const searchTerm = ref('')
const selectedMaterialType = ref<MaterialFilter>('all')
const selectedLanguage = ref('all')
const yearRange = reactive<{ from: string; to: string }>({ from: '', to: '' })
const sortOption = ref<SortOption>('recent')

const page = ref(0)
const pageSize = ref<number>(defaultPageSize)
const totalPages = ref(0)
const totalCount = ref(0)

const results = ref<CatalogRecord[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const lastUpdated = ref<string | null>(null)
const selectedRecord = ref<CatalogRecord | null>(null)

const availableLanguages = computed(() => {
  const languages = new Set<string>()
  results.value.forEach(record => {
    if (record.dil) {
      languages.add(record.dil)
    }
  })
  return Array.from(languages).sort((a, b) => a.localeCompare(b, 'tr-TR'))
})

const activeFilterLabel = computed(() => {
  const parts: string[] = []

  const materialOption = materialTypeOptions.find(option => option.value === selectedMaterialType.value.valueOf())
  if (materialOption && materialOption.value !== 'all') {
    parts.push(materialOption.label)
  }

  if (selectedLanguage.value !== 'all') {
    parts.push(selectedLanguage.value)
  }

  if (yearRange.from || yearRange.to) {
    parts.push(`Yayın yılı ${yearRange.from || '??'}-${yearRange.to || '??'}`)
  }

  if (parts.length === 0) {
    return 'Filtre uygulanmadı'
  }

  return parts.join(' • ')
})

const filteredSummary = computed(() => {
  if (!searchTerm.value.trim()) {
    return 'Anahtar kelime girilmedi'
  }
  return `“${searchTerm.value.trim()}” ifadesi için sonuçlar`
})

const showingRange = computed(() => {
  if (results.value.length === 0 || totalCount.value === 0) {
    return '0'
  }
  const start = page.value * pageSize.value + 1
  const end = Math.min(start + results.value.length - 1, totalCount.value)
  return `${start.toLocaleString('tr-TR')} - ${end.toLocaleString('tr-TR')}`
})

const visiblePageButtons = computed(() => {
  const buttons: number[] = []
  const windowSize = 5
  const start = Math.max(0, Math.min(page.value - Math.floor(windowSize / 2), totalPages.value - windowSize))
  const end = Math.min(totalPages.value, start + windowSize)
  for (let i = start; i < end; i += 1) {
    buttons.push(i)
  }
  if (buttons.length === 0 && totalPages.value > 0) {
    buttons.push(0)
  }
  return buttons
})

const buildDynamicQuery = () => {
  const filters: any[] = []

  const trimmedSearch = searchTerm.value.trim()
  if (trimmedSearch.length > 0) {
    filters.push({
      Logic: 'or',
      Filters: [
        { Field: 'Baslik', Operator: 'Contains', Value: trimmedSearch },
        { Field: 'AltBaslik', Operator: 'Contains', Value: trimmedSearch },
        { Field: 'Isbn', Operator: 'Contains', Value: trimmedSearch }
      ]
    })
  }

  if (selectedMaterialType.value !== 'all') {
    filters.push({
      Field: 'MateryalTuru',
      Operator: 'Equal',
      Value: selectedMaterialType.value
    })
  }

  if (selectedLanguage.value !== 'all') {
    filters.push({
      Field: 'Dil',
      Operator: 'Equal',
      Value: selectedLanguage.value
    })
  }

  const fromYear = yearRange.from ? Number.parseInt(yearRange.from, 10) : NaN
  const toYear = yearRange.to ? Number.parseInt(yearRange.to, 10) : NaN

  if (!Number.isNaN(fromYear)) {
    filters.push({
      Field: 'YayinYili',
      Operator: 'GreaterThanOrEqual',
      Value: fromYear
    })
  }

  if (!Number.isNaN(toYear)) {
    filters.push({
      Field: 'YayinYili',
      Operator: 'LessThanOrEqual',
      Value: toYear
    })
  }

  let filter: any
  if (filters.length === 1) {
    filter = filters[0]
  } else if (filters.length > 1) {
    filter = {
      Logic: 'and',
      Filters: filters
    }
  }

  const sortDescriptors: Array<{ Field: string; Dir: 'asc' | 'desc' }> = []

  switch (sortOption.value) {
    case 'recent':
      sortDescriptors.push({ Field: 'KayitTarihi', Dir: 'desc' })
      break
    case 'title':
      sortDescriptors.push({ Field: 'Baslik', Dir: 'asc' })
      break
    case 'year-desc':
      sortDescriptors.push({ Field: 'YayinYili', Dir: 'desc' })
      break
    case 'year-asc':
      sortDescriptors.push({ Field: 'YayinYili', Dir: 'asc' })
      break
    default:
      break
  }

  const dynamicQuery: Record<string, any> = {}
  if (filter) {
    dynamicQuery.Filter = filter
  }
  if (sortDescriptors.length > 0) {
    dynamicQuery.Sort = sortDescriptors
  }

  return dynamicQuery
}

const fetchCatalogRecords = async () => {
  loading.value = true
  error.value = null

  try {
    const dynamicQuery = buildDynamicQuery()
    const response = await apiClient.post<PagedResponse<CatalogRecord>>(
      '/KatalogKayitlari/GetListByDynamic',
      dynamicQuery,
      {
        params: {
          'PageRequest.PageIndex': page.value,
          'PageRequest.PageSize': pageSize.value
        }
      }
    )

    const data = response.data
    results.value = data.items ?? []
    totalCount.value = data.count ?? results.value.length
    totalPages.value = data.pages ?? Math.ceil(totalCount.value / pageSize.value)
    lastUpdated.value = new Date().toISOString()

    if (page.value >= totalPages.value && totalPages.value > 0) {
      page.value = totalPages.value - 1
      await fetchCatalogRecords()
    }
  } catch (err) {
    error.value = getErrorMessage(err)
    results.value = []
    totalCount.value = 0
    totalPages.value = 0
  } finally {
    loading.value = false
  }
}

const debouncedFetch = useDebounceFn(() => {
  page.value = 0
  fetchCatalogRecords()
}, 350)

const selectMaterialType = (value: MaterialFilter) => {
  selectedMaterialType.value = value
}

const resetFilters = () => {
  searchTerm.value = ''
  selectedMaterialType.value = 'all'
  selectedLanguage.value = 'all'
  yearRange.from = ''
  yearRange.to = ''
  sortOption.value = 'recent'
  page.value = 0
  fetchCatalogRecords()
}

const triggerImmediateSearch = () => {
  page.value = 0
  fetchCatalogRecords()
}

const goToPage = (target: number) => {
  if (target < 0 || target >= totalPages.value || target === page.value) {
    return
  }
  page.value = target
  fetchCatalogRecords()
}

const handlePageSizeChange = () => {
  page.value = 0
  fetchCatalogRecords()
}

const openRecord = (record: CatalogRecord) => {
  selectedRecord.value = record
}

const closeDetail = () => {
  selectedRecord.value = null
}

const materialTypeLabel = (type: number) => materialTypeLabels[type] ?? 'Belirtilmemiş'

const truncate = (value: string | null | undefined, max = 180) => {
  if (!value) return ''
  return value.length > max ? `${value.substring(0, max)}…` : value
}

const formatDate = (value: string | null | undefined) => {
  if (!value) return '-'
  return new Date(value).toLocaleDateString('tr-TR', {
    day: '2-digit',
    month: 'short',
    year: 'numeric'
  })
}

const formatDateTime = (value: string | null) => {
  if (!value) return ' - '
  return new Date(value).toLocaleString('tr-TR', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

watch(selectedMaterialType, () => debouncedFetch())
watch(selectedLanguage, () => debouncedFetch())
watch(() => yearRange.from, () => debouncedFetch())
watch(() => yearRange.to, () => debouncedFetch())
watch(sortOption, () => debouncedFetch())
watch(searchTerm, () => debouncedFetch())

onMounted(() => {
  fetchCatalogRecords()
})
</script>

<style scoped>
.catalog-search-page {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.hero-card {
  background: linear-gradient(135deg, rgba(254, 226, 226, 0.95), rgba(248, 113, 113, 0.15));
  border-radius: 28px;
  padding: 2.4rem;
  position: relative;
  overflow: hidden;
  border: 1px solid rgba(248, 113, 113, 0.3);
  box-shadow:
    0 18px 40px rgba(248, 113, 113, 0.12),
    inset 0 0 0 1px rgba(255, 255, 255, 0.35);
}

.hero-card::after {
  content: '';
  position: absolute;
  top: -40%;
  right: -30%;
  width: 60%;
  height: 160%;
  background: radial-gradient(circle, rgba(248, 113, 113, 0.25) 0%, rgba(239, 68, 68, 0) 70%);
  transform: rotate(-12deg);
}

.hero-content {
  position: relative;
  z-index: 1;
  max-width: 560px;
}

.hero-content h1 {
  font-size: 2.4rem;
  margin: 0;
  font-weight: 700;
  color: #7f1d1d;
  letter-spacing: -0.02em;
}

.hero-content p {
  margin-top: 0.75rem;
  font-size: 1.05rem;
  color: rgba(127, 29, 29, 0.8);
}

.hero-actions {
  margin-top: 2rem;
  position: relative;
  z-index: 1;
  display: flex;
  flex-direction: column;
  gap: 1.75rem;
}

.search-input {
  display: flex;
  align-items: center;
  background: #fff;
  border-radius: 18px;
  padding: 0.35rem;
  border: 1px solid rgba(248, 113, 113, 0.35);
  box-shadow: 0 12px 25px rgba(248, 113, 113, 0.15);
}

.search-icon {
  font-size: 1.35rem;
  margin: 0 0.9rem;
  color: rgba(127, 29, 29, 0.6);
}

.search-input input {
  flex: 1;
  border: none;
  outline: none;
  font-size: 1.05rem;
  padding: 0.85rem 0.5rem;
  background: transparent;
  color: #7f1d1d;
}

.search-input button {
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: #fff;
  border: none;
  padding: 0.85rem 1.5rem;
  border-radius: 14px;
  font-weight: 600;
  cursor: pointer;
  box-shadow: 0 18px 30px rgba(248, 113, 113, 0.25);
}

.hero-stats {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 1rem;
}

.stat-card {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 18px;
  padding: 1.1rem 1.25rem;
  border: 1px solid rgba(248, 113, 113, 0.25);
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  color: #7f1d1d;
}

.stat-label {
  font-size: 0.85rem;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  color: rgba(127, 29, 29, 0.7);
}

.stat-card strong {
  font-size: 1.8rem;
  font-weight: 700;
}

.stat-card small {
  font-size: 0.85rem;
  color: rgba(127, 29, 29, 0.6);
}

.filters-bar {
  background: #fff;
  border-radius: 18px;
  padding: 1.5rem 2rem;
  display: grid;
  grid-template-columns: 1fr repeat(4, auto);
  gap: 1.5rem;
  align-items: center;
  border: 1px solid rgba(203, 213, 225, 0.65);
  box-shadow: 0 10px 30px rgba(15, 23, 42, 0.05);
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.filter-group.compact {
  min-width: 160px;
}

.filter-label {
  font-size: 0.85rem;
  font-weight: 600;
  color: rgba(15, 23, 42, 0.65);
  text-transform: uppercase;
  letter-spacing: 0.08em;
}

.chip-group {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
}

.chip {
  display: inline-flex;
  align-items: center;
  gap: 0.45rem;
  border-radius: 999px;
  border: 1px solid rgba(203, 213, 225, 0.8);
  padding: 0.45rem 0.9rem;
  background: #fff;
  color: #0f172a;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.chip-icon {
  font-size: 1.1rem;
}

.chip:hover {
  border-color: rgba(248, 113, 113, 0.6);
  color: #b91c1c;
}

.chip.active {
  background: rgba(248, 113, 113, 0.12);
  border-color: rgba(248, 113, 113, 0.7);
  color: #b91c1c;
  box-shadow: inset 0 0 0 1px rgba(248, 113, 113, 0.2);
}

.filter-group select,
.filter-group input {
  border-radius: 12px;
  border: 1px solid rgba(203, 213, 225, 0.8);
  padding: 0.65rem 0.75rem;
  min-width: 140px;
  background: #fff;
  color: #0f172a;
}

.year-range {
  min-width: 220px;
}

.year-inputs {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.year-inputs input {
  width: 100px;
  text-align: center;
}

.year-separator {
  color: rgba(107, 114, 128, 0.8);
  font-weight: 600;
}

.clear-button button {
  align-self: flex-end;
  border: none;
  background: rgba(248, 113, 113, 0.1);
  color: #b91c1c;
  font-weight: 600;
  padding: 0.6rem 0.9rem;
  border-radius: 12px;
  cursor: pointer;
}

.results-section {
  background: #fff;
  border-radius: 18px;
  padding: 2rem;
  border: 1px solid rgba(226, 232, 240, 0.8);
  box-shadow: 0 20px 50px rgba(15, 23, 42, 0.05);
}

.results-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1.25rem;
  margin-bottom: 1.75rem;
}

.results-header h2 {
  margin: 0;
  font-size: 1.45rem;
  color: #0f172a;
}

.results-header p {
  margin: 0.25rem 0 0;
  color: rgba(15, 23, 42, 0.68);
}

.results-controls label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.9rem;
  color: rgba(15, 23, 42, 0.75);
}

.results-controls select {
  border-radius: 10px;
  border: 1px solid rgba(203, 213, 225, 0.9);
  padding: 0.45rem 0.65rem;
}

.skeleton-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 1.2rem;
}

.skeleton-card {
  background: #f8fafc;
  padding: 1.4rem;
  border-radius: 16px;
  border: 1px solid rgba(226, 232, 240, 0.6);
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.skeleton-line {
  height: 14px;
  background: linear-gradient(90deg, #f8fafc 0px, #e2e8f0 40px, #f8fafc 80px);
  background-size: 600px;
  animation: shimmer 1.6s infinite linear;
  border-radius: 8px;
}

.skeleton-line.title {
  width: 80%;
  height: 18px;
}

.skeleton-line.short {
  width: 55%;
}

@keyframes shimmer {
  0% {
    background-position: -120px;
  }
  100% {
    background-position: 200px;
  }
}

.error-state,
.empty-state {
  text-align: center;
  padding: 3rem 2rem;
  border-radius: 18px;
  background: rgba(248, 113, 113, 0.06);
  border: 1px dashed rgba(248, 113, 113, 0.35);
  color: #7f1d1d;
}

.error-icon,
.empty-icon {
  font-size: 2.75rem;
  margin-bottom: 1rem;
}

.error-state button,
.empty-state button {
  margin-top: 1.5rem;
  border: none;
  padding: 0.75rem 1.5rem;
  background: #dc2626;
  color: #fff;
  border-radius: 12px;
  cursor: pointer;
}

.results-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.4rem;
}

.record-card {
  padding: 1.6rem;
  border-radius: 18px;
  border: 1px solid rgba(226, 232, 240, 0.9);
  background: linear-gradient(180deg, #ffffff 0%, #fff7f7 100%);
  display: flex;
  flex-direction: column;
  gap: 1rem;
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
  min-height: 220px;
}

.record-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 16px 36px rgba(248, 113, 113, 0.18);
  border-color: rgba(248, 113, 113, 0.45);
}

.record-title {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  justify-content: space-between;
}

.record-card h3 {
  margin: 0;
  font-size: 1.2rem;
  color: #7f1d1d;
  flex: 1;
}

.subtitle {
  margin: 0.25rem 0 0;
  color: rgba(127, 29, 29, 0.7);
}

.badge {
  display: inline-flex;
  align-items: center;
  padding: 0.25rem 0.5rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
}

.badge--rda {
  background: rgba(248, 250, 252, 0.9);
  color: #dc2626;
  border: 1px solid rgba(248, 113, 113, 0.5);
}

.record-metadata {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.meta-chip {
  font-size: 0.78rem;
  background: rgba(248, 113, 113, 0.12);
  color: #b91c1c;
  padding: 0.35rem 0.6rem;
  border-radius: 999px;
  border: 1px solid rgba(248, 113, 113, 0.2);
}

.record-summary {
  margin: 0;
  color: rgba(15, 23, 42, 0.7);
  line-height: 1.5;
  font-size: 0.95rem;
}

.record-card footer {
  margin-top: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.meta-info {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  color: rgba(51, 65, 85, 0.85);
  font-size: 0.85rem;
}

.detail-button {
  border: none;
  background: transparent;
  color: #dc2626;
  font-weight: 600;
  cursor: pointer;
}

.pagination-bar {
  margin-top: 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
  color: rgba(15, 23, 42, 0.7);
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}

.pagination-controls button {
  border: none;
  background: rgba(248, 113, 113, 0.12);
  color: #b91c1c;
  padding: 0.55rem 0.95rem;
  border-radius: 999px;
  font-weight: 600;
  cursor: pointer;
}

.pagination-controls button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.page-indicators {
  display: flex;
  gap: 0.4rem;
}

.page-button {
  border: 1px solid rgba(248, 113, 113, 0.35);
  background: #fff;
  color: #b91c1c;
  padding: 0.45rem 0.75rem;
  border-radius: 12px;
  cursor: pointer;
}

.page-button.active {
  background: #dc2626;
  color: #fff;
  border-color: transparent;
}

.detail-overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.45);
  backdrop-filter: blur(6px);
  display: flex;
  justify-content: flex-end;
  z-index: 60;
}

.detail-panel {
  background: #fff;
  width: min(400px, 92vw);
  height: 100%;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  overflow-y: auto;
  position: relative;
}

.detail-panel header h3 {
  margin: 0;
  font-size: 1.55rem;
  color: #7f1d1d;
}

.detail-panel header p {
  margin: 0.45rem 0 0;
  color: rgba(127, 29, 29, 0.7);
}

.detail-tags {
  margin-top: 0.85rem;
  display: flex;
  flex-wrap: wrap;
  gap: 0.45rem;
}

.close-button {
  position: absolute;
  top: 1.25rem;
  right: 1.25rem;
  border: none;
  background: rgba(248, 113, 113, 0.12);
  color: #b91c1c;
  width: 34px;
  height: 34px;
  border-radius: 50%;
  font-size: 1rem;
  cursor: pointer;
}

.detail-body {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.detail-block h4 {
  margin: 0 0 0.45rem;
  color: #0f172a;
  font-size: 1rem;
}

.detail-block p {
  margin: 0;
  color: rgba(51, 65, 85, 0.85);
  line-height: 1.6;
}

.detail-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
  gap: 1rem;
}

.detail-grid h5 {
  margin: 0 0 0.35rem;
  color: #0f172a;
}

.detail-grid ul {
  margin: 0;
  padding-left: 1rem;
  color: rgba(51, 65, 85, 0.85);
  line-height: 1.5;
  font-size: 0.92rem;
}

.detail-footer {
  margin-top: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.85rem;
  color: rgba(107, 114, 128, 0.85);
}

.primary-action {
  border: none;
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: #fff;
  padding: 0.65rem 1.25rem;
  border-radius: 12px;
  cursor: pointer;
  font-weight: 600;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.24s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@media (max-width: 1200px) {
  .filters-bar {
    grid-template-columns: 1fr 1fr;
  }
}

@media (max-width: 768px) {
  .filters-bar {
    grid-template-columns: 1fr;
  }

  .hero-card {
    padding: 1.8rem;
  }

  .hero-actions {
    gap: 1.2rem;
  }

  .hero-stats {
    grid-template-columns: 1fr;
  }

  .results-header {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>
