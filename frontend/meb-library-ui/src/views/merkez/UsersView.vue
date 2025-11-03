<template>
  <div class="users-page">
    <section class="hero">
      <div class="hero__main">
        <h1>Kullanıcı Yönetimi</h1>
        <p>Kullanıcı hesaplarını yönetin, rolleri güncelleyin ve erişimleri kontrol edin.</p>
        <dl class="hero__metrics">
          <div>
            <dt>Aktif kullanıcı</dt>
            <dd>{{ metrics.activeCount }}</dd>
          </div>
          <div>
            <dt>Toplam kullanıcı</dt>
            <dd>{{ totalCount }}</dd>
          </div>
          <div>
            <dt>Son güncelleme</dt>
            <dd>{{ lastUpdatedLabel }}</dd>
          </div>
        </dl>
      </div>
      <div class="hero__actions">
        <button type="button" class="btn btn--primary" @click="openCreateModal">
          <span class="btn__icon">+</span>
          Yeni kullanıcı
        </button>
      </div>
    </section>

    <transition name="alert">
      <div
        v-if="feedback.message"
        class="alert"
        :class="feedback.kind === 'error' ? 'alert--error' : 'alert--success'"
      >
        <strong>{{ feedback.kind === 'error' ? 'İşlem başarısız' : 'İşlem başarılı' }}</strong>
        <span>{{ feedback.message }}</span>
      </div>
    </transition>

    <section class="panel">
      <header class="panel__header">
        <div>
          <h2>Filtreler</h2>
          <p>Rol, durum ve ada göre arama yapın.</p>
        </div>
        <button type="button" class="btn btn--ghost" @click="resetFilters" :disabled="loading">Sıfırla</button>
      </header>
      <div class="filters">
        <label>
          <span>Rol</span>
          <select v-model.number="filterForm.role" :disabled="loading || roleOptions.length === 0">
            <option :value="0">Tümü</option>
            <option v-for="claim in roleOptions" :key="claim.id" :value="claim.id">
              {{ getRoleLabel(claim.name) }}
            </option>
          </select>
        </label>
        <label>
          <span>Durum</span>
          <select v-model="filterForm.status" :disabled="loading">
            <option value="all">Tümü</option>
            <option value="active">Aktif</option>
            <option value="inactive">Pasif</option>
          </select>
        </label>
        <label class="filters__search">
          <span>Arama</span>
          <input
            type="text"
            v-model.trim="filterForm.search"
            placeholder="Ad, soyad veya e-posta"
            :disabled="loading"
          />
        </label>
        <div class="filters__actions">
          <button type="button" class="btn btn--primary" @click="applyFilters" :disabled="loading">
            Filtreleri uygula
          </button>
        </div>
      </div>
    </section>

    <section class="panel">
      <header class="panel__header">
        <div>
          <h2>Kullanıcı listesi</h2>
          <p>Son güncelleme: {{ lastUpdatedLabel }}</p>
        </div>
      </header>

      <div v-if="error" class="message message--error">
        <strong>Bir hata oluştu.</strong>
        <span>{{ error }}</span>
      </div>

      <div v-else class="table-wrapper" :class="{ 'table-wrapper--loading': loading }">
        <table class="users-table">
          <thead>
            <tr>
              <th>Kullanıcı</th>
              <th>Roller</th>
              <th>Durum</th>
              <th class="table-actions">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="loading">
              <td colspan="4" class="table-state">Kullanıcılar yükleniyor...</td>
            </tr>
            <tr v-else-if="visibleUsers.length === 0">
              <td colspan="4" class="table-state">Kayıt bulunamadı.</td>
            </tr>
            <tr v-for="user in visibleUsers" :key="user.id">
              <td>
                <div class="user-cell">
                  <span class="user-cell__name">{{ user.firstName }} {{ user.lastName }}</span>
                  <span class="user-cell__meta">{{ user.email }}</span>
                </div>
              </td>
              <td>
                <div class="role-chips">
                  <span v-for="role in getUserRoleLabels(user.id)" :key="user.id + '-' + role" class="chip">
                    {{ role }}
                  </span>
                  <span v-if="getUserRoleLabels(user.id).length === 0" class="role-chips__empty">Rol atanmadı</span>
                </div>
              </td>
              <td>
                <span class="status" :class="user.status ? 'status--active' : 'status--passive'">
                  {{ user.status ? 'Aktif' : 'Pasif' }}
                </span>
              </td>
              <td class="table-actions">
                <button type="button" class="btn btn--ghost btn--sm" @click="openEditModal(user)" :disabled="loading">
                  Düzenle
                </button>
                <button
                  type="button"
                  class="btn btn--danger btn--sm"
                  @click="deleteUser(user)"
                  :disabled="pendingDeleteId === user.id || loading"
                >
                  {{ pendingDeleteId === user.id ? 'Siliniyor...' : 'Sil' }}
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <footer class="table-footer" v-if="totalPages > 1 && !loading">
        <span>Sayfa {{ page + 1 }} / {{ totalPages }} · {{ totalCount }} kayıt</span>
        <div class="table-footer__actions">
          <button type="button" class="btn btn--ghost btn--sm" @click="prevPage" :disabled="page === 0">
            Önceki
          </button>
          <button
            type="button"
            class="btn btn--ghost btn--sm"
            @click="nextPage"
            :disabled="page + 1 >= totalPages"
          >
            Sonraki
          </button>
        </div>
      </footer>
    </section>

    <transition name="modal">
      <div v-if="modal.visible" class="modal-backdrop">
        <div class="modal" role="dialog" aria-modal="true">
          <header class="modal__header">
            <h3>{{ modal.mode === 'create' ? 'Yeni kullanıcı' : 'Kullanıcıyı düzenle' }}</h3>
            <button type="button" class="modal__close" @click="closeModal" :disabled="modal.saving" aria-label="Kapat">
              ×
            </button>
          </header>
          <form class="modal__body" @submit.prevent="submitModal">
            <div class="form-grid">
              <label>
                <span>Ad *</span>
                <input type="text" v-model.trim="modal.firstName" :disabled="modal.saving" required />
              </label>
              <label>
                <span>Soyad *</span>
                <input type="text" v-model.trim="modal.lastName" :disabled="modal.saving" required />
              </label>
              <label class="form-grid__full">
                <span>E-posta *</span>
                <input type="email" v-model.trim="modal.email" :disabled="modal.saving" required />
              </label>
              <label class="form-grid__full">
                <span>Şifre *</span>
                <input
                  type="password"
                  v-model="modal.password"
                  minlength="4"
                  autocomplete="new-password"
                  :placeholder="modal.mode === 'create' ? 'En az 4 karakter' : 'Güncelleme için yeni şifre yazın'"
                  :disabled="modal.saving"
                  required
                />
                <small>Güncelleme sırasında yeni bir şifre belirlenmesi gerekir.</small>
              </label>
              <fieldset class="form-grid__full">
                <legend>Roller</legend>
                <div class="role-toolbar">
                  <div class="role-toolbar__search">
                    <input
                      type="text"
                      v-model.trim="modalRoleSearch"
                      :disabled="modal.saving"
                      placeholder="Rol adıyla ara"
                    />
                  </div>
                  <span class="role-toolbar__count">
                    {{ modal.roles.length }} rol seçili
                  </span>
                </div>
                <div v-if="filteredRoleGroups.length > 0" class="role-grid">
                  <article class="role-card" v-for="group in filteredRoleGroups" :key="group.key">
                    <header class="role-card__header">
                      <h4>{{ group.label }}</h4>
                      <span>{{ group.items.length }} rol</span>
                    </header>
                    <div class="role-card__list">
                      <label v-for="claim in group.items" :key="claim.id" class="role-card__item">
                        <input type="checkbox" :value="claim.id" v-model="modal.roles" :disabled="modal.saving" />
                        <span>{{ getRoleLabel(claim.name) }}</span>
                      </label>
                    </div>
                  </article>
                </div>
                <div v-else class="role-empty">
                  <strong>Rol bulunamadı.</strong>
                  <span>{{ roleOptions.length === 0 ? 'Tanımlı rol bulunamadı.' : 'Aramanızı değiştirerek tekrar deneyin.' }}</span>
                </div>
              </fieldset>
            </div>
            <p v-if="modal.error" class="modal__error">{{ modal.error }}</p>
            <div class="modal__actions">
              <button type="button" class="btn btn--ghost" @click="closeModal" :disabled="modal.saving">Vazgeç</button>
              <button type="submit" class="btn btn--primary" :disabled="modal.saving">
                {{ modal.saving ? 'Kaydediliyor...' : modal.mode === 'create' ? 'Kullanıcı oluştur' : 'Kaydet' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import type { AxiosError } from 'axios'
import apiClient from '@/stores/api'

interface UserListItem {
  id: string
  firstName: string
  lastName: string
  email: string
  status: boolean
}

interface OperationClaim {
  id: number
  name: string
}

interface UserOperationClaim {
  id: string
  userId: string
  operationClaimId: number
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

type FilterStatus = 'all' | 'active' | 'inactive'

type MaybePagedResponse<T> = Partial<PagedResponse<T>> & {
  Items?: T[]
  Count?: number
  Pages?: number
}

interface FilterState {
  role: number
  status: FilterStatus
  search: string
}

interface RoleGroup {
  key: string
  label: string
  items: OperationClaim[]
}

type DynamicFilter = Record<string, unknown>

const users = ref<UserListItem[]>([])
const userOperationClaims = ref<UserOperationClaim[]>([])
const roleOptions = ref<OperationClaim[]>([])

const totalCount = ref(0)
const totalPages = ref(0)
const page = ref(0)
const pageSize = ref(20)
const lastUpdated = ref<string | null>(null)

const loading = ref(false)
const error = ref<string | null>(null)
const pendingDeleteId = ref<string | null>(null)

const feedback = reactive<{ message: string; kind: 'success' | 'error' | '' }>({
  message: '',
  kind: ''
})

const metrics = reactive<{ activeCount: number }>({ activeCount: 0 })

const filterForm = reactive<FilterState>({
  role: 0,
  status: 'all',
  search: ''
})

const filters = ref<FilterState>({
  role: 0,
  status: 'all',
  search: ''
})

const modalRoleSearch = ref('')

const modal = reactive({
  visible: false,
  mode: 'create' as 'create' | 'edit',
  saving: false,
  error: '',
  id: '',
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  roles: [] as number[]
})

const ROLE_LABELS: Record<string, string> = {
  'Role.SistemYoneticisi': 'Sistem Yöneticisi',
  'Role.BakanlikYetkilisi': 'Bakanlık Yetkilisi',
  'Role.IlYetkilisi': 'İl Yetkilisi',
  'Role.IlceYetkilisi': 'İlçe Yetkilisi',
  'Role.OkulKutuphaneYoneticisi': 'Okul Kütüphane Yöneticisi'
}

const roleNameById = computed(() => {
  const map = new Map<number, string>()
  for (const option of roleOptions.value) {
    map.set(option.id, option.name)
  }
  return map
})

const userRoleRecordsMap = computed<Record<string, UserOperationClaim[]>>(() => {
  const map: Record<string, UserOperationClaim[]> = {}
  for (const record of userOperationClaims.value) {
    const list = map[record.userId]
    if (list) {
      list.push(record)
    } else {
      map[record.userId] = [record]
    }
  }
  return map
})

const userRoleLabelsMap = computed<Record<string, string[]>>(() => {
  const result: Record<string, string[]> = {}
  const nameMap = roleNameById.value
  for (const [userId, records] of Object.entries(userRoleRecordsMap.value)) {
    result[userId] = records.map(record => {
      const roleName = nameMap.get(record.operationClaimId) ?? 'Rol ' + record.operationClaimId.toString()
      return getRoleLabel(roleName)
    })
  }
  return result
})

const visibleUsers = computed(() => {
  return users.value.filter(user => {
    if (filters.value.status === 'active' && !user.status) {
      return false
    }
    if (filters.value.status === 'inactive' && user.status) {
      return false
    }
    if (filters.value.role && filters.value.role !== 0) {
      const roleRecords = userRoleRecordsMap.value[user.id] ?? []
      const hasRole = roleRecords.some(record => record.operationClaimId === filters.value.role)
      if (!hasRole) {
        return false
      }
    }
    if (filters.value.search) {
      const query = filters.value.search.toLowerCase()
      const fullName = (user.firstName + ' ' + user.lastName).toLowerCase()
      const email = user.email.toLowerCase()
      if (!fullName.includes(query) && !email.includes(query)) {
        return false
      }
    }
    return true
  })
})

const lastUpdatedLabel = computed(() => {
  if (!lastUpdated.value) {
    return 'Henüz güncellenmedi'
  }
  return new Intl.DateTimeFormat('tr-TR', {
    dateStyle: 'medium',
    timeStyle: 'short'
  }).format(new Date(lastUpdated.value))
})

const getRoleLabel = (roleKey: string): string => {
  const mapped = ROLE_LABELS[roleKey]
  if (mapped) {
    return mapped
  }
  if (roleKey.startsWith('Role.')) {
    const raw = roleKey.slice(5)
    return raw.replace(/([A-Z])/g, ' $1').trim()
  }
  return roleKey
}

const extractPagedPayload = <T>(
  payload: MaybePagedResponse<T> | undefined | null
): MaybePagedResponse<T> | undefined | null => {
  if (!payload) return payload

  if (
    payload.items !== undefined ||
    payload.Items !== undefined ||
    payload.count !== undefined ||
    payload.Count !== undefined
  ) {
    return payload
  }

  const nested = (payload as Record<string, unknown>).data ?? (payload as Record<string, unknown>).Data
  if (nested && typeof nested === 'object') {
    return extractPagedPayload<T>(nested as MaybePagedResponse<T>)
  }

  return payload
}

const normalizePagedResponse = <T>(payload: MaybePagedResponse<T> | undefined | null) => {
  const source = extractPagedPayload(payload)

  const items = source?.items ?? source?.Items ?? []
  const count =
    source?.count ??
    source?.Count ??
    (typeof items.length === 'number' ? items.length : 0)
  const pages = source?.pages ?? source?.Pages ?? 0

  return { items, count, pages }
}

const getRoleGroupLabel = (rawGroup: string): string => {
  const labels: Record<string, string> = {
    auth: 'Kimlik Doğrulama',
    operationclaims: 'Sistem Roller',
    useroperationclaims: 'Kullanıcı Rol Yetkileri',
    users: 'Kullanıcı Yönetimi',
    katalog: 'Katalog Yetkileri',
    raporlama: 'Raporlama',
    loans: 'Ödünç İşlemleri',
    odunc: 'Ödünç İşlemleri'
  }
  const normalized = rawGroup.toLowerCase()
  if (labels[normalized]) {
    return labels[normalized]
  }
  return rawGroup.replace(/([A-Z])/g, ' $1').replace(/[-_.]/g, ' ').trim() || 'Diğer Yetkiler'
}

const roleGroups = computed<RoleGroup[]>(() => {
  const buckets = new Map<string, RoleGroup>()
  for (const option of roleOptions.value) {
    const prefix = option.name.includes('.') ? option.name.split('.')[0] : option.name
    const key = prefix.toLowerCase()
    let group = buckets.get(key)
    if (!group) {
      group = { key, label: getRoleGroupLabel(prefix), items: [] }
      buckets.set(key, group)
    }
    group.items.push(option)
  }
  return Array.from(buckets.values())
    .map(group => ({
      ...group,
      items: [...group.items].sort((a, b) =>
        getRoleLabel(a.name).localeCompare(getRoleLabel(b.name), 'tr', { sensitivity: 'base' })
      )
    }))
    .sort((a, b) => a.label.localeCompare(b.label, 'tr', { sensitivity: 'base' }))
})

const filteredRoleGroups = computed<RoleGroup[]>(() => {
  const search = modalRoleSearch.value.trim().toLowerCase()
  if (!search) {
    return roleGroups.value
  }
  return roleGroups.value
    .map(group => {
      const items = group.items.filter(option => {
        const label = getRoleLabel(option.name).toLowerCase()
        return label.includes(search) || option.name.toLowerCase().includes(search)
      })
      return items.length > 0 ? { ...group, items } : null
    })
    .filter((group): group is RoleGroup => group !== null)
})

const getUserRoleLabels = (userId: string): string[] => {
  return userRoleLabelsMap.value[userId] ?? []
}

const getUserRoleIds = (userId: string): number[] => {
  return (userRoleRecordsMap.value[userId] ?? []).map(record => record.operationClaimId)
}

const showFeedback = (message: string, kind: 'success' | 'error') => {
  feedback.message = message
  feedback.kind = kind
  window.setTimeout(() => {
    feedback.message = ''
    feedback.kind = ''
  }, 4000)
}

const getErrorMessage = (err: unknown): string => {
  if (typeof err === 'string') {
    return err
  }
  const axiosError = err as AxiosError<{ message?: string }>
  return axiosError?.response?.data?.message || axiosError?.message || 'İşlem sırasında bir hata oluştu.'
}

const resolveUserIdsForRole = async (roleId: number): Promise<string[]> => {
  try {
    const response = await apiClient.post<PagedResponse<UserOperationClaim>>(
      '/UserOperationClaims/GetListByDynamic',
      {
        Filter: {
          Field: 'OperationClaimId',
          Operator: 'eq',
          Value: roleId
        }
      },
      {
        params: {
          'PageRequest.PageIndex': 0,
          'PageRequest.PageSize': 500
        }
      }
    )
    return (response.data?.items ?? []).map(item => item.userId)
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
    return []
  }
}

const buildUserDynamicQuery = async (): Promise<{
  query: Record<string, unknown>
  empty: boolean
}> => {
  const filtersStack: DynamicFilter[] = []

  if (filters.value.role && filters.value.role !== 0) {
    const userIds = await resolveUserIdsForRole(filters.value.role)
    if (userIds.length === 0) {
      return { query: {}, empty: true }
    }
    filtersStack.push(
      userIds.length === 1
        ? { Field: 'Id', Operator: 'eq', Value: userIds[0] }
        : {
            Logic: 'or',
            Filters: userIds.map(id => ({
              Field: 'Id',
              Operator: 'eq',
              Value: id
            }))
          }
    )
  }

  if (filters.value.status === 'active') {
    filtersStack.push({ Field: 'Status', Operator: 'eq', Value: true })
  } else if (filters.value.status === 'inactive') {
    filtersStack.push({ Field: 'Status', Operator: 'eq', Value: false })
  }

  if (filters.value.search) {
    const searchBlock = {
      Logic: 'or',
      Filters: [
        { Field: 'FirstName', Operator: 'contains', Value: filters.value.search },
        { Field: 'LastName', Operator: 'contains', Value: filters.value.search },
        { Field: 'Email', Operator: 'contains', Value: filters.value.search }
      ]
    }
    filtersStack.push(searchBlock)
  }

  let filterBody: DynamicFilter | null = null
  if (filtersStack.length === 1) {
    filterBody = filtersStack[0]
  } else if (filtersStack.length > 1) {
    filterBody = { Logic: 'and', Filters: filtersStack }
  }

  const query: Record<string, unknown> = {
    Sort: [
      { Field: 'FirstName', Dir: 'asc' },
      { Field: 'LastName', Dir: 'asc' }
    ]
  }

  if (filterBody) {
    query.Filter = filterBody
  }

  return { query, empty: false }
}

const loadOperationClaims = async () => {
  try {
    const response = await apiClient.get<PagedResponse<OperationClaim>>('/OperationClaims', {
      params: {
        'PageRequest.PageIndex': 0,
        'PageRequest.PageSize': 200
      }
    })
    const { items } = normalizePagedResponse<OperationClaim>(response.data as MaybePagedResponse<OperationClaim>)
    roleOptions.value = items
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
  }
}

const loadUserRoles = async (userIds: string[]) => {
  if (userIds.length === 0) {
    userOperationClaims.value = []
    return
  }

  const filter =
    userIds.length === 1
      ? { Field: 'UserId', Operator: 'eq', Value: userIds[0] }
      : {
          Logic: 'or',
          Filters: userIds.map(id => ({
            Field: 'UserId',
            Operator: 'eq',
            Value: id
          }))
        }

  try {
      const response = await apiClient.post<PagedResponse<UserOperationClaim>>(
        '/UserOperationClaims/GetListByDynamic',
        { Filter: filter },
        {
          params: {
            'PageRequest.PageIndex': 0,
            'PageRequest.PageSize': 500
          }
        }
      )
      const { items } = normalizePagedResponse<UserOperationClaim>(
        response.data as MaybePagedResponse<UserOperationClaim>
      )
      userOperationClaims.value = items
    } catch (err) {
      console.error('Kullanıcı rolleri alınamadı', err)
      userOperationClaims.value = []
    }
  }

const loadMetrics = async () => {
  try {
    const response = await apiClient.post<PagedResponse<UserListItem>>(
      '/Users/GetListByDynamic',
      {
        Filter: {
          Field: 'Status',
          Operator: 'eq',
          Value: true
        }
      },
      {
        params: {
          'PageRequest.PageIndex': 0,
          'PageRequest.PageSize': 1
        }
      }
    )
    const { count } = normalizePagedResponse<UserListItem>(
      response.data as MaybePagedResponse<UserListItem>
    )
    metrics.activeCount = typeof count === 'number' ? count : metrics.activeCount
  } catch (err) {
    console.error('Aktif kullanıcı metriği alınamadı', err)
  }
}

const loadUsers = async () => {
  loading.value = true
  error.value = null

  try {
    const dynamic = await buildUserDynamicQuery()
    if (dynamic.empty) {
      users.value = []
      userOperationClaims.value = []
      totalCount.value = 0
      totalPages.value = 0
      lastUpdated.value = new Date().toISOString()
      return
    }

    const response = await apiClient.post<PagedResponse<UserListItem>>(
      '/Users/GetListByDynamic',
      dynamic.query,
      {
        params: {
          'PageRequest.PageIndex': page.value,
          'PageRequest.PageSize': pageSize.value
        }
      }
    )

    const normalized = normalizePagedResponse<UserListItem>(
      response.data as MaybePagedResponse<UserListItem>
    )
    users.value = normalized.items
    totalCount.value = normalized.count ?? normalized.items.length
    const pagesFromServer = normalized.pages ?? 0
    totalPages.value =
      pagesFromServer > 0 ? pagesFromServer : totalCount.value > 0 ? Math.ceil(totalCount.value / pageSize.value) : 0
    lastUpdated.value = new Date().toISOString()

    if (users.value.length === 0 && totalPages.value > 0 && page.value >= totalPages.value) {
      page.value = Math.max(0, totalPages.value - 1)
      await loadUsers()
      return
    }

    await loadUserRoles(users.value.map(user => user.id))
  } catch (err) {
    error.value = getErrorMessage(err)
    users.value = []
    userOperationClaims.value = []
    totalCount.value = 0
    totalPages.value = 0
  } finally {
    loading.value = false
  }
}

const syncUserRoles = async (userId: string, desiredRoleIds: number[]) => {
  const currentRecords = userRoleRecordsMap.value[userId] ?? []
  const desiredSet = new Set(desiredRoleIds)

  const toAdd = desiredRoleIds.filter(id => !currentRecords.some(record => record.operationClaimId === id))
  const toRemove = currentRecords.filter(record => !desiredSet.has(record.operationClaimId))

  for (const claimId of toAdd) {
    await apiClient.post('/UserOperationClaims', {
      userId,
      operationClaimId: claimId
    })
  }

  for (const record of toRemove) {
    await apiClient.delete('/UserOperationClaims', {
      data: { id: record.id }
    })
  }
}

const applyFilters = async () => {
  if (loading.value) return
  filters.value = {
    role: filterForm.role,
    status: filterForm.status,
    search: filterForm.search
  }
  page.value = 0
  await loadUsers()
}

const resetFilters = async () => {
  filterForm.role = 0
  filterForm.status = 'all'
  filterForm.search = ''
  await applyFilters()
}

const prevPage = async () => {
  if (page.value === 0) return
  page.value -= 1
  await loadUsers()
}

const nextPage = async () => {
  if (page.value + 1 >= totalPages.value) return
  page.value += 1
  await loadUsers()
}

const openCreateModal = () => {
  modal.visible = true
  modal.mode = 'create'
  modal.error = ''
  modal.id = ''
  modal.firstName = ''
  modal.lastName = ''
  modal.email = ''
  modal.password = ''
  modal.roles = []
  modalRoleSearch.value = ''
}

const openEditModal = (user: UserListItem) => {
  modal.visible = true
  modal.mode = 'edit'
  modal.error = ''
  modal.id = user.id
  modal.firstName = user.firstName
  modal.lastName = user.lastName
  modal.email = user.email
  modal.password = ''
  modal.roles = getUserRoleIds(user.id).slice()
  modalRoleSearch.value = ''
}

const closeModal = () => {
  if (modal.saving) return
  modal.visible = false
  modal.error = ''
}

const validateModal = (): string | null => {
  if (!modal.firstName || modal.firstName.length < 2) {
    return 'Ad en az 2 karakter olmalıdır.'
  }
  if (!modal.lastName || modal.lastName.length < 2) {
    return 'Soyad en az 2 karakter olmalıdır.'
  }
  if (!modal.email) {
    return 'E-posta adresi gereklidir.'
  }
  if (!modal.password || modal.password.length < 4) {
    return 'Şifre en az 4 karakter olmalıdır.'
  }
  return null
}

const submitModal = async () => {
  const validationError = validateModal()
  if (validationError) {
    modal.error = validationError
    return
  }

  modal.error = ''
  modal.saving = true

  try {
    if (modal.mode === 'create') {
      const response = await apiClient.post<{ id?: string; Id?: string }>('/Users', {
        firstName: modal.firstName,
        lastName: modal.lastName,
        email: modal.email,
        password: modal.password
      })
      const createdId = response.data?.id ?? response.data?.Id
      if (createdId && modal.roles.length > 0) {
        await syncUserRoles(createdId, modal.roles)
      }
      showFeedback('Kullanıcı başarıyla oluşturuldu.', 'success')
    } else {
      await apiClient.put('/Users', {
        id: modal.id,
        firstName: modal.firstName,
        lastName: modal.lastName,
        email: modal.email,
        password: modal.password
      })
      await syncUserRoles(modal.id, modal.roles)
      showFeedback('Kullanıcı bilgileri güncellendi.', 'success')
    }

    modal.visible = false
    await loadUsers()
    await loadMetrics()
  } catch (err) {
    modal.error = getErrorMessage(err)
  } finally {
    modal.saving = false
  }
}

const deleteUser = async (user: UserListItem) => {
  if (pendingDeleteId.value) return
  const confirmed = window.confirm(
    user.firstName + ' ' + user.lastName + ' kullanıcısını silmek istediğinize emin misiniz?'
  )
  if (!confirmed) return

  pendingDeleteId.value = user.id
  try {
    await apiClient.delete('/Users', {
      data: { id: user.id }
    })
    showFeedback('Kullanıcı silindi.', 'success')
    await loadUsers()
    await loadMetrics()
  } catch (err) {
    showFeedback(getErrorMessage(err), 'error')
  } finally {
    pendingDeleteId.value = null
  }
}

onMounted(async () => {
  await loadOperationClaims()
  await loadMetrics()
  await loadUsers()
})
</script>

<style scoped>
.users-page {
  display: flex;
  flex-direction: column;
  gap: 1.75rem;
  padding: 2.5rem 3rem 4rem;
  background: linear-gradient(180deg, #f8fafc 0%, #e2e8f0 100%);
  color: #0f172a;
}

.hero {
  display: flex;
  justify-content: space-between;
  gap: 1.5rem;
  padding: 2.2rem 2.5rem;
  border-radius: 20px;
  background: linear-gradient(135deg, #2563eb 0%, #7c3aed 100%);
  color: #fff;
  box-shadow: 0 24px 48px -32px rgba(59, 130, 246, 0.6);
}

.hero__main {
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
  max-width: 600px;
}

.hero__main h1 {
  margin: 0;
  font-size: 2.2rem;
  font-weight: 600;
  letter-spacing: -0.01em;
}

.hero__main p {
  margin: 0;
  font-size: 1rem;
  line-height: 1.6;
  opacity: 0.9;
}

.hero__metrics {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
  gap: 0.9rem;
  margin: 0;
  padding: 0;
}

.hero__metrics > div {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  background: rgba(15, 23, 42, 0.18);
  border-radius: 14px;
  padding: 0.8rem 1rem;
}

.hero__metrics dt {
  margin: 0;
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  opacity: 0.8;
}

.hero__metrics dd {
  margin: 0;
  font-size: 1.3rem;
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
  gap: 0.5rem;
  padding: 0.75rem 1.2rem;
  border-radius: 12px;
  border: none;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.15s ease, box-shadow 0.15s ease;
}

.btn:focus-visible {
  outline: 2px solid rgba(59, 130, 246, 0.5);
  outline-offset: 2px;
}

.btn:hover {
  transform: translateY(-1px);
}

.btn--primary {
  background: #f8fafc;
  color: #1d4ed8;
}

.btn--ghost {
  background: transparent;
  color: #1f2937;
  border: 1px solid rgba(30, 64, 175, 0.25);
}

.btn--danger {
  background: rgba(239, 68, 68, 0.12);
  color: #b91c1c;
}

.btn--sm {
  padding: 0.55rem 0.9rem;
  font-size: 0.82rem;
  border-radius: 10px;
}

.btn__icon {
  font-size: 1.25rem;
  font-weight: 700;
}

.alert {
  display: grid;
  gap: 0.3rem;
  padding: 0.9rem 1.1rem;
  border-radius: 14px;
  border: 1px solid transparent;
  font-size: 0.9rem;
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
  background: rgba(254, 202, 202, 0.25);
  border-color: rgba(248, 113, 113, 0.3);
  color: #b91c1c;
}

.panel {
  background: rgba(255, 255, 255, 0.9);
  border-radius: 18px;
  padding: 1.8rem 2rem;
  box-shadow: 0 16px 40px -32px rgba(30, 64, 175, 0.35);
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

.panel__header {
  display: flex;
  justify-content: space-between;
  gap: 1rem;
}

.panel__header h2 {
  margin: 0;
  font-size: 1.35rem;
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
  gap: 1rem;
}

.filters label {
  display: flex;
  flex-direction: column;
  gap: 0.45rem;
  font-size: 0.88rem;
  color: #1f2937;
}

.filters select,
.filters input {
  border: 1px solid rgba(148, 163, 184, 0.45);
  border-radius: 12px;
  padding: 0.7rem 0.9rem;
  background: #f8fafc;
  font-size: 0.95rem;
  transition: border-color 0.15s ease, box-shadow 0.15s ease;
}

.filters select:focus,
.filters input:focus {
  border-color: rgba(59, 130, 246, 0.6);
  box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.15);
  outline: none;
}

.filters__actions {
  display: flex;
  align-items: flex-end;
}

.table-wrapper {
  border-radius: 16px;
  border: 1px solid rgba(226, 232, 240, 0.8);
  overflow: hidden;
}

.table-wrapper--loading {
  opacity: 0.85;
}

.users-table {
  width: 100%;
  border-collapse: collapse;
}

.users-table thead {
  background: rgba(99, 102, 241, 0.1);
  font-size: 0.85rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  color: #475569;
}

.users-table th,
.users-table td {
  padding: 0.85rem 1rem;
  border-bottom: 1px solid rgba(226, 232, 240, 0.9);
  text-align: left;
}

.table-actions {
  display: flex;
  gap: 0.6rem;
}

.table-state {
  text-align: center;
  padding: 2.2rem 1rem;
  color: #475569;
  font-size: 0.95rem;
}

.table-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 0.88rem;
  color: #475569;
}

.table-footer__actions {
  display: flex;
  gap: 0.6rem;
}

.user-cell {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.user-cell__name {
  font-weight: 600;
  color: #0f172a;
}

.user-cell__meta {
  font-size: 0.82rem;
  color: #94a3b8;
}

.role-chips {
  display: flex;
  flex-wrap: wrap;
  gap: 0.4rem;
}

.chip {
  display: inline-flex;
  align-items: center;
  padding: 0.28rem 0.65rem;
  border-radius: 999px;
  background: rgba(196, 181, 253, 0.3);
  color: #5b21b6;
  font-size: 0.78rem;
  font-weight: 600;
}

.role-chips__empty {
  font-size: 0.82rem;
  color: #94a3b8;
}

.status {
  display: inline-flex;
  align-items: center;
  padding: 0.3rem 0.65rem;
  border-radius: 999px;
  font-size: 0.8rem;
  font-weight: 600;
}

.status--active {
  background: rgba(134, 239, 172, 0.3);
  color: #15803d;
}

.status--passive {
  background: rgba(203, 213, 225, 0.4);
  color: #475569;
}

.message {
  display: grid;
  gap: 0.25rem;
  padding: 0.95rem 1.2rem;
  border-radius: 12px;
  font-size: 0.9rem;
}

.message--error {
  background: rgba(254, 226, 226, 0.7);
  color: #b91c1c;
}

.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.45);
  display: grid;
  place-items: center;
  padding: 1.5rem;
  z-index: 1200;
}

.modal {
  width: min(640px, 100%);
  background: #ffffff;
  border-radius: 18px;
  box-shadow: 0 40px 80px -40px rgba(15, 23, 42, 0.6);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.modal__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  padding: 1.35rem 1.6rem;
  background: rgba(59, 130, 246, 0.12);
}

.modal__header h3 {
  margin: 0;
  font-size: 1.2rem;
  font-weight: 600;
  color: #1e293b;
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
  gap: 1.2rem;
  padding: 1.4rem 1.6rem 1.6rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.form-grid label,
.form-grid fieldset {
  display: flex;
  flex-direction: column;
  gap: 0.45rem;
  font-size: 0.88rem;
  color: #1f2937;
}

.form-grid input {
  border: 1px solid rgba(148, 163, 184, 0.55);
  border-radius: 12px;
  padding: 0.7rem 0.95rem;
  font-size: 0.95rem;
  transition: border-color 0.15s ease, box-shadow 0.15s ease;
}

.form-grid input:focus {
  border-color: rgba(79, 70, 229, 0.6);
  box-shadow: 0 0 0 2px rgba(129, 140, 248, 0.2);
  outline: none;
}

.form-grid__full {
  grid-column: 1 / -1;
}

.role-toolbar {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 0.85rem;
}

.role-toolbar__search {
  position: relative;
  flex: 1;
}

.role-toolbar__search input {
  width: 100%;
  border: 1px solid rgba(148, 163, 184, 0.55);
  border-radius: 12px;
  padding: 0.65rem 0.95rem;
  background: rgba(248, 250, 252, 0.9);
  font-size: 0.92rem;
}

.role-toolbar__search input:focus {
  border-color: rgba(79, 70, 229, 0.6);
  box-shadow: 0 0 0 2px rgba(129, 140, 248, 0.18);
  outline: none;
}

.role-toolbar__count {
  font-size: 0.82rem;
  color: #64748b;
  white-space: nowrap;
}

.role-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 0.9rem;
  max-height: 360px;
  overflow-y: auto;
  padding-right: 0.4rem;
  scrollbar-width: thin;
  scrollbar-color: rgba(148, 163, 184, 0.45) transparent;
}

.role-grid::-webkit-scrollbar {
  width: 6px;
}

.role-grid::-webkit-scrollbar-thumb {
  background: rgba(148, 163, 184, 0.45);
  border-radius: 999px;
}

.role-grid::-webkit-scrollbar-track {
  background: transparent;
}

.role-card {
  display: flex;
  flex-direction: column;
  border: 1px solid rgba(148, 163, 184, 0.35);
  border-radius: 14px;
  background: rgba(248, 250, 252, 0.85);
  box-shadow: 0 10px 20px -18px rgba(15, 23, 42, 0.4);
}

.role-card__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 0.5rem;
  padding: 0.85rem 1rem;
  border-bottom: 1px solid rgba(203, 213, 225, 0.6);
}

.role-card__header h4 {
  margin: 0;
  font-size: 0.95rem;
  font-weight: 600;
  color: #1e293b;
}

.role-card__header span {
  font-size: 0.78rem;
  color: #94a3b8;
}

.role-card__list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  padding: 0.85rem 1rem 1rem;
}

.role-card__item {
  display: inline-flex;
  align-items: center;
  gap: 0.55rem;
  font-size: 0.86rem;
  color: #1f2937;
}

.role-card__item input {
  width: 16px;
  height: 16px;
}

.role-empty {
  display: grid;
  gap: 0.35rem;
  padding: 1.1rem;
  border-radius: 12px;
  background: rgba(226, 232, 240, 0.45);
  color: #475569;
  text-align: center;
  font-size: 0.9rem;
}

.modal__error {
  margin: 0;
  padding: 0.85rem 1rem;
  border-radius: 12px;
  background: rgba(248, 113, 113, 0.15);
  color: #b91c1c;
  font-size: 0.88rem;
}

.modal__actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.7rem;
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
  .users-page {
    padding: 1.8rem;
  }

  .hero {
    flex-direction: column;
  }

  .hero__actions {
    align-self: flex-start;
  }
}

@media (max-width: 720px) {
  .panel__header {
    flex-direction: column;
    align-items: flex-start;
  }

  .table-actions {
    flex-direction: column;
  }

  .modal__body {
    padding: 1.2rem;
  }
}
</style>
