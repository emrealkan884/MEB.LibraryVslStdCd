export enum LoanStatus {
  Aktif = 1,
  IadeEdildi = 2,
  Gecikmis = 3,
  Iptal = 4,
}

export type LoanFilterStatus = 'open' | 'all' | 'active' | 'overdue' | 'returned'

export interface LoanListItem {
  id: string
  kutuphaneId: string
  kullaniciId: string
  nushaId: string
  alinmaTarihi: string
  sonTeslimTarihi: string
  iadeTarihi?: string | null
  durumu: LoanStatus
  not?: string | null
}

export interface LoanSummaryMetrics {
  active: number
  overdue: number
  returnedToday: number
  total: number
}

export interface LoanPagination {
  pageIndex: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasNext: boolean
  hasPrevious: boolean
}

export interface LoanFilters {
  status: LoanFilterStatus
  kutuphaneId?: string | null
  kullaniciId?: string | null
  nushaId?: string | null
  search?: string
}

export interface CreateLoanPayload {
  kutuphaneId: string
  kullaniciId: string
  nushaId: string
  oduncSuresiGun?: number | null
  not?: string | null
}

export interface ExtendLoanPayload {
  id: string
  ekGun: number
  uzatmaNedeni?: string | null
}

export interface ReturnLoanPayload {
  id: string
  iadeNotu?: string | null
}

export const loanStatusLabels: Record<LoanStatus, string> = {
  [LoanStatus.Aktif]: 'Aktif',
  [LoanStatus.IadeEdildi]: 'Iade edildi',
  [LoanStatus.Gecikmis]: 'Gecikmis',
  [LoanStatus.Iptal]: 'Iptal',
}

export const defaultPagination: LoanPagination = {
  pageIndex: 0,
  pageSize: 20,
  totalCount: 0,
  totalPages: 0,
  hasNext: false,
  hasPrevious: false,
}
