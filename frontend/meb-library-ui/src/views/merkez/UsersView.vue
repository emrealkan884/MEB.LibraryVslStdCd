<template>
  <div class="space-y-6">
    <!-- Sayfa Başlığı -->
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Kullanıcı Yönetimi</h1>
        <p class="text-gray-600">Kullanıcı hesapları ve yetki yönetimi</p>
      </div>
      <button class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center">
        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
        </svg>
        Yeni Kullanıcı
      </button>
    </div>

    <!-- Kullanıcı Filtreleme -->
    <div class="bg-white rounded-lg shadow-sm border p-6">
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Rol</label>
          <select class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500">
            <option>Tümü</option>
            <option>Bakanlık Yetkilisi</option>
            <option>İl Yetkilisi</option>
            <option>İlçe Yetkilisi</option>
            <option>Okul Kütüphane Yöneticisi</option>
            <option>Öğretmen</option>
            <option>Öğrenci</option>
          </select>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Kütüphane Türü</label>
          <select class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500">
            <option>Tümü</option>
            <option>Merkez Kütüphane</option>
            <option>Okul Kütüphanesi</option>
          </select>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Arama</label>
          <input
            type="text"
            placeholder="Ad, soyad veya T.C. No"
            class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>
        <div class="flex items-end">
          <button class="w-full bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded-md">
            Filtrele
          </button>
        </div>
      </div>
    </div>

    <!-- Kullanıcı Tablosu -->
    <div class="bg-white rounded-lg shadow-sm border">
      <div class="p-6 border-b">
        <h3 class="text-lg font-medium text-gray-900">Kullanıcı Listesi</h3>
      </div>
      <div class="p-6">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Ad Soyad</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">T.C. No</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Rol</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Kütüphane</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Durum</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Son Giriş</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">İşlemler</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">Ahmet Yılmaz</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">12345678901</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-purple-100 text-purple-800">
                    Bakanlık Yetkilisi
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">Merkez Kütüphane</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800">
                    Aktif
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">2 saat önce</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <button class="text-blue-600 hover:text-blue-900 mr-3">Düzenle</button>
                  <button class="text-red-600 hover:text-red-900">Sil</button>
                </td>
              </tr>
              <tr>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">Ayşe Kara</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">12345678902</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-blue-100 text-blue-800">
                    Okul Kütüphane Yöneticisi
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">Çankaya Lisesi</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800">
                    Aktif
                  </span>
                </td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-500">1 gün önce</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
                  <button class="text-blue-600 hover:text-blue-900 mr-3">Düzenle</button>
                  <button class="text-red-600 hover:text-red-900">Sil</button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Sayfa içeriği hazır - şablon olarak kullanılacak
</script>