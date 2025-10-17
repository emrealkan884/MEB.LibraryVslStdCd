<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Header -->
    <header class="bg-white shadow-sm border-b">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex justify-between items-center h-16">
          <!-- Logo ve Başlık -->
          <div class="flex items-center">
            <div class="flex-shrink-0">
              <h1 class="text-xl font-semibold text-gray-900">
                MEB Kütüphane Otomasyon Sistemi
              </h1>
            </div>
          </div>

          <!-- Sağ Menü -->
          <div class="flex items-center space-x-4">
            <!-- Dil Seçimi -->
            <select
              v-model="currentLanguage"
              @change="changeLanguage"
              class="text-sm border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500"
            >
              <option value="tr">🇹🇷 Türkçe</option>
              <option value="en">🇺🇸 English</option>
            </select>

            <!-- Kullanıcı Menüsü -->
            <div class="relative">
              <button
                @click="showUserMenu = !showUserMenu"
                class="flex items-center space-x-2 text-sm rounded-full focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                <div class="h-8 w-8 rounded-full bg-blue-500 flex items-center justify-center">
                  <span class="text-white text-sm font-medium">
                    {{ user?.firstName?.[0] }}{{ user?.lastName?.[0] }}
                  </span>
                </div>
                <span class="text-gray-700">{{ user?.firstName }} {{ user?.lastName }}</span>
              </button>

              <!-- Dropdown Menu -->
              <div
                v-if="showUserMenu"
                class="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg py-1 z-50 border"
              >
                <router-link
                  to="/profile"
                  class="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                >
                  Profil
                </router-link>
                <button
                  @click="logout"
                  class="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                >
                  Çıkış Yap
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Ana İçerik -->
    <main class="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
      <!-- Rol Tabanlı Yönlendirme -->
      <div v-if="user?.libraryType === 'Merkez'" class="text-center py-12">
        <h2 class="text-2xl font-bold text-gray-900 mb-4">
          Merkez Kütüphane Paneli
        </h2>
        <p class="text-gray-600 mb-8">
          Tüm kütüphane yönetim fonksiyonlarına erişebilirsiniz.
        </p>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Hızlı Erişim Kartları -->
          <router-link
            to="/merkez/kataloglama"
            class="p-6 bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow"
          >
            <div class="flex items-center">
              <div class="p-2 bg-blue-100 rounded-lg">
                <svg class="h-6 w-6 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-lg font-medium text-gray-900">Kataloglama</h3>
                <p class="text-sm text-gray-500">MARC21 ve RDA standartları</p>
              </div>
            </div>
          </router-link>

          <router-link
            to="/merkez/dolasim"
            class="p-6 bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow"
          >
            <div class="flex items-center">
              <div class="p-2 bg-green-100 rounded-lg">
                <svg class="h-6 w-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-lg font-medium text-gray-900">Dolaşım</h3>
                <p class="text-sm text-gray-500">Ödünç ve iade işlemleri</p>
              </div>
            </div>
          </router-link>

          <router-link
            to="/merkez/raporlar"
            class="p-6 bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow"
          >
            <div class="flex items-center">
              <div class="p-2 bg-purple-100 rounded-lg">
                <svg class="h-6 w-6 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-lg font-medium text-gray-900">Raporlar</h3>
                <p class="text-sm text-gray-500">Detaylı istatistikler</p>
              </div>
            </div>
          </router-link>
        </div>
      </div>

      <div v-else-if="user?.libraryType === 'Okul'" class="text-center py-12">
        <h2 class="text-2xl font-bold text-gray-900 mb-4">
          Okul Kütüphanesi Paneli
        </h2>
        <p class="text-gray-600 mb-8">
          Öğrenci ve öğretmenlerin kitap erişimi için özelleştirilmiş arayüz.
        </p>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <!-- Okul İçin Hızlı Erişim Kartları -->
          <router-link
            to="/okul/opac"
            class="p-6 bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow"
          >
            <div class="flex items-center">
              <div class="p-2 bg-indigo-100 rounded-lg">
                <svg class="h-6 w-6 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-lg font-medium text-gray-900">Kitap Arama</h3>
                <p class="text-sm text-gray-500">OPAC katalog tarama</p>
              </div>
            </div>
          </router-link>

          <router-link
            to="/okul/odunc-islemleri"
            class="p-6 bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow"
          >
            <div class="flex items-center">
              <div class="p-2 bg-yellow-100 rounded-lg">
                <svg class="h-6 w-6 text-yellow-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1" />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-lg font-medium text-gray-900">Ödünç İşlemleri</h3>
                <p class="text-sm text-gray-500">Kitap ödünç alma ve iade</p>
              </div>
            </div>
          </router-link>

          <router-link
            to="/okul/etkinlikler"
            class="p-6 bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow"
          >
            <div class="flex items-center">
              <div class="p-2 bg-pink-100 rounded-lg">
                <svg class="h-6 w-6 text-pink-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
              </div>
              <div class="ml-4">
                <h3 class="text-lg font-medium text-gray-900">Etkinlikler</h3>
                <p class="text-sm text-gray-500">Okuma kültürü etkinlikleri</p>
              </div>
            </div>
          </router-link>
        </div>
      </div>

      <!-- Bilinmeyen rol için -->
      <div v-else class="text-center py-12">
        <h2 class="text-2xl font-bold text-gray-900 mb-4">
          Hoş Geldiniz
        </h2>
        <p class="text-gray-600">
          Rolünüz belirlenene kadar ana sayfadasınız.
        </p>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18nStore } from '@/stores/i18n'

const router = useRouter()
const authStore = useAuthStore()
const i18nStore = useI18nStore()

const showUserMenu = ref(false)
const currentLanguage = computed(() => i18nStore.currentLanguage)

const { user } = authStore

const changeLanguage = (event: Event) => {
  const target = event.target as HTMLSelectElement
  i18nStore.setLanguage(target.value as 'tr' | 'en')
}

const logout = () => {
  authStore.logout()
  router.push('/login')
}

// Sayfa yüklendiğinde dil ayarlarını başlat
onMounted(() => {
  i18nStore.initializeLanguage()
})
</script>