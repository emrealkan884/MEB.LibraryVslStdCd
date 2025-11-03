<template>
  <div class="circulation-page">
    <section class="page-header">
      <div>
        <h1>Dolashim yonetimi</h1>
        <p>Odunc verme, iade ve sure uzatma islemlerini tek yerden yonetin.</p>
      </div>
      <button type="button" class="btn btn-refresh" @click="refresh" :disabled="listBusy">Yenile</button>
    </section>

    <section class="summary-grid">
      <article class="summary-card">
        <header>Aktif odunc</header>
        <strong>{{ summaryMetrics.active.toLocaleString('tr-TR') }}</strong>
        <small>Simdi teslim tarihi bekleyenler</small>
      </article>
      <article class="summary-card">
        <header>Gecikmis odunc</header>
        <strong class="text-warning">{{ summaryMetrics.overdue.toLocaleString('tr-TR') }}</strong>
        <small>Sureyi asan kayitlar</small>
      </article>
      <article class="summary-card">
        <header>Bugun iade</header>
        <strong class="text-positive">{{ summaryMetrics.returnedToday.toLocaleString('tr-TR') }}</strong>
        <small>Bugun tamamlanan iadeler</small>
      </article>
      <article class="summary-card">
        <header>Toplam kayit</header>
        <strong>{{ summaryMetrics.total.toLocaleString('tr-TR') }}</strong>
        <small>Filtre uygulanmadan onceki toplam</small>
      </article>
    </section>

    <transition name="fade">
      <div v-if="feedback" class="feedback" :class="feedback.kind">
        <span>{{ feedback.message }}</span>
        <button type="button" @click="clearFeedback" aria-label="Kapat">x</button>
      </div>
    </transition>

    <transition name="fade">
      <div v-if="errors.length" class="alert">
        <strong>Islem tamamlanamadi.</strong>
        <ul>
          <li v-for="error in errors" :key="error">{{ error }}</li>
        </ul>
      </div>
    </transition>

    <section class="actions-panel">
      <div class="quick-actions">
        <button type="button" class="btn btn-primary" @click="openCreateModal">Yeni odunc</button>
        <button type="button" class="btn btn-outline" @click="openReturnModal()">Iade al</button>
        <button type="button" class="btn btn-outline" @click="openExtendModal()">Sure uzat</button>
      </div>
      <div class="filters">
        <label>
          <span>Durum</span>
          <select v-model="filters.status">
            <option v-for="option in statusOptions" :key="option.value" :value="option.value">
              {{ option.label }} ({{ statusCounts[option.value] ?? 0 }})
            </option>
          </select>
        </label>
        <label>
          <span>Kutuphane</span>
          <select v-model="filters.kutuphaneId" @change="applyLibraryFilter" :disabled="librariesLoading">
            <option value="">Tum kutuphaneler</option>
            <option v-for="library in libraries" :key="library.id" :value="library.id">
              {{ library.name }}
            </option>
          </select>
        </label>
        <label>
          <span>Arama</span>
          <input
            type="text"
            v-model.trim="searchTerm"
            placeholder="Not veya ID ile ara"
            @keyup.enter="applySearch"
          />
        </label>
        <div class="filters-actions">
          <button type="button" class="btn btn-outline" @click="applySearch" :disabled="listBusy">Ara</button>
          <button type="button" class="btn btn-ghost" @click="resetFilters" :disabled="listBusy">Sifirla</button>
        </div>
      </div>
    </section>

    <section class="table-panel">
      <header class="panel-header">
        <div>
          <h2>Odunc kayitlari</h2>
          <p v-if="lastFetchedAt">Son guncelleme: {{ formatDateTime(lastFetchedAt) }}</p>
        </div>
        <div class="panel-controls">
          <label>
            <span>Sayfa boyutu</span>
            <select :value="pagination.pageSize" @change="changePageSize">
              <option v-for="size in pageSizeOptions" :key="size" :value="size">{{ size }}</option>
            </select>
          </label>
        </div>
      </header>

      <div v-if="listBusy" class="table-state table-loading">
        <span class="spinner" aria-hidden="true"></span>
        <p>Kayitlar yukleniyor...</p>
      </div>
      <div v-else-if="filteredLoans.length === 0" class="table-state">
        <p>Gosterilecek kayit bulunmuyor.</p>
      </div>
      <div v-else class="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>Durum</th>
              <th>Odunc ID</th>
              <th>Kullanici ID</th>
              <th>Nusha ID</th>
              <th>Alinma</th>
              <th>Son teslim</th>
              <th>Iade</th>
              <th>Not</th>
              <th>Islemler</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="loan in filteredLoans" :key="loan.id">
              <td>
                <span :class="statusClass(loan.durumu)">
                  {{ loanStatusLabels[loan.durumu] ?? loan.durumu }}
                </span>
              </td>
              <td>{{ loan.id }}</td>
              <td>{{ loan.kullaniciId }}</td>
              <td>{{ loan.nushaId }}</td>
              <td>{{ formatDate(loan.alinmaTarihi) }}</td>
              <td>{{ formatDate(loan.sonTeslimTarihi) }}</td>
              <td>{{ formatDate(loan.iadeTarihi) }}</td>
              <td>{{ loan.not || '-' }}</td>
              <td class="table-actions">
                <button
                  type="button"
                  class="link"
                  @click="openReturnModal(loan)"
                  :disabled="loan.durumu === LoanStatusEnum.IadeEdildi || actionBusy"
                >
                  Iade al
                </button>
                <button
                  type="button"
                  class="link"
                  @click="openExtendModal(loan)"
                  :disabled="loan.durumu !== LoanStatusEnum.Aktif || actionBusy"
                >
                  Sure uzat
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div v-if="pagination.totalPages > 1" class="pagination">
        <button type="button" class="btn btn-outline" @click="goToPreviousPage" :disabled="!pagination.hasPrevious || listBusy">
          Onceki
        </button>
        <span>Sayfa {{ pagination.pageIndex + 1 }} / {{ pagination.totalPages }}</span>
        <button type="button" class="btn btn-outline" @click="goToNextPage" :disabled="!pagination.hasNext || listBusy">
          Sonraki
        </button>
      </div>
    </section>

    <section v-if="recentLoans.length" class="recent-panel">
      <h3>Son islemler</h3>
      <ul>
        <li v-for="loan in recentLoans" :key="loan.id">
          <div class="recent-status">
            <span :class="statusClass(loan.durumu)">{{ recentLabel(loan.durumu) }}</span>
            <small>{{ loan.id }}</small>
          </div>
          <div class="recent-meta">
            <span>Kullanici: {{ loan.kullaniciId }}</span>
            <span>Materyal: {{ loan.nushaId }}</span>
          </div>
          <time>{{ formatDateTime(loan.iadeTarihi || loan.alinmaTarihi) }}</time>
        </li>
      </ul>
    </section>

    <transition name="fade">
      <div v-if="showCreateModal" class="modal" @click.self="closeCreateModal">
        <div class="modal-content">
          <header>
            <h4>Yeni odunc</h4>
            <button type="button" @click="closeCreateModal" aria-label="Kapat">x</button>
          </header>
          <form @submit.prevent="submitCreate">
            <label>
              <span>Kutuphane ID</span>
              <input v-model.trim="createForm.kutuphaneId" type="text" required />
            </label>
            <label>
              <span>Kullanici ID</span>
              <input v-model.trim="createForm.kullaniciId" type="text" required />
            </label>
            <label>
              <span>Nusha ID</span>
              <input v-model.trim="createForm.nushaId" type="text" required />
            </label>
            <label>
              <span>Odunc suresi (gun)</span>
              <input v-model.number="createForm.oduncSuresiGun" type="number" min="1" max="60" />
            </label>
            <label>
              <span>Not (opsiyonel)</span>
              <textarea v-model.trim="createForm.not" rows="2" />
            </label>
            <footer>
              <button type="button" class="btn btn-ghost" @click="closeCreateModal">Iptal</button>
              <button type="submit" class="btn btn-primary" :disabled="actionBusy">Kaydet</button>
            </footer>
          </form>
        </div>
      </div>
    </transition>

    <transition name="fade">
      <div v-if="showReturnModal" class="modal" @click.self="closeReturnModal">
        <div class="modal-content">
          <header>
            <h4>Iade al</h4>
            <button type="button" @click="closeReturnModal" aria-label="Kapat">x</button>
          </header>
          <form @submit.prevent="submitReturn">
            <label>
              <span>Odunc ID</span>
              <input v-model.trim="returnForm.id" type="text" required />
            </label>
            <label>
              <span>Iade notu (opsiyonel)</span>
              <textarea v-model.trim="returnForm.iadeNotu" rows="2" />
            </label>
            <footer>
              <button type="button" class="btn btn-ghost" @click="closeReturnModal">Iptal</button>
              <button type="submit" class="btn btn-primary" :disabled="actionBusy">Iade al</button>
            </footer>
          </form>
        </div>
      </div>
    </transition>

    <transition name="fade">
      <div v-if="showExtendModal" class="modal" @click.self="closeExtendModal">
        <div class="modal-content">
          <header>
            <h4>Sure uzat</h4>
            <button type="button" @click="closeExtendModal" aria-label="Kapat">x</button>
          </header>
          <form @submit.prevent="submitExtend">
            <label>
              <span>Odunc ID</span>
              <input v-model.trim="extendForm.id" type="text" required />
            </label>
            <label>
              <span>Ek gun</span>
              <input v-model.number="extendForm.ekGun" type="number" min="1" max="7" required />
            </label>
            <label>
              <span>Uzatma nedeni (opsiyonel)</span>
              <textarea v-model.trim="extendForm.uzatmaNedeni" rows="2" />
            </label>
            <footer>
              <button type="button" class="btn btn-ghost" @click="closeExtendModal">Iptal</button>
              <button type="submit" class="btn btn-primary" :disabled="actionBusy">Sure uzat</button>
            </footer>
          </form>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { storeToRefs } from 'pinia'
