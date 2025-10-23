
<template>
  <div class="space-y-10">
    <section class="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
      <div>
        <h1 class="text-3xl font-semibold text-gray-900">Raporlama Merkezi</h1>
        <p class="text-gray-600">
          Kütüphane operasyonlarınızı izleyin ve stratejik kararlar alın.
        </p>
      </div>
      <div class="flex flex-col gap-3 sm:flex-row sm:items-center">
        <div class="flex flex-col gap-2 sm:flex-row sm:items-center">
          <label for="report-format" class="text-sm font-medium text-gray-600">Rapor formatı</label>
          <select
            id="report-format"
            v-model="selectedFormat"
            class="w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200 sm:w-40"
          >
            <option v-for="format in reportFormats" :key="format.value" :value="format.value">
              {{ format.label }}
            </option>
          </select>
        </div>
        <button
          type="button"
          class="inline-flex items-center justify-center rounded-lg border border-blue-600 bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:border-blue-300 disabled:bg-blue-300"
          :disabled="isRefreshingAll"
          @click="refreshAll"
        >
          <svg class="mr-2 h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 15.75l2.25 2.25L9 15.75m-4.5 0A7.5 7.5 0 1 1 12 19.5m-7.5-3.75A7.5 7.5 0 0 0 12 4.5m0 0L9.75 6.75 12 9m0-4.5h-5.25" />
          </svg>
          Tümünü yenile
        </button>
      </div>
    </section>

    <section v-if="errors.length" class="rounded-xl border border-red-200 bg-red-50 px-4 py-4">
      <div class="flex items-start justify-between gap-4">
        <div>
          <p class="text-sm font-semibold text-red-700">Hata</p>
          <ul class="mt-2 space-y-1 text-sm text-red-600">
            <li v-for="message in errors" :key="message">• {{ message }}</li>
          </ul>
        </div>
        <button
          type="button"
          class="text-sm font-medium text-red-600 transition hover:text-red-800"
          @click="clearErrors"
        >
          Temizle
        </button>
      </div>
    </section>

    <section class="space-y-6">
      <div class="flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
        <div>
          <h2 class="text-xl font-semibold text-gray-900">Özet göstergeler</h2>
          <p class="text-sm text-gray-500">Sistem genelindeki güncel metrikler</p>
        </div>
        <div class="flex flex-wrap items-center gap-3 text-sm text-gray-500">
          <span>
            Son güncelleme:
            <span class="font-medium text-gray-700">{{ formatDateTime(lastUpdated.summaries) }}</span>
          </span>
          <button
            type="button"
            class="inline-flex items-center rounded-lg border border-gray-300 px-3 py-1.5 font-medium text-gray-700 transition hover:border-blue-400 hover:text-blue-600 disabled:cursor-not-allowed disabled:opacity-60"
            :disabled="isRefreshingSummaries || loading.summaries"
            @click="refreshSummaries"
          >
            <svg class="mr-2 h-4 w-4" fill="none" stroke="currentColor" stroke-width="1.5" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" d="M4.5 15.75l2.25 2.25L9 15.75m-4.5 0A7.5 7.5 0 1 1 12 19.5m-7.5-3.75A7.5 7.5 0 0 0 12 4.5m0 0L9.75 6.75 12 9m0-4.5h-5.25" />
            </svg>
            Yenile
          </button>
        </div>
      </div>

      <div v-if="loading.summaries" class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        <div
          v-for="skeleton in 3"
          :key="`summary-skeleton-${skeleton}`"
          class="h-full rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
        >
          <div class="h-5 w-32 animate-pulse rounded bg-gray-200"></div>
          <div class="mt-6 grid grid-cols-2 gap-4">
            <div v-for="item in 4" :key="item" class="space-y-3">
              <div class="h-3 w-24 animate-pulse rounded bg-gray-200"></div>
              <div class="h-6 w-16 animate-pulse rounded bg-gray-200"></div>
            </div>
          </div>
        </div>
      </div>

      <div v-else-if="summaryCards.length" class="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        <div
          v-for="card in summaryCards"
          :key="card.title"
          class="flex h-full flex-col rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
        >
          <div class="flex items-center justify-between">
            <h3 class="text-lg font-semibold text-gray-900">{{ card.title }}</h3>
            <span class="rounded-full bg-blue-50 px-3 py-1 text-xs font-medium text-blue-600">
              {{ card.metrics.length }} metrik
            </span>
          </div>
          <div class="mt-6 grid grid-cols-2 gap-x-6 gap-y-4">
            <div
              v-for="metric in card.metrics"
              :key="metric.key"
              class="space-y-1"
            >
              <p class="text-xs font-semibold uppercase tracking-wide text-gray-500">{{ metric.label }}</p>
              <p class="text-2xl font-semibold text-gray-900">
                {{ metric.value.toLocaleString('tr-TR') }}
              </p>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="rounded-xl border border-dashed border-gray-300 bg-gray-50 px-6 py-12 text-center text-sm text-gray-500">
        Henüz özet verisi bulunmuyor.
      </div>
    </section>

    <section v-if="roleDistribution.length" class="rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
      <div class="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h2 class="text-xl font-semibold text-gray-900">Rol dağılımı</h2>
          <p class="text-sm text-gray-500">
            Toplam {{ totalUsers.toLocaleString('tr-TR') }} kullanıcı
          </p>
        </div>
        <span class="text-sm text-gray-500">
          Son güncelleme:
          <span class="font-medium text-gray-700">{{ formatDateTime(lastUpdated.summaries) }}</span>
        </span>
      </div>
      <div class="mt-6 space-y-4">
        <div v-for="role in roleDistribution" :key="role.label" class="space-y-2">
          <div class="flex items-center justify-between text-sm">
            <span class="font-medium text-gray-700">{{ role.label }}</span>
            <span class="text-gray-500">
              {{ role.count.toLocaleString('tr-TR') }} · %{{ role.formattedPercent }}
            </span>
          </div>
          <div class="h-2 rounded-full bg-gray-100">
            <div
              class="h-full rounded-full bg-blue-500 transition-all duration-300"
              :style="{ width: `${Math.min(role.percent, 100).toFixed(1)}%` }"
            ></div>
          </div>
        </div>
      </div>
    </section>

    <section class="space-y-6 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
      <div class="flex flex-col gap-2 lg:flex-row lg:items-center lg:justify-between">
        <div>
          <h2 class="text-xl font-semibold text-gray-900">En çok ödünç verilenler</h2>
          <p class="text-sm text-gray-500">
            Seçilen boyut: {{ dimensionLabels[loanAggregateFilters.dimension] }}
          </p>
        </div>
        <span class="text-sm text-gray-500">
          Son güncelleme:
          <span class="font-medium text-gray-700">{{ formatDateTime(lastUpdated.loanAggregates) }}</span>
        </span>
      </div>

      <div class="grid gap-4 lg:grid-cols-5">
        <div class="lg:col-span-2">
          <label class="text-sm font-medium text-gray-700">Boyut</label>
          <div class="mt-2 flex flex-wrap gap-2">
            <button
              v-for="option in dimensionOptions"
              :key="option.value"
              type="button"
              class="rounded-lg border px-3 py-2 text-sm font-medium transition"
              :class="loanAggregateFilters.dimension === option.value
                ? 'border-blue-600 bg-blue-600 text-white shadow'
                : 'border-gray-300 bg-white text-gray-600 hover:border-blue-300 hover:text-blue-600'"
              @click="onSelectAggregateDimension(option.value)"
            >
              {{ option.label }}
            </button>
          </div>
        </div>

        <div>
          <label for="aggregate-top" class="text-sm font-medium text-gray-700">Top N</label>
          <select
            id="aggregate-top"
            v-model.number="loanAggregateFilters.top"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
          >
            <option :value="5">5</option>
            <option :value="10">10</option>
            <option :value="20">20</option>
            <option :value="50">50</option>
          </select>
        </div>

        <div>
          <label for="aggregate-library" class="text-sm font-medium text-gray-700">Kütüphane</label>
          <select
            id="aggregate-library"
            v-model="loanAggregateFilters.kutuphaneId"
            :disabled="loading.libraries"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200 disabled:cursor-not-allowed disabled:bg-gray-100"
          >
            <option value="">Tümü</option>
            <option v-for="library in libraries" :key="library.id" :value="library.id">
              {{ library.name }}
            </option>
          </select>
        </div>

        <div>
          <label for="aggregate-start" class="text-sm font-medium text-gray-700">Başlangıç</label>
          <input
            id="aggregate-start"
            v-model="loanAggregateFilters.baslangic"
            type="date"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
          />
        </div>

        <div>
          <label for="aggregate-end" class="text-sm font-medium text-gray-700">Bitiş</label>
          <input
            id="aggregate-end"
            v-model="loanAggregateFilters.bitis"
            type="date"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
          />
        </div>
      </div>

      <div class="flex flex-wrap items-center gap-3">
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-blue-600 bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:border-blue-300 disabled:bg-blue-300"
          :disabled="loading.loanAggregates"
          @click="applyLoanAggregatesFilters"
        >
          Filtreleri uygula
        </button>
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-gray-300 px-3 py-2 text-sm font-medium text-gray-700 transition hover:border-gray-400 hover:text-gray-900"
          @click="handleResetLoanAggregates"
        >
          Sıfırla
        </button>
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-emerald-600 bg-emerald-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-emerald-700 disabled:cursor-not-allowed disabled:border-emerald-300 disabled:bg-emerald-300"
          :disabled="exporting.loanAggregates || loading.loanAggregates || loanAggregates.length === 0"
          @click="exportLoanAggregates"
        >
          Dışa aktar
        </button>
      </div>

      <div>
        <div v-if="loading.loanAggregates" class="space-y-3">
          <div
            v-for="row in 8"
            :key="`aggregate-skeleton-${row}`"
            class="h-12 animate-pulse rounded-lg border border-gray-200 bg-gray-100"
          ></div>
        </div>
        <div v-else-if="loanAggregates.length" class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200 text-left text-sm">
            <thead class="bg-gray-50">
              <tr>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Sıra</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Ad</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Ödünç sayısı</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-100">
              <tr v-for="(aggregate, index) in loanAggregates" :key="aggregate.entityId ?? `${aggregate.dimension}-${index}`" class="hover:bg-gray-50">
                <td class="px-4 py-3 text-sm font-semibold text-gray-900">#{{ index + 1 }}</td>
                <td class="px-4 py-3">
                  <div class="flex flex-col">
                    <span class="font-medium text-gray-900">{{ aggregate.displayName }}</span>
                    <span class="text-xs text-gray-500">
                      {{ dimensionLabels[aggregate.dimension] }} · ID: {{ aggregate.entityId ?? 'Belirtilmemiş' }}
                    </span>
                  </div>
                </td>
                <td class="px-4 py-3">
                  <span class="rounded-full bg-blue-50 px-3 py-1 text-sm font-semibold text-blue-600">
                    {{ aggregate.loanCount.toLocaleString('tr-TR') }}
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else class="rounded-lg border border-dashed border-gray-300 bg-gray-50 px-6 py-10 text-center text-sm text-gray-500">
          Seçilen kriterler için veri bulunamadı. Farklı filtrelerle tekrar deneyin.
        </div>
      </div>
    </section>

    <section class="space-y-6 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
      <div class="flex flex-col gap-2 lg:flex-row lg:items-center lg:justify-between">
        <div>
          <h2 class="text-xl font-semibold text-gray-900">Ödünç kullanım istatistikleri</h2>
          <p class="text-sm text-gray-500">{{ loanUsageCaption }}</p>
        </div>
        <span class="text-sm text-gray-500">
          Son güncelleme:
          <span class="font-medium text-gray-700">{{ formatDateTime(lastUpdated.loanUsage) }}</span>
        </span>
      </div>

      <div class="grid gap-4 md:grid-cols-4">
        <div>
          <label for="usage-library" class="text-sm font-medium text-gray-700">Kütüphane</label>
          <select
            id="usage-library"
            v-model="loanUsageFilters.kutuphaneId"
            :disabled="loading.libraries"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200 disabled:cursor-not-allowed disabled:bg-gray-100"
          >
            <option value="">Tümü</option>
            <option v-for="library in libraries" :key="`usage-lib-${library.id}`" :value="library.id">
              {{ library.name }}
            </option>
          </select>
        </div>
        <div>
          <label for="usage-start" class="text-sm font-medium text-gray-700">Başlangıç</label>
          <input
            id="usage-start"
            v-model="loanUsageFilters.baslangic"
            type="date"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
          />
        </div>
        <div>
          <label for="usage-end" class="text-sm font-medium text-gray-700">Bitiş</label>
          <input
            id="usage-end"
            v-model="loanUsageFilters.bitis"
            type="date"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
          />
        </div>
      </div>

      <div class="flex flex-wrap items-center gap-3">
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-blue-600 bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:border-blue-300 disabled:bg-blue-300"
          :disabled="loading.loanUsage"
          @click="applyLoanUsageFilters"
        >
          Filtreleri uygula
        </button>
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-gray-300 px-3 py-2 text-sm font-medium text-gray-700 transition hover:border-gray-400 hover:text-gray-900"
          @click="handleResetLoanUsage"
        >
          Sıfırla
        </button>
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-emerald-600 bg-emerald-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-emerald-700 disabled:cursor-not-allowed disabled:border-emerald-300 disabled:bg-emerald-300"
          :disabled="exporting.loanUsage || loading.loanUsage || loanUsage.length === 0"
          @click="exportLoanUsage"
        >
          Dışa aktar
        </button>
      </div>

      <div>
        <div v-if="loading.loanUsage" class="space-y-3">
          <div v-for="row in 8" :key="`usage-skeleton-${row}`" class="h-12 animate-pulse rounded-lg border border-gray-200 bg-gray-100"></div>
        </div>
        <div v-else-if="loanUsage.length" class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200 text-left text-sm">
            <thead class="bg-gray-50">
              <tr>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Materyal</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Toplam ödünç</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Aktif</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Geciken</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">İade edildi</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-100">
              <tr v-for="item in loanUsage" :key="`${item.kutuphaneId}-${item.materyalId}`" class="hover:bg-gray-50">
                <td class="px-4 py-3">
                  <div class="font-medium text-gray-900">
                    {{ item.materyalBaslik ?? 'Başlık bulunamadı' }}
                  </div>
                  <div class="text-xs text-gray-500">
                    Materyal ID: {{ item.materyalId }} · Kütüphane ID: {{ item.kutuphaneId }}
                  </div>
                </td>
                <td class="px-4 py-3 font-semibold text-gray-900">
                  {{ item.toplamOdunc.toLocaleString('tr-TR') }}
                </td>
                <td class="px-4 py-3 text-gray-700">
                  {{ item.aktifOdunc.toLocaleString('tr-TR') }}
                </td>
                <td class="px-4 py-3 text-gray-700">
                  <span class="rounded-full bg-amber-50 px-3 py-1 text-sm font-medium text-amber-600">
                    {{ item.gecikenOdunc.toLocaleString('tr-TR') }}
                  </span>
                </td>
                <td class="px-4 py-3 text-gray-700">
                  {{ item.iadeEdilenOdunc.toLocaleString('tr-TR') }}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else class="rounded-lg border border-dashed border-gray-300 bg-gray-50 px-6 py-10 text-center text-sm text-gray-500">
          Seçilen filtrelerle sonuç bulunamadı.
        </div>
      </div>
    </section>

    <section class="space-y-6 rounded-xl border border-gray-200 bg-white p-6 shadow-sm">
      <div class="flex flex-col gap-2 lg:flex-row lg:items-center lg:justify-between">
        <div>
          <h2 class="text-xl font-semibold text-gray-900">Gecikmiş ödünçler</h2>
          <p class="text-sm text-gray-500">
            Aktif ödünçler içinde son teslim tarihini aşan kayıtlar
          </p>
        </div>
        <span class="text-sm text-gray-500">
          Son güncelleme:
          <span class="font-medium text-gray-700">{{ formatDateTime(lastUpdated.overdueLoans) }}</span>
        </span>
      </div>

      <div class="grid gap-4 md:grid-cols-3">
        <div>
          <label for="overdue-library" class="text-sm font-medium text-gray-700">Kütüphane</label>
          <select
            id="overdue-library"
            v-model="overdueFilters.kutuphaneId"
            :disabled="loading.libraries"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200 disabled:cursor-not-allowed disabled:bg-gray-100"
          >
            <option value="">Tümü</option>
            <option v-for="library in libraries" :key="`overdue-lib-${library.id}`" :value="library.id">
              {{ library.name }}
            </option>
          </select>
        </div>
        <div>
          <label for="overdue-user" class="text-sm font-medium text-gray-700">Kullanıcı ID</label>
          <input
            id="overdue-user"
            v-model="overdueFilters.kullaniciId"
            type="text"
            placeholder="Guid veya kimlik"
            class="mt-2 w-full rounded-lg border border-gray-300 px-3 py-2 text-sm text-gray-700 placeholder:text-gray-400 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
          />
        </div>
      </div>

      <div class="flex flex-wrap items-center gap-3">
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-blue-600 bg-blue-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:border-blue-300 disabled:bg-blue-300"
          :disabled="loading.overdueLoans"
          @click="applyOverdueFilters"
        >
          Filtreleri uygula
        </button>
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-gray-300 px-3 py-2 text-sm font-medium text-gray-700 transition hover:border-gray-400 hover:text-gray-900"
          @click="handleResetOverdue"
        >
          Sıfırla
        </button>
        <button
          type="button"
          class="inline-flex items-center rounded-lg border border-emerald-600 bg-emerald-600 px-4 py-2 text-sm font-semibold text-white transition hover:bg-emerald-700 disabled:cursor-not-allowed disabled:border-emerald-300 disabled:bg-emerald-300"
          :disabled="exporting.overdueLoans || loading.overdueLoans || overdueLoans.length === 0"
          @click="exportOverdueLoans"
        >
          Dışa aktar
        </button>
      </div>

      <div>
        <div v-if="loading.overdueLoans" class="space-y-3">
          <div v-for="row in 8" :key="`overdue-skeleton-${row}`" class="h-14 animate-pulse rounded-lg border border-gray-200 bg-gray-100"></div>
        </div>
        <div v-else-if="overdueLoans.length" class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200 text-left text-sm">
            <thead class="bg-gray-50">
              <tr>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Materyal</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Kullanıcı</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Son teslim</th>
                <th scope="col" class="px-4 py-3 font-semibold text-gray-600">Geciken gün</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-100">
              <tr v-for="loan in overdueLoans" :key="loan.oduncIslemiId" class="align-top hover:bg-gray-50">
                <td class="px-4 py-3">
                  <div class="font-medium text-gray-900">
                    {{ loan.materyalBaslik ?? 'Materyal başlığı bulunamadı' }}
                  </div>
                  <div class="mt-1 text-xs text-gray-500">
                    Ödünç ID: {{ loan.oduncIslemiId }} · Nüsha ID: {{ loan.nushaId }}
                  </div>
                </td>
                <td class="px-4 py-3 text-sm text-gray-700">
                  {{ loan.kullaniciId }}
                  <div class="text-xs text-gray-500">Kütüphane ID: {{ loan.kutuphaneId }}</div>
                </td>
                <td class="px-4 py-3 text-sm text-gray-700">
                  <div>{{ formatDate(loan.sonTeslimTarihi) }}</div>
                  <div class="text-xs text-gray-500">
                    Alınma: {{ formatDate(loan.alinmaTarihi) }}
                  </div>
                </td>
                <td class="px-4 py-3">
                  <div class="flex flex-col gap-2">
                    <span class="inline-flex items-center justify-center rounded-full bg-red-50 px-3 py-1 text-sm font-semibold text-red-600">
                      {{ loan.gecikenGun.toLocaleString('tr-TR') }} gün
                    </span>
                    <div class="h-1.5 rounded-full bg-red-100">
                      <div
                        class="h-full rounded-full bg-red-500 transition-all duration-300"
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
        <div v-else class="rounded-lg border border-dashed border-gray-300 bg-gray-50 px-6 py-10 text-center text-sm text-gray-500">
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
