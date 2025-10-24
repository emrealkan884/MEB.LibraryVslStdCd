<template>
  <div class="reporting-page">
    <section class="hero">
      <div class="hero__content">
        <div class="hero__status">
          <span class="hero__pulse"></span>
          Son özet güncellemesi: <strong>{{ formatDateTime(lastUpdated.summaries) }}</strong>
        </div>
        <h1 class="hero__title">Raporlama Merkezi</h1>
        <p class="hero__subtitle">
          Kütüphane operasyonlarınızı izleyin, trendleri keşfedin ve stratejik kararlar alın.
        </p>
        <ul class="hero__highlights">
          <li v-for="feature in heroHighlights" :key="feature.label" class="hero__highlight">
            <span class="hero__highlight-dot" :style="{ background: feature.color }"></span>
            {{ feature.label }}
          </li>
        </ul>
      </div>
      <div class="hero__panel">
        <label class="field">
          <span class="field__label">Rapor formatı</span>
          <select v-model="selectedFormat" class="select">
            <option v-for="format in reportFormats" :key="format.value" :value="format.value">
              {{ format.label }}
            </option>
          </select>
        </label>
        <button
          type="button"
          class="btn btn--primary hero__cta"
          :disabled="isRefreshingAll"
          @click="refreshAll"
        >
          <span class="btn__icon">⟳</span>
          Tüm verileri yenile
        </button>
        <p class="hero__panel-note">Seçilen format tüm dışa aktarmalara uygulanır.</p>
      </div>
    </section>

    <transition name="fade">
      <section v-if="errors.length" class="alert alert--error">
        <div class="alert__content">
          <h3 class="alert__title">Hata bildirimi</h3>
          <ul class="alert__list">
            <li v-for="message in errors" :key="message">{{ message }}</li>
          </ul>
        </div>
        <button type="button" class="btn btn--ghost alert__dismiss" @click="clearErrors">
          Temizle
        </button>
      </section>
    </transition>

    <section class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">Özet göstergeler</h2>
          <p class="module__subtitle">Sistem genelindeki güncel metrikler</p>
        </div>
        <div class="module__actions">
          <div class="module__timestamp">
            <span class="module__label">Son güncelleme</span>
            <span class="module__value">{{ formatDateTime(lastUpdated.summaries) }}</span>
          </div>
          <button
            type="button"
            class="btn btn--outline"
            :disabled="isRefreshingSummaries || loading.summaries"
            @click="refreshSummaries"
          >
            <span class="btn__icon">↻</span>
            Yenile
          </button>
        </div>
      </header>

      <div v-if="loading.summaries" class="summary-grid">
        <article v-for="skeleton in 3" :key="`summary-skeleton-${skeleton}`" class="module-card module-card--skeleton">
          <div class="skeleton skeleton--title"></div>
          <div class="metric-grid">
            <div v-for="placeholder in 4" :key="placeholder" class="metric metric--skeleton">
              <div class="skeleton skeleton--text"></div>
              <div class="skeleton skeleton--value"></div>
            </div>
          </div>
        </article>
      </div>

      <div v-else-if="summaryCards.length" class="summary-grid">
        <article v-for="card in summaryCards" :key="card.title" class="module-card">
          <header class="module-card__header">
            <h3 class="module-card__title">{{ card.title }}</h3>
            <span class="tag">{{ card.metrics.length }} metrik</span>
          </header>
          <div class="metric-grid">
            <div v-for="metric in card.metrics" :key="metric.key" class="metric">
              <p class="metric__label">{{ metric.label }}</p>
              <p class="metric__value">{{ metric.value.toLocaleString('tr-TR') }}</p>
            </div>
          </div>
        </article>
      </div>

      <div v-else class="empty-state">
        Henüz özet verisi bulunmuyor.
      </div>
    </section>

    <section v-if="roleDistribution.length" class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">Rol dağılımı</h2>
          <p class="module__subtitle">Toplam {{ totalUsers.toLocaleString('tr-TR') }} kullanıcı</p>
        </div>
        <div class="module__timestamp">
          <span class="module__label">Son güncelleme</span>
          <span class="module__value">{{ formatDateTime(lastUpdated.summaries) }}</span>
        </div>
      </header>
      <div class="distribution-list">
        <div v-for="role in roleDistribution" :key="role.label" class="distribution-item">
          <div class="distribution-item__header">
            <span class="distribution-item__title">{{ role.label }}</span>
            <span class="distribution-item__value">
              {{ role.count.toLocaleString('tr-TR') }} · %{{ role.formattedPercent }}
            </span>
          </div>
          <div class="distribution-item__bar">
            <div class="distribution-item__progress" :style="{ width: `${Math.min(role.percent, 100)}%` }"></div>
          </div>
        </div>
      </div>
    </section>

    <section class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">En çok ödünç verilenler</h2>
          <p class="module__subtitle">Seçilen boyut: {{ dimensionLabels[loanAggregateFilters.dimension] }}</p>
        </div>
        <div class="module__timestamp">
          <span class="module__label">Son güncelleme</span>
          <span class="module__value">{{ formatDateTime(lastUpdated.loanAggregates) }}</span>
        </div>
      </header>

      <div class="filters">
        <div class="filter-group">
          <span class="filter-group__label">Boyut</span>
          <div class="chip-group">
            <button
              v-for="option in dimensionOptions"
              :key="option.value"
              type="button"
              class="chip"
              :class="{ 'chip--active': loanAggregateFilters.dimension === option.value }"
              @click="onSelectAggregateDimension(option.value)"
            >
              {{ option.label }}
            </button>
          </div>
        </div>
        <div class="filter-grid">
          <label class="field">
            <span class="field__label">Top N</span>
            <select v-model.number="loanAggregateFilters.top" class="select">
              <option :value="5">5</option>
              <option :value="10">10</option>
              <option :value="20">20</option>
              <option :value="50">50</option>
            </select>
          </label>
          <label class="field">
            <span class="field__label">Kütüphane</span>
            <select v-model="loanAggregateFilters.kutuphaneId" :disabled="loading.libraries" class="select">
              <option value="">Tümü</option>
              <option v-for="library in libraries" :key="library.id" :value="library.id">
                {{ library.name }}
              </option>
            </select>
          </label>
          <label class="field">
            <span class="field__label">Başlangıç</span>
            <input v-model="loanAggregateFilters.baslangic" type="date" class="input" />
          </label>
          <label class="field">
            <span class="field__label">Bitiş</span>
            <input v-model="loanAggregateFilters.bitis" type="date" class="input" />
          </label>
        </div>
        <div class="button-group">
          <button
            type="button"
            class="btn btn--primary"
            :disabled="loading.loanAggregates"
            @click="applyLoanAggregatesFilters"
          >
            Filtreleri uygula
          </button>
          <button type="button" class="btn btn--ghost" @click="handleResetLoanAggregates">Sıfırla</button>
          <button
            type="button"
            class="btn btn--emerald"
            :disabled="exporting.loanAggregates || loading.loanAggregates || loanAggregates.length === 0"
            @click="exportLoanAggregates"
          >
            Dışa aktar
          </button>
        </div>
      </div>

      <div>
        <div v-if="loading.loanAggregates" class="skeleton-list">
          <div v-for="row in 8" :key="`aggregate-skeleton-${row}`" class="skeleton-row"></div>
        </div>
        <div v-else-if="loanAggregates.length" class="table-wrapper">
          <table class="data-table">
            <thead>
              <tr>
                <th scope="col">Sıra</th>
                <th scope="col">Ad</th>
                <th scope="col">Ödünç sayısı</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(aggregate, index) in loanAggregates" :key="aggregate.entityId ?? `${aggregate.dimension}-${index}`">
                <td class="data-table__rank">#{{ index + 1 }}</td>
                <td>
                  <div class="entity">
                    <span class="entity__title">{{ aggregate.displayName }}</span>
                    <span class="entity__meta">
                      {{ dimensionLabels[aggregate.dimension] }} · ID: {{ aggregate.entityId ?? 'Belirtilmemiş' }}
                    </span>
                  </div>
                </td>
                <td>
                  <span class="badge badge--info">
                    {{ aggregate.loanCount.toLocaleString('tr-TR') }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else class="empty-state">
          Seçilen kriterler için veri bulunamadı. Farklı filtrelerle tekrar deneyin.
        </div>
      </div>
    </section>

    <section class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">Ödünç kullanım istatistikleri</h2>
          <p class="module__subtitle">{{ loanUsageCaption }}</p>
        </div>
        <div class="module__timestamp">
          <span class="module__label">Son güncelleme</span>
          <span class="module__value">{{ formatDateTime(lastUpdated.loanUsage) }}</span>
        </div>
      </header>

      <div class="filter-grid">
        <label class="field">
          <span class="field__label">Kütüphane</span>
          <select v-model="loanUsageFilters.kutuphaneId" :disabled="loading.libraries" class="select">
            <option value="">Tümü</option>
            <option v-for="library in libraries" :key="`usage-lib-${library.id}`" :value="library.id">
              {{ library.name }}
            </option>
          </select>
        </label>
        <label class="field">
          <span class="field__label">Başlangıç</span>
          <input v-model="loanUsageFilters.baslangic" type="date" class="input" />
        </label>
        <label class="field">
          <span class="field__label">Bitiş</span>
          <input v-model="loanUsageFilters.bitis" type="date" class="input" />
        </label>
      </div>

      <div class="button-group">
        <button
          type="button"
          class="btn btn--primary"
          :disabled="loading.loanUsage"
          @click="applyLoanUsageFilters"
        >
          Filtreleri uygula
        </button>
        <button type="button" class="btn btn--ghost" @click="handleResetLoanUsage">Sıfırla</button>
        <button
          type="button"
          class="btn btn--emerald"
          :disabled="exporting.loanUsage || loading.loanUsage || loanUsage.length === 0"
          @click="exportLoanUsage"
        >
          Dışa aktar
        </button>
      </div>

      <div>
        <div v-if="loading.loanUsage" class="skeleton-list">
          <div v-for="row in 8" :key="`usage-skeleton-${row}`" class="skeleton-row"></div>
        </div>
        <div v-else-if="loanUsage.length" class="table-wrapper">
          <table class="data-table">
            <thead>
              <tr>
                <th scope="col">Materyal</th>
                <th scope="col">Toplam ödünç</th>
                <th scope="col">Aktif</th>
                <th scope="col">Geciken</th>
                <th scope="col">İade edildi</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in loanUsage" :key="`${item.kutuphaneId}-${item.materyalId}`">
                <td>
                  <div class="entity">
                    <span class="entity__title">{{ item.materyalBaslik ?? 'Başlık bulunamadı' }}</span>
                    <span class="entity__meta">Materyal ID: {{ item.materyalId }} · Kütüphane ID: {{ item.kutuphaneId }}</span>
                  </div>
                </td>
                <td class="metric__value">{{ item.toplamOdunc.toLocaleString('tr-TR') }}</td>
                <td>{{ item.aktifOdunc.toLocaleString('tr-TR') }}</td>
                <td>
                  <span class="badge badge--warning">{{ item.gecikenOdunc.toLocaleString('tr-TR') }}</span>
                </td>
                <td>{{ item.iadeEdilenOdunc.toLocaleString('tr-TR') }}</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else class="empty-state">
          Seçilen filtrelerle sonuç bulunamadı.
        </div>
      </div>
    </section>

    <section class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">Gecikmiş ödünçler</h2>
          <p class="module__subtitle">Aktif ödünçler içinde son teslim tarihini aşan kayıtlar</p>
        </div>
        <div class="module__timestamp">
          <span class="module__label">Son güncelleme</span>
          <span class="module__value">{{ formatDateTime(lastUpdated.overdueLoans) }}</span>
        </div>
      </header>

      <div class="filter-grid">
        <label class="field">
          <span class="field__label">Kütüphane</span>
          <select v-model="overdueFilters.kutuphaneId" :disabled="loading.libraries" class="select">
            <option value="">Tümü</option>
            <option v-for="library in libraries" :key="`overdue-lib-${library.id}`" :value="library.id">
              {{ library.name }}
            </option>
          </select>
        </label>
        <label class="field">
          <span class="field__label">Kullanıcı ID</span>
          <input v-model="overdueFilters.kullaniciId" type="text" placeholder="Guid veya kimlik" class="input" />
        </label>
      </div>

      <div class="button-group">
        <button
          type="button"
          class="btn btn--primary"
          :disabled="loading.overdueLoans"
          @click="applyOverdueFilters"
        >
          Filtreleri uygula
        </button>
        <button type="button" class="btn btn--ghost" @click="handleResetOverdue">Sıfırla</button>
        <button
          type="button"
          class="btn btn--emerald"
          :disabled="exporting.overdueLoans || loading.overdueLoans || overdueLoans.length === 0"
          @click="exportOverdueLoans"
        >
          Dışa aktar
        </button>
      </div>

      <div>
        <div v-if="loading.overdueLoans" class="skeleton-list">
          <div v-for="row in 8" :key="`overdue-skeleton-${row}`" class="skeleton-row skeleton-row--tall"></div>
        </div>
        <div v-else-if="overdueLoans.length" class="table-wrapper">
          <table class="data-table">
            <thead>
              <tr>
                <th scope="col">Ödünç</th>
                <th scope="col">Kullanıcı</th>
                <th scope="col">Teslim tarihi</th>
                <th scope="col">Gecikme</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="loan in overdueLoans" :key="loan.oduncIslemiId">
                <td>
                  <div class="entity">
                    <span class="entity__title">{{ loan.materyalBaslik ?? 'Başlık bulunamadı' }}</span>
                    <span class="entity__meta">Ödünç ID: {{ loan.oduncIslemiId }} · Nüsha ID: {{ loan.nushaId }}</span>
                  </div>
                </td>
                <td>
                  <div class="entity">
                    <span class="entity__title">{{ loan.kullaniciId }}</span>
                    <span class="entity__meta">Kütüphane ID: {{ loan.kutuphaneId }}</span>
                  </div>
                </td>
                <td>
                  <div class="entity">
                    <span class="entity__title">{{ formatDate(loan.sonTeslimTarihi) }}</span>
                    <span class="entity__meta">Alınma: {{ formatDate(loan.alinmaTarihi) }}</span>
                  </div>
                </td>
                <td>
                  <div class="delay">
                    <span class="badge badge--danger">{{ loan.gecikenGun.toLocaleString('tr-TR') }} gün</span>
                    <div class="delay__bar">
                      <div
                        class="delay__progress"
                        :style="{
                          width: maxOverdueDays
                            ? `${Math.max(10, Math.round((loan.gecikenGun / maxOverdueDays) * 100))}%`
                            : '25%'
                        }"
                      ></div>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else class="empty-state">
          Seçilen kriterler için gecikmiş ödünç bulunmadı.
        </div>
      </div>
    </section>
  </div>