import { useCirculationStore } from '@/stores/circulation'
import { useReportingStore } from '@/stores/reporting'
import { loanStatusLabels, LoanStatus } from '@/types/circulation'
import type { LoanListItem } from '@/types/circulation'
import { useRoute, useRouter } from 'vue-router'

const circulationStore = useCirculationStore()
const reportingStore = useReportingStore()
const route = useRoute()
const router = useRouter()

const {
  filteredLoans,
  summaryMetrics,
  recentLoans,
  statusCounts,
  pagination,
  filters,
  loading,
  errors,
  lastFetchedAt,
} = storeToRefs(circulationStore)

const { libraries } = storeToRefs(reportingStore)

const searchTerm = ref(filters.value.search ?? '')
const showCreateModal = ref(false)
const showReturnModal = ref(false)
const showExtendModal = ref(false)
const feedback = ref<{ kind: 'success' | 'info' | 'error'; message: string } | null>(null)

const createForm = ref({
  kutuphaneId: '',
  kullaniciId: '',
  nushaId: '',
  oduncSuresiGun: 14,
  not: '',
})

const returnForm = ref({
  id: '',
  iadeNotu: '',
})

const extendForm = ref({
  id: '',
  ekGun: 3,
  uzatmaNedeni: '',
})

const pageSizeOptions = [10, 20, 50, 100]

