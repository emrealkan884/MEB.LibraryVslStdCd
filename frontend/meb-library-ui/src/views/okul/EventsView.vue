<template>
  <div class="space-y-10">
    <header class="space-y-2">
      <h1 class="text-3xl font-semibold text-gray-900">Library Events</h1>
      <p class="text-gray-600">
        Plan workshops, reading sessions, and community meetings across the school year.
      </p>
    </header>

    <section class="grid gap-6 lg:grid-cols-12">
      <aside class="space-y-4 lg:col-span-4">
        <article class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
          <h2 class="text-lg font-semibold text-gray-900">Upcoming Highlights</h2>
          <ul class="mt-4 space-y-3 text-sm text-gray-600">
            <li class="flex items-start gap-3">
              <span class="mt-1 inline-flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-xs font-semibold text-white">
                {{ upcomingEvents.length }}
              </span>
              <div>
                <p class="font-medium text-gray-800">Events scheduled</p>
                <p>Organise facilitators and send invitations in advance.</p>
              </div>
            </li>
            <li class="flex items-start gap-3">
              <span class="mt-1 inline-flex h-6 w-6 items-center justify-center rounded-full bg-emerald-100 text-xs font-semibold text-emerald-700">
                {{ interestedCount }}
              </span>
              <div>
                <p class="font-medium text-gray-800">Interested participants</p>
                <p>Track attention and follow up with targeted reminders.</p>
              </div>
            </li>
          </ul>
        </article>

        <article class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
          <h2 class="text-lg font-semibold text-gray-900">Filters</h2>
          <div class="mt-4 space-y-4 text-sm text-gray-700">
            <label class="space-y-2">
              <span class="font-medium">Search</span>
              <input
                v-model="filters.search"
                type="search"
                placeholder="Workshop, speaker, audience..."
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              />
            </label>
            <label class="space-y-2">
              <span class="font-medium">Audience</span>
              <select
                v-model="filters.audience"
                class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              >
                <option value="all">All</option>
                <option v-for="audience in audiences" :key="audience" :value="audience">
                  {{ audience }}
                </option>
              </select>
            </label>
            <label class="flex items-center gap-2 text-sm text-gray-700">
              <input
                v-model="filters.upcomingOnly"
                type="checkbox"
                class="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-500"
              />
              Show only upcoming events
            </label>
          </div>
        </article>
      </aside>

      <main class="space-y-6 lg:col-span-8">
        <section class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
          <div class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
            <div>
              <h2 class="text-xl font-semibold text-gray-900">Event Timeline</h2>
              <p class="text-sm text-gray-500">{{ filteredEvents.length }} event(s) match your filter</p>
            </div>
            <button
              type="button"
              class="inline-flex items-center rounded-lg border border-blue-600 bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-700"
              @click="toggleCompact"
            >
              <svg class="mr-2 h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" d="M4 6h16M4 12h16m-7 6h7" />
              </svg>
              {{ compactView ? 'Detailed view' : 'Compact view' }}
            </button>
          </div>

          <ol class="mt-6 space-y-4">
            <li
              v-for="event in filteredEvents"
              :key="event.id"
              class="flex flex-col gap-4 rounded-lg border border-gray-200 p-4 transition hover:border-blue-200 hover:bg-blue-50 md:flex-row md:items-center md:justify-between"
            >
              <div class="flex items-start gap-4">
                <div class="rounded-lg border border-blue-200 bg-blue-50 px-3 py-2 text-center text-xs font-semibold uppercase text-blue-700">
                  <span class="block text-lg">{{ formatDate(event.date, 'day') }}</span>
                  <span>{{ formatDate(event.date, 'month') }}</span>
                </div>
                <div>
                  <div class="flex flex-wrap items-center gap-2">
                    <h3 class="text-lg font-semibold text-gray-900">{{ event.title }}</h3>
                    <span
                      class="inline-flex items-center rounded-full px-2.5 py-1 text-xs font-semibold"
                      :class="audienceBadge(event.audience)"
                    >
                      {{ event.audience }}
                    </span>
                    <span class="inline-flex items-center rounded-full bg-gray-200 px-2.5 py-1 text-xs font-medium text-gray-700">
                      {{ event.type }}
                    </span>
                  </div>
                  <p class="mt-1 text-sm text-gray-600">{{ event.description }}</p>
                  <div v-if="!compactView" class="mt-3 grid gap-3 text-xs text-gray-500 md:grid-cols-2">
                    <span class="inline-flex items-center gap-1">
                      <svg class="h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M12 8v4l2.5 2.5" />
                      </svg>
                      {{ event.start }} - {{ event.end }}
                    </span>
                    <span class="inline-flex items-center gap-1">
                      <svg class="h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M16.5 10.5a4.5 4.5 0 1 1-9 0 4.5 4.5 0 0 1 9 0Z" />
                        <path stroke-linecap="round" stroke-linejoin="round" d="M12 14.25v4.5" />
                      </svg>
                      Facilitator: {{ event.facilitator }}
                    </span>
                    <span class="inline-flex items-center gap-1">
                      <svg class="h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M12 6.75v10.5m0 0-7.5-7.5m7.5 7.5 7.5-7.5" />
                      </svg>
                      {{ event.location }}
                    </span>
                    <span class="inline-flex items-center gap-1">
                      <svg class="h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 6.75h7.5m-7.5 3.75h7.5M15 18h.008v.008H15Z" />
                      </svg>
                      Resources: {{ event.resources }}
                    </span>
                  </div>
                </div>
              </div>

              <div class="flex flex-col items-end gap-2">
                <button
                  type="button"
                  class="inline-flex items-center rounded-lg border border-emerald-600 bg-emerald-600 px-3 py-1.5 text-sm font-semibold text-white transition hover:bg-emerald-700 disabled:cursor-not-allowed disabled:border-emerald-300 disabled:bg-emerald-300"
                  :disabled="interested.has(event.id)"
                  @click="toggleInterest(event.id)"
                >
                  <svg class="mr-2 h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M5.25 5.25 12 3l6.75 2.25v6.75c0 4.556-3.358 8.874-6.75 9.75-3.392-.876-6.75-5.194-6.75-9.75V5.25Z" />
                  </svg>
                  {{ interested.has(event.id) ? 'Registered' : 'I am interested' }}
                </button>
                <span class="text-xs text-gray-500">{{ event.attendees }} confirmed attendee(s)</span>
              </div>
            </li>
          </ol>
        </section>

        <section class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
          <h2 class="text-xl font-semibold text-gray-900">Event Ideas Backlog</h2>
          <p class="mt-1 text-sm text-gray-500">Capture quick ideas and assign priorities to build future programs.</p>

          <div class="mt-5 grid gap-4 md:grid-cols-2">
            <article
              v-for="idea in ideas"
              :key="idea.id"
              class="flex flex-col justify-between rounded-lg border border-dashed border-gray-300 bg-gray-50 p-4"
            >
              <div>
                <div class="flex items-center justify-between">
                  <h3 class="text-base font-semibold text-gray-900">{{ idea.title }}</h3>
                  <span class="rounded-full bg-amber-100 px-3 py-1 text-xs font-semibold text-amber-700">
                    {{ idea.priority }} priority
                  </span>
                </div>
                <p class="mt-2 text-sm text-gray-600">{{ idea.description }}</p>
              </div>
              <div class="mt-4 flex items-center justify-between text-xs text-gray-500">
                <span>Proposed by {{ idea.owner }}</span>
                <span>{{ formatDate(idea.createdAt, 'short') }}</span>
              </div>
            </article>
          </div>
        </section>
      </main>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'

