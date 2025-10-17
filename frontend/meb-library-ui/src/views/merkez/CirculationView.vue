<template>
  <div class="space-y-6">
    <!-- Sayfa Başlığı -->
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Dolaşım Yönetimi</h1>
        <p class="text-gray-600">Ödünç verme, iade ve süre uzatma işlemleri</p>
      </div>
      <div class="flex space-x-3">
        <button class="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          Yeni Ödünç
        </button>
        <button class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center">
          <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
          </svg>
          İade Al
        </button>
      </div>
    </div>

    <!-- Ödünç İşlemleri Kartları -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
      <!-- Aktif Ödünç -->
      <div class="bg-white rounded-lg shadow-sm border p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Aktif Ödünç</p>
            <p class="text-2xl font-semibold text-green-600">1,247</p>
          </div>
          <div class="p-3 bg-green-100 rounded-full">
            <svg class="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Geciken Ödünç -->
      <div class="bg-white rounded-lg shadow-sm border p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Geciken Ödünç</p>
            <p class="text-2xl font-semibold text-red-600">23</p>
          </div>
          <div class="p-3 bg-red-100 rounded-full">
            <svg class="w-6 h-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
        </div>
      </div>

      <!-- Bugünkü İade -->
      <div class="bg-white rounded-lg shadow-sm border p-6">
        <div class="flex items-center justify-between">
          <div>
            <p class="text-sm font-medium text-gray-600">Bugünkü İade</p>
            <p class="text-2xl font-semibold text-blue-600">45</p>
          </div>
          <div class="p-3 bg-blue-100 rounded-full">
            <svg class="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
            </svg>
          </div>
        </div>
      </div>
    </div>

    <!-- Hızlı İşlemler -->
    <div class="bg-white rounded-lg shadow-sm border">
      <div class="p-6 border-b">
        <h3 class="text-lg font-medium text-gray-900">Hızlı İşlemler</h3>
      </div>
      <div class="p-6">
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          <button class="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-blue-500 hover:bg-blue-50 transition-colors text-center">
            <svg class="w-8 h-8 mx-auto mb-2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
            <span class="text-sm font-medium text-gray-600">Yeni Ödünç</span>
          </button>

          <button class="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-green-500 hover:bg-green-50 transition-colors text-center">
            <svg class="w-8 h-8 mx-auto mb-2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
            </svg>
            <span class="text-sm font-medium text-gray-600">İade Al</span>
          </button>

          <button class="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-purple-500 hover:bg-purple-50 transition-colors text-center">
            <svg class="w-8 h-8 mx-auto mb-2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <span class="text-sm font-medium text-gray-600">Süre Uzat</span>
          </button>

          <button class="p-4 border-2 border-dashed border-gray-300 rounded-lg hover:border-orange-500 hover:bg-orange-50 transition-colors text-center">
            <svg class="w-8 h-8 mx-auto mb-2 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 17v-2m3 2v-4m3 4v-6m2 10H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
            <span class="text-sm font-medium text-gray-600">Rapor Al</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Son İşlemler Tablosu -->
    <div class="bg-white rounded-lg shadow-sm border">
      <div class="p-6 border-b">
        <h3 class="text-lg font-medium text-gray-900">Son İşlemler</h3>
      </div>
      <div class="p-6">
        <div class="space-y-4">
          <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 bg-green-400 rounded-full"></div>
              <div>
                <p class="text-sm font-medium text-gray-900">Ödünç Verildi</p>
                <p class="text-xs text-gray-500">Benim Adım Kırmızı - Orhan Pamuk</p>
              </div>
            </div>
            <span class="text-xs text-gray-500">2 dk önce</span>
          </div>

          <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 bg-blue-400 rounded-full"></div>
              <div>
                <p class="text-sm font-medium text-gray-900">İade Alındı</p>
                <p class="text-xs text-gray-500">1984 - George Orwell</p>
              </div>
            </div>
            <span class="text-xs text-gray-500">5 dk önce</span>
          </div>

          <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 bg-purple-400 rounded-full"></div>
              <div>
                <p class="text-sm font-medium text-gray-900">Süre Uzatıldı</p>
                <p class="text-xs text-gray-500">Suç ve Ceza - Dostoyevski</p>
              </div>
            </div>
            <span class="text-xs text-gray-500">10 dk önce</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Sayfa içeriği hazır - şablon olarak kullanılacak
</script>