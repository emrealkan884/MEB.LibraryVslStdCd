<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Listeler</h1>
        <p class="text-gray-600">Kitap ve materyal listeleri</p>
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div class="bg-white rounded-lg shadow-sm border p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Yeni Kitaplar</h3>
        <div class="space-y-3">
          <div class="flex items-center space-x-3">
            <div class="w-2 h-2 bg-blue-400 rounded-full"></div>
            <div>
              <p class="text-sm font-medium">Benim Adım Kırmızı</p>
              <p class="text-xs text-gray-500">Orhan Pamuk - 2024</p>
            </div>
          </div>
          <div class="flex items-center space-x-3">
            <div class="w-2 h-2 bg-green-400 rounded-full"></div>
            <div>
              <p class="text-sm font-medium">1984</p>
              <p class="text-xs text-gray-500">George Orwell - 2024</p>
            </div>
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow-sm border p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Popüler Kitaplar</h3>
        <div class="space-y-3">
          <div class="flex items-center space-x-3">
            <div class="w-2 h-2 bg-purple-400 rounded-full"></div>
            <div>
              <p class="text-sm font-medium">Suç ve Ceza</p>
              <p class="text-xs text-gray-500">Dostoyevski - 5 ödünç</p>
            </div>
          </div>
          <div class="flex items-center space-x-3">
            <div class="w-2 h-2 bg-orange-400 rounded-full"></div>
            <div>
              <p class="text-sm font-medium">Kürk Mantolu Madonna</p>
              <p class="text-xs text-gray-500">Sabahattin Ali - 3 ödünç</p>
            </div>
          </div>
        </div>
      </div>

      <div class="bg-white rounded-lg shadow-sm border p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Kategorilere Göre</h3>
        <div class="space-y-3">
          <div class="flex items-center justify-between">
            <span class="text-sm text-gray-600">Roman</span>
            <span class="text-sm font-medium">245</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-sm text-gray-600">Bilim</span>
            <span class="text-sm font-medium">89</span>
          </div>
          <div class="flex items-center justify-between">
            <span class="text-sm text-gray-600">Tarih</span>
            <span class="text-sm font-medium">156</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Sayfa içeriği hazır
</script>