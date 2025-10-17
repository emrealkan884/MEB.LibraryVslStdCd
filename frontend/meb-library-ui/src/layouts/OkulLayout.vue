<template>
  <div class="min-h-screen bg-gray-50">
    <!-- Sidebar -->
    <div class="fixed inset-y-0 left-0 w-64 bg-white shadow-lg">
      <!-- Logo -->
      <div class="flex items-center justify-center h-16 px-4 bg-green-600">
        <h1 class="text-white text-lg font-bold">🏫 Okul Kütüphanesi</h1>
      </div>

      <!-- Navigation Menu -->
      <nav class="mt-8">
        <div class="px-4 space-y-2">
          <!-- Dashboard -->
          <router-link
            to="/okul/dashboard"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-green-50 hover:text-green-700 rounded-lg transition-colors"
            :class="{ 'bg-green-50 text-green-700': $route.name === 'okul-dashboard' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2H5a2 2 0 00-2-2z" />
            </svg>
            Ana Sayfa
          </router-link>

          <!-- OPAC Arama -->
          <router-link
            to="/okul/opac"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-green-50 hover:text-green-700 rounded-lg transition-colors"
            :class="{ 'bg-green-50 text-green-700': $route.name === 'okul-opac' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
            Kitap Arama
          </router-link>

          <!-- Ödünç İşlemleri -->
          <router-link
            to="/okul/odunc-islemleri"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-green-50 hover:text-green-700 rounded-lg transition-colors"
            :class="{ 'bg-green-50 text-green-700': $route.name === 'okul-circulation' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4" />
            </svg>
            Ödünç İşlemleri
          </router-link>

          <!-- Yeni Katalog Talebi -->
          <router-link
            to="/okul/yeni-katalog-talebi"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-green-50 hover:text-green-700 rounded-lg transition-colors"
            :class="{ 'bg-green-50 text-green-700': $route.name === 'okul-catalog-request' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
            Katalog Talebi
          </router-link>

          <!-- Etkinlikler -->
          <router-link
            to="/okul/etkinlikler"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-green-50 hover:text-green-700 rounded-lg transition-colors"
            :class="{ 'bg-green-50 text-green-700': $route.name === 'okul-events' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
            </svg>
            Etkinlikler
          </router-link>

          <!-- Raporlar -->
          <router-link
            to="/okul/raporlar"
            class="flex items-center px-4 py-2 text-gray-700 hover:bg-green-50 hover:text-green-700 rounded-lg transition-colors"
            :class="{ 'bg-green-50 text-green-700': $route.name === 'okul-reports' }"
          >
            <svg class="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
            </svg>
            Raporlar
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
                class="text-sm border-gray-300 rounded-md focus:ring-green-500 focus:border-green-500"
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
                  <div class="h-8 w-8 rounded-full bg-green-500 flex items-center justify-center">
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

const getPageTitle = (routeName: string) => {
  const titles: Record<string, string> = {
    'okul-dashboard': 'Ana Sayfa',
    'okul-opac': 'Kitap Arama',
    'okul-circulation': 'Ödünç İşlemleri',
    'okul-catalog-request': 'Katalog Talebi',
    'okul-events': 'Etkinlikler',
    'okul-reports': 'Raporlar'
  }
  return titles[routeName] || 'Sayfa'
}

const getPageDescription = (routeName: string) => {
  const descriptions: Record<string, string> = {
    'okul-dashboard': 'Okul kütüphanesi genel durumu',
    'okul-opac': 'Katalogda kitap arama ve tarama',
    'okul-circulation': 'Öğrenci ödünç ve iade işlemleri',
    'okul-catalog-request': 'Merkez kütüphaneye yeni kitap talebi',
    'okul-events': 'Okuma kültürü etkinlikleri',
    'okul-reports': 'Okul kütüphanesi raporları'
  }
  return descriptions[routeName] || ''
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