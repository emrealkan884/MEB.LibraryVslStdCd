<template>
  <div class="space-y-10">
    <header class="space-y-2">
      <h1 class="text-3xl font-semibold text-gray-900">Catalog Request Center</h1>
      <p class="text-gray-600">
        Submit new material requests for the district catalog and track the review pipeline.
      </p>
    </header>

    <section class="grid gap-6 lg:grid-cols-3">
      <article class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm lg:col-span-2">
        <div class="flex items-center justify-between">
          <div>
            <h2 class="text-xl font-semibold text-gray-900">Create New Request</h2>
            <p class="text-sm text-gray-500">Fill in the details below and send the proposal to the central team.</p>
          </div>
          <span class="rounded-full bg-blue-50 px-3 py-1 text-xs font-semibold text-blue-600">
            {{ priorities.length }} priority levels
          </span>
        </div>

        <form class="mt-6 space-y-4" @submit.prevent="submitRequest">
          <div class="grid gap-4 md:grid-cols-2">
            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Başlık *</span>
              <input
                v-model="newRequest.baslik"
                type="text"
                required
                placeholder="Kitap veya kaynak başlığı"
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              />
            </label>
            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Yazar / Yaratıcı *</span>
              <input
                v-model="newRequest.yazarMetni"
                type="text"
                required
                placeholder="Yazar adı"
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              />
            </label>
          </div>

          <label class="space-y-2 text-sm text-gray-700">
            <span class="font-medium">Açıklama *</span>
            <textarea
              v-model="newRequest.aciklama"
              rows="3"
              required
              placeholder="Kısa özet, öğrenme çıktıları veya müfredat bağlantıları"
              class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
            ></textarea>
          </label>

          <div class="grid gap-4 md:grid-cols-3">
            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Materyal Türü *</span>
              <select
                v-model="newRequest.materyalTuru"
                required
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option value="">Materyal türü seçin</option>
                <option value="Kitap">Kitap</option>
                <option value="E-Kitap">E-Kitap</option>
                <option value="Sesli Kitap">Sesli Kitap</option>
                <option value="Çoklu Ortam">Çoklu Ortam</option>
                <option value="Dergi">Dergi</option>
                <option value="Gazete">Gazete</option>
              </select>
            </label>

            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Priority</span>
              <select
                v-model="newRequest.priority"
                required
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option v-for="priority in priorities" :key="priority" :value="priority">
                  {{ priority }}
                </option>
              </select>
            </label>

            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Hedef Kitle *</span>
              <select
                v-model="newRequest.target"
                required
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option value="">Hedef kitle seçin</option>
                <option value="İlkokul">İlkokul</option>
                <option value="Ortaokul">Ortaokul</option>
                <option value="Lise">Lise</option>
                <option value="Öğretmenler">Öğretmenler</option>
                <option value="Genel">Genel</option>
              </select>
            </label>
          </div>

          <div class="flex flex-wrap items-center gap-3">
            <button
              type="submit"
              class="inline-flex items-center rounded-lg border border-blue-600 bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:border-blue-300 disabled:bg-blue-300"
              :disabled="isSubmitting"
            >
              <svg class="mr-2 h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 12h15m0 0-6-6m6 6-6 6" />
              </svg>
              Submit Request
            </button>
            <button
              type="button"
              class="inline-flex items-center rounded-lg border border-gray-300 px-3 py-2 text-sm font-medium text-gray-700 transition hover:border-gray-400 hover:text-gray-900"
              @click="resetForm"
              :disabled="isSubmitting"
            >
              Clear Form
            </button>
            <p v-if="formError" class="text-sm text-red-600">{{ formError }}</p>
            <p v-if="successMessage" class="text-sm text-emerald-600">{{ successMessage }}</p>
          </div>
        </form>
      </article>

      <aside class="space-y-4">
        <div class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
          <h3 class="text-lg font-semibold text-gray-900">Workflow Overview</h3>
          <ul class="mt-4 space-y-3 text-sm text-gray-600">
            <li class="flex items-start gap-3">
              <span class="mt-1 inline-flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-xs font-semibold text-white">
                1
              </span>
              <div>
                <p class="font-medium text-gray-800">Submission</p>
                <p>Send your proposal with curriculum alignment hints and usage plans.</p>
              </div>
            </li>
            <li class="flex items-start gap-3">
              <span class="mt-1 inline-flex h-6 w-6 items-center justify-center rounded-full bg-blue-100 text-xs font-semibold text-blue-700">
                2
              </span>
              <div>
                <p class="font-medium text-gray-800">Review</p>
                <p>Province librarians validate metadata, licensing, and duplication.</p>
              </div>
            </li>
            <li class="flex items-start gap-3">
              <span class="mt-1 inline-flex h-6 w-6 items-center justify-center rounded-full bg-emerald-100 text-xs font-semibold text-emerald-700">
                3
              </span>
              <div>
                <p class="font-medium text-gray-800">Decision</p>
                <p>Approved items appear in the shared catalog; feedback arrives via e-mail.</p>
              </div>
            </li>
          </ul>
        </div>

        <div class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
          <h3 class="text-lg font-semibold text-gray-900">Submission Tips</h3>
          <ul class="mt-3 list-disc space-y-2 pl-5 text-sm text-gray-600">
            <li>Include ISBN where possible to speed up validation.</li>
            <li>Add classroom scenarios that highlight practical value.</li>
            <li>Bundle similar requests to avoid duplicates.</li>
          </ul>
        </div>
      </aside>
    </section>

    <section class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
      <div class="flex flex-col gap-2 md:flex-row md:items-center md:justify-between">
        <div>
          <h2 class="text-xl font-semibold text-gray-900">Recent Requests</h2>
          <p class="text-sm text-gray-500">
            {{ filteredRequests.length }} request(s) match your filter
          </p>
        </div>
        <div class="flex flex-wrap items-center gap-3">
          <label class="text-sm text-gray-600">
            <span class="mr-2 font-medium">Filter by status:</span>
            <select
              v-model="filters.status"
              class="rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
            >
              <option value="All">All</option>
              <option v-for="status in statuses" :key="status" :value="status">
                {{ status }}
              </option>
            </select>
          </label>
          <label class="text-sm text-gray-600">
            <span class="sr-only">Search</span>
            <input
              v-model="filters.search"
              type="search"
              placeholder="Search title or requester"
              class="rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
            />
          </label>
        </div>
      </div>

      <div class="mt-6 space-y-3">
        <article
          v-for="request in filteredRequests"
          :key="request.id"
          class="rounded-lg border border-gray-200 bg-gray-50 p-4 transition hover:border-blue-200 hover:bg-white"
        >
          <div class="flex flex-wrap items-center justify-between gap-3">
            <div>
              <h3 class="text-lg font-semibold text-gray-900">{{ request.baslik }}</h3>
              <p class="text-sm text-gray-500">{{ request.talepEdenKutuphaneId }} · {{ formatDate(request.talepTarihi) }}</p>
            </div>
            <div class="flex flex-wrap items-center gap-2">
              <span
                class="inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold"
                :class="statusBadgeClass(request.durum)"
              >
                {{ statusMap[request.durum] || request.durum }}
              </span>
            </div>
          </div>
          <p class="mt-3 text-sm text-gray-700">{{ request.aciklama }}</p>
          <div class="mt-3 flex flex-wrap items-center gap-3 text-xs text-gray-500">
            <span class="inline-flex items-center gap-1">
              <svg class="h-4 w-4 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 12.75h15m-15 0a2.25 2.25 0 0 1 0-4.5h15a2.25 2.25 0 0 1 0 4.5Zm0 0a2.25 2.25 0 0 0 0 4.5h15a2.25 2.25 0 0 0 0-4.5" />
              </svg>
              {{ request.materyalTuru }}
              <span v-if="request.materyalAltTuru">({{ request.materyalAltTuru }})</span>
            </span>
            <span class="inline-flex items-center gap-1">
              <svg class="h-4 w-4 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 5.25v13.5m-7.5-13.5v13.5" />
              </svg>
              Yazar: {{ request.yazarMetni }}
            </span>
            <span v-if="request.sonGuncellemeTarihi" class="inline-flex items-center gap-1">
              <svg class="h-4 w-4 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6l3 3" />
              </svg>
              Güncellendi: {{ formatDate(request.sonGuncellemeTarihi) }}
            </span>
          </div>
        </article>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref, onMounted } from 'vue'
