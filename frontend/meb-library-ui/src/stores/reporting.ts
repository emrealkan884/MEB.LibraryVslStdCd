import { defineStore } from 'pinia'
import apiClient from './api'
import { getErrorMessage } from '@/utils/error'
import type {
  CatalogSummary,
  LoanAggregate,
  LoanAggregateDimension,
  LoanAggregateFilters,
  LoanUsageFilters,
  LoanUsageStatistic,
  OverdueLoan,
  OverdueLoanFilters,
  ReservationSummary,
  SummaryMetric,
  UserSummary,
} from '@/types/reporting'

interface LibraryOption {
  id: string
  name: string
  type?: string
}

interface ReportingState {
  catalogSummary: CatalogSummary | null
  reservationSummary: ReservationSummary | null
  userSummary: UserSummary | null
  loanUsage: LoanUsageStatistic[]
  loanAggregates: LoanAggregate[]
  overdueLoans: OverdueLoan[]
  libraries: LibraryOption[]
  loading: {
    summaries: boolean
    loanUsage: boolean
    loanAggregates: boolean
    overdueLoans: boolean
    libraries: boolean
  }
  errors: string[]
  lastUpdated: {
    summaries: string | null
    loanUsage: string | null
    loanAggregates: string | null
    overdueLoans: string | null
  }
}

function isoStringOrNull(value?: string | null): string | undefined {
  if (!value) return undefined
  return value
}

function toQueryParams(filters: Record<string, string | number | undefined | null>) {
  return Object.fromEntries(
    Object.entries(filters).filter(([, value]) => value !== undefined && value !== null && value !== '')
  )
}

