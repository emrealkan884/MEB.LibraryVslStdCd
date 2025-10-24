import { ref, computed } from 'vue'
import { defineStore } from 'pinia'
import apiClient from './api'

const MOCK_AUTH_ENABLED = import.meta.env.VITE_USE_MOCK_AUTH === 'true'
const TOKEN_STORAGE_KEY = 'auth_token'
const MOCK_USER_KEY = 'mock_user_profile'

export interface User {
  id: string
  firstName: string
  lastName: string
  email?: string
  roles: string[]
  libraryType: 'Merkez' | 'Okul' | 'Unknown'
  schoolCode?: string
  isAuthenticated: boolean
}

interface CurrentUserProfile {
  id: string
  firstName: string
  lastName: string
  email: string
  roles: string[]
  libraryType: 'Merkez' | 'Okul' | 'Unknown' | string
  schoolCode?: string | null
}

export interface LoginCredentials {
  email: string
  password: string
  rememberMe?: boolean
  authenticatorCode?: string
}

type StorageKind = 'local' | 'session'

function getStorage(kind: StorageKind): Storage | null {
  if (typeof window === 'undefined') {
    return null
  }

  try {
    return kind === 'local' ? window.localStorage : window.sessionStorage
  } catch {
    return null
  }
}

function tryGetItem(kind: StorageKind, key: string): string | null {
  const storage = getStorage(kind)
  if (!storage) {
    return null
  }

  try {
    return storage.getItem(key)
  } catch {
    return null
  }
}

function getStoredValue(key: string): string | null {
  const localValue = tryGetItem('local', key)
  if (localValue !== null) {
    return localValue
  }

  return tryGetItem('session', key)
}

function persistAuthState(options: { token: string; rememberMe?: boolean; mockUser?: User }) {
  const primaryKind: StorageKind = options.rememberMe ? 'local' : 'session'
  const secondaryKind: StorageKind = options.rememberMe ? 'session' : 'local'

  const primary = getStorage(primaryKind)
  if (primary) {
    try {
      primary.setItem(TOKEN_STORAGE_KEY, options.token)
    } catch {
      // ignore storage write errors
    }

    if (options.mockUser) {
      try {
        primary.setItem(MOCK_USER_KEY, JSON.stringify(options.mockUser))
      } catch {
        // ignore storage write errors
      }
    } else {
      try {
        primary.removeItem(MOCK_USER_KEY)
      } catch {
        // ignore storage write errors
      }
    }
  }

  const secondary = getStorage(secondaryKind)
  if (secondary) {
    try {
      secondary.removeItem(TOKEN_STORAGE_KEY)
      secondary.removeItem(MOCK_USER_KEY)
    } catch {
      // ignore storage write errors
    }
  }
}

function clearAuthStorage() {
  ;(['local', 'session'] as const).forEach(kind => {
    const storage = getStorage(kind)
    if (!storage) {
      return
    }

    try {
      storage.removeItem(TOKEN_STORAGE_KEY)
      storage.removeItem(MOCK_USER_KEY)
    } catch {
      // ignore storage write errors
    }
  })
}

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const token = ref<string | null>(null)
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => !!user.value && !!token.value)

  const hasRole = (role: string) => user.value?.roles.includes(role) ?? false
  const isMerkezKutuphane = computed(() => user.value?.libraryType === 'Merkez')
  const isOkulKutuphane = computed(() => user.value?.libraryType === 'Okul')

  const mapProfileToUser = (profile: CurrentUserProfile): User => ({
    id: profile.id,
    firstName: profile.firstName,
    lastName: profile.lastName,
    email: profile.email,
    roles: Array.isArray(profile.roles) ? profile.roles : [],
    libraryType: profile.libraryType === 'Okul' ? 'Okul' : profile.libraryType === 'Merkez' ? 'Merkez' : 'Unknown',
    schoolCode: profile.schoolCode ?? undefined,
    isAuthenticated: true
  })

  const fetchCurrentUser = async () => {
    try {
      const response = await apiClient.get('/Auth/Me')
      const profile = response.data as CurrentUserProfile
      user.value = mapProfileToUser(profile)
    } catch (err) {
      user.value = null
      token.value = null
      clearAuthStorage()
      throw err
    }
  }

  const login = async (credentials: LoginCredentials) => {
    isLoading.value = true
    error.value = null

    try {
      if (MOCK_AUTH_ENABLED) {
        const mockUser: User = {
          id: crypto.randomUUID(),
          firstName: 'Kutuphane',
          lastName: 'Yoneticisi',
          email: credentials.email,
          roles: ['Role.Library.Manager'],
          libraryType: 'Merkez',
          schoolCode: undefined,
          isAuthenticated: true
        }

        token.value = 'mock-access-token'
        user.value = mockUser
        persistAuthState({
          token: token.value,
          rememberMe: credentials.rememberMe,
          mockUser
        })

        return { success: true }
      }

      const response = await apiClient.post('/Auth/Login', {
        email: credentials.email,
        password: credentials.password,
        authenticatorCode: credentials.authenticatorCode
      })

      const accessToken: string | undefined =
        response.data?.accessToken?.token ?? response.data?.accessToken?.Token ?? response.data?.accessToken

      if (!accessToken) {
        throw new Error('Access token alınamadı.')
      }

      token.value = accessToken

      persistAuthState({
        token: accessToken,
        rememberMe: credentials.rememberMe
      })

      await fetchCurrentUser()

      return { success: true }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'Giris yapilirken bir hata olustu'
      return { success: false, error: error.value }
    } finally {
      isLoading.value = false
    }
  }

  const logout = () => {
    user.value = null
    token.value = null
    clearAuthStorage()
  }

  const checkAuth = async () => {
    const savedToken = getStoredValue(TOKEN_STORAGE_KEY)

    if (MOCK_AUTH_ENABLED) {
      const savedUser = getStoredValue(MOCK_USER_KEY)

      if (savedToken && savedUser) {
        try {
          const parsedUser: User = JSON.parse(savedUser)
          token.value = savedToken
          user.value = { ...parsedUser, isAuthenticated: true }
        } catch {
          logout()
        }
      } else {
        logout()
      }

      return
    }

    if (savedToken) {
      token.value = savedToken

      try {
        await fetchCurrentUser()
      } catch {
        // profile fetch already clears auth state
      }
    } else {
      clearAuthStorage()
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
