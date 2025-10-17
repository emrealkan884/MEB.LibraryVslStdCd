<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Sidebar -->
    <div class="fixed inset-y-0 left-0 w-64 bg-white shadow-lg">
      <!-- Logo -->
      <div class="flex items-center justify-center h-16 px-4 bg-blue-600">
        <h1 class="text-white text-lg font-bold">🏛️ Merkez Kütüphane</h1>
      </div>

      <!-- Navigation Menu -->
      <nav class="mt-8">
        <div class="px-4 space-y-2">
          <!-- Dashboard -->
          <router-link
            to="/merkez/dashboard"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-dashboard' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2H5a2 2 0 00-2-2z" />
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 5a2 2 0 012-2h4a2 2 0 012 2v2H8V5z" />
            </svg>
            Ana Sayfa
          </router-link>

          <!-- Kataloglama -->
          <router-link
            to="/merkez/kataloglama"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-cataloging' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
            Kataloglama
          </router-link>

          <!-- Dolaşım -->
          <router-link
            to="/merkez/dolasim"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-circulation' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
            </svg>
            Dolaşım
          </router-link>

          <!-- Kullanıcılar -->
          <router-link
            to="/merkez/kullanicilar"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-users' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z" />
            </svg>
            Kullanıcılar
          </router-link>

          <!-- Otoriteler -->
          <router-link
            to="/merkez/otoriteler"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-authorities' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
            </svg>
            Otoriteler
          </router-link>

          <!-- Raporlar -->
          <router-link
            to="/merkez/raporlar"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-reports' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
            </svg>
            Raporlar
          </router-link>

          <!-- Yönetim -->
          <router-link
            to="/merkez/yonetim"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-blue-50 hover:text-blue-700 rounded-lg transition-colors"
            :class="{ 'bg-blue-50 text-blue-700': $route.name === 'merkez-management' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" />
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
            </svg>
            Yönetim
          </router-link>
        </div>
      </nav>
    </div>

    <!-- Main Content -->
    <div class="ml-64">
      <!-- Top Header -->
      <header class="bg-white shadow-sm border-b">
        <div class="px-6 py-4">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="text-2xl font-bold text-gray-900">
                {{ getPageTitle(String($route.name)) }}
              </h2>
              <p class="text-sm text-gray-600 mt-1">
                {{ getPageDescription(String($route.name)) }}
              </p>
            </div>

            <!-- User Info & Language -->
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

              <!-- User Menu -->
              <div class="relative">
                <button
                  @click="showUserMenu = !showUserMenu"
                  class="flex items-center space-x-2 text-sm rounded-full hover:bg-gray-100 p-2"
                >
                  <div class="h-8 w-8 rounded-full bg-blue-500 flex items-center justify-center">
                    <span class="text-white text-sm font-medium">
                      {{ user?.firstName?.[0] }}{{ user?.lastName?.[0] }}
                    </span>
                  </div>
                  <span class="text-gray-700">{{ user?.firstName }} {{ user?.lastName }}</span>
                </button>

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

      <!-- Page Content -->
      <main class="p-6">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18nStore } from '@/stores/i18n'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()
const i18nStore = useI18nStore()

const showUserMenu = ref(false)
const currentLanguage = computed(() => i18nStore.currentLanguage)

const { user } = authStore

const getPageTitle = (routeName: string | undefined) => {
  const titles: Record<string, string> = {
    'merkez-dashboard': 'Ana Sayfa',
    'merkez-cataloging': 'Kataloglama',
    'merkez-circulation': 'Dolaşım',
    'merkez-users': 'Kullanıcı Yönetimi',
    'merkez-authorities': 'Otorite Kontrolü',
    'merkez-reports': 'Raporlar',
    'merkez-management': 'Sistem Yönetimi'
  }
  return titles[routeName || ''] || 'Sayfa'
}

const getPageDescription = (routeName: string | undefined) => {
  const descriptions: Record<string, string> = {
    'merkez-dashboard': 'Merkez kütüphane genel durumu ve istatistikler',
    'merkez-cataloging': 'MARC21 ve RDA standartlarında kataloglama işlemleri',
    'merkez-circulation': 'Ödünç verme ve iade işlemleri yönetimi',
    'merkez-users': 'Kullanıcı hesapları ve yetki yönetimi',
    'merkez-authorities': 'Yazar ve konu otorite kayıtları',
    'merkez-reports': 'Detaylı raporlar ve istatistikler',
    'merkez-management': 'Sistem konfigürasyonu ve ayarları'
  }
  return descriptions[routeName || ''] || ''
}

const changeLanguage = (event: Event) => {
  const target = event.target as HTMLSelectElement
  i18nStore.setLanguage(target.value as 'tr' | 'en')
}

const logout = () => {
  authStore.logout()
  router.push('/login')
}
</script>