import { useToast } from 'primevue/usetoast'
import { useAuthStore } from '@/stores/auth'
import apiClient from '@/stores/api'

interface CatalogRequest {
  id: string
  talepEdenKutuphaneId: string
  baslik: string
  altBaslik?: string
  yazarMetni?: string
  isbn?: string
  issn?: string
  materyalTuru?: string
  materyalAltTuru?: string
  dil?: string
  yayinevi?: string
  yayinYeri?: string
  yayinYili?: number
  aciklama?: string
  redGerekcesi?: string
  durum: string
  talepTarihi: string
  sonGuncellemeTarihi?: string
  katalogKaydiId?: string
}

interface CreateCatalogRequest {
  talepEdenKutuphaneId: string
  baslik: string
  altBaslik?: string
  yazarMetni?: string
  isbn?: string
  issn?: string
  materyalTuru?: string
  materyalAltTuru?: string
  dil?: string
  yayinevi?: string
  yayinYeri?: string
  yayinYili?: number
  aciklama?: string
}

const toast = useToast()
const authStore = useAuthStore()

const priorities = ['High', 'Medium', 'Low']
const statuses = ['Beklemede', 'Inceleniyor', 'Onaylandi', 'Reddedildi']

const newRequest = reactive({
  baslik: '',
  yazarMetni: '',
  aciklama: '',
  materyalTuru: '',
  priority: 'Medium',
  target: '',
  altBaslik: '',
  isbn: '',
  issn: '',
  dil: '',
  yayinevi: '',
  yayinYeri: '',
  yayinYili: undefined as number | undefined,
})

const filters = reactive({
  status: 'All',
  search: '',
})

