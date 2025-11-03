import { defineStore } from 'pinia'
import apiClient from './api'
import { getErrorMessage } from '@/utils/error'
import { LoanStatus, defaultPagination } from '@/types/circulation'
import type {
  LoanListItem,
  LoanFilters,
  LoanPagination,
  LoanSummaryMetrics,
  LoanFilterStatus,
  CreateLoanPayload,
  ExtendLoanPayload,
  ReturnLoanPayload,
} from '@/types/circulation'

interface LoadingFlags {
  list: boolean
  action: boolean
}

interface CirculationState {
  items: LoanListItem[]
  loading: LoadingFlags
  errors: string[]
  pagination: LoanPagination
  filters: LoanFilters
  lastFetchedAt: string | null
}

function normalizeStatus(value: unknown): LoanStatus {
  if (typeof value === 'number') {
    if (value in LoanStatus) {
      return value as LoanStatus
    }
    return LoanStatus.Aktif
  }

  const parsed = Number.parseInt(String(value ?? ''), 10)
  if (!Number.isNaN(parsed) && parsed in LoanStatus) {
    return parsed as LoanStatus
  }

  return LoanStatus.Aktif
}

function asIsoString(value: unknown): string {
  if (!value) return ''
  return typeof value === 'string' ? value : String(value)
}

function normalizeLoan(raw: any): LoanListItem {
  return {
    id: raw?.id ?? raw?.Id ?? '',
    kutuphaneId: raw?.kutuphaneId ?? raw?.KutuphaneId ?? '',
    kullaniciId: raw?.kullaniciId ?? raw?.KullaniciId ?? '',
    nushaId: raw?.nushaId ?? raw?.NushaId ?? '',
    alinmaTarihi: asIsoString(raw?.alinmaTarihi ?? raw?.AlinmaTarihi),
    sonTeslimTarihi: asIsoString(raw?.sonTeslimTarihi ?? raw?.SonTeslimTarihi),
    iadeTarihi: raw?.iadeTarihi ?? raw?.IadeTarihi ?? null,
    durumu: normalizeStatus(raw?.durumu ?? raw?.Durumu),
    not: raw?.not ?? raw?.Not ?? null,
  }
}

function buildDynamicQuery(filters: LoanFilters) {
  const dynamicQuery: Record<string, any> = {}
  const filterItems: any[] = []

  if (filters.kutuphaneId) {
    filterItems.push({ Field: 'KutuphaneId', Operator: 'eq', Value: filters.kutuphaneId })
  }

  if (filters.kullaniciId) {
    filterItems.push({ Field: 'KullaniciId', Operator: 'eq', Value: filters.kullaniciId })
  }

  if (filters.nushaId) {
    filterItems.push({ Field: 'NushaId', Operator: 'eq', Value: filters.nushaId })
  }

  if (filters.search && filters.search.trim().length > 0) {
    filterItems.push({
      Field: 'Not',
      Operator: 'contains',
      Value: filters.search.trim(),
    })
  }

  if (filterItems.length === 1) {
    dynamicQuery.Filter = filterItems[0]
  } else if (filterItems.length > 1) {
    dynamicQuery.Filter = {
      Field: 'Id',
      Operator: 'isnotnull',
      Logic: 'and',
      Filters: filterItems,
    }
  }

  dynamicQuery.Sort = [{ Field: 'AlinmaTarihi', Dir: 'desc' }]

  return dynamicQuery
}

function collectSummary(items: LoanListItem[]): LoanSummaryMetrics {
  const todayKey = new Date().toISOString().slice(0, 10)
  let active = 0
  let overdue = 0
  let returnedToday = 0

  for (const loan of items) {
    if (loan.durumu === LoanStatus.Aktif) {
      active += 1
    } else if (loan.durumu === LoanStatus.Gecikmis) {
      overdue += 1
    } else if (loan.durumu === LoanStatus.IadeEdildi) {
      const dateKey = loan.iadeTarihi ? String(loan.iadeTarihi).slice(0, 10) : ''
      if (dateKey === todayKey) {
        returnedToday += 1
      }
    }
  }

  return {
    active,
    overdue,
    returnedToday,
    total: items.length,
  }
}