const statusOptions = [
  { value: 'open', label: 'Acil (aktif + gecikmis)' },
  { value: 'active', label: 'Yalniz aktif' },
  { value: 'overdue', label: 'Gecikmis' },
  { value: 'returned', label: 'Iade edilen' },
  { value: 'all', label: 'Tum kayitlar' },
] as const

const LoanStatusEnum = LoanStatus

const loadingFlags = computed(() => loading.value)
const actionBusy = computed(() => loadingFlags.value.action)
const listBusy = computed(() => loadingFlags.value.list)
const librariesLoading = computed(() => reportingStore.loading.libraries)

function setFeedback(kind: 'success' | 'info' | 'error', message: string) {
  feedback.value = { kind, message }
  window.setTimeout(() => {
    if (feedback.value?.message === message) {
      feedback.value = null
    }
  }, 4000)
}

function clearFeedback() {
  feedback.value = null
}

function formatDate(value: string | null | undefined) {
  if (!value) return '-'
  const date = new Date(value)
  if (Number.isNaN(date.getTime())) return value
  return date.toLocaleDateString('tr-TR')
}

function formatDateTime(value: string | null | undefined) {
  if (!value) return '-'
  const date = new Date(value)
  if (Number.isNaN(date.getTime())) return value
  return date.toLocaleString('tr-TR', { dateStyle: 'short', timeStyle: 'short' })
}

function statusClass(status: LoanStatus) {
  if (status === LoanStatus.Aktif) return 'status-chip status-active'
  if (status === LoanStatus.Gecikmis) return 'status-chip status-overdue'
  if (status === LoanStatus.IadeEdildi) return 'status-chip status-returned'
  return 'status-chip status-default'
}

function recentLabel(status: LoanStatus) {
  if (status === LoanStatus.IadeEdildi) return 'Iade alindi'
  if (status === LoanStatus.Gecikmis) return 'Gecikme suruyor'
  if (status === LoanStatus.Aktif) return 'Odunc verildi'
  return 'Islem guncellendi'
}

