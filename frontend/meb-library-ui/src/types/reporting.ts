export interface SummaryMetric {
  key: string
  label: string
  value: number
}

export interface CatalogSummary {
  metrics: SummaryMetric[]
}

export interface ReservationSummary {
  metrics: SummaryMetric[]
}

export interface RoleCount {
  roleKey: string
  roleLabel: string
  userCount: number
}

export interface UserSummary {
  metrics: SummaryMetric[]
  roleBreakdown: RoleCount[]
}

export interface LoanUsageStatistic {
  kutuphaneId: string
  materyalId: string
  materyalBaslik?: string | null
  toplamOdunc: number
  aktifOdunc: number
  gecikenOdunc: number
  iadeEdilenOdunc: number
}

export interface LoanAggregate {
  dimension: LoanAggregateDimension
  entityId?: string | null
  displayName: string
  loanCount: number
}

export enum LoanAggregateDimension {
  Book = 'Book',
  Author = 'Author',
  Library = 'Library',
}

export interface LoanUsageFilters {
  kutuphaneId?: string | null
  baslangicTarihi?: string | null
  bitisTarihi?: string | null
}

export interface LoanAggregateFilters {
  dimension: LoanAggregateDimension
  top?: number
  kutuphaneId?: string | null
  baslangicTarihi?: string | null
  bitisTarihi?: string | null
}

export interface OverdueLoan {
  oduncIslemiId: string
  kutuphaneId: string
  kullaniciId: string
  nushaId: string
  materyalId?: string | null
  materyalBaslik?: string | null
  alinmaTarihi: string
  sonTeslimTarihi: string
  gecikenGun: number
}

export interface OverdueLoanFilters {
  kutuphaneId?: string | null
  kullaniciId?: string | null
}