</template>
<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { storeToRefs } from 'pinia'
import apiClient from '@/stores/api'
import { useReportingStore } from '@/stores/reporting'
import { LoanAggregateDimension } from '@/types/reporting'
import { getErrorMessage } from '@/utils/error'

type ReportFormatOption = 'Csv' | 'Excel' | 'Pdf'

const reportingStore = useReportingStore()
const {
  summaryCards,
  topRoles,
  loanAggregates,
  loanUsage,
  overdueLoans,
  loading,
  libraries,
  errors,
  lastUpdated,
  userSummary,
} = storeToRefs(reportingStore)

const loanAggregateFilters = reactive({
  dimension: LoanAggregateDimension.Book,
  top: 10,
  kutuphaneId: '',
  baslangic: '',
  bitis: '',
})

const loanUsageFilters = reactive({
  kutuphaneId: '',
  baslangic: '',
  bitis: '',
})

const overdueFilters = reactive({
  kutuphaneId: '',
  kullaniciId: '',
})

const exporting = reactive({
  loanAggregates: false,
  loanUsage: false,
  overdueLoans: false,
})

const isRefreshingSummaries = ref(false)
const isRefreshingAll = ref(false)

const heroHighlights = [
  { label: 'Özet göstergeler', color: 'linear-gradient(135deg, #34d399, #10b981)' },
  { label: 'Ödünç alma trendleri', color: 'linear-gradient(135deg, #60a5fa, #3b82f6)' },
  { label: 'Gecikmiş işlemler takibi', color: 'linear-gradient(135deg, #f97316, #ef4444)' },
] as const
const reportFormats: Array<{ label: string; value: ReportFormatOption }> = [
  { label: 'CSV', value: 'Csv' },
  { label: 'Excel', value: 'Excel' },
  { label: 'PDF', value: 'Pdf' },
]
const selectedFormat = ref<ReportFormatOption>('Csv')