function normalizeActionQuery(value: unknown): string | null {
  if (Array.isArray(value)) {
    const first = value[0]
    return typeof first === 'string' ? first : null
  }
  return typeof value === 'string' ? value : null
}

function clearActionQuery() {
  const nextQuery = { ...route.query }
  if ('action' in nextQuery) {
    delete nextQuery.action
    router.replace({ query: nextQuery })
  }
}

function handleRouteAction(action: string) {
  switch (action) {
    case 'new-loan':
      openCreateModal()
      clearActionQuery()
      break
    case 'return-loan':
      openReturnModal()
      clearActionQuery()
      break
    case 'extend-loan':
      openExtendModal()
      clearActionQuery()
      break
    default:
      break
  }
}

function openCreateModal() {
  createForm.value = {
    kutuphaneId: filters.value.kutuphaneId ?? '',
    kullaniciId: '',
    nushaId: '',
    oduncSuresiGun: 14,
    not: '',
  }
  clearFeedback()
  showCreateModal.value = true
}

function openReturnModal(loan?: LoanListItem) {
  returnForm.value = {
    id: loan?.id ?? '',
    iadeNotu: '',
  }
  clearFeedback()
  showReturnModal.value = true
}

function openExtendModal(loan?: LoanListItem) {
  extendForm.value = {
    id: loan?.id ?? '',
    ekGun: 3,
    uzatmaNedeni: '',
  }
  clearFeedback()
  showExtendModal.value = true
}

function closeCreateModal() {
  showCreateModal.value = false
}

function closeReturnModal() {
  showReturnModal.value = false
}

function closeExtendModal() {
  showExtendModal.value = false
}

async function submitCreate() {
  if (!createForm.value.kutuphaneId || !createForm.value.kullaniciId || !createForm.value.nushaId) {
    setFeedback('error', 'Tum zorunlu alanlari doldurun.')
    return
  }

  const ok = await circulationStore.createLoan({
    kutuphaneId: createForm.value.kutuphaneId,
    kullaniciId: createForm.value.kullaniciId,
    nushaId: createForm.value.nushaId,
    oduncSuresiGun: createForm.value.oduncSuresiGun || undefined,
    not: createForm.value.not?.trim() || undefined,
  })

  if (ok) {
    setFeedback('success', 'Odunc islemi kaydedildi.')
    closeCreateModal()
    searchTerm.value = filters.value.search ?? ''
  }
}

async function submitReturn() {
  if (!returnForm.value.id) {
    setFeedback('error', 'Odunc ID zorunludur.')
    return
  }

  const ok = await circulationStore.returnLoan({
    id: returnForm.value.id,
    iadeNotu: returnForm.value.iadeNotu?.trim() || undefined,
  })

  if (ok) {
    setFeedback('success', 'Iade islemi tamamlandi.')
    closeReturnModal()
  }
}

async function submitExtend() {
  if (!extendForm.value.id) {
    setFeedback('error', 'Odunc ID zorunludur.')
    return
  }
  if (!extendForm.value.ekGun || extendForm.value.ekGun < 1) {
    setFeedback('error', 'Ek gun 1 veya uzeri olmalidir.')
    return
  }

  const ok = await circulationStore.extendLoan({
    id: extendForm.value.id,
    ekGun: extendForm.value.ekGun,
    uzatmaNedeni: extendForm.value.uzatmaNedeni?.trim() || undefined,
  })

  if (ok) {
    setFeedback('success', 'Sure uzatma islemi tamamlandi.')
    closeExtendModal()
  }
}

async function refresh() {
  await circulationStore.refresh()
}

async function applyLibraryFilter() {
  await circulationStore.fetchLoans({
    pageIndex: 0,
    filters: { search: searchTerm.value },
  })
}

async function applySearch() {
  await circulationStore.fetchLoans({
    pageIndex: 0,
    filters: { search: searchTerm.value },
  })
}

async function resetFilters() {
  searchTerm.value = ''
  circulationStore.resetFilters()
  await circulationStore.fetchLoans({
    pageIndex: 0,
    filters: { search: '' },
  })
}

async function changePageSize(event: Event) {
  const value = Number((event.target as HTMLSelectElement).value)
  if (!Number.isNaN(value) && value > 0) {
    await circulationStore.changePageSize(value)
  }
}

