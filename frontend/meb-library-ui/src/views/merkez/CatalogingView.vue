<template>
  <div class="space-y-6">
    <!-- Sayfa Başlığı -->
    <div class="flex justify-between items-center">
      <div>
        <h1 class="text-2xl font-bold text-gray-900">Kataloglama</h1>
        <p class="text-gray-600">MARC21 ve RDA standartlarında kitap kataloglama</p>
      </div>
      <button class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center">
        <svg class="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
        </svg>
        Yeni Katalog Ekle
      </button>
    </div>

    <!-- Kataloglama Formu -->
    <div class="bg-white rounded-lg shadow-sm border">
      <div class="p-6 border-b">
        <h3 class="text-lg font-medium text-gray-900">Yeni Katalog Kaydı</h3>
      </div>
      <div class="p-6">
        <form class="space-y-6">
          <!-- ISBN ve Temel Bilgiler -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">ISBN</label>
              <input
                type="text"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="9789754707083"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Başlık</label>
              <input
                type="text"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Kitap başlığı"
              />
            </div>
          </div>

          <!-- Yazar ve Yayın Bilgileri -->
          <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Yazar</label>
              <input
                type="text"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Yazar adı"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Yayınevi</label>
              <input
                type="text"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Yayınevi"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Yayın Yılı</label>
              <input
                type="number"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="2024"
              />
            </div>
          </div>

          <!-- MARC21 Alanı -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">MARC21 Verisi</label>
            <textarea
              rows="4"
              class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder='{"245": {"a": "Kitap Başlığı", "b": "Alt başlık"}}'
            ></textarea>
          </div>

          <!-- Dewey Sınıflaması -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Dewey Sınıfı</label>
              <select class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500">
                <option>000 - Genel Konular</option>
                <option>100 - Felsefe</option>
                <option>200 - Din</option>
                <option>300 - Sosyal Bilimler</option>
                <option>400 - Dil</option>
                <option>500 - Bilim</option>
                <option>600 - Teknoloji</option>
                <option>700 - Sanat</option>
                <option>800 - Edebiyat</option>
                <option>900 - Tarih ve Coğrafya</option>
              </select>
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Materyal Türü</label>
              <select class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500">
                <option>Kitap</option>
                <option>Dergi</option>
                <option>Gazete</option>
                <option>CD/DVD</option>
                <option>E-Kitap</option>
              </select>
            </div>
          </div>

          <!-- Form Butonları -->
          <div class="flex justify-end space-x-3">
            <button
              type="button"
              class="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
            >
              İptal
            </button>
            <button
              type="submit"
              class="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700"
            >
              Kaydet
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- Mevcut Kataloglar Tablosu -->
    <div class="bg-white rounded-lg shadow-sm border">
      <div class="p-6 border-b">
        <h3 class="text-lg font-medium text-gray-900">Mevcut Kataloglar</h3>
      </div>
      <div class="p-6">
        <div class="overflow-x-auto">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">ISBN</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Başlık</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Yazar</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Yayın Yılı</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Durum</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">İşlemler</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">9789754707083</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">Benim Adım Kırmızı</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">Orhan Pamuk</td>
                <td class="px-6 py-4 whitespace-nowrap text-sm text-gray-900">1998</td>
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="inline-flex px-2 py-1 text-xs font-semibold rounded-full bg-green-100 text-green-800">
                    Aktif
                  </span>
                </td>
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