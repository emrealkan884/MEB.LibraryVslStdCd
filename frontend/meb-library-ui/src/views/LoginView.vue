<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100 flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-8">
      <!-- Header -->
      <div class="text-center">
        <h2 class="mt-6 text-3xl font-bold text-gray-900">
          {{ $t('login.title') }}
        </h2>
        <p class="mt-2 text-sm text-gray-600">
          MEBBİS veya e-Okul bilgileri ile giriş yapın
        </p>
      </div>

      <!-- Login Form -->
      <form class="mt-8 space-y-6" @submit.prevent="handleLogin">
        <div class="bg-white shadow-xl rounded-lg p-8">
          <!-- Library Type Selection -->
          <div class="mb-6">
            <label class="block text-sm font-medium text-gray-700 mb-3">
              Kütüphane Türü
            </label>
            <div class="grid grid-cols-2 gap-3">
              <button
                type="button"
                @click="selectedLibraryType = 'Merkez'"
                :class="[
                  'p-3 border rounded-lg text-sm font-medium transition-all',
                  selectedLibraryType === 'Merkez'
                    ? 'border-blue-500 bg-blue-50 text-blue-700'
                    : 'border-gray-300 hover:border-gray-400'
                ]"
              >
                🏛️ Merkez Kütüphane
              </button>
              <button
                type="button"
                @click="selectedLibraryType = 'Okul'"
                :class="[
                  'p-3 border rounded-lg text-sm font-medium transition-all',
                  selectedLibraryType === 'Okul'
                    ? 'border-green-500 bg-green-50 text-green-700'
                    : 'border-gray-300 hover:border-gray-400'
                ]"
              >
                🏫 Okul Kütüphanesi
              </button>
            </div>
          </div>

          <!-- Login Method Selection -->
          <div class="mb-6">
            <label class="block text-sm font-medium text-gray-700 mb-3">
              Giriş Yöntemi
            </label>
            <div class="grid grid-cols-2 gap-3">
              <button
                type="button"
                @click="loginMethod = 'meb'"
                :class="[
                  'p-3 border rounded-lg text-sm font-medium transition-all',
                  loginMethod === 'meb'
                    ? 'border-purple-500 bg-purple-50 text-purple-700'
                    : 'border-gray-300 hover:border-gray-400'
                ]"
              >
                👤 MEBBİS
              </button>
              <button
                type="button"
                @click="loginMethod = 'eokul'"
                :class="[
                  'p-3 border rounded-lg text-sm font-medium transition-all',
                  loginMethod === 'eokul'
                    ? 'border-orange-500 bg-orange-50 text-orange-700'
                    : 'border-gray-300 hover:border-gray-400'
                ]"
              >
                🎓 e-Okul
              </button>
            </div>
          </div>

          <!-- TC No Input -->
          <div>
            <label for="tcNo" class="block text-sm font-medium text-gray-700">
              {{ $t('login.tcNo') }}
            </label>
            <input
              id="tcNo"
              v-model="credentials.tcNo"
              type="text"
              required
              class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              placeholder="11 haneli T.C. kimlik numarası"
              maxlength="11"
            />
          </div>

          <!-- Password Input -->
          <div class="mt-4">
            <label for="password" class="block text-sm font-medium text-gray-700">
              {{ $t('login.password') }}
            </label>
            <input
              id="password"
              v-model="credentials.password"
              type="password"
              required
              class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-blue-500 focus:border-blue-500"
              placeholder="Şifreniz"
            />
          </div>

          <!-- Remember Me -->
          <div class="mt-4">
            <label class="flex items-center">
              <input
                v-model="credentials.rememberMe"
                type="checkbox"
                class="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
              />
              <span class="ml-2 text-sm text-gray-700">
                {{ $t('login.rememberMe') }}
              </span>
            </label>
          </div>

          <!-- Error Message -->
          <div v-if="error" class="mt-4 p-3 bg-red-50 border border-red-200 rounded-md">
            <p class="text-sm text-red-600">{{ error }}</p>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="isLoading"
            class="mt-6 w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span v-if="isLoading" class="mr-2">
              <svg class="animate-spin h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
              </svg>
            </span>
            {{ $t('login.submit') }}
          </button>
        </div>
      </form>

      <!-- Footer -->
      <div class="text-center">
        <p class="text-xs text-gray-500">
          Milli Eğitim Bakanlığı Kütüphane Otomasyon Sistemi
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18nStore } from '@/stores/i18n'

const router = useRouter()
const authStore = useAuthStore()
const i18nStore = useI18nStore()

const selectedLibraryType = ref<'Merkez' | 'Okul'>('Merkez')
const loginMethod = ref<'meb' | 'eokul'>('meb')

const credentials = reactive({
  tcNo: '',
  password: '',
  rememberMe: false
})

const isLoading = ref(false)
const error = ref<string | null>(null)

const handleLogin = async () => {
  isLoading.value = true
  error.value = null

  try {
    const result = await authStore.login({
      tcNo: credentials.tcNo,
      password: credentials.password,
      rememberMe: credentials.rememberMe
    })

    if (result.success) {
      // Başarılı giriş - rol tabanlı yönlendirme
      if (authStore.isMerkezKutuphane) {
        router.push('/merkez/dashboard')
      } else if (authStore.isOkulKutuphane) {
        router.push('/okul/dashboard')
      } else {
        router.push('/')
      }
    } else {
      error.value = result.error || 'Giriş başarısız'
    }
  } catch (err) {
    error.value = 'Bir hata oluştu. Lütfen tekrar deneyin.'
  } finally {
    isLoading.value = false
  }
}

// Sayfa yüklendiğinde dil ayarlarını başlat
i18nStore.initializeLanguage()
</script>