import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import apiClient from './api'

export interface User {
  id: string
  firstName: string
  lastName: string
  email?: string
  roles: string[]
  libraryType: 'Merkez' | 'Okul'
  schoolCode?: string
  isAuthenticated: boolean
}

export interface LoginCredentials {
  email: string
  password: string
  rememberMe?: boolean
  authenticatorCode?: string
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!user.value && !!token.value)

  const hasRole = (role: string) => {
    return user.value?.roles.includes(role) || false
  }

  const isMerkezKutuphane = computed(() => user.value?.libraryType === 'Merkez')
  const isOkulKutuphane = computed(() => user.value?.libraryType === 'Okul')

  const login = async (credentials: LoginCredentials) => {
    isLoading.value = true
    error.value = null

    try {
      if (import.meta.env.VITE_USE_MOCK_AUTH !== 'false') {
        token.value = 'mock-access-token'
        user.value = {
          id: crypto.randomUUID(),
          firstName: 'Kutuphane',
          lastName: 'Yöneticisi',
          email: credentials.email,
          roles: ['Role.Library.Manager'],
          libraryType: 'Merkez',
          schoolCode: undefined,
          isAuthenticated: true
        }

        if (credentials.rememberMe) {
          localStorage.setItem('auth_token', token.value)
        } else {
          localStorage.removeItem('auth_token')
        }

        return { success: true }
      }

      const response = await apiClient.post('/Auth/Login', {
        email: credentials.email,
        password: credentials.password,
        authenticatorCode: credentials.authenticatorCode
      })

      token.value = response.data.accessToken
      user.value = {
        id: response.data.user.id,
        firstName: response.data.user.firstName,
        lastName: response.data.user.lastName,
        email: response.data.user.email,
        roles: response.data.user.roles,
        libraryType: response.data.user.libraryType,
        schoolCode: response.data.user.schoolCode,
        isAuthenticated: true
      }

      if (token.value) {
        if (credentials.rememberMe) {
          localStorage.setItem('auth_token', token.value)
        } else {
          localStorage.removeItem('auth_token')
        }
      }

      return { success: true }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Giriş yapılırken bir hata oluştu'
      return { success: false, error: error.value }
    } finally {
      isLoading.value = false
    }
  }

  const logout = () => {
    user.value = null
    token.value = null
    localStorage.removeItem('auth_token')
  }

  const checkAuth = async () => {
    const savedToken = localStorage.getItem('auth_token')
    if (savedToken) {
      token.value = savedToken

      try {
        // Kullanıcı bilgilerini al
        const response = await apiClient.get('/Auth/Me')
        user.value = {
          ...response.data,
          isAuthenticated: true
        }
      } catch (err) {
        logout()
      }
    }
  }

  return {
    user,
    token,
    isLoading,
    error,
    isAuthenticated,
    hasRole,
    isMerkezKutuphane,
    isOkulKutuphane,
    login,
    logout,
    checkAuth
  }
})