const dimensionOptions = [
  { label: 'Kitap', value: LoanAggregateDimension.Book },
  { label: 'Yazar', value: LoanAggregateDimension.Author },
  { label: 'Kütüphane', value: LoanAggregateDimension.Library },
]

const dimensionLabels: Record<LoanAggregateDimension, string> = {
  [LoanAggregateDimension.Book]: 'Kitap',
  [LoanAggregateDimension.Author]: 'Yazar',
  [LoanAggregateDimension.Library]: 'Kütüphane',
}

const totalUsers = computed(
  () => userSummary.value?.metrics.find((metric) => metric.key === 'totalUsers')?.value ?? 0
)

const roleDistribution = computed(() =>
  topRoles.value.map((role) => {
    const percent = totalUsers.value > 0 ? (role.count / totalUsers.value) * 100 : 0
    return {
      ...role,
      percent,
      formattedPercent: percent.toLocaleString('tr-TR', { maximumFractionDigits: 1 }),
    }
  })
)

const maxOverdueDays = computed(() => {
  if (overdueLoans.value.length === 0) return 0
  return overdueLoans.value.reduce((max, item) => (item.gecikenGun > max ? item.gecikenGun : max), 0)
})

const totalLoanUsage = computed(() => loanUsage.value.reduce((sum, item) => sum + item.toplamOdunc, 0))

