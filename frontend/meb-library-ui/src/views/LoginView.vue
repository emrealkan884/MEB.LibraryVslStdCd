<template>
  <div class="login-page">
    <div class="login-layout">
      <section class="login-branding">
        <div class="branding-badge">
          <span>ME</span>
          <span class="badge-shadow"></span>
        </div>
        <h1>{{ $t('login.title') }}</h1>
        <p>MEBBİS veya e-Okul kimliklerinizle tek noktadan erişim.</p>

        <ul class="feature-list">
          <li>
            <span class="feature-icon">📚</span>
            <div>
              <strong>Katalog Yönetimi</strong>
              <p>Merkez ve okul kütüphaneleri için birleşik kataloglama.</p>
            </div>
          </li>
          <li>
            <span class="feature-icon">🔐</span>
            <div>
              <strong>Güvenli Oturum</strong>
              <p>MFA, rol tabanlı yetkilendirme ve gelişmiş denetimler.</p>
            </div>
          </li>
          <li>
            <span class="feature-icon">📊</span>
            <div>
              <strong>Dinamik Raporlar</strong>
              <p>Ödünç işlemleri, kullanıcı özetleri ve rezervasyon analizi.</p>
            </div>
          </li>
        </ul>

        <div class="contact-card">
          <span class="contact-icon">🕘</span>
          <div>
            <strong>Canlı destek</strong>
            <p>Hafta içi 09:00 - 18:00 • 444 0 632</p>
          </div>
        </div>
      </section>

      <section class="login-form-area">
        <div class="login-card">
          <header class="card-header">
            <div class="badge">v2.0 Preview</div>
            <h2>Hoş geldiniz</h2>
            <p>MEB dijital kütüphane ekosistemine bağlanmak için bilgilerinizi girin.</p>
          </header>

          <form class="card-body" @submit.prevent="handleLogin">
            <div class="input-group">
              <span class="input-label">Kütüphane türü</span>
              <div class="segment">
                <button
                  v-for="option in libraryOptions"
                  :key="option.value"
                  type="button"
                  :class="['segment__button', { 'is-active': selectedLibraryType === option.value }]"
                  @click="selectedLibraryType = option.value"
                >
                  <span class="segment__icon">{{ option.icon }}</span>
                  <span>{{ option.label }}</span>
                </button>
              </div>
            </div>

            <div class="input-group">
              <span class="input-label">Giriş yöntemi</span>
              <div class="segment segment--alt">
                <button
                  v-for="option in loginOptions"
                  :key="option.value"
                  type="button"
                  :class="['segment__button', { 'is-active': loginMethod === option.value }]"
                  @click="loginMethod = option.value"
                >
                  <span class="segment__icon">{{ option.icon }}</span>
                  <span>{{ option.label }}</span>
                </button>
              </div>
            </div>

            <div class="input-field">
              <label for="email">E-posta Adresi</label>
              <div class="input-wrapper">
                <span class="input-wrapper__icon">📧</span>
                <input
                  id="email"
                  v-model="credentials.email"
                  type="email"
                  autocomplete="username"
                  placeholder="kutuphane.yonetici@example.com"
                  required
                />
              </div>
            </div>

            <div class="input-field">
              <label for="password">{{ $t('login.password') }}</label>
              <div class="input-wrapper">
                <span class="input-wrapper__icon">🔑</span>
                <input
                  id="password"
                  v-model="credentials.password"
                  type="password"
                  autocomplete="current-password"
                  placeholder="••••••••"
                  required
                />
              </div>
            </div>

            <div class="form-meta">
              <label class="checkbox">
                <input v-model="credentials.rememberMe" type="checkbox" />
                <span>{{ $t('login.rememberMe') }}</span>
              </label>

              <button class="link-button" type="button">
                Şifremi unuttum
              </button>
            </div>

            <transition name="fade">
              <div v-if="error" class="alert alert--error">
                <span>⚠️</span>
                <p>{{ error }}</p>
              </div>
            </transition>

            <button class="primary-btn" type="submit" :disabled="isLoading">
              <span v-if="isLoading" class="loader"></span>
              <span>{{ isLoading ? 'Giriş yapılıyor...' : $t('login.submit') }}</span>
            </button>

            <div class="card-footer">
              <p>
                Bu sisteme giriş yaparak <a href="#">KVKK ve güvenlik politikasını</a> kabul etmiş olursunuz.
              </p>
            </div>
          </form>
        </div>

        <footer class="login-footer">
          <p>© {{ new Date().getFullYear() }} Milli Eğitim Bakanlığı • Bilgi İşlem Genel Müdürlüğü</p>
          <div class="pill-group">
            <span class="pill">Günlük bakım 02:00 - 02:30</span>
            <span class="pill">Sürüm 2025.02</span>
          </div>
        </footer>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18nStore } from '@/stores/i18n'

