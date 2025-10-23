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
              <span class="font-medium">Title</span>
              <input
                v-model="newRequest.title"
                type="text"
                required
                placeholder="Book or resource title"
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              />
            </label>
            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Author / Creator</span>
              <input
                v-model="newRequest.author"
                type="text"
                required
                placeholder="Author name"
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              />
            </label>
          </div>

          <label class="space-y-2 text-sm text-gray-700">
            <span class="font-medium">Summary</span>
            <textarea
              v-model="newRequest.description"
              rows="3"
              required
              placeholder="Short summary, learning outcomes, or curriculum connections"
              class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
            ></textarea>
          </label>

          <div class="grid gap-4 md:grid-cols-3">
            <label class="space-y-2 text-sm text-gray-700">
              <span class="font-medium">Format</span>
              <select
                v-model="newRequest.format"
                required
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option value="">Select format</option>
                <option value="Print">Print book</option>
                <option value="eBook">E-book</option>
                <option value="Audiobook">Audiobook</option>
                <option value="Multimedia">Multimedia</option>
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
              <span class="font-medium">Intended Audience</span>
              <select
                v-model="newRequest.target"
                required
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option value="">Select audience</option>
                <option value="Primary">Primary</option>
                <option value="Middle School">Middle school</option>
                <option value="High School">High school</option>
                <option value="Teachers">Teachers</option>
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
              <h3 class="text-lg font-semibold text-gray-900">{{ request.title }}</h3>
              <p class="text-sm text-gray-500">Requested by {{ request.requestedBy }} · {{ formatDate(request.createdAt) }}</p>
            </div>
            <div class="flex flex-wrap items-center gap-2">
              <span
                class="inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold"
                :class="priorityBadgeClass(request.priority)"
              >
                {{ request.priority }} priority
              </span>
              <span
                class="inline-flex items-center rounded-full px-3 py-1 text-xs font-semibold"
                :class="statusBadgeClass(request.status)"
              >
                {{ request.status }}
              </span>
            </div>
          </div>
          <p class="mt-3 text-sm text-gray-700">{{ request.description }}</p>
          <div class="mt-3 flex flex-wrap items-center gap-3 text-xs text-gray-500">
            <span class="inline-flex items-center gap-1">
              <svg class="h-4 w-4 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 12.75h15m-15 0a2.25 2.25 0 0 1 0-4.5h15a2.25 2.25 0 0 1 0 4.5Zm0 0a2.25 2.25 0 0 0 0 4.5h15a2.25 2.25 0 0 0 0-4.5" />
              </svg>
              {{ request.format }}
            </span>
            <span class="inline-flex items-center gap-1">
              <svg class="h-4 w-4 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 5.25v13.5m-7.5-13.5v13.5" />
              </svg>
              Audience: {{ request.target }}
            </span>
            <span class="inline-flex items-center gap-1">
              <svg class="h-4 w-4 text-gray-400" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6l3 3" />
              </svg>
              Updated: {{ formatDate(request.updatedAt) }}
            </span>
          </div>
        </article>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'

interface CatalogRequest {
  id: number
  title: string
  author: string
  description: string
  format: string
  priority: string
  target: string
  status: string
  requestedBy: string
  createdAt: string
  updatedAt: string
}

const priorities = ['High', 'Medium', 'Low']
const statuses = ['In Review', 'Approved', 'Rejected', 'Needs Update']

const newRequest = reactive({
  title: '',
  author: '',
  description: '',
  format: '',
  priority: 'Medium',
  target: '',
})

const filters = reactive({
  status: 'All',
  search: '',
})

const catalogRequests = ref<CatalogRequest[]>([
  {
    id: 1,
    title: 'Modern Physics for Schools',
    author: 'Ismail Demir',
    description: 'Updated physics topics aligned with Grade 11 curriculum and national exam focus.',
    format: 'Print',
    priority: 'High',
    target: 'High School',
    status: 'In Review',
    requestedBy: 'Ankara Science High School',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 2).toISOString(),
    updatedAt: new Date(Date.now() - 1000 * 60 * 60 * 3).toISOString(),
  },
  {
    id: 2,
    title: 'Interactive Coding Projects',
    author: 'CodeLab Collective',
    description: 'Project-based activities that introduce algorithms and basic programming for middle schoolers.',
    format: 'Multimedia',
    priority: 'Medium',
    target: 'Middle School',
    status: 'Approved',
    requestedBy: 'Etimesgut Secondary School',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 10).toISOString(),
    updatedAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 1).toISOString(),
  },
  {
    id: 3,
    title: 'Inclusive Classroom Strategies',
    author: 'Inclusive Schools Network',
    description: 'Guidelines, templates, and case studies to support inclusive practices for teachers.',
    format: 'eBook',
    priority: 'Low',
    target: 'Teachers',
    status: 'Needs Update',
    requestedBy: 'Bursa Teacher Development Center',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 20).toISOString(),
    updatedAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 5).toISOString(),
  },
])

const isSubmitting = ref(false)
const formError = ref<string | null>(null)
const successMessage = ref<string | null>(null)

const filteredRequests = computed(() => {
  return catalogRequests.value.filter((request) => {
    const matchesStatus = filters.status === 'All' || request.status === filters.status
    const term = filters.search.trim().toLowerCase()
    const matchesSearch =
      term.length === 0 ||
      request.title.toLowerCase().includes(term) ||
      request.requestedBy.toLowerCase().includes(term)

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
  switch (priority) {
    case 'High':
      return 'bg-red-100 text-red-700'
    case 'Low':
      return 'bg-gray-200 text-gray-700'
    default:
      return 'bg-amber-100 text-amber-700'
  }
}

function statusBadgeClass(status: string) {
  switch (status) {
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

function submitRequest() {
  formError.value = null
  successMessage.value = null

  if (isSubmitting.value) return
  if (!newRequest.title || !newRequest.author || !newRequest.description || !newRequest.format || !newRequest.target) {
    formError.value = 'Please fill in all required fields.'
    return
  }

  isSubmitting.value = true
  setTimeout(() => {
    const now = new Date().toISOString()
    catalogRequests.value.unshift({
      id: Date.now(),
      title: newRequest.title,
      author: newRequest.author,
      description: newRequest.description,
      format: newRequest.format,
      priority: newRequest.priority,
      target: newRequest.target,
      status: 'In Review',
      requestedBy: 'Current School',
      createdAt: now,
      updatedAt: now,
    })

    successMessage.value = 'Request submitted. You will receive updates via e-mail.'
    resetForm()
    isSubmitting.value = false
  }, 600)
}

function resetForm() {
  newRequest.title = ''
  newRequest.author = ''
  newRequest.description = ''
  newRequest.format = ''
  newRequest.priority = 'Medium'
  newRequest.target = ''
  formError.value = null
}
</script>