const loanUsageCaption = computed(() => {
  if (loanUsage.value.length === 0) return 'Kayıt bulunamadı'
  return `${loanUsage.value.length} materyal · ${totalLoanUsage.value.toLocaleString('tr-TR')} ödünç`
})

function formatDateTime(iso?: string | null) {
  if (!iso) return 'Henüz güncellenmedi'
  const date = new Date(iso)
  if (Number.isNaN(date.getTime())) return 'Henüz güncellenmedi'
  return date.toLocaleString('tr-TR', { dateStyle: 'medium', timeStyle: 'short' })
}

function formatDate(iso?: string) {
  if (!iso) return '-'
  const date = new Date(iso)
  if (Number.isNaN(date.getTime())) return '-'
  return date.toLocaleDateString('tr-TR', { day: '2-digit', month: 'short', year: 'numeric' })
}

function toStartOfDayIso(value?: string) {
  if (!value) return undefined
  return new Date(`${value}T00:00:00Z`).toISOString()
}

function toEndOfDayIso(value?: string) {
  if (!value) return undefined
  return new Date(`${value}T23:59:59Z`).toISOString()
}

function ensureValidDateRange(start?: string, end?: string) {
  if (start && end) {
    const startDate = new Date(start)
    const endDate = new Date(end)
    if (startDate > endDate) {
      reportingStore.addError('Bitiş tarihi başlangıç tarihinden önce olamaz.')
      return false
    }
  }
  return true
}