const catalogRequests = ref<CatalogRequest[]>([])
const isSubmitting = ref(false)
const isLoading = ref(false)
const formError = ref<string | null>(null)
const successMessage = ref<string | null>(null)

// Priority mapping
const priorityMap: Record<string, string> = {
  'High': 'Yuksek',
  'Medium': 'Orta',
  'Low': 'Dusuk'
}

const reversePriorityMap: Record<string, string> = {
  'Yuksek': 'High',
  'Orta': 'Medium',
  'Dusuk': 'Low'
}

// Status mapping for UI
const statusMap: Record<string, string> = {
  'Beklemede': 'In Review',
  'Inceleniyor': 'In Review',
  'Onaylandi': 'Approved',
  'Reddedildi': 'Rejected'
}

const filteredRequests = computed(() => {
  return catalogRequests.value.filter((request) => {
    const uiStatus = statusMap[request.durum] || request.durum
    const matchesStatus = filters.status === 'All' || uiStatus === filters.status
    const term = filters.search.trim().toLowerCase()
    const matchesSearch =
      term.length === 0 ||
      request.baslik.toLowerCase().includes(term) ||
      (request.yazarMetni && request.yazarMetni.toLowerCase().includes(term))

    return matchesStatus && matchesSearch
  })
})

function formatDate(value: string) {
  return new Date(value).toLocaleString('tr-TR', {
    day: '2-digit',
    month: 'short',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  })
}

function priorityBadgeClass(priority: string) {
  // For backend data, map priority from UI values or use default
  const uiPriority = reversePriorityMap[priority] || priority
  switch (uiPriority) {
    case 'High':
      return 'bg-red-100 text-red-700'
    case 'Low':
      return 'bg-gray-200 text-gray-700'
    default:
      return 'bg-amber-100 text-amber-700'
  }
}

function statusBadgeClass(status: string) {
  const uiStatus = statusMap[status] || status
  switch (uiStatus) {
    case 'Approved':
      return 'bg-emerald-100 text-emerald-700'
    case 'Rejected':
      return 'bg-red-100 text-red-700'
    case 'Needs Update':
      return 'bg-amber-100 text-amber-700'
    default:
      return 'bg-blue-100 text-blue-700'
  }
}

async function fetchRequests() {
  try {
    isLoading.value = true
    const response = await apiClient.get('/YeniKatalogTalepleri', {
      params: { PageIndex: 0, PageSize: 100 } // Basic pagination
    })
    catalogRequests.value = response.data.items || []
  } catch (error: any) {
    console.error('Failed to fetch requests:', error)
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: 'Failed to load catalog requests',
      life: 3000
    })
  } finally {
    isLoading.value = false
  }
}

async function submitRequest() {
  formError.value = null
  successMessage.value = null

  if (isSubmitting.value) return
  if (!newRequest.baslik || !newRequest.yazarMetni || !newRequest.aciklama || !newRequest.materyalTuru || !newRequest.target) {
    formError.value = 'Please fill in all required fields.'
    return
  }

  // Get current user's library ID (assuming it's available via auth store)
  const currentUserLibraryId = authStore.user?.schoolCode || 'unknown'

  isSubmitting.value = true
  try {
    const requestData: CreateCatalogRequest = {
      talepEdenKutuphaneId: currentUserLibraryId,
      baslik: newRequest.baslik,
      altBaslik: newRequest.altBaslik || undefined,
      yazarMetni: newRequest.yazarMetni,
      isbn: newRequest.isbn || undefined,
      issn: newRequest.issn || undefined,
      materyalTuru: newRequest.materyalTuru,
      materyalAltTuru: newRequest.target, // Using target as sub-type for now
      dil: newRequest.dil || undefined,
      yayinevi: newRequest.yayinevi || undefined,
      yayinYeri: newRequest.yayinYeri || undefined,
      yayinYili: newRequest.yayinYili,
      aciklama: newRequest.aciklama
    }

    const response = await apiClient.post('/YeniKatalogTalepleri', requestData)

    toast.add({
      severity: 'success',
      summary: 'Success',
      detail: 'Catalog request submitted successfully',
      life: 3000
    })

    resetForm()
    await fetchRequests() // Refresh the list
  } catch (error: any) {
    console.error('Failed to submit request:', error)
    formError.value = error.response?.data?.message || 'Failed to submit request'
    toast.add({
      severity: 'error',
      summary: 'Error',
      detail: error.response?.data?.message || 'Failed to submit catalog request',
      life: 3000
    })
  } finally {
    isSubmitting.value = false
  }
}

function resetForm() {
  newRequest.baslik = ''
  newRequest.yazarMetni = ''
  newRequest.aciklama = ''
  newRequest.materyalTuru = ''
  newRequest.priority = 'Medium'
  newRequest.target = ''
  newRequest.altBaslik = ''
  newRequest.isbn = ''
  newRequest.issn = ''
  newRequest.dil = ''
  newRequest.yayinevi = ''
  newRequest.yayinYeri = ''
  newRequest.yayinYili = undefined
  formError.value = null
}

// Lifecycle hook to fetch requests on component mount
onMounted(() => {
  fetchRequests()
})
</script>