type LibraryType = 'Merkez' | 'Okul'
type LoginMethod = 'meb' | 'eokul'

const router = useRouter()
const authStore = useAuthStore()
const i18nStore = useI18nStore()

const selectedLibraryType = ref<LibraryType>('Merkez')
const loginMethod = ref<LoginMethod>('meb')

const credentials = reactive({
  email: '',
  password: '',
  rememberMe: false,
})

const isLoading = ref(false)
const error = ref<string | null>(null)

const libraryOptions: Array<{ value: LibraryType; label: string; icon: string }> = [
  { value: 'Merkez', label: 'Merkez Kütüphane', icon: '🏛️' },
  { value: 'Okul', label: 'Okul Kütüphanesi', icon: '🎓' },
]

const loginOptions: Array<{ value: LoginMethod; label: string; icon: string }> = [
  { value: 'meb', label: 'MEBBİS', icon: '🛡️' },
  { value: 'eokul', label: 'e-Okul', icon: '✨' },
]

const handleLogin = async () => {
  isLoading.value = true
  error.value = null

  try {
    console.log('Login attempt:', {
      email: credentials.email.trim(),
      selectedLibraryType,
      mockEnabled: import.meta.env.VITE_USE_MOCK_AUTH
    })

    const result = await authStore.login({
      email: credentials.email.trim(),
      password: credentials.password,
      rememberMe: credentials.rememberMe,
    })

    console.log('Login result:', result)
    console.log('Auth store state:', {
      user: authStore.user,
      token: authStore.token,
      isAuthenticated: authStore.isAuthenticated,
      isMerkezKutuphane: authStore.isMerkezKutuphane,
      isOkulKutuphane: authStore.isOkulKutuphane
    })

    if (result.success) {
      console.log('Login successful, redirecting...')

      // Force a delay to ensure auth store is updated
      await new Promise(resolve => setTimeout(resolve, 100))

      if (authStore.isMerkezKutuphane) {
        console.log('Redirecting to merkez dashboard')
        await router.push('/merkez/dashboard')
        console.log('Router push completed')
      } else if (authStore.isOkulKutuphane) {
        console.log('Redirecting to okul dashboard')
        await router.push('/okul/dashboard')
        console.log('Router push completed')
      } else {
        console.log('Redirecting to home')
        await router.push('/')
        console.log('Router push completed')
      }
    } else {
      error.value = result.error || 'Giriş başarısız. Bilgilerinizi kontrol edin.'
    }
  } catch {
    error.value = 'Bağlantı sırasında bir sorun oluştu, lütfen tekrar deneyin.'
  } finally {
    isLoading.value = false
  }
}

i18nStore.initializeLanguage()
</script>