async function refreshSummaries() {
  reportingStore.clearErrors()
  isRefreshingSummaries.value = true
  try {
    await reportingStore.fetchSummaries()
  } finally {
    isRefreshingSummaries.value = false
  }
}

async function refreshLoanAggregates() {
  if (!ensureValidDateRange(loanAggregateFilters.baslangic, loanAggregateFilters.bitis)) return
  await reportingStore.fetchLoanAggregates({
    dimension: loanAggregateFilters.dimension,
    top: loanAggregateFilters.top,
    kutuphaneId: loanAggregateFilters.kutuphaneId || undefined,
    baslangicTarihi: toStartOfDayIso(loanAggregateFilters.baslangic),
    bitisTarihi: toEndOfDayIso(loanAggregateFilters.bitis),
  })
}

function resetLoanAggregateFilters() {
  loanAggregateFilters.kutuphaneId = ''
  loanAggregateFilters.baslangic = ''
  loanAggregateFilters.bitis = ''
  loanAggregateFilters.top = 10
}

async function onSelectAggregateDimension(value: LoanAggregateDimension) {
  loanAggregateFilters.dimension = value
  await applyLoanAggregatesFilters()
}

async function applyLoanAggregatesFilters() {
  reportingStore.clearErrors()
  await refreshLoanAggregates()
}

async function handleResetLoanAggregates() {
  resetLoanAggregateFilters()
  await applyLoanAggregatesFilters()
}

async function refreshLoanUsage() {
  if (!ensureValidDateRange(loanUsageFilters.baslangic, loanUsageFilters.bitis)) return
  await reportingStore.fetchLoanUsage({
    kutuphaneId: loanUsageFilters.kutuphaneId || undefined,
    baslangicTarihi: toStartOfDayIso(loanUsageFilters.baslangic),
    bitisTarihi: toEndOfDayIso(loanUsageFilters.bitis),
  })
}

function resetLoanUsageFilters() {
  loanUsageFilters.kutuphaneId = ''
  loanUsageFilters.baslangic = ''
  loanUsageFilters.bitis = ''
}

async function applyLoanUsageFilters() {
  reportingStore.clearErrors()
  await refreshLoanUsage()
}

async function handleResetLoanUsage() {
  resetLoanUsageFilters()
  await applyLoanUsageFilters()
}

async function refreshOverdueLoans() {
  const trimmedUserId = overdueFilters.kullaniciId.trim()
  await reportingStore.fetchOverdueLoans({
    kutuphaneId: overdueFilters.kutuphaneId || undefined,
    kullaniciId: trimmedUserId.length ? trimmedUserId : undefined,
  })
}

function resetOverdueFilters() {
  overdueFilters.kutuphaneId = ''
  overdueFilters.kullaniciId = ''
}

async function applyOverdueFilters() {
  reportingStore.clearErrors()
  await refreshOverdueLoans()
}

async function handleResetOverdue() {
  resetOverdueFilters()
  await applyOverdueFilters()
}

function buildFallbackFileName(base: string, format: ReportFormatOption) {
  const extension = format === 'Excel' ? 'xlsx' : format === 'Pdf' ? 'pdf' : 'csv'
  return `${base}.${extension}`
}

