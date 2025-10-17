<template>
  <div class="space-y-6">
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Okul Kütüphanesi Ana Sayfası</h1>
        <p class="text-gray-600">Öğrenci ve öğretmenlerin kitap erişimi için özelleştirilmiş arayüz</p>
      </div>
    </div>

    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <div class="bg-white rounded-lg shadow-sm border p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Kitap Arama (OPAC)</h3>
        <p class="text-gray-600 text-sm mb-4">Katalogda kitap arama ve tarama</p>
        <router-link
          to="/okul/opac"
          class="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-lg text-sm font-medium w-full text-center block"
        >
          Kitap Ara
        </router-link>
      </div>

      <div class="bg-white rounded-lg shadow-sm border p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Ödünç İşlemleri</h3>
        <p class="text-gray-600 text-sm mb-4">Kitap ödünç alma ve iade işlemleri</p>
        <router-link
          to="/okul/odunc-islemleri"
          class="bg-yellow-600 hover:bg-yellow-700 text-white px-4 py-2 rounded-lg text-sm font-medium w-full text-center block"
        >
          İşlemler
        </router-link>
      </div>

      <div class="bg-white rounded-lg shadow-sm border p-6">
        <h3 class="text-lg font-medium text-gray-900 mb-4">Etkinlikler</h3>
        <p class="text-gray-600 text-sm mb-4">Okuma kültürü etkinlikleri</p>
        <router-link
          to="/okul/etkinlikler"
          class="bg-pink-600 hover:bg-pink-700 text-white px-4 py-2 rounded-lg text-sm font-medium w-full text-center block"
        >
          Etkinlikler
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Sayfa içeriği hazır
</script>