interface LibraryEvent {
  id: number
  title: string
  description: string
  date: string
  start: string
  end: string
  audience: string
  type: string
  resources: string
  location: string
  facilitator: string
  attendees: number
}

interface EventIdea {
  id: number
  title: string
  description: string
  owner: string
  priority: 'High' | 'Medium' | 'Low'
  createdAt: string
}

const audiences = ['Students', 'Teachers', 'Parents', 'Community']

const events = ref<LibraryEvent[]>([
  {
    id: 1,
    title: 'Coding with Micro:bit',
    description: 'Hands-on coding workshop introducing Micro:bit for 7th grade STEM classes.',
    date: new Date(Date.now() + 1000 * 60 * 60 * 24 * 2).toISOString(),
    start: '10:00',
    end: '12:00',
    audience: 'Students',
    type: 'Workshop',
    resources: 'Micro:bit kits, laptops, quick start cards',
    location: 'STEM Lab 2',
    facilitator: 'Ayse Kaya',
    attendees: 24,
  },
  {
    id: 2,
    title: 'Family Reading Night',
    description: 'Storytelling, book tasting corners, and reading games for families.',
    date: new Date(Date.now() + 1000 * 60 * 60 * 24 * 7).toISOString(),
    start: '18:30',
    end: '20:30',
    audience: 'Parents',
    type: 'Community',
    resources: 'Picture books, snacks, sign-in sheets',
    location: 'Library Main Hall',
    facilitator: 'Library Team',
    attendees: 40,
  },
  {
    id: 3,
    title: 'Teacher Innovation Meetup',
    description: 'Peer sharing on digital literacy, new tools, and upcoming curriculum updates.',
    date: new Date(Date.now() + 1000 * 60 * 60 * 24 * 15).toISOString(),
    start: '15:00',
    end: '17:00',
    audience: 'Teachers',
    type: 'Professional Learning',
    resources: 'Slides, breakout prompts, resource deck',
    location: 'Hybrid (Library + Teams)',
    facilitator: 'Mehmet Demir',
    attendees: 32,
  },
  {
    id: 4,
    title: 'Archive Digitisation Showcase',
    description: 'Showcase of student-led digitisation of local history documents and media.',
    date: new Date(Date.now() - 1000 * 60 * 60 * 24 * 5).toISOString(),
    start: '11:00',
    end: '12:30',
    audience: 'Community',
    type: 'Exhibition',
    resources: 'Display boards, tablets, archival prints',
    location: 'Library Gallery',
    facilitator: 'History Club',
    attendees: 55,
  },
])