function extractFileName(header?: string) {
  if (!header) return null
  const match = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/.exec(header)
  if (!match) return null
  const [, raw] = match
  if (!raw) return null
  return decodeURIComponent(raw.replace(/['"]/g, '').trim())
}

async function downloadReport(endpoint: string, payload: unknown, fallbackName: string) {
  const format = selectedFormat.value
  const response = await apiClient.post(endpoint, payload, {
    params: { format },
    responseType: 'blob',
  })
  const blob = response.data as Blob
  const fileName =
    extractFileName(response.headers['content-disposition'] as string | undefined) ??
    buildFallbackFileName(fallbackName, format)
  const url = window.URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = fileName
  document.body.appendChild(link)
  link.click()
  link.remove()
  window.URL.revokeObjectURL(url)
}

async function exportLoanAggregates() {
  exporting.loanAggregates = true
  try {
    await downloadReport(
      '/Raporlama/odunc/toplamlar/export',
      {
        Dimension: loanAggregateFilters.dimension,
        Top: loanAggregateFilters.top,
        KutuphaneId: loanAggregateFilters.kutuphaneId || undefined,
        BaslangicTarihi: toStartOfDayIso(loanAggregateFilters.baslangic),
        BitisTarihi: toEndOfDayIso(loanAggregateFilters.bitis),
      },
      'loan-aggregates'
    )
  } catch (error) {
    reportingStore.addError(getErrorMessage(error))
  } finally {
    exporting.loanAggregates = false
  }
}

async function exportLoanUsage() {
  if (!ensureValidDateRange(loanUsageFilters.baslangic, loanUsageFilters.bitis)) return
  exporting.loanUsage = true
  try {
    await downloadReport(
      '/Raporlama/odunc/kullanim/export',
      {
        KutuphaneId: loanUsageFilters.kutuphaneId || undefined,
        BaslangicTarihi: toStartOfDayIso(loanUsageFilters.baslangic),
        BitisTarihi: toEndOfDayIso(loanUsageFilters.bitis),
      },
      'loan-usage-statistics'
    )
  } catch (error) {
    reportingStore.addError(getErrorMessage(error))
  } finally {
    exporting.loanUsage = false
  }
}

async function exportOverdueLoans() {
  exporting.overdueLoans = true
  try {
    await downloadReport(
      '/Raporlama/odunc/gecikmis/export',
      {
        KutuphaneId: overdueFilters.kutuphaneId || undefined,
        KullaniciId: overdueFilters.kullaniciId.trim() || undefined,
      },
      'overdue-loans'
    )
  } catch (error) {
    reportingStore.addError(getErrorMessage(error))
  } finally {
    exporting.overdueLoans = false
  }
}

async function refreshAll() {
  reportingStore.clearErrors()
  isRefreshingAll.value = true
  try {
    await Promise.all([
      reportingStore.fetchSummaries(),
      reportingStore.fetchLibraries(true),
      refreshLoanAggregates(),
      refreshLoanUsage(),
      refreshOverdueLoans(),
    ])
  } finally {
    isRefreshingAll.value = false
  }
}

function clearErrors() {
  reportingStore.clearErrors()
}

onMounted(async () => {
  await reportingStore.fetchLibraries()
  await reportingStore.fetchSummaries()
  await Promise.all([refreshLoanAggregates(), refreshLoanUsage(), refreshOverdueLoans()])
})
</script>
<style scoped>
.reporting-page {
  display: flex;
  flex-direction: column;
  gap: 2.5rem;
  padding: 2.5rem 3rem 4rem;
  background: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
  color: #0f172a;
}

.hero {
  position: relative;
  display: grid;
  gap: 2rem;
  padding: 3rem;
  border-radius: 32px;
  background: linear-gradient(135deg, #2563eb 0%, #7c3aed 100%);
  box-shadow: 0 30px 70px -40px rgba(59, 7, 100, 0.7);
  overflow: hidden;
}

.hero::after {
  content: '';
  position: absolute;
  inset: 0;
  background: radial-gradient(circle at top right, rgba(255, 255, 255, 0.35), transparent 45%);
  pointer-events: none;
}

.hero__content {
  position: relative;
  z-index: 1;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  max-width: 640px;
  color: #fff;
}

.hero__status {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.5rem 1.1rem;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.18);
  font-size: 0.9rem;
  backdrop-filter: blur(6px);
}

.hero__pulse {
  width: 0.55rem;
  height: 0.55rem;
  border-radius: 50%;
  background: #22d3ee;
  box-shadow: 0 0 0 0 rgba(34, 211, 238, 0.7);
  animation: pulse 2s infinite;
}

.hero__title {
  margin: 0;
  font-size: 2.6rem;
  font-weight: 600;
  letter-spacing: -0.02em;
}

.hero__subtitle {
  margin: 0;
  max-width: 520px;
  font-size: 1.05rem;
  line-height: 1.7;
  color: rgba(255, 255, 255, 0.86);
}

.hero__highlights {
  display: flex;
  flex-wrap: wrap;
  gap: 0.85rem;
  list-style: none;
  padding: 0;
  margin: 0;
}

.hero__highlight {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.45rem 1.1rem;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.15);
  font-size: 0.9rem;
  backdrop-filter: blur(6px);
}

.hero__highlight-dot {
  width: 0.65rem;
  height: 0.65rem;
  border-radius: 50%;
  display: inline-block;
}

.hero__panel {
  position: relative;
  z-index: 1;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  padding: 1.8rem;
  border-radius: 24px;
  background: rgba(15, 23, 42, 0.12);
  color: #fff;
  max-width: 280px;
}

.hero__panel-note {
  margin: 0;
  font-size: 0.85rem;
  color: rgba(255, 255, 255, 0.75);
}

.hero__cta {
  width: 100%;
}

.btn__icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  margin-right: 0.5rem;
  font-size: 1rem;
}

.alert {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1.25rem;
  padding: 1.1rem 1.4rem;
  border-radius: 18px;
  border: 1px solid;
}

.alert--error {
  background: rgba(254, 226, 226, 0.7);
  border-color: rgba(248, 113, 113, 0.6);
  color: #b91c1c;
}

.alert__content {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.alert__title {
  margin: 0;
  font-size: 0.95rem;
  font-weight: 600;
}

.alert__list {
  margin: 0;
  padding-left: 1.2rem;
  font-size: 0.9rem;
}

.alert__dismiss {
  white-space: nowrap;
}

.module {
  background: #fff;
  border-radius: 28px;
  border: 1px solid rgba(148, 163, 184, 0.18);
  box-shadow: 0 28px 70px -48px rgba(15, 23, 42, 0.55);
  padding: 1.9rem;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.module__header {
  display: flex;
  flex-wrap: wrap;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1.25rem;
}

.module__title {
  margin: 0;
  font-size: 1.35rem;
  font-weight: 600;
  color: #0f172a;
}

.module__subtitle {
  margin: 0.3rem 0 0;
  font-size: 0.95rem;
  color: #64748b;
}

.module__actions {
  display: flex;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
}

.module__timestamp {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
  font-size: 0.85rem;
  color: #64748b;
}

.module__label {
  font-weight: 600;
  color: #475569;
}

.module__value {
  font-weight: 600;
  color: #0f172a;
}

.summary-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
  gap: 1.2rem;
}

.module-card {
  border-radius: 22px;
  border: 1px solid rgba(148, 163, 184, 0.2);
  padding: 1.4rem;
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
  background: #f8fafc;
}

.module-card__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.75rem;
}

.module-card__title {
  margin: 0;
  font-size: 1.05rem;
  font-weight: 600;
  color: #0f172a;
}

.tag {
  display: inline-flex;
  align-items: center;
  padding: 0.35rem 0.75rem;
  border-radius: 999px;
  background: rgba(59, 130, 246, 0.12);
  color: #1d4ed8;
  font-size: 0.75rem;
  font-weight: 600;
}

.metric-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 1.1rem;
}

