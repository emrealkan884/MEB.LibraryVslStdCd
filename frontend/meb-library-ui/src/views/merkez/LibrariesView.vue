<template>
  <div class="libraries-page">
    <section class="hero">
      <div class="hero__texts">
        <h1>Kütüphane Yönetimi</h1>
        <p>Merkez ve okul kütüphanelerini, bölümlerini ve raf yapılarını yönetin.</p>
        <div class="hero__meta">
          <div>
            <span class="meta-label">Toplam Kütüphane</span>
            <span class="meta-value">{{ totalCount }}</span>
          </div>
          <div>
            <span class="meta-label">Bölüm</span>
            <span class="meta-value">{{ sectionsCount }}</span>
          </div>
          <div>
            <span class="meta-label">Raf</span>
            <span class="meta-value">{{ shelvesCount }}</span>
          </div>
        </div>
      </div>
      <div class="hero__actions">
        <button type="button" class="btn btn--primary" @click="openLibraryModal('create')">
          <span class="btn__icon">+</span>
          Yeni kütüphane
        </button>
      </div>
    </section>

    <transition name="alert">
      <div
        v-if="feedback.message"
        class="alert"
        :class="feedback.kind === 'error' ? 'alert--error' : 'alert--success'"
      >
        <strong>{{ feedback.kind === 'error' ? 'İşlem başarısız' : 'İşlem tamamlandı' }}</strong>
        <span>{{ feedback.message }}</span>
      </div>
    </transition>

    <section class="panel">
      <header class="panel__header">
        <div>
          <h2>Kütüphane listesi</h2>
          <p>Arama ve filtreler ile kütüphane kayıtlarını yönetin.</p>
        </div>
      </header>

      <div class="filters">
        <label>
          <span>Ara</span>
          <input
            type="text"
            v-model.trim="libraryFilters.search"
            placeholder="Kod, ad veya adres"
            :disabled="librariesLoading"
          />
        </label>
        <label>
          <span>Tür</span>
          <select v-model="libraryFilters.type" :disabled="librariesLoading">
            <option value="all">Tümü</option>
            <option value="Merkez">Merkez Kütüphane</option>
            <option value="Okul">Okul Kütüphanesi</option>
          </select>
        </label>
        <label>
          <span>Durum</span>
          <select v-model="libraryFilters.status" :disabled="librariesLoading">
            <option value="all">Tümü</option>
            <option value="active">Aktif</option>
            <option value="inactive">Pasif</option>
          </select>
        </label>
        <div class="filters__actions">
          <button type="button" class="btn btn--ghost" @click="resetLibraryFilters" :disabled="librariesLoading">
            Temizle
          </button>
          <button type="button" class="btn btn--primary" @click="applyLibraryFilters" :disabled="librariesLoading">
            Filtreleri uygula
          </button>
        </div>
      </div>

      <div class="table-wrapper" :class="{ 'table-wrapper--loading': librariesLoading }">
        <table class="data-table">
          <thead>
            <tr>
              <th>Kütüphane</th>
              <th>Tür</th>
              <th>Durum</th>
              <th>İletişim</th>
              <th class="table-actions">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="librariesLoading">
              <td colspan="5" class="table-state">Kütüphaneler yükleniyor...</td>
            </tr>
            <tr v-else-if="libraries.length === 0">
              <td colspan="5" class="table-state">Kayıt bulunamadı.</td>
            </tr>
            <tr v-for="library in libraries" :key="library.id" :class="{ selected: library.id === selectedLibraryId }" class="table-row" @click="selectLibrary(library.id)">
              <td>
                <div class="entity">
                  <span class="entity__title">{{ library.ad }}</span>
                  <span class="entity__meta">{{ library.kod }}</span>
                </div>
              </td>
              <td>{{ getLibraryTypeLabel(library.tip) }}</td>
              <td>
                <span class="badge" :class="library.aktif ? 'badge--success' : 'badge--muted'">
                  {{ library.aktif ? 'Aktif' : 'Pasif' }}
                </span>
              </td>
              <td class="contact-cell">
                <span v-if="library.telefon">{{ library.telefon }}</span>
                <span v-else class="contact-empty">Telefon yok</span>
                <span v-if="library.ePosta">{{ library.ePosta }}</span>
              </td>
              <td class="table-actions">
                <button
                  type="button"
                  class="btn btn--ghost btn--sm"
                  @click.stop="selectLibrary(library.id)"
                  :disabled="librariesLoading"
                >
                  Detay
                </button>
                <button type="button" class="btn btn--ghost btn--sm" @click.stop="openLibraryModal('edit', library)">
                  Düzenle
                </button>
                <button type="button" class="btn btn--danger btn--sm" @click.stop="deleteLibrary(library)">
                  Sil
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <footer class="table-footer" v-if="totalPages > 1 && !librariesLoading">
        <span>Sayfa {{ page + 1 }} / {{ totalPages }} · {{ totalCount }} kayıt</span>
        <div class="table-footer__actions">
          <button type="button" class="btn btn--ghost btn--sm" @click="prevPage" :disabled="page === 0">Önceki</button>
          <button type="button" class="btn btn--ghost btn--sm" @click="nextPage" :disabled="page + 1 >= totalPages">
            Sonraki
          </button>
        </div>
      </footer>
    </section>

    <section v-if="selectedLibrary" class="panel">
      <header class="detail-header">
        <div>
          <h3>{{ selectedLibrary.ad }}</h3>
          <p>{{ selectedLibrary.kod }} · {{ getLibraryTypeLabel(selectedLibrary.tip) }}</p>
        </div>
        <div class="detail-actions">
          <button type="button" class="btn btn--ghost btn--sm" @click="openLibraryModal('edit', selectedLibrary)">
            Kütüphaneyi düzenle
          </button>
          <button type="button" class="btn btn--danger btn--sm" @click="deleteLibrary(selectedLibrary)">
            Kütüphaneyi sil
          </button>
        </div>
      </header>

      <div class="detail-meta">
        <div>
          <span>Adres</span>
          <p>{{ selectedLibrary.adres }}</p>
        </div>
        <div>
          <span>Telefon</span>
          <p>{{ selectedLibrary.telefon || 'Tanımlı değil' }}</p>
        </div>
        <div>
          <span>E-posta</span>
          <p>{{ selectedLibrary.ePosta || 'Tanımlı değil' }}</p>
        </div>
      </div>

      <div class="detail-grid">
        <article class="detail-card">
          <header>
            <div>
              <h4>Bölümler</h4>
              <p>{{ sections.length }} kayıt</p>
            </div>
            <button type="button" class="btn btn--ghost btn--sm" @click="openSectionModal('create')">
              Bölüm ekle
            </button>
          </header>
          <div v-if="sectionsLoading" class="card-state">Bölümler yükleniyor...</div>
          <ul v-else class="section-list">
            <li
              v-for="section in sections"
              :key="section.id"
              :class="{ active: section.id === selectedSectionId }"
            >
              <button type="button" class="section__select" @click="selectSection(section.id)">
                <strong>{{ section.ad }}</strong>
                <small>{{ section.aciklama || 'Açıklama yok' }}</small>
              </button>
              <div class="section__actions">
                <button type="button" @click.stop="openSectionModal('edit', section)">Düzenle</button>
                <button type="button" @click.stop="deleteSection(section)">Sil</button>
              </div>
            </li>
            <li v-if="sections.length === 0" class="section-empty">
              Bu kütüphaneye ait bölüm bulunamadı.
            </li>
          </ul>
        </article>

        <article class="detail-card">
          <header>
            <div>
              <h4>Raflar</h4>
              <p v-if="selectedSection">Seçili bölüm: {{ selectedSection.ad }}</p>
              <p v-else>Önce bir bölüm seçin</p>
            </div>
            <button
              type="button"
              class="btn btn--ghost btn--sm"
              @click="openShelfModal('create')"
              :disabled="!selectedSection"
            >
              Raf ekle
            </button>
          </header>
          <div v-if="shelvesLoading" class="card-state">Raflar yükleniyor...</div>
          <div v-else-if="!selectedSection" class="card-state card-state--muted">
            Rafları görmek için bir bölüm seçin.
          </div>
          <table v-else class="detail-table">
            <thead>
              <tr>
                <th>Kod</th>
                <th>Açıklama</th>
                <th class="table-actions">İşlemler</th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="shelves.length === 0">
                <td colspan="3" class="table-state">Seçili bölüm için raf bulunamadı.</td>
              </tr>
              <tr v-for="shelf in shelves" :key="shelf.id">
                <td>{{ shelf.kod }}</td>
                <td>{{ shelf.aciklama || 'Açıklama yok' }}</td>
                <td class="table-actions">
                  <button type="button" class="btn btn--ghost btn--sm" @click="openShelfModal('edit', shelf)">
                    Düzenle
                  </button>
                  <button type="button" class="btn btn--danger btn--sm" @click="deleteShelf(shelf)">
                    Sil
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </article>
      </div>
    </section>

    <transition name="modal">
      <div v-if="libraryModal.visible" class="modal-backdrop">
        <div class="modal" role="dialog" aria-modal="true">
          <header class="modal__header">
            <h3>{{ libraryModal.mode === 'create' ? 'Yeni kütüphane' : 'Kütüphaneyi düzenle' }}</h3>
            <button type="button" class="modal__close" @click="closeLibraryModal" :disabled="libraryModal.saving">
              ×
            </button>
          </header>
          <form class="modal__body" @submit.prevent="submitLibrary">
            <div class="form-grid">
              <label>
                <span>Kod *</span>
                <input type="text" v-model.trim="libraryModal.kod" :disabled="libraryModal.saving" required />
              </label>
              <label>
                <span>Ad *</span>
                <input type="text" v-model.trim="libraryModal.ad" :disabled="libraryModal.saving" required />
              </label>
              <label>
                <span>Tür *</span>
                <select v-model="libraryModal.tip" :disabled="libraryModal.saving">
                  <option value="Merkez">Merkez</option>
                  <option value="Okul">Okul</option>
                </select>
              </label>
              <label>
                <span>Durum</span>
                <select v-model="libraryModal.aktif" :disabled="libraryModal.saving">
                  <option :value="true">Aktif</option>
                  <option :value="false">Pasif</option>
                </select>
              </label>
              <label class="form-grid__full">
                <span>Adres *</span>
                <textarea
                  v-model.trim="libraryModal.adres"
                  rows="2"
                  :disabled="libraryModal.saving"
                  required
                ></textarea>
              </label>
              <label>
                <span>Telefon</span>
                <input type="text" v-model.trim="libraryModal.telefon" :disabled="libraryModal.saving" />
              </label>
              <label>
                <span>E-posta</span>
                <input type="email" v-model.trim="libraryModal.ePosta" :disabled="libraryModal.saving" />
              </label>
            </div>
            <p v-if="libraryModal.error" class="modal__error">{{ libraryModal.error }}</p>
            <div class="modal__actions">
              <button type="button" class="btn btn--ghost" @click="closeLibraryModal" :disabled="libraryModal.saving">
                Vazgeç
              </button>
              <button type="submit" class="btn btn--primary" :disabled="libraryModal.saving">
                {{ libraryModal.saving ? 'Kaydediliyor...' : libraryModal.mode === 'create' ? 'Kütüphane oluştur' : 'Kaydet' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </transition>

    <transition name="modal">
      <div v-if="sectionModal.visible" class="modal-backdrop">
        <div class="modal" role="dialog" aria-modal="true">
          <header class="modal__header">
            <h3>{{ sectionModal.mode === 'create' ? 'Bölüm ekle' : 'Bölümü düzenle' }}</h3>
            <button type="button" class="modal__close" @click="closeSectionModal" :disabled="sectionModal.saving">
              ×
            </button>
          </header>
          <form class="modal__body" @submit.prevent="submitSection">
            <div class="form-grid">
              <label class="form-grid__full">
                <span>Ad *</span>
                <input type="text" v-model.trim="sectionModal.ad" :disabled="sectionModal.saving" required />
              </label>
              <label class="form-grid__full">
                <span>Açıklama</span>
                <textarea v-model.trim="sectionModal.aciklama" rows="2" :disabled="sectionModal.saving"></textarea>
              </label>
            </div>
            <p v-if="sectionModal.error" class="modal__error">{{ sectionModal.error }}</p>
            <div class="modal__actions">
              <button type="button" class="btn btn--ghost" @click="closeSectionModal" :disabled="sectionModal.saving">
                Vazgeç
              </button>
              <button type="submit" class="btn btn--primary" :disabled="sectionModal.saving">
                {{ sectionModal.saving ? 'Kaydediliyor...' : sectionModal.mode === 'create' ? 'Bölüm oluştur' : 'Kaydet' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </transition>

    <transition name="modal">
      <div v-if="shelfModal.visible" class="modal-backdrop">
        <div class="modal" role="dialog" aria-modal="true">
          <header class="modal__header">
            <h3>{{ shelfModal.mode === 'create' ? 'Raf ekle' : 'Rafı düzenle' }}</h3>
            <button type="button" class="modal__close" @click="closeShelfModal" :disabled="shelfModal.saving">
              ×
            </button>
          </header>
          <form class="modal__body" @submit.prevent="submitShelf">
            <div class="form-grid">
              <label class="form-grid__full">
                <span>Kod *</span>
                <input type="text" v-model.trim="shelfModal.kod" :disabled="shelfModal.saving" required />
              </label>
              <label class="form-grid__full">
                <span>Açıklama</span>
                <textarea v-model.trim="shelfModal.aciklama" rows="2" :disabled="shelfModal.saving"></textarea>
              </label>
            </div>
            <p v-if="shelfModal.error" class="modal__error">{{ shelfModal.error }}</p>
            <div class="modal__actions">
              <button type="button" class="btn btn--ghost" @click="closeShelfModal" :disabled="shelfModal.saving">
                Vazgeç
              </button>
              <button type="submit" class="btn btn--primary" :disabled="shelfModal.saving">
                {{ shelfModal.saving ? 'Kaydediliyor...' : shelfModal.mode === 'create' ? 'Raf oluştur' : 'Kaydet' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue'
import type { AxiosError } from 'axios'
import apiClient from '@/stores/api'

interface PagedResponse<T> {
  items: T[]
  index: number
  size: number
  count: number
  pages: number
  hasNext: boolean
  hasPrevious: boolean
}

interface Library {
  id: string
  kod: string
  ad: string
  tip: number | string
  adres: string
  telefon?: string | null
  ePosta?: string | null
  aktif: boolean
}

interface LibrarySection {
  id: string
  kutuphaneId: string
  ad: string
  aciklama?: string | null
}

interface Shelf {
  id: string
  kutuphaneBolumuId: string
  kod: string
  aciklama?: string | null
}

type FeedbackKind = 'success' | 'error' | ''
type LibraryTypeFilter = 'all' | 'Merkez' | 'Okul'
type LibraryStatusFilter = 'all' | 'active' | 'inactive'
type DynamicFilter = Record<string, unknown>

interface LibraryFilterState {
  search: string
  type: LibraryTypeFilter
  status: LibraryStatusFilter
}

const page = ref(0)
const pageSize = 15
const totalPages = ref(0)
const totalCount = ref(0)

const librariesLoading = ref(false)
const libraries = ref<Library[]>([])

const libraryFilters = reactive<LibraryFilterState>({
  search: '',
  type: 'all',
  status: 'all'
})

const appliedLibraryFilters = ref<LibraryFilterState>({
  search: '',
  type: 'all',
  status: 'all'
})

const feedback = reactive<{ message: string; kind: FeedbackKind }>({
  message: '',
  kind: ''
})

const selectedLibraryId = ref<string | null>(null)
const sections = ref<LibrarySection[]>([])
const sectionsLoading = ref(false)
const sectionsCount = ref(0)
const selectedSectionId = ref<string | null>(null)

const shelves = ref<Shelf[]>([])
const shelvesLoading = ref(false)
const shelvesCount = ref(0)

const libraryModal = reactive({
  visible: false,
  mode: 'create' as 'create' | 'edit',
  saving: false,
  error: '',
  id: '',
  kod: '',
  ad: '',
  tip: 'Merkez' as 'Merkez' | 'Okul',
  adres: '',
  telefon: '',
  ePosta: '',
  aktif: true
})

const sectionModal = reactive({
  visible: false,
  mode: 'create' as 'create' | 'edit',
  saving: false,
  error: '',
  id: '',
  ad: '',
  aciklama: ''
})

const shelfModal = reactive({
  visible: false,
  mode: 'create' as 'create' | 'edit',
  saving: false,
  error: '',
  id: '',
  kod: '',
  aciklama: ''
})

const selectedLibrary = computed(() => libraries.value.find(item => item.id === selectedLibraryId.value) ?? null)
const selectedSection = computed(() => sections.value.find(item => item.id === selectedSectionId.value) ?? null)

const showFeedback = (message: string, kind: 'success' | 'error') => {
  feedback.message = message
  feedback.kind = kind
  window.setTimeout(() => {
    feedback.message = ''
    feedback.kind = ''
  }, 3500)
}

const getErrorMessage = (err: unknown): string => {
  if (typeof err === 'string') {
    return err
  }
  const axiosError = err as AxiosError<{ message?: string }>
  return axiosError?.response?.data?.message || axiosError?.message || 'Islem sirasinda bir hata olustu.'
}

const normalizeLibraryType = (value: Library['tip']): 'Merkez' | 'Okul' => {
  if (typeof value === 'number') {
    return value === 2 ? 'Okul' : 'Merkez'
  }
  if (value === 'Okul' || value === 'Merkez') {
    return value
  }
  const lowered = value?.toString().toLowerCase()
  if (lowered === 'okul') return 'Okul'
  return 'Merkez'
}

const libraryTypeToValue = (value: 'Merkez' | 'Okul'): number => (value === 'Okul' ? 2 : 1)

const parseLibraryTypeFilter = (filter: LibraryTypeFilter): number | null => {
  if (filter === 'all') {
    return null
  }
  return libraryTypeToValue(filter)
}

const getLibraryTypeLabel = (value: Library['tip']): string => {
  const normalized = normalizeLibraryType(value)
  return normalized === 'Okul' ? 'Okul Kutuphane' : 'Merkez Kutuphane'
}

const buildLibraryDynamicQuery = (): Record<string, unknown> => {
  const filtersStack: DynamicFilter[] = []

  const typeValue = parseLibraryTypeFilter(appliedLibraryFilters.value.type)
  if (typeValue !== null) {
    filtersStack.push({ Field: 'Tip', Operator: 'eq', Value: typeValue })
  }

  if (appliedLibraryFilters.value.status === 'active') {
    filtersStack.push({ Field: 'Aktif', Operator: 'eq', Value: true })
  } else if (appliedLibraryFilters.value.status === 'inactive') {
    filtersStack.push({ Field: 'Aktif', Operator: 'eq', Value: false })
  }

  if (appliedLibraryFilters.value.search) {
    filtersStack.push({
      Logic: 'or',
      Filters: [
        { Field: 'Kod', Operator: 'contains', Value: appliedLibraryFilters.value.search },
        { Field: 'Ad', Operator: 'contains', Value: appliedLibraryFilters.value.search },
        { Field: 'Adres', Operator: 'contains', Value: appliedLibraryFilters.value.search }
      ]
    })
  }

  let filterBody: DynamicFilter | null = null
  if (filtersStack.length === 1) {
    filterBody = filtersStack[0]
  } else if (filtersStack.length > 1) {
    filterBody = { Logic: 'and', Filters: filtersStack }
  }

  const query: Record<string, unknown> = {
    Sort: [
      { Field: 'Ad', Dir: 'asc' },
      { Field: 'Kod', Dir: 'asc' }
    ]
  }

  if (filterBody) {
    query.Filter = filterBody
  }

  return query
}

const loadLibraries = async () => {
  librariesLoading.value = true
  try {
    const query = buildLibraryDynamicQuery()
    const response = await apiClient.post<PagedResponse<Library>>('/Kutuphaneler/GetListByDynamic', query, {
      params: {
        'PageRequest.PageIndex': page.value,
        'PageRequest.PageSize': pageSize
      }
    })

    const data = response.data
    libraries.value = data?.items ?? []
    totalCount.value = data?.count ?? libraries.value.length

    const pagesFromServer = data?.pages ?? 0
    totalPages.value =
      pagesFromServer > 0 ? pagesFromServer : totalCount.value > 0 ? Math.ceil(totalCount.value / pageSize) : 0

    if (libraries.value.length === 0) {
      selectedLibraryId.value = null
      sections.value = []
      sectionsCount.value = 0
      shelves.value = []
      shelvesCount.value = 0
      selectedSectionId.value = null
      return
    }

    if (page.value >= totalPages.value && totalPages.value > 0) {
      page.value = Math.max(0, totalPages.value - 1)
      await loadLibraries()
      return
    }

    if (!selectedLibraryId.value || !libraries.value.some(item => item.id === selectedLibraryId.value)) {
      selectedLibraryId.value = libraries.value[0]?.id ?? null
    }
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
    libraries.value = []
    totalCount.value = 0
    totalPages.value = 0
    selectedLibraryId.value = null
    sections.value = []
    sectionsCount.value = 0
    shelves.value = []
    shelvesCount.value = 0
    selectedSectionId.value = null
  } finally {
    librariesLoading.value = false
  }
}

const loadSections = async (libraryId: string) => {
  sectionsLoading.value = true
  const previousSection = selectedSectionId.value
  selectedSectionId.value = null
  sections.value = []
  sectionsCount.value = 0
  shelves.value = []
  shelvesCount.value = 0

  try {
    const response = await apiClient.post<PagedResponse<LibrarySection>>(
      '/KutuphaneBolumleri/GetListByDynamic',
      {
        Filter: { Field: 'KutuphaneId', Operator: 'eq', Value: libraryId },
        Sort: [{ Field: 'Ad', Dir: 'asc' }]
      },
      {
        params: {
          'PageRequest.PageIndex': 0,
          'PageRequest.PageSize': 200
        }
      }
    )

    const data = response.data
    sections.value = data?.items ?? []
    sectionsCount.value = data?.count ?? sections.value.length

    if (sections.value.length === 0) {
      selectedSectionId.value = null
      return
    }

    const existing = previousSection ? sections.value.find(item => item.id === previousSection) : null
    selectedSectionId.value = existing?.id ?? sections.value[0].id
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
    sections.value = []
    sectionsCount.value = 0
    selectedSectionId.value = null
  } finally {
    sectionsLoading.value = false
  }
}

const loadShelves = async (sectionId: string) => {
  shelvesLoading.value = true
  shelves.value = []
  shelvesCount.value = 0

  try {
    const response = await apiClient.post<PagedResponse<Shelf>>(
      '/Raflar/GetListByDynamic',
      {
        Filter: { Field: 'KutuphaneBolumuId', Operator: 'eq', Value: sectionId },
        Sort: [{ Field: 'Kod', Dir: 'asc' }]
      },
      {
        params: {
          'PageRequest.PageIndex': 0,
          'PageRequest.PageSize': 200
        }
      }
    )

    const data = response.data
    shelves.value = data?.items ?? []
    shelvesCount.value = data?.count ?? shelves.value.length
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
    shelves.value = []
    shelvesCount.value = 0
  } finally {
    shelvesLoading.value = false
  }
}

const applyLibraryFilters = async () => {
  if (librariesLoading.value) return
  appliedLibraryFilters.value = {
    search: libraryFilters.search,
    type: libraryFilters.type,
    status: libraryFilters.status
  }
  page.value = 0
  await loadLibraries()
}

const resetLibraryFilters = async () => {
  libraryFilters.search = ''
  libraryFilters.type = 'all'
  libraryFilters.status = 'all'
  await applyLibraryFilters()
}

const prevPage = async () => {
  if (page.value === 0) return
  page.value -= 1
  await loadLibraries()
}

const nextPage = async () => {
  if (page.value + 1 >= totalPages.value) return
  page.value += 1
  await loadLibraries()
}

const selectLibrary = (id: string) => {
  if (librariesLoading.value) return
  if (selectedLibraryId.value === id) return
  selectedLibraryId.value = id
}

const selectSection = (id: string) => {
  if (sectionsLoading.value) return
  if (selectedSectionId.value === id) return
  selectedSectionId.value = id
}

const openLibraryModal = (mode: 'create' | 'edit', library?: Library) => {
  libraryModal.visible = true
  libraryModal.mode = mode
  libraryModal.error = ''
  libraryModal.saving = false

  if (mode === 'edit' && library) {
    libraryModal.id = library.id
    libraryModal.kod = library.kod ?? ''
    libraryModal.ad = library.ad ?? ''
    libraryModal.tip = normalizeLibraryType(library.tip)
    libraryModal.adres = library.adres ?? ''
    libraryModal.telefon = library.telefon ?? ''
    libraryModal.ePosta = library.ePosta ?? ''
    libraryModal.aktif = Boolean(library.aktif)
  } else {
    libraryModal.id = ''
    libraryModal.kod = ''
    libraryModal.ad = ''
    libraryModal.tip = 'Merkez'
    libraryModal.adres = ''
    libraryModal.telefon = ''
    libraryModal.ePosta = ''
    libraryModal.aktif = true
  }
}

const closeLibraryModal = () => {
  if (libraryModal.saving) return
  libraryModal.visible = false
  libraryModal.error = ''
}

const submitLibrary = async () => {
  if (!libraryModal.visible || libraryModal.saving) return
  if (!libraryModal.kod || !libraryModal.ad || !libraryModal.adres) {
    libraryModal.error = 'Kod, ad ve adres alanlari zorunludur.'
    return
  }

  libraryModal.error = ''
  libraryModal.saving = true

  const payload = {
    kod: libraryModal.kod,
    ad: libraryModal.ad,
    tip: libraryTypeToValue(libraryModal.tip),
    adres: libraryModal.adres,
    telefon: libraryModal.telefon || null,
    ePosta: libraryModal.ePosta || null,
    aktif: libraryModal.aktif
  }

  try {
    if (libraryModal.mode === 'create') {
      const response = await apiClient.post<{ id?: string; Id?: string }>('/Kutuphaneler', payload)
      const createdId = response.data?.id ?? response.data?.Id ?? null
      showFeedback('Kutuphane basariyla olusturuldu.', 'success')
      libraryModal.visible = false
      await loadLibraries()
      if (createdId) {
        selectedLibraryId.value = createdId
      }
    } else {
      await apiClient.put('/Kutuphaneler', { id: libraryModal.id, ...payload })
      showFeedback('Kutuphane guncellendi.', 'success')
      libraryModal.visible = false
      await loadLibraries()
    }
  } catch (err) {
    libraryModal.error = getErrorMessage(err)
  } finally {
    libraryModal.saving = false
  }
}

const deleteLibrary = async (library: Library) => {
  if (!library?.id) return
  const confirmed = window.confirm(`'${library.ad}' kutuphanesini silmek istediginizden emin misiniz?`)
  if (!confirmed) return
  try {
    await apiClient.delete(`/Kutuphaneler/${library.id}`)
    showFeedback('Kutuphane silindi.', 'success')
    if (selectedLibraryId.value === library.id) {
      selectedLibraryId.value = null
    }
    await loadLibraries()
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
  }
}

const openSectionModal = (mode: 'create' | 'edit', section?: LibrarySection) => {
  if (!selectedLibrary.value) {
    showFeedback('Lutfen once bir kutuphane secin.', 'error')
    return
  }

  sectionModal.visible = true
  sectionModal.mode = mode
  sectionModal.error = ''
  sectionModal.saving = false

  if (mode === 'edit' && section) {
    sectionModal.id = section.id
    sectionModal.ad = section.ad ?? ''
    sectionModal.aciklama = section.aciklama ?? ''
  } else {
    sectionModal.id = ''
    sectionModal.ad = ''
    sectionModal.aciklama = ''
  }
}

const closeSectionModal = () => {
  if (sectionModal.saving) return
  sectionModal.visible = false
  sectionModal.error = ''
}

const submitSection = async () => {
  if (!sectionModal.visible || sectionModal.saving) return
  if (!sectionModal.ad) {
    sectionModal.error = 'Bolum adi zorunludur.'
    return
  }
  if (!selectedLibraryId.value) {
    sectionModal.error = 'Kutuphane bilgisi bulunamadi.'
    return
  }

  sectionModal.error = ''
  sectionModal.saving = true

  const payload = {
    kutuphaneId: selectedLibraryId.value,
    ad: sectionModal.ad,
    aciklama: sectionModal.aciklama || null
  }

  try {
    if (sectionModal.mode === 'create') {
      const response = await apiClient.post<{ id?: string; Id?: string }>('/KutuphaneBolumleri', payload)
      const createdId = response.data?.id ?? response.data?.Id ?? null
      showFeedback('Bolum basariyla kaydedildi.', 'success')
      sectionModal.visible = false
      await loadSections(selectedLibraryId.value)
      if (createdId) {
        selectedSectionId.value = createdId
      }
    } else {
      await apiClient.put('/KutuphaneBolumleri', { id: sectionModal.id, ...payload })
      showFeedback('Bolum guncellendi.', 'success')
      sectionModal.visible = false
      await loadSections(selectedLibraryId.value)
    }
  } catch (err) {
    sectionModal.error = getErrorMessage(err)
  } finally {
    sectionModal.saving = false
  }
}

const deleteSection = async (section: LibrarySection) => {
  if (!section?.id) return
  const confirmed = window.confirm(`'${section.ad}' bolumunu silmek istediginizden emin misiniz?`)
  if (!confirmed) return
  try {
    await apiClient.delete(`/KutuphaneBolumleri/${section.id}`)
    showFeedback('Bolum silindi.', 'success')
    if (selectedLibraryId.value) {
      await loadSections(selectedLibraryId.value)
    }
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
  }
}

const openShelfModal = (mode: 'create' | 'edit', shelf?: Shelf) => {
  if (!selectedSection.value) {
    showFeedback('Lutfen once bir bolum secin.', 'error')
    return
  }

  shelfModal.visible = true
  shelfModal.mode = mode
  shelfModal.error = ''
  shelfModal.saving = false

  if (mode === 'edit' && shelf) {
    shelfModal.id = shelf.id
    shelfModal.kod = shelf.kod ?? ''
    shelfModal.aciklama = shelf.aciklama ?? ''
  } else {
    shelfModal.id = ''
    shelfModal.kod = ''
    shelfModal.aciklama = ''
  }
}

const closeShelfModal = () => {
  if (shelfModal.saving) return
  shelfModal.visible = false
  shelfModal.error = ''
}

const submitShelf = async () => {
  if (!shelfModal.visible || shelfModal.saving) return
  if (!shelfModal.kod) {
    shelfModal.error = 'Raf kodu zorunludur.'
    return
  }
  if (!selectedSectionId.value) {
    shelfModal.error = 'Bolum bilgisi bulunamadi.'
    return
  }

  shelfModal.error = ''
  shelfModal.saving = true

  const payload = {
    kutuphaneBolumuId: selectedSectionId.value,
    kod: shelfModal.kod,
    aciklama: shelfModal.aciklama || null
  }

  try {
    if (shelfModal.mode === 'create') {
      await apiClient.post('/Raflar', payload)
      showFeedback('Raf basariyla kaydedildi.', 'success')
      shelfModal.visible = false
      await loadShelves(selectedSectionId.value)
    } else {
      await apiClient.put('/Raflar', { id: shelfModal.id, ...payload })
      showFeedback('Raf guncellendi.', 'success')
      shelfModal.visible = false
      await loadShelves(selectedSectionId.value)
    }
  } catch (err) {
    shelfModal.error = getErrorMessage(err)
  } finally {
    shelfModal.saving = false
  }
}

const deleteShelf = async (shelf: Shelf) => {
  if (!shelf?.id) return
  const confirmed = window.confirm(`'${shelf.kod}' rafini silmek istediginizden emin misiniz?`)
  if (!confirmed) return
  try {
    await apiClient.delete(`/Raflar/${shelf.id}`)
    showFeedback('Raf silindi.', 'success')
    if (selectedSectionId.value) {
      await loadShelves(selectedSectionId.value)
    }
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
  }
}

watch(selectedLibraryId, async newLibraryId => {
  if (!newLibraryId) {
    sections.value = []
    sectionsCount.value = 0
    selectedSectionId.value = null
    shelves.value = []
    shelvesCount.value = 0
    return
  }
  await loadSections(newLibraryId)
})

watch(selectedSectionId, async newSectionId => {
  if (!newSectionId) {
    shelves.value = []
    shelvesCount.value = 0
    return
  }
  await loadShelves(newSectionId)
})

onMounted(async () => {
  appliedLibraryFilters.value = {
    search: libraryFilters.search,
    type: libraryFilters.type,
    status: libraryFilters.status
  }
  await loadLibraries()
})
</script>

<style scoped>
.libraries-page {
  display: flex;
  flex-direction: column;
  gap: 1.6rem;
  padding: 2.5rem 3rem 4rem;
  background: linear-gradient(180deg, #f8fafc 0%, #e0f2fe 100%);
  color: #0f172a;
}

.hero {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1.5rem;
  padding: 2rem 2.4rem;
  border-radius: 20px;
  background: linear-gradient(135deg, #0ea5e9 0%, #6366f1 100%);
  color: #fff;
  box-shadow: 0 20px 48px -32px rgba(14, 165, 233, 0.6);
}

.hero__texts {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.hero__texts h1 {
  margin: 0;
  font-size: 2.1rem;
  font-weight: 600;
}

.hero__texts p {
  margin: 0;
  font-size: 1rem;
  opacity: 0.9;
}

.hero__meta {
  display: flex;
  gap: 1.2rem;
}

.hero__meta div {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  padding: 0.75rem 1rem;
  border-radius: 14px;
  background: rgba(15, 23, 42, 0.2);
}

.meta-label {
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  opacity: 0.8;
}

.meta-value {
  font-size: 1.2rem;
  font-weight: 600;
}

.hero__actions {
  display: flex;
  align-items: flex-start;
}

.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.45rem;
  padding: 0.7rem 1.05rem;
  border-radius: 12px;
  border: none;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.15s ease, box-shadow 0.15s ease;
}

.btn:hover {
  transform: translateY(-1px);
}

.btn--primary {
  background: #f8fafc;
  color: #0f172a;
}

.btn--ghost {
  background: transparent;
  color: #1f2937;
  border: 1px solid rgba(148, 163, 184, 0.4);
}

.btn--danger {
  background: rgba(239, 68, 68, 0.12);
  color: #b91c1c;
}

.btn--sm {
  padding: 0.5rem 0.85rem;
  font-size: 0.8rem;
  border-radius: 10px;
}

.btn__icon {
  font-size: 1.2rem;
  font-weight: 700;
}

.alert {
  display: grid;
  gap: 0.2rem;
  padding: 0.85rem 1.1rem;
  border-radius: 14px;
  font-size: 0.9rem;
  border: 1px solid transparent;
}

.alert strong {
  font-weight: 600;
}

.alert--success {
  background: rgba(134, 239, 172, 0.25);
  border-color: rgba(34, 197, 94, 0.3);
  color: #166534;
}

.alert--error {
  background: rgba(254, 205, 211, 0.35);
  border-color: rgba(244, 63, 94, 0.3);
  color: #b91c1c;
}

.panel {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
  padding: 1.8rem 2rem;
  background: rgba(255, 255, 255, 0.94);
  border-radius: 18px;
  box-shadow: 0 18px 44px -30px rgba(15, 23, 42, 0.4);
}

.panel__header h2 {
  margin: 0;
  font-size: 1.4rem;
  font-weight: 600;
  color: #0f172a;
}

.panel__header p {
  margin: 0.2rem 0 0;
  font-size: 0.9rem;
  color: #64748b;
}

.filters {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 0.9rem;
}

.filters label {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  font-size: 0.85rem;
  color: #1f2937;
}

.filters input,
.filters select {
  border: 1px solid rgba(203, 213, 225, 0.8);
  border-radius: 12px;
  padding: 0.6rem 0.85rem;
  font-size: 0.9rem;
}

.filters input:focus,
.filters select:focus {
  border-color: rgba(14, 165, 233, 0.6);
  box-shadow: 0 0 0 2px rgba(14, 165, 233, 0.15);
  outline: none;
}

.filters__actions {
  display: flex;
  align-items: flex-end;
  gap: 0.6rem;
}

.table-wrapper {
  border-radius: 16px;
  border: 1px solid rgba(226, 232, 240, 0.9);
  overflow: hidden;
}

.table-wrapper--loading {
  opacity: 0.85;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
}

.data-table thead {
  background: rgba(14, 165, 233, 0.08);
  font-size: 0.78rem;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  color: #475569;
}

.data-table th,
.data-table td {
  padding: 0.8rem 1rem;
  border-bottom: 1px solid rgba(226, 232, 240, 0.95);
  text-align: left;
}

.data-table tr.table-row {
  cursor: pointer;
  transition: background 0.15s ease, box-shadow 0.15s ease;
}

.data-table tr.table-row:hover:not(.selected) {
  background: rgba(14, 165, 233, 0.05);
  box-shadow: inset 0 0 0 1px rgba(14, 165, 233, 0.12);
}

.data-table tr.selected {
  background: rgba(14, 165, 233, 0.08);
}

.table-actions {
  display: flex;
  gap: 0.5rem;
}

.table-state {
  text-align: center;
  padding: 2rem 1rem;
  color: #475569;
  font-size: 0.9rem;
}

.table-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.88rem;
  color: #475569;
}

.table-footer__actions {
  display: flex;
  gap: 0.6rem;
}

.entity {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.entity__title {
  font-weight: 600;
  color: #0f172a;
}

.entity__meta {
  font-size: 0.78rem;
  color: #94a3b8;
}

.badge {
  display: inline-flex;
  align-items: center;
  padding: 0.28rem 0.6rem;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 600;
}

.badge--success {
  background: rgba(134, 239, 172, 0.35);
  color: #15803d;
}

.badge--muted {
  background: rgba(203, 213, 225, 0.45);
  color: #475569;
}

.contact-cell {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
  font-size: 0.85rem;
  color: #475569;
}

.contact-empty {
  color: #94a3b8;
  font-style: italic;
}

.detail-header {
  display: flex;
  justify-content: space-between;
  gap: 1rem;
  align-items: flex-start;
}

.detail-header h3 {
  margin: 0;
  font-size: 1.3rem;
  font-weight: 600;
}

.detail-header p {
  margin: 0.15rem 0 0;
  color: #64748b;
}

.detail-actions {
  display: flex;
  gap: 0.6rem;
}

.detail-meta {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 0.8rem;
}

.detail-meta div {
  background: rgba(241, 245, 249, 0.9);
  border-radius: 12px;
  padding: 0.8rem 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.detail-meta span {
  font-size: 0.78rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: #64748b;
}

.detail-meta p {
  margin: 0;
  font-size: 0.92rem;
  color: #0f172a;
}

.detail-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1rem;
}

.detail-card {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  border: 1px solid rgba(203, 213, 225, 0.7);
  border-radius: 14px;
  background: #fff;
  min-height: 240px;
  box-shadow: 0 12px 30px -28px rgba(15, 23, 42, 0.45);
}

.detail-card header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  padding: 0.95rem 1.1rem 0 1.1rem;
}

.detail-card header h4 {
  margin: 0;
  font-size: 1.05rem;
  font-weight: 600;
}

.detail-card header p {
  margin: 0.25rem 0 0;
  font-size: 0.85rem;
  color: #64748b;
}

.section-list {
  list-style: none;
  margin: 0;
  padding: 0 1.1rem 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}

.section-list li {
  display: flex;
  justify-content: space-between;
  align-items: stretch;
  gap: 0.6rem;
  padding: 0.75rem 0.85rem;
  border: 1px solid transparent;
  border-radius: 12px;
  background: rgba(248, 250, 252, 0.9);
}

.section-list li.active {
  border-color: rgba(59, 130, 246, 0.4);
  background: rgba(191, 219, 254, 0.4);
}

.section__select {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  text-align: left;
  background: transparent;
  border: none;
  cursor: pointer;
  color: #0f172a;
}

.section__select strong {
  font-size: 0.95rem;
  font-weight: 600;
}

.section__select small {
  font-size: 0.78rem;
  color: #64748b;
}

.section__actions {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.section__actions button {
  background: transparent;
  border: none;
  font-size: 0.78rem;
  color: #0f172a;
  cursor: pointer;
}

.section__actions button:hover {
  text-decoration: underline;
}

.section-empty {
  justify-content: center;
  text-align: center;
  color: #94a3b8;
  font-size: 0.85rem;
}

.card-state {
  padding: 1rem 1.2rem;
  font-size: 0.9rem;
  color: #1f2937;
}

.card-state--muted {
  color: #94a3b8;
}

.detail-table {
  width: 100%;
  border-collapse: collapse;
}

.detail-table th,
.detail-table td {
  padding: 0.75rem 1rem;
  border-bottom: 1px solid rgba(226, 232, 240, 0.8);
  text-align: left;
  font-size: 0.9rem;
}

.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.45);
  display: grid;
  place-items: center;
  padding: 1.5rem;
  z-index: 1300;
}

.modal {
  width: min(600px, 100%);
  background: #ffffff;
  border-radius: 18px;
  box-shadow: 0 36px 80px -40px rgba(15, 23, 42, 0.65);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.modal__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1.3rem 1.5rem;
  background: rgba(14, 165, 233, 0.15);
}

.modal__header h3 {
  margin: 0;
  font-size: 1.2rem;
  font-weight: 600;
  color: #0f172a;
}

.modal__close {
  background: transparent;
  border: none;
  font-size: 1.6rem;
  line-height: 1;
  cursor: pointer;
  color: #475569;
}

.modal__body {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1.3rem 1.5rem 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 0.9rem;
}

.form-grid label {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
  font-size: 0.85rem;
  color: #1f2937;
}

.form-grid textarea,
.form-grid input,
.form-grid select {
  border: 1px solid rgba(203, 213, 225, 0.9);
  border-radius: 12px;
  padding: 0.6rem 0.85rem;
  font-size: 0.9rem;
  resize: vertical;
}

.form-grid textarea:focus,
.form-grid input:focus,
.form-grid select:focus {
  border-color: rgba(59, 130, 246, 0.6);
  box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.18);
  outline: none;
}

.form-grid__full {
  grid-column: 1 / -1;
}

.modal__error {
  margin: 0;
  padding: 0.75rem 0.9rem;
  border-radius: 12px;
  background: rgba(248, 113, 113, 0.18);
  color: #b91c1c;
  font-size: 0.88rem;
}

.modal__actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.6rem;
}

.alert-enter-active,
.alert-leave-active,
.modal-enter-active,
.modal-leave-active {
  transition: opacity 0.2s ease;
}

.alert-enter-from,
.alert-leave-to,
.modal-enter-from,
.modal-leave-to {
  opacity: 0;
}

@media (max-width: 960px) {
  .libraries-page {
    padding: 1.8rem;
  }

  .hero {
    flex-direction: column;
  }

  .detail-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 720px) {
  .panel {
    padding: 1.4rem;
  }

  .filters__actions {
    flex-direction: column;
    align-items: stretch;
  }

  .table-actions {
    flex-direction: column;
  }
}
</style>




