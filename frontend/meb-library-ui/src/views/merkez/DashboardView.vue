<template>
  <div class="space-y-6">
    <!-- İstatistik Kartları -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <div class="bg-white rounded-lg shadow-sm p-6 border">
        <div class="flex items-center">
          <div class="p-2 bg-blue-100 rounded-lg">
            <svg class="h-6 w-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
          </div>
          <div class="ml-4">
            <p class="text-sm font-medium text-gray-600">Toplam Kitap</p>
            <p class="text-2xl font-semibold text-gray-900">12,543</p>
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow-sm p-6 border">
        <div class="flex items-center">
          <div class="p-2 bg-green-100 rounded-lg">
            <svg class="h-6 w-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
            </svg>
          </div>
          <div class="ml-4">
            <p class="text-sm font-medium text-gray-600">Aktif Ödünç</p>
            <p class="text-2xl font-semibold text-gray-900">1,247</p>
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow-sm p-6 border">
        <div class="flex items-center">
          <div class="p-2 bg-red-100 rounded-lg">
            <svg class="h-6 w-6 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
          </div>
          <div class="ml-4">
            <p class="text-sm font-medium text-gray-600">Geciken Ödünç</p>
            <p class="text-2xl font-semibold text-gray-900">23</p>
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow-sm p-6 border">
        <div class="flex items-center">
          <div class="p-2 bg-purple-100 rounded-lg">
            <svg class="h-6 w-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z" />
            </svg>
          </div>
          <div class="ml-4">
            <p class="text-sm font-medium text-gray-600">Toplam Kullanıcı</p>
            <p class="text-2xl font-semibold text-gray-900">8,901</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Grafikler ve Listeler -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Son Aktiviteler -->
      <div class="bg-white rounded-lg shadow-sm border">
        <div class="p-6 border-b">
          <h3 class="text-lg font-medium text-gray-900">Son Aktiviteler</h3>
        </div>
        <div class="p-6">
          <div class="space-y-4">
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 bg-green-400 rounded-full"></div>
              <div class="flex-1">
                <p class="text-sm text-gray-900">Yeni kitap kataloglandı</p>
                <p class="text-xs text-gray-500">2 dakika önce</p>
              </div>
            </div>
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 bg-blue-400 rounded-full"></div>
              <div class="flex-1">
                <p class="text-sm text-gray-900">Ödünç işlemi tamamlandı</p>
                <p class="text-xs text-gray-500">5 dakika önce</p>
              </div>
            </div>
            <div class="flex items-center space-x-3">
              <div class="w-2 h-2 bg-purple-400 rounded-full"></div>
              <div class="flex-1">
                <p class="text-sm text-gray-900">Yeni kullanıcı kaydı</p>
                <p class="text-xs text-gray-500">10 dakika önce</p>
              </div>
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
          <div class="grid grid-cols-2 gap-3">
            <button class="p-3 bg-blue-50 hover:bg-blue-100 rounded-lg text-center transition-colors">
              <svg class="w-6 h-6 mx-auto mb-2 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
              </svg>
              <span class="text-sm font-medium text-blue-700">Kitap Ekle</span>
            </button>

            <button class="p-3 bg-green-50 hover:bg-green-100 rounded-lg text-center transition-colors">
              <svg class="w-6 h-6 mx-auto mb-2 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
              </svg>
              <span class="text-sm font-medium text-green-700">Ödünç Ver</span>
            </button>

            <button class="p-3 bg-purple-50 hover:bg-purple-100 rounded-lg text-center transition-colors">
              <svg class="w-6 h-6 mx-auto mb-2 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
              </svg>
              <span class="text-sm font-medium text-purple-700">Rapor Al</span>
            </button>

            <button class="p-3 bg-orange-50 hover:bg-orange-100 rounded-lg text-center transition-colors">
              <svg class="w-6 h-6 mx-auto mb-2 text-orange-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z" />
              </svg>
              <span class="text-sm font-medium text-orange-700">Kullanıcı Ekle</span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Sistem Durumu -->
    <div class="bg-white rounded-lg shadow-sm border">
      <div class="p-6 border-b">
        <h3 class="text-lg font-medium text-gray-900">Sistem Durumu</h3>
      </div>
      <div class="p-6">
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
          <div class="text-center">
            <div class="inline-flex items-center justify-center w-12 h-12 bg-green-100 rounded-full mb-3">
              <svg class="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <p class="text-sm font-medium text-gray-900">Veritabanı</p>
            <p class="text-xs text-gray-500">Bağlı ve Çalışıyor</p>
          </div>

          <div class="text-center">
            <div class="inline-flex items-center justify-center w-12 h-12 bg-blue-100 rounded-full mb-3">
              <svg class="w-6 h-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" />
              </svg>
            </div>
            <p class="text-sm font-medium text-gray-900">API Servisleri</p>
            <p class="text-xs text-gray-500">Aktif</p>
          </div>

          <div class="text-center">
            <div class="inline-flex items-center justify-center w-12 h-12 bg-purple-100 rounded-full mb-3">
              <svg class="w-6 h-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
              </svg>
            </div>
            <p class="text-sm font-medium text-gray-900">Yedekleme</p>
            <p class="text-xs text-gray-500">Son: 2 saat önce</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Sayfa içeriği hazır - şablon olarak kullanılacak
</script>