.metric {
  display: flex;
  flex-direction: column;
  gap: 0.3rem;
}

.metric__label {
  margin: 0;
  font-size: 0.78rem;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  color: #94a3b8;
  font-weight: 600;
}

.metric__value {
  margin: 0;
  font-size: 1.6rem;
  font-weight: 600;
  color: #0f172a;
}

.module-card--skeleton {
  background: rgba(248, 250, 252, 0.8);
  border-style: dashed;
}

.metric--skeleton {
  gap: 0.45rem;
}

.skeleton {
  position: relative;
  overflow: hidden;
  background: linear-gradient(90deg, rgba(226, 232, 240, 0.65), rgba(203, 213, 225, 0.45), rgba(226, 232, 240, 0.65));
  background-size: 200% 100%;
  animation: shimmer 1.4s ease infinite;
  border-radius: 8px;
}

.skeleton--title {
  height: 1.1rem;
  width: 55%;
}

.skeleton--text {
  height: 0.6rem;
  width: 70%;
}

.skeleton--value {
  height: 0.9rem;
  width: 45%;
}

.distribution-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.distribution-item__header {
  display: flex;
  justify-content: space-between;
  font-size: 0.9rem;
  font-weight: 600;
  color: #0f172a;
}

.distribution-item__value {
  color: #64748b;
  font-weight: 500;
}

.distribution-item__bar {
  position: relative;
  height: 10px;
  border-radius: 999px;
  background: rgba(226, 232, 240, 0.8);
  overflow: hidden;
}