async function goToPreviousPage() {
  if (pagination.value.hasPrevious) {
    await circulationStore.goToPage(pagination.value.pageIndex - 1)
  }
}

async function goToNextPage() {
  if (pagination.value.hasNext) {
    await circulationStore.goToPage(pagination.value.pageIndex + 1)
  }
}

watch(
  () => filters.value.search,
  (value) => {
    if ((value ?? '') !== searchTerm.value) {
      searchTerm.value = value ?? ''
    }
  }
)

watch(
  () => route.query.action,
  (value) => {
    const action = normalizeActionQuery(value)
    if (action) {
      handleRouteAction(action)
    }
  },
  { immediate: true }
)

onMounted(async () => {
  await circulationStore.fetchLoans()
  if (!libraries.value.length) {
    await reportingStore.fetchLibraries()
  }
})
</script>

<style scoped>
.circulation-page {
  display: flex;
  flex-direction: column;
  gap: 1.6rem;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.page-header h1 {
  margin: 0;
  font-size: 1.8rem;
  color: #111827;
}

.page-header p {
  margin: 0.3rem 0 0;
  color: #6b7280;
}

.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  border: none;
  border-radius: 0.6rem;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s ease;
  padding: 0.55rem 1.1rem;
}

.btn-primary {
  background: linear-gradient(135deg, #2563eb, #7c3aed);
  color: #ffffff;
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-outline {
  border: 1px solid #d1d5db;
  background: #ffffff;
  color: #1f2937;
}

.btn-outline:hover:not(:disabled) {
  background: #f3f4f6;
}

.btn-ghost {
  background: transparent;
  color: #6b7280;
}

.btn-refresh {
  background: #f1f5f9;
  color: #1e293b;
}

.btn-refresh:hover:not(:disabled) {
  background: #e2e8f0;
}

.summary-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 1rem;
}

.summary-card {
  border: 1px solid rgba(148, 163, 184, 0.35);
  border-radius: 1rem;
  padding: 1.1rem;
  background: rgba(248, 250, 252, 0.9);
  display: grid;
  gap: 0.4rem;
}

.summary-card header {
  font-size: 0.9rem;
  font-weight: 600;
  color: #334155;
}

.summary-card strong {
  font-size: 1.6rem;
  color: #0f172a;
}

.summary-card small {
  color: #64748b;
}

.text-warning {
  color: #b45309;
}

.text-positive {
  color: #15803d;
}

.feedback {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 0.8rem;
  padding: 0.65rem 0.9rem;
  border-radius: 0.75rem;
  font-size: 0.9rem;
}

.feedback.success {
  background: rgba(134, 239, 172, 0.25);
  color: #166534;
}

.feedback.info {
  background: rgba(191, 219, 254, 0.25);
  color: #1d4ed8;
}

.feedback.error {
  background: rgba(254, 202, 202, 0.3);
  color: #b91c1c;
}

.feedback button {
  background: none;
  border: none;
  color: inherit;
  font-size: 1.2rem;
  cursor: pointer;
}

.alert {
  padding: 0.9rem 1rem;
  border-radius: 0.9rem;
  border: 1px solid rgba(248, 113, 113, 0.4);
  background: rgba(254, 242, 242, 0.8);
  color: #b91c1c;
}

.alert ul {
  margin: 0.4rem 0 0;
  padding-left: 1.2rem;
}

.actions-panel {
  border: 1px solid rgba(203, 213, 225, 0.6);
  border-radius: 1rem;
  padding: 1.2rem;
  background: #ffffff;
  display: grid;
  gap: 1rem;
}

.quick-actions {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
}

.filters {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 0.9rem;
  align-items: end;
}

.filters label {
  display: grid;
  gap: 0.45rem;
  font-size: 0.85rem;
  color: #475569;
}

.filters input,
.filters select,
.filters textarea {
  border: 1px solid #cbd5f5;
  border-radius: 0.6rem;
  padding: 0.5rem 0.7rem;
  font-size: 0.9rem;
}

.filters-actions {
  display: flex;
  gap: 0.6rem;
}

.table-panel {
  border: 1px solid rgba(203, 213, 225, 0.6);
  border-radius: 1rem;
  background: #ffffff;
  padding: 1.2rem;
  display: grid;
  gap: 1rem;
}

.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.panel-header h2 {
  margin: 0;
  font-size: 1.2rem;
  color: #1f2937;
}

.panel-header p {
  margin: 0.35rem 0 0;
  color: #64748b;
}

.panel-controls {
  display: flex;
  gap: 0.8rem;
  align-items: center;
}

.panel-controls label {
  display: grid;
  gap: 0.3rem;
  font-size: 0.8rem;
  color: #6b7280;
}

.panel-controls select {
  border-radius: 0.5rem;
  border: 1px solid #d1d5db;
  padding: 0.45rem 0.6rem;
}

.table-wrapper {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  min-width: 760px;
}

thead {
  background: #f8fafc;
  color: #475569;
  text-align: left;
}

th,
td {
  padding: 0.75rem 0.9rem;
  border-bottom: 1px solid #e2e8f0;
  font-size: 0.9rem;
}

tbody tr:hover {
  background: rgba(248, 250, 252, 0.6);
}

.table-actions {
  display: flex;
  gap: 0.6rem;
}

.link {
  background: none;
  border: none;
  color: #2563eb;
  font-weight: 600;
  cursor: pointer;
  padding: 0;
}

.link:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.status-chip {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 90px;
  padding: 0.25rem 0.6rem;
  border-radius: 999px;
  font-size: 0.78rem;
  font-weight: 600;
  text-transform: uppercase;
}

.status-active {
  background: rgba(134, 239, 172, 0.35);
  color: #166534;
}

.status-overdue {
  background: rgba(254, 226, 226, 0.55);
  color: #b91c1c;
}

.status-returned {
  background: rgba(191, 219, 254, 0.55);
  color: #1d4ed8;
}

.status-default {
  background: rgba(226, 232, 240, 0.6);
  color: #475569;
}

.table-state {
  text-align: center;
  padding: 2rem 1rem;
  color: #64748b;
}

.table-loading {
  display: grid;
  justify-items: center;
  gap: 0.8rem;
}

.spinner {
  width: 36px;
  height: 36px;
  border: 4px solid #e2e8f0;
  border-top-color: #6366f1;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.8rem;
}

.recent-panel {
  border: 1px solid rgba(226, 232, 240, 0.75);
  border-radius: 1rem;
  padding: 1.1rem;
  background: rgba(248, 250, 252, 0.85);
  display: grid;
  gap: 0.8rem;
}

.recent-panel h3 {
  margin: 0;
  font-size: 1.1rem;
  color: #1f2937;
}

.recent-panel ul {
  list-style: none;
  padding: 0;
  margin: 0;
  display: grid;
  gap: 0.7rem;
}

.recent-panel li {
  display: grid;
  grid-template-columns: 1fr auto;
  gap: 0.5rem;
  align-items: center;
  background: #ffffff;
  border-radius: 0.8rem;
  padding: 0.75rem 0.9rem;
  border: 1px solid rgba(203, 213, 225, 0.5);
}

.recent-status {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.recent-status small {
  color: #6b7280;
}

.recent-meta {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
  font-size: 0.85rem;
  color: #475569;
}

.recent-panel time {
  font-size: 0.8rem;
  color: #6b7280;
}

.modal {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.38);
  display: grid;
  place-items: center;
  padding: 1.4rem;
  z-index: 30;
}

.modal-content {
  background: #ffffff;
  border-radius: 1rem;
  border: 1px solid rgba(203, 213, 225, 0.7);
  padding: 1.2rem;
  width: min(420px, 100%);
  display: grid;
  gap: 0.8rem;
  box-shadow: 0 20px 50px -30px rgba(15, 23, 42, 0.45);
}

.modal-content header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 0.6rem;
}

.modal-content header h4 {
  margin: 0;
  font-size: 1.1rem;
  color: #1f2937;
}

.modal-content header button {
  border: none;
  background: rgba(248, 113, 113, 0.15);
  color: #b91c1c;
  width: 32px;
  height: 32px;
  border-radius: 0.75rem;
  font-size: 1.1rem;
  cursor: pointer;
}

.modal-content form {
  display: grid;
  gap: 0.8rem;
}

.modal-content label {
  display: grid;
  gap: 0.35rem;
  font-size: 0.85rem;
  color: #475569;
}

.modal-content input,
.modal-content textarea {
  border: 1px solid #d1d5db;
  border-radius: 0.6rem;
  padding: 0.55rem 0.7rem;
  font-size: 0.9rem;
}

.modal-content textarea {
  resize: vertical;
}

.modal-content footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.6rem;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.15s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@media (max-width: 768px) {
  .page-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .panel-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .table-wrapper {
    margin: 0 -1.2rem;
  }
}
</style>
