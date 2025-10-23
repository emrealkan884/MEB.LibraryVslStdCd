import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import piniaPluginPersistedState from 'pinia-plugin-persistedstate'

import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura'
import ToastService from 'primevue/toastservice'

import App from './App.vue'
import router from './router'
import { useI18nStore } from '@/stores/i18n'

const app = createApp(App)
const pinia = createPinia()

pinia.use(piniaPluginPersistedState)
app.use(pinia)
app.use(router)
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      prefix: 'p',
      darkModeSelector: '.dark',
      cssLayer: false
    }
  }
})
app.use(ToastService)

const i18nStore = useI18nStore(pinia)
i18nStore.initializeLanguage()

app.config.globalProperties.$t = (key: string) => i18nStore.t(key)

app.mount('#app')