const ideas = ref<EventIdea[]>([
  {
    id: 101,
    title: 'Makerspace for Parents',
    description: 'Short course on using makerspace tools safely with children at home.',
    owner: 'Technology Department',
    priority: 'Medium',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 1).toISOString(),
  },
  {
    id: 102,
    title: 'Poetry Slam Finals',
    description: 'District-wide slam final hosted at the library with guest poets.',
    owner: 'Literature Committee',
    priority: 'High',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 3).toISOString(),
  },
  {
    id: 103,
    title: 'Archive Hackathon',
    description: 'Challenge teams to create interactive exhibits using the digital archive.',
    owner: 'Student Council',
    priority: 'Low',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 6).toISOString(),
  },
  {
    id: 104,
    title: 'Reading Mentors Kick-off',
    description: 'Launch meeting for high school students mentoring primary readers.',
    owner: 'Community Service Office',
    priority: 'High',
    createdAt: new Date(Date.now() - 1000 * 60 * 60 * 24 * 8).toISOString(),
  },
])

const filters = reactive({
  search: '',
  audience: 'all',
  upcomingOnly: true,
})

const compactView = ref(false)
const interested = ref<Set<number>>(new Set())

const filteredEvents = computed(() => {
  return events.value
    .filter((event) => {
      if (filters.upcomingOnly && isPast(event.date)) {
        return false
      }

      if (filters.audience !== 'all' && event.audience !== filters.audience) {
        return false
      }

      const term = filters.search.trim().toLowerCase()
      if (term.length === 0) return true

      return (
        event.title.toLowerCase().includes(term) ||
        event.description.toLowerCase().includes(term) ||
        event.facilitator.toLowerCase().includes(term) ||
        event.type.toLowerCase().includes(term)
      )
    })
    .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
})

const upcomingEvents = computed(() => filteredEvents.value.filter((event) => !isPast(event.date)))
const interestedCount = computed(() => interested.value.size)

function isPast(date: string) {
  return new Date(date).getTime() < Date.now()
}

function formatDate(value: string, mode: 'day' | 'month' | 'short' = 'short') {
  const date = new Date(value)
  switch (mode) {
    case 'day':
      return date.toLocaleString('tr-TR', { day: '2-digit' })
    case 'month':
      return date.toLocaleString('tr-TR', { month: 'short' })
    default:
      return date.toLocaleString('tr-TR', {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
      })
  }
}

function audienceBadge(audience: string) {
  switch (audience) {
    case 'Students':
      return 'bg-blue-100 text-blue-700'
    case 'Teachers':
      return 'bg-purple-100 text-purple-700'
    case 'Parents':
      return 'bg-emerald-100 text-emerald-700'
    case 'Community':
      return 'bg-amber-100 text-amber-700'
    default:
      return 'bg-gray-200 text-gray-700'
  }
}

function toggleInterest(id: number) {
  if (interested.value.has(id)) return
  interested.value.add(id)
}

function toggleCompact() {
  compactView.value = !compactView.value
}
</script>