.distribution-item__progress {
  position: absolute;
  inset: 0;
  border-radius: 999px;
  background: linear-gradient(135deg, #60a5fa, #2563eb);
  transition: width 0.3s ease;
}

.filters {
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

.filter-group__label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #475569;
}

.chip-group {
  display: flex;
  flex-wrap: wrap;
  gap: 0.6rem;
  margin-top: 0.6rem;
}

.chip {
  border: 1px solid rgba(148, 163, 184, 0.6);
  background: #fff;
  color: #475569;
  padding: 0.45rem 0.9rem;
  border-radius: 999px;
  font-size: 0.85rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.chip--active {
  background: linear-gradient(135deg, #4f46e5, #7c3aed);
  border-color: transparent;
  color: #fff;
  box-shadow: 0 18px 30px -24px rgba(99, 102, 241, 0.7);
}

.filter-grid {
  display: grid;
  gap: 1rem;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.field__label {
  font-size: 0.84rem;
  font-weight: 600;
  color: #475569;
}

.select,
.input {
  border-radius: 12px;
  border: 1px solid rgba(148, 163, 184, 0.55);
  padding: 0.65rem 0.85rem;
  font-size: 0.95rem;
  color: #1f2937;
  background: #fff;
  transition: border 0.18s ease, box-shadow 0.18s ease;
  font-family: inherit;
}

.select:focus,
.input:focus {
  outline: none;
  border-color: rgba(79, 70, 229, 0.6);
  box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.18);
}

.select:disabled,
.input:disabled {
  background: rgba(226, 232, 240, 0.5);
  color: #94a3b8;
  cursor: not-allowed;
}

.button-group {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
}

.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.35rem;
  border-radius: 12px;
  border: none;
  padding: 0.65rem 1.4rem;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.16s ease, box-shadow 0.16s ease, background 0.16s ease;
}

.btn--primary {
  background: linear-gradient(135deg, #4f46e5, #7c3aed);
  color: #fff;
  box-shadow: 0 18px 30px -24px rgba(79, 70, 229, 0.65);
}

.btn--primary:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 26px 38px -26px rgba(79, 70, 229, 0.6);
}

.btn--ghost {
  background: rgba(15, 23, 42, 0.05);
  color: #1f2937;
}

.btn--ghost:hover:not(:disabled) {
  background: rgba(15, 23, 42, 0.1);
}

.btn--outline {
  border: 1px solid rgba(148, 163, 184, 0.6);
  background: #fff;
  color: #1f2937;
}

.btn--outline:hover:not(:disabled) {
  border-color: rgba(79, 70, 229, 0.6);
  color: #4f46e5;
}

.btn--emerald {
  background: linear-gradient(135deg, #10b981, #047857);
  color: #fff;
  box-shadow: 0 18px 30px -24px rgba(16, 185, 129, 0.6);
}

.btn--emerald:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 26px 38px -26px rgba(16, 185, 129, 0.55);
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}

.table-wrapper {
  overflow-x: auto;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.92rem;
}

.data-table thead {
  background: rgba(99, 102, 241, 0.08);
}

.data-table th,
.data-table td {
  padding: 0.85rem 1rem;
  text-align: left;
  border-bottom: 1px solid rgba(226, 232, 240, 0.9);
}

.data-table__rank {
  font-weight: 600;
  color: #0f172a;
}

.entity {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.entity__title {
  font-weight: 600;
  color: #0f172a;
}

.entity__meta {
  font-size: 0.75rem;
  color: #94a3b8;
}

.badge {
  display: inline-flex;
  align-items: center;
  padding: 0.3rem 0.75rem;
  border-radius: 999px;
  font-size: 0.82rem;
  font-weight: 600;
}

.badge--info {
  background: rgba(219, 234, 254, 0.9);
  color: #1d4ed8;
}

.badge--warning {
  background: rgba(254, 243, 199, 0.9);
  color: #b45309;
}

.badge--danger {
  background: rgba(254, 226, 226, 0.9);
  color: #b91c1c;
}

.delay {
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}

.delay__bar {
  height: 8px;
  border-radius: 999px;
  background: rgba(254, 226, 226, 0.8);
  overflow: hidden;
}

.delay__progress {
  height: 100%;
  border-radius: 999px;
  background: linear-gradient(135deg, #f97316, #ef4444);
  transition: width 0.3s ease;
}

.skeleton-list {
  display: flex;
  flex-direction: column;
  gap: 0.8rem;
}

.skeleton-row {
  height: 48px;
  border-radius: 14px;
  border: 1px solid rgba(226, 232, 240, 0.8);
  background: linear-gradient(90deg, rgba(241, 245, 249, 0.8), rgba(226, 232, 240, 0.6), rgba(241, 245, 249, 0.8));
  background-size: 200% 100%;
  animation: shimmer 1.4s ease infinite;
}

.skeleton-row--tall {
  height: 56px;
}

.empty-state {
  border-radius: 18px;
  border: 1px dashed rgba(148, 163, 184, 0.5);
  padding: 2.2rem;
  text-align: center;
  font-size: 0.92rem;
  color: #64748b;
  background: rgba(248, 250, 252, 0.75);
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@keyframes shimmer {
 0% { background-position: -200% 0; }
 100% { background-position: 200% 0; }
}

@keyframes pulse {
 0% {
   transform: scale(1);
   box-shadow: 0 0 0 0 rgba(34, 211, 238, 0.6);
 }
 70% {
   transform: scale(1.05);
   box-shadow: 0 0 0 8px rgba(34, 211, 238, 0);
 }
 100% {
   transform: scale(1);
   box-shadow: 0 0 0 0 rgba(34, 211, 238, 0);
 }
}

@media (max-width: 1024px) {
  .hero {
    grid-template-columns: 1fr;
  }

  .hero__panel {
    max-width: none;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    gap: 1rem;
  }

  .hero__panel-note {
    text-align: right;
  }
}

@media (max-width: 768px) {
  .reporting-page {
    padding: 1.6rem;
  }

  .hero {
    padding: 2.2rem;
  }

  .hero__panel {
    flex-direction: column;
    align-items: flex-start;
  }

  .module {
    padding: 1.5rem;
  }

  .module__header {
    flex-direction: column;
    align-items: flex-start;
  }

  .button-group {
    width: 100%;
  }

  .button-group .btn {
    flex: 1 1 auto;
  }
}
</style>