<style scoped>
.login-page {
  min-height: 100vh;
  background: radial-gradient(circle at top left, #fef2f2 0%, #fee2e2 25%, #fefce8 65%, #fef2f2 100%);
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 3rem 1.5rem;
}

.login-layout {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 3rem;
  max-width: 1150px;
  width: 100%;
}

.login-branding {
  background: linear-gradient(180deg, rgba(248, 113, 113, 0.18), rgba(220, 38, 38, 0.22));
  backdrop-filter: blur(12px);
  border-radius: 32px;
  padding: 3rem;
  color: #0f172a;
  display: flex;
  flex-direction: column;
  gap: 2.25rem;
  position: relative;
  border: 1px solid rgba(248, 113, 113, 0.35);
  box-shadow: 0 25px 55px rgba(220, 38, 38, 0.15);
}

.branding-badge {
  width: 64px;
  height: 64px;
  border-radius: 18px;
  background: linear-gradient(135deg, #dc2626, #fb7185);
  color: white;
  font-weight: 700;
  font-size: 1.75rem;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  box-shadow: 0 18px 35px rgba(248, 113, 113, 0.4);
}

.badge-shadow {
  position: absolute;
  bottom: -12px;
  height: 8px;
  width: 36px;
  border-radius: 50%;
  background: rgba(248, 113, 113, 0.28);
  filter: blur(4px);
}

.login-branding h1 {
  font-size: 2.4rem;
  line-height: 1.2;
  margin: 0;
  color: #7f1d1d;
  letter-spacing: -0.02em;
}

.login-branding > p {
  font-size: 1.05rem;
  color: rgba(127, 29, 29, 0.72);
  margin: 0;
}

.feature-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.feature-list li {
  display: grid;
  grid-template-columns: 52px 1fr;
  gap: 1.25rem;
  align-items: start;
  background: rgba(255, 255, 255, 0.45);
  border-radius: 18px;
  padding: 1.15rem 1.25rem;
  border: 1px solid rgba(248, 113, 113, 0.22);
}

.feature-icon {
  font-size: 1.75rem;
  display: flex;
  align-items: center;
  justify-content: center;
}

.feature-list strong {
  display: block;
  font-size: 1.05rem;
  margin-bottom: 0.25rem;
  color: #7f1d1d;
}

.feature-list p {
  margin: 0;
  color: rgba(127, 29, 29, 0.68);
  font-size: 0.95rem;
}

.contact-card {
  display: flex;
  align-items: center;
  gap: 1rem;
  background: rgba(255, 228, 230, 0.85);
  padding: 1.1rem 1.25rem;
  border-radius: 16px;
  border: 1px solid rgba(248, 113, 113, 0.28);
}

.contact-icon {
  font-size: 1.65rem;
}

.contact-card strong {
  color: #7f1d1d;
  font-size: 1.05rem;
}

.contact-card p {
  margin: 0;
  color: rgba(127, 29, 29, 0.68);
  font-size: 0.95rem;
}

.login-form-area {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.login-card {
  background: #ffffff;
  border-radius: 28px;
  padding: 2.75rem;
  border: 1px solid rgba(252, 165, 165, 0.6);
  box-shadow:
    0 25px 85px rgba(127, 29, 29, 0.08),
    0 12px 35px rgba(248, 113, 113, 0.16);
}

.card-header {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
  margin-bottom: 2.25rem;
}

.badge {
  align-self: flex-start;
  background: linear-gradient(135deg, rgba(248, 113, 113, 0.2), rgba(244, 63, 94, 0.18));
  color: #b91c1c;
  font-weight: 600;
  padding: 0.4rem 0.75rem;
  border-radius: 999px;
  font-size: 0.82rem;
  border: 1px solid rgba(248, 113, 113, 0.22);
}

.card-header h2 {
  margin: 0;
  font-size: 2.05rem;
  color: #7f1d1d;
  letter-spacing: -0.015em;
}

.card-header p {
  margin: 0;
  color: rgba(127, 29, 29, 0.68);
  font-size: 1.02rem;
  line-height: 1.5;
}

.card-body {
  display: flex;
  flex-direction: column;
  gap: 1.6rem;
}

.input-group {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.input-label {
  font-weight: 600;
  color: rgba(127, 29, 29, 0.85);
}

.segment {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  background: rgba(252, 165, 165, 0.25);
  border-radius: 16px;
  padding: 0.4rem;
  gap: 0.4rem;
}

.segment--alt {
  background: rgba(254, 215, 170, 0.35);
}

.segment__button {
  display: flex;
  align-items: center;
  gap: 0.55rem;
  justify-content: center;
  padding: 0.9rem 1rem;
  border-radius: 12px;
  border: none;
  background: rgba(255, 255, 255, 0.45);
  color: rgba(127, 29, 29, 0.65);
  font-weight: 600;
  cursor: pointer;
  transition: all 0.25s ease;
}

.segment__button:hover {
  background: rgba(255, 255, 255, 0.72);
}

.segment__button.is-active {
  background: #ffffff;
  color: #b91c1c;
  box-shadow: 0 12px 24px rgba(248, 113, 113, 0.22);
}

.segment__icon {
  font-size: 1.25rem;
}

.input-field {
  display: flex;
  flex-direction: column;
  gap: 0.65rem;
}

.input-field label {
  font-weight: 600;
  color: rgba(127, 29, 29, 0.85);
}

.input-wrapper {
  position: relative;
  display: flex;
  align-items: center;
  background: #fff1f2;
  border-radius: 14px;
  border: 1px solid rgba(252, 165, 165, 0.45);
  padding: 0.85rem 1.05rem 0.85rem 3.1rem;
  transition: border 0.2s ease, box-shadow 0.2s ease;
}

.input-wrapper:focus-within {
  border-color: rgba(220, 38, 38, 0.6);
  box-shadow: 0 0 0 4px rgba(248, 113, 113, 0.18);
}

.input-wrapper__icon {
  position: absolute;
  left: 1.1rem;
  font-size: 1.2rem;
  opacity: 0.65;
}

.input-wrapper input {
  border: none;
  background: transparent;
  width: 100%;
  font-size: 1rem;
  color: #7f1d1d;
  outline: none;
}

.form-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.95rem;
}

.checkbox {
  display: inline-flex;
  align-items: center;
  gap: 0.55rem;
  color: rgba(127, 29, 29, 0.72);
  font-weight: 500;
}

.checkbox input {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(252, 165, 165, 0.6);
  border-radius: 6px;
  accent-color: #dc2626;
}

.link-button {
  background: none;
  border: none;
  color: #dc2626;
  font-weight: 600;
  cursor: pointer;
  padding: 0;
}

.link-button:hover {
  text-decoration: underline;
}

.alert {
  display: flex;
  align-items: center;
  gap: 0.85rem;
  padding: 0.95rem 1.1rem;
  border-radius: 14px;
  font-size: 0.95rem;
}

.alert--error {
  background: rgba(254, 202, 202, 0.3);
  border: 1px solid rgba(239, 68, 68, 0.35);
  color: #b91c1c;
}

.primary-btn {
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: white;
  border: none;
  border-radius: 14px;
  padding: 0.95rem 1.05rem;
  font-size: 1.05rem;
  font-weight: 600;
  cursor: pointer;
  box-shadow: 0 20px 40px rgba(248, 113, 113, 0.35);
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.primary-btn:hover {
  transform: translateY(-1px);
  box-shadow: 0 25px 45px rgba(248, 113, 113, 0.4);
}

.primary-btn:disabled {
  cursor: progress;
  opacity: 0.8;
  box-shadow: none;
}

.loader {
  width: 1.1rem;
  height: 1.1rem;
  border-radius: 50%;
  border: 2px solid rgba(255, 255, 255, 0.55);
  border-top-color: #fff;
  animation: spin 0.65s linear infinite;
}

.card-footer {
  font-size: 0.9rem;
  color: rgba(127, 29, 29, 0.62);
  text-align: center;
}

.card-footer a {
  color: #dc2626;
  font-weight: 600;
  text-decoration: none;
}

.card-footer a:hover {
  text-decoration: underline;
}

.login-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: rgba(127, 29, 29, 0.6);
  font-size: 0.9rem;
  padding: 0 0.5rem;
}

.pill-group {
  display: flex;
  gap: 0.5rem;
}

.pill {
  background: rgba(252, 165, 165, 0.28);
  color: #b91c1c;
  border-radius: 999px;
  padding: 0.3rem 0.9rem;
  font-weight: 600;
  font-size: 0.82rem;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.25s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

@media (max-width: 1050px) {
  .login-layout {
    grid-template-columns: 1fr;
  }

  .login-branding {
    display: none;
  }

  .login-page {
    padding: 2.5rem 1.25rem;
  }

  .login-card {
    padding: 2.25rem;
  }
}

@media (max-width: 640px) {
  .segment {
    grid-template-columns: 1fr;
  }

  .form-meta {
    flex-direction: column;
    gap: 0.85rem;
    align-items: flex-start;
  }

  .login-footer {
    flex-direction: column;
    gap: 0.9rem;
    align-items: flex-start;
  }

  .pill-group {
    flex-wrap: wrap;
  }
}
</style>