export const useCirculationStore = defineStore('circulation', {
  state: (): CirculationState => ({
    items: [],
    loading: {
      list: false,
      action: false,
    },
    errors: [],
    pagination: { ...defaultPagination },
    filters: {
      status: 'open',
      kutuphaneId: null,
      kullaniciId: null,
      nushaId: null,
      search: '',
    },
    lastFetchedAt: null,
  }),
  getters: {
    filteredLoans(state): LoanListItem[] {
      const status = state.filters.status
      if (!state.items.length) return []

      const matchesStatus = (loan: LoanListItem): boolean => {
        switch (status) {
          case 'open':
            return loan.durumu === LoanStatus.Aktif || loan.durumu === LoanStatus.Gecikmis
          case 'active':
            return loan.durumu === LoanStatus.Aktif
          case 'overdue':
            return loan.durumu === LoanStatus.Gecikmis
          case 'returned':
            return loan.durumu === LoanStatus.IadeEdildi
          case 'all':
          default:
            return true
        }
      }

      return state.items.filter(matchesStatus)
    },
    summaryMetrics(state): LoanSummaryMetrics {
      return collectSummary(state.items)
    },
    recentLoans(state): LoanListItem[] {
      if (!state.items.length) return []

      const getTimestamp = (loan: LoanListItem) => {
        const candidate = loan.iadeTarihi ?? loan.alinmaTarihi ?? loan.sonTeslimTarihi
        const date = candidate ? new Date(candidate) : null
        return date && !Number.isNaN(date.getTime()) ? date.getTime() : 0
      }

      return [...state.items].sort((a, b) => getTimestamp(b) - getTimestamp(a)).slice(0, 6)
    },
    statusCounts(state): Record<LoanFilterStatus, number> {
      const counts: Record<LoanFilterStatus, number> = {
        open: 0,
        active: 0,
        overdue: 0,
        returned: 0,
        all: state.items.length,
      }

      for (const loan of state.items) {
        if (loan.durumu === LoanStatus.Aktif) {
          counts.active += 1
          counts.open += 1
        } else if (loan.durumu === LoanStatus.Gecikmis) {
          counts.overdue += 1
          counts.open += 1
        } else if (loan.durumu === LoanStatus.IadeEdildi) {
          counts.returned += 1
        }
      }

      return counts
    },
    hasErrors(state): boolean {
      return state.errors.length > 0
    },
  },
  actions: {
    setFilter(partial: Partial<LoanFilters>) {
      this.filters = { ...this.filters, ...partial }
    },
    resetFilters() {
      this.filters = {
        status: 'open',
        kutuphaneId: null,
        kullaniciId: null,
        nushaId: null,
        search: '',
      }
    },
    clearErrors() {
      this.errors = []
    },
    addError(message: string) {
      if (!message) return
      if (!this.errors.includes(message)) {
        this.errors.push(message)
      }
    },
    async fetchLoans(options?: { pageIndex?: number; pageSize?: number; filters?: Partial<LoanFilters> }) {
      if (options?.filters) {
        this.filters = { ...this.filters, ...options.filters }
      }

      if (typeof options?.pageSize === 'number' && options.pageSize > 0) {
        this.pagination.pageSize = options.pageSize
      }

      if (typeof options?.pageIndex === 'number' && options.pageIndex >= 0) {
        this.pagination.pageIndex = options.pageIndex
      }

      this.loading.list = true
      this.clearErrors()

      try {
        const params = {
          'PageRequest.PageIndex': this.pagination.pageIndex,
          'PageRequest.PageSize': this.pagination.pageSize,
        }

        const response = await apiClient.post('/OduncIslemleri/GetListByDynamic', buildDynamicQuery(this.filters), {
          params,
        })

        const data = response.data ?? {}
        const rawItems = data.items ?? data.Items ?? []

        this.items = rawItems.map(normalizeLoan)

        const pageIndex = data.index ?? data.pageIndex ?? this.pagination.pageIndex ?? 0
        const pageSize = data.size ?? data.pageSize ?? this.pagination.pageSize ?? defaultPagination.pageSize
        const totalCount = data.count ?? data.totalCount ?? rawItems.length
        const totalPages =
          data.pages ??
          data.totalPages ??
          (pageSize > 0 ? Math.ceil(totalCount / pageSize) : defaultPagination.totalPages)

        this.pagination = {
          pageIndex,
          pageSize,
          totalCount,
          totalPages,
          hasNext: data.hasNext ?? data.HasNext ?? pageIndex + 1 < totalPages,
          hasPrevious: data.hasPrevious ?? data.HasPrevious ?? pageIndex > 0,
        }

        this.lastFetchedAt = new Date().toISOString()
      } catch (error) {
        this.addError(getErrorMessage(error))
        this.items = []
        this.pagination = {
          ...this.pagination,
          totalCount: 0,
          totalPages: 0,
          hasNext: false,
          hasPrevious: false,
        }
      } finally {
        this.loading.list = false
      }
    },
    async refresh() {
      await this.fetchLoans({ pageIndex: this.pagination.pageIndex })
    },
    async changePageSize(size: number) {
      if (size <= 0) return
      await this.fetchLoans({ pageIndex: 0, pageSize: size })
    },
    async goToPage(index: number) {
      if (index < 0 || index === this.pagination.pageIndex) return
      await this.fetchLoans({ pageIndex: index })
    },
    async createLoan(payload: CreateLoanPayload) {
      this.loading.action = true
      this.clearErrors()
      try {
        await apiClient.post('/OduncIslemleri', {
          kutuphaneId: payload.kutuphaneId,
          kullaniciId: payload.kullaniciId,
          nushaId: payload.nushaId,
          oduncSuresiGun: payload.oduncSuresiGun ?? undefined,
          not: payload.not ?? undefined,
        })

        await this.fetchLoans({ pageIndex: 0 })
        return true
      } catch (error) {
        this.addError(getErrorMessage(error))
        return false
      } finally {
        this.loading.action = false
      }
    },
    async returnLoan(payload: ReturnLoanPayload) {
      this.loading.action = true
      this.clearErrors()
      try {
        const { id, ...body } = payload
        await apiClient.put(`/OduncIslemleri/${id}/iade`, body)
        await this.refresh()
        return true
      } catch (error) {
        this.addError(getErrorMessage(error))
        return false
      } finally {
        this.loading.action = false
      }
    },
    async extendLoan(payload: ExtendLoanPayload) {
      this.loading.action = true
      this.clearErrors()
      try {
        const { id, ...body } = payload
        await apiClient.put(`/OduncIslemleri/${id}/extend`, body)
        await this.refresh()
        return true
      } catch (error) {
        this.addError(getErrorMessage(error))
        return false
      } finally {
        this.loading.action = false
      }
    },
  },
})