export const useReportingStore = defineStore('reporting', {
  state: (): ReportingState => ({
    catalogSummary: null,
    reservationSummary: null,
    userSummary: null,
    loanUsage: [],
    loanAggregates: [],
    overdueLoans: [],
    libraries: [],
    loading: {
      summaries: false,
      loanUsage: false,
      loanAggregates: false,
      overdueLoans: false,
      libraries: false,
    },
    errors: [],
    lastUpdated: {
      summaries: null,
      loanUsage: null,
      loanAggregates: null,
      overdueLoans: null,
    },
  }),
  getters: {
    summaryCards(state): ReadonlyArray<{ title: string; metrics: SummaryMetric[] }> {
      const cards = []
      if (state.catalogSummary) {
        cards.push({ title: 'Katalog Özeti', metrics: state.catalogSummary.metrics })
      }
      if (state.userSummary) {
        cards.push({ title: 'Kullanıcı Özeti', metrics: state.userSummary.metrics })
      }
      if (state.reservationSummary) {
        cards.push({ title: 'Rezervasyon Özeti', metrics: state.reservationSummary.metrics })
      }
      return cards
    },
    topRoles(state): ReadonlyArray<{ label: string; count: number }> {
      if (!state.userSummary) return []
      return state.userSummary.roleBreakdown.map((role) => ({
        label: role.roleLabel || role.roleKey,
        count: role.userCount,
      }))
    },
    hasAnyData(state): boolean {
      return (
        !!state.catalogSummary ||
        !!state.userSummary ||
        !!state.reservationSummary ||
        state.loanUsage.length > 0 ||
        state.loanAggregates.length > 0 ||
        state.overdueLoans.length > 0
      )
    },
  },
  actions: {
    addError(message: string) {
      if (!message) return
      if (!this.errors.includes(message)) {
        this.errors.push(message)
      }
    },
    clearErrors() {
      this.errors = []
    },
    async fetchLibraries(force = false) {
      if (this.libraries.length > 0 && !force) return
      this.loading.libraries = true
      try {
        const response = await apiClient.get('/Kutuphaneler', {
          params: {
            'PageRequest.PageIndex': 0,
            'PageRequest.PageSize': 100,
          },
        })
        const data = response.data as {
          items?: Array<{ id: string; ad: string; tip?: string }>
          Items?: Array<{ Id: string; Ad: string; Tip?: string }>
        }
        const items =
          data.items ??
          data.Items?.map((item) => ({
            id: (item as { Id: string }).Id,
            ad: (item as { Ad: string }).Ad,
            tip: (item as { Tip?: string }).Tip,
          })) ??
          []
        this.libraries = items.map((item: any) => ({
          id: item.id ?? item.Id,
          name: item.ad ?? item.Ad,
          type: item.tip ?? item.Tip,
        }))
      } catch (error) {
        this.addError(getErrorMessage(error))
      } finally {
        this.loading.libraries = false
      }
    },
    async fetchSummaries() {
      this.loading.summaries = true
      this.clearErrors()
      try {
        const [catalog, reservation, user] = await Promise.all([
          apiClient.get<CatalogSummary>('/Raporlama/katalog/ozet'),
          apiClient.get<ReservationSummary>('/Raporlama/rezervasyon/ozet'),
          apiClient.get<UserSummary>('/Raporlama/kullanici/ozet'),
        ])
        this.catalogSummary = catalog.data
        this.reservationSummary = reservation.data
        this.userSummary = user.data
        const now = new Date().toISOString()
        this.lastUpdated.summaries = now
      } catch (error) {
        this.addError(getErrorMessage(error))
      } finally {
        this.loading.summaries = false
      }
    },
    async fetchLoanUsage(filters: LoanUsageFilters = {}) {
      this.loading.loanUsage = true
      try {
        const params = toQueryParams({
          KutuphaneId: filters.kutuphaneId,
          BaslangicTarihi: isoStringOrNull(filters.baslangicTarihi),
          BitisTarihi: isoStringOrNull(filters.bitisTarihi),
        })
        const response = await apiClient.get<LoanUsageStatistic[]>('/Raporlama/odunc/kullanim', {
          params,
        })
        this.loanUsage = response.data
        this.lastUpdated.loanUsage = new Date().toISOString()
      } catch (error) {
        this.addError(getErrorMessage(error))
      } finally {
        this.loading.loanUsage = false
      }
    },
    async fetchLoanAggregates(filters: LoanAggregateFilters) {
      this.loading.loanAggregates = true
      try {
        const params = toQueryParams({
          Dimension: filters.dimension,
          Top: filters.top,
          KutuphaneId: filters.kutuphaneId,
          BaslangicTarihi: isoStringOrNull(filters.baslangicTarihi),
          BitisTarihi: isoStringOrNull(filters.bitisTarihi),
        })
        const response = await apiClient.get<LoanAggregate[]>('/Raporlama/odunc/toplamlar', { params })
        this.loanAggregates = response.data
        this.lastUpdated.loanAggregates = new Date().toISOString()
      } catch (error) {
        this.addError(getErrorMessage(error))
      } finally {
        this.loading.loanAggregates = false
      }
    },
    async fetchOverdueLoans(filters: OverdueLoanFilters = {}) {
      this.loading.overdueLoans = true
      try {
        const params = toQueryParams({
          KutuphaneId: filters.kutuphaneId,
          KullaniciId: filters.kullaniciId,
        })
        const response = await apiClient.get<OverdueLoan[]>('/Raporlama/odunc/gecikmis', {
          params,
        })
        this.overdueLoans = response.data
        this.lastUpdated.overdueLoans = new Date().toISOString()
      } catch (error) {
        this.addError(getErrorMessage(error))
      } finally {
        this.loading.overdueLoans = false
      }
    },
    resetOverdueLoans() {
      this.overdueLoans = []
      this.lastUpdated.overdueLoans = null
    },
    resetLoanUsage() {
      this.loanUsage = []
      this.lastUpdated.loanUsage = null
    },
    resetLoanAggregates() {
      this.loanAggregates = []
      this.lastUpdated.loanAggregates = null
    },
  },
})
