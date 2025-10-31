<template>
  <div class="layout-shell">
    <aside class="sidebar-panel">
      <div class="brand">
        <div class="brand-symbol">MK</div>
        <div>
          <h1>Merkez Kutuphane</h1>
          <p>Yonetim Konsolu</p>
        </div>
      </div>

      <nav class="nav-list">
        <RouterLink
          v-for="item in navItems"
          :key="item.routeName"
          :to="item.to"
          class="nav-link"
          :class="{ active: normalizedRouteName === item.routeName }"
          :aria-current="normalizedRouteName === item.routeName ? 'page' : undefined"
        >
          <span
            class="nav-icon"
            :class="item.accent"
            :data-initial="item.initial"
          ></span>
          <div class="nav-text">
            <span>{{ item.label }}</span>
            <small>{{ item.navHint }}</small>
          </div>
        </RouterLink>
      </nav>

      <div class="sidebar-footer">
        <p>v2.0 - Onizleme</p>
        <small>Son senkronizasyon: 3 dk once</small>
      </div>
    </aside>

    <div class="content-region">
      <header class="layout-header">
        <div class="layout-header-content">
          <div>
            <h2>{{ pageTitle }}</h2>
            <p>{{ pageDescription }}</p>
          </div>
          <div class="layout-header-controls" ref="headerControls">
            <select :value="currentLanguage" @change="changeLanguage">
              <option value="tr">TR - Turkce</option>
              <option value="en">EN - English</option>
            </select>

            <button
              type="button"
              class="avatar-menu"
              aria-haspopup="menu"
              :aria-expanded="showUserMenu ? 'true' : 'false'"
              @click.stop="toggleUserMenu"
            >
              <div class="avatar">{{ userInitials }}</div>
              <div class="avatar-info">
                <span>{{ user?.firstName }} {{ user?.lastName }}</span>
                <small>{{ userRoleLabel }}</small>
              </div>
              <span class="chevron" aria-hidden="true"></span>
            </button>

            <transition name="fade">
              <div v-if="showUserMenu" class="user-dropdown" role="menu" @click.stop>
                <RouterLink to="/profile">Profil</RouterLink>
                <button type="button" @click="logout">Oturumu Kapat</button>
              </div>
            </transition>
          </div>
        </div>
      </header>

      <main class="layout-main">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onBeforeUnmount } from 'vue'
import { storeToRefs } from 'pinia'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { useI18nStore } from '@/stores/i18n'

interface NavItem {
  routeName: string
  to: string
  label: string
  navHint: string
  pageIntro: string
  initial: string
  accent: string
}

const navItems: NavItem[] = [
  {
    routeName: 'merkez-dashboard',
    to: '/merkez/dashboard',
    label: 'Ana Sayfa',
    navHint: 'Ozet ve izleme',
    pageIntro: 'Genel durum, ozet metrikler ve son aktiviteler',
    initial: 'A',
    accent: 'nav-icon--indigo'
  },
  {
    routeName: 'merkez-cataloging',
    to: '/merkez/kataloglama',
    label: 'Kataloglama',
    navHint: 'Kayit olusturma',
    pageIntro: 'MARC ve RDA standartlarinda kataloglama islemleri',
    initial: 'K',
    accent: 'nav-icon--sky'
  },
  {
    routeName: 'merkez-catalog-search',
    to: '/merkez/katalog-arama',
    label: 'Katalog Arama',
    navHint: 'Arama ve filtreleme',
    pageIntro: 'Kayitlari filtreleyin ve disari aktarim yapin',
    initial: 'S',
    accent: 'nav-icon--emerald'
  },
  {
    routeName: 'merkez-circulation',
    to: '/merkez/dolasim',
    label: 'Dolasim',
    navHint: 'Odunc akisi',
    pageIntro: 'Odunc verme, iade ve rezervasyon surecleri',
    initial: 'D',
    accent: 'nav-icon--amber'
  },
  {
    routeName: 'merkez-libraries',
    to: '/merkez/kutuphaneler',
    label: 'Kutuphaneler',
    navHint: 'Birim ve raflar',
    pageIntro: 'Kurum kutuphanelerini, bolumleri ve raf yapilarini yonetin',
    initial: 'K',
    accent: 'nav-icon--teal'
  },
  {
    routeName: 'merkez-users',
    to: '/merkez/kullanicilar',
    label: 'Kullanicilar',
    navHint: 'Ekip ve roller',
    pageIntro: 'Kullanici hesaplarini ve yetkileri yonetin',
    initial: 'U',
    accent: 'nav-icon--purple'
  },
  {
    routeName: 'merkez-authorities',
    to: '/merkez/otoriteler',
    label: 'Otoriteler',
    navHint: 'Otorite kayitlari',
    pageIntro: 'Yazar ve konu otorite kayitlarini guncel tutun',
    initial: 'O',
    accent: 'nav-icon--rose'
  },
  {
    routeName: 'merkez-reports',
    to: '/merkez/raporlar',
    label: 'Raporlar',
    navHint: 'Analiz ve raporlar',
    pageIntro: 'Detayli raporlar ve performans gostergeleri',
    initial: 'R',
    accent: 'nav-icon--orange'
  },
  {
    routeName: 'merkez-management',
    to: '/merkez/yonetim',
    label: 'Yonetim',
    navHint: 'Ayarlar ve entegrasyon',
    pageIntro: 'Sistem ayarlari, entegrasyonlar ve bakim araclari',
    initial: 'Y',
    accent: 'nav-icon--slate'
  }
]

const navLookup = navItems.reduce<Record<string, NavItem>>((map, item) => {
  map[item.routeName] = item
  return map
}, {})

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()
const i18nStore = useI18nStore()

const { user } = storeToRefs(authStore)

const showUserMenu = ref(false)
const headerControls = ref<HTMLElement | null>(null)

const normalizedRouteName = computed(() => (typeof route.name === 'string' ? route.name : undefined))
const pageTitle = computed(() => navLookup[normalizedRouteName.value || '']?.label ?? 'Kutuphane Yonetimi')
const pageDescription = computed(
  () => navLookup[normalizedRouteName.value || '']?.pageIntro ?? 'Merkez kutuphane yonetimi paneli'
)
const currentLanguage = computed(() => i18nStore.currentLanguage)

const userInitials = computed(() => {
  const first = user.value?.firstName?.charAt(0) ?? 'K'
  const last = user.value?.lastName?.charAt(0) ?? 'Y'
  return `${first}${last}`.toUpperCase()
})

const userRoleLabel = computed(() =>
  user.value?.libraryType === 'Merkez' ? 'Merkez kutuphanesi' : 'Okul kutuphanesi'
)

watch(
  () => route.fullPath,
  () => {
    showUserMenu.value = false
  }
)

function changeLanguage(event: Event) {
  const target = event.target as HTMLSelectElement
  i18nStore.setLanguage(target.value as 'tr' | 'en')
}

function toggleUserMenu() {
  showUserMenu.value = !showUserMenu.value
}

function logout() {
  showUserMenu.value = false
  authStore.logout()
  router.push('/login')
}

function handleGlobalClick(event: MouseEvent) {
  if (!showUserMenu.value) {
    return
  }

  const target = event.target as HTMLElement | null
  if (headerControls.value && target && !headerControls.value.contains(target)) {
    showUserMenu.value = false
  }
}

function handleEscapeKey(event: KeyboardEvent) {
  if (event.key === 'Escape') {
    showUserMenu.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', handleGlobalClick)
  document.addEventListener('keydown', handleEscapeKey)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleGlobalClick)
  document.removeEventListener('keydown', handleEscapeKey)
})
</script>

<style scoped>
.layout-shell {
  min-height: 100vh;
  display: flex;
  background: linear-gradient(180deg, #f6f7fb 0%, #eef2ff 100%);
}

.sidebar-panel {
  width: 264px;
  background: #ffffff;
  border-right: 1px solid rgba(226, 232, 240, 0.85);
  box-shadow: 8px 0 32px rgba(15, 23, 42, 0.08);
  padding: 2.1rem 1.75rem;
  display: flex;
  flex-direction: column;
  gap: 2.2rem;
}

.brand {
  display: flex;
  align-items: center;
  gap: 0.95rem;
}

.brand-symbol {
  width: 48px;
  height: 48px;
  border-radius: 16px;
  display: grid;
  place-items: center;
  background: linear-gradient(135deg, #2563eb, #7c3aed);
  color: #ffffff;
  font-size: 1.2rem;
  font-weight: 700;
  letter-spacing: 1px;
}

.brand h1 {
  margin: 0;
  font-size: 1.16rem;
  font-weight: 700;
  color: #0f172a;
}

.brand p {
  margin: 0.15rem 0 0;
  font-size: 0.82rem;
  color: rgba(71, 85, 105, 0.72);
  letter-spacing: 0.01em;
}

.nav-list {
  display: flex;
  flex-direction: column;
  gap: 0.6rem;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 0.85rem;
  padding: 0.65rem 0.75rem;
  border-radius: 14px;
  text-decoration: none;
  color: #475569;
  font-weight: 600;
  transition: background 0.2s ease, color 0.2s ease, transform 0.2s ease, box-shadow 0.2s ease;
}

.nav-link:hover {
  transform: translateX(6px);
  background: rgba(37, 99, 235, 0.12);
  color: #1e293b;
}

.nav-link.active {
  color: #1f2937;
  background: rgba(37, 99, 235, 0.16);
  box-shadow: inset 4px 0 0 rgba(37, 99, 235, 0.85);
}

.nav-text {
  display: flex;
  flex-direction: column;
  gap: 0.15rem;
}

.nav-text span {
  font-size: 0.95rem;
}

.nav-text small {
  font-size: 0.78rem;
  font-weight: 500;
  color: rgba(71, 85, 105, 0.8);
}

.nav-link.active .nav-text small {
  color: rgba(37, 99, 235, 0.85);
}

.nav-icon {
  width: 36px;
  height: 36px;
  border-radius: 12px;
  display: grid;
  place-items: center;
  color: #ffffff;
  font-weight: 700;
  font-size: 0.85rem;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.nav-icon::before {
  content: attr(data-initial);
}

.nav-link.active .nav-icon {
  transform: scale(1.05);
  box-shadow: 0 10px 20px rgba(37, 99, 235, 0.35);
}

.nav-icon--indigo {
  background: linear-gradient(135deg, #6366f1, #4338ca);
}

.nav-icon--sky {
  background: linear-gradient(135deg, #0ea5e9, #2563eb);
}

.nav-icon--emerald {
  background: linear-gradient(135deg, #10b981, #047857);
}

.nav-icon--amber {
  background: linear-gradient(135deg, #f59e0b, #d97706);
}

.nav-icon--purple {
  background: linear-gradient(135deg, #a855f7, #7c3aed);
}

.nav-icon--rose {
  background: linear-gradient(135deg, #f43f5e, #e11d48);
}

.nav-icon--orange {
  background: linear-gradient(135deg, #fb923c, #ea580c);
}

.nav-icon--teal {
  background: linear-gradient(135deg, #14b8a6, #0f766e);
}

.nav-icon--slate {
  background: linear-gradient(135deg, #475569, #1f2937);
}

.sidebar-footer {
  margin-top: auto;
  border-top: 1px solid rgba(226, 232, 240, 0.7);
  padding-top: 1.4rem;
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
  color: rgba(71, 85, 105, 0.75);
  font-size: 0.82rem;
}

.content-region {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.layout-header {
  position: sticky;
  top: 0;
  z-index: 5;
  background: linear-gradient(115deg, rgba(255, 255, 255, 0.9), rgba(239, 246, 255, 0.95));
  backdrop-filter: blur(12px);
  border-bottom: 1px solid rgba(226, 232, 240, 0.8);
}

.layout-header::before {
  content: '';
  position: absolute;
  inset: 0;
  background: radial-gradient(circle at top right, rgba(59, 130, 246, 0.18), transparent 55%);
  pointer-events: none;
}

.layout-header-content {
  position: relative;
  padding: 1.6rem 2.2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1.5rem;
}

.layout-header-content h2 {
  margin: 0;
  font-size: 1.7rem;
  color: #111827;
}

.layout-header-content p {
  margin: 0.35rem 0 0;
  color: rgba(75, 85, 99, 0.7);
  font-size: 0.95rem;
}

.layout-header-controls {
  display: flex;
  align-items: center;
  gap: 1rem;
  position: relative;
}

.layout-header-controls select {
  border-radius: 10px;
  border: 1px solid rgba(203, 213, 225, 0.65);
  padding: 0.45rem 0.7rem;
  background: rgba(255, 255, 255, 0.88);
  font-weight: 500;
  color: #1f2937;
}

.avatar-menu {
  display: flex;
  align-items: center;
  gap: 0.7rem;
  padding: 0.45rem 0.65rem;
  background: rgba(255, 255, 255, 0.9);
  border-radius: 12px;
  border: 1px solid rgba(203, 213, 225, 0.6);
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.avatar-menu:hover {
  transform: translateY(-2px);
  box-shadow: 0 16px 30px rgba(148, 163, 184, 0.18);
}

.avatar-menu:focus-visible {
  outline: 2px solid rgba(37, 99, 235, 0.55);
  outline-offset: 2px;
}

.avatar-menu[aria-expanded='true'] .chevron {
  transform: rotate(180deg);
}

.avatar {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  display: grid;
  place-items: center;
  background: linear-gradient(135deg, #ef4444, #f97316);
  color: #ffffff;
  font-weight: 700;
}

.avatar-info span {
  display: block;
  font-weight: 600;
  color: #1f2937;
}

.avatar-info small {
  color: rgba(55, 65, 81, 0.6);
  font-size: 0.78rem;
}

.chevron {
  width: 0;
  height: 0;
  border-left: 5px solid transparent;
  border-right: 5px solid transparent;
  border-top: 6px solid rgba(55, 65, 81, 0.5);
  transition: transform 0.2s ease;
}

.user-dropdown {
  position: absolute;
  top: calc(100% + 0.6rem);
  right: 0;
  background: #ffffff;
  border-radius: 12px;
  border: 1px solid rgba(226, 232, 240, 0.85);
  box-shadow: 0 24px 46px rgba(15, 23, 42, 0.15);
  padding: 0.6rem;
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
  min-width: 190px;
  z-index: 10;
}

.user-dropdown a,
.user-dropdown button {
  border: none;
  background: none;
  text-align: left;
  padding: 0.55rem 0.75rem;
  border-radius: 10px;
  color: #1f2937;
  font-size: 0.9rem;
  cursor: pointer;
  transition: background 0.2s ease;
}

.user-dropdown a:hover,
.user-dropdown button:hover {
  background: rgba(248, 113, 113, 0.12);
}

.layout-main {
  flex: 1;
  padding: 2rem 2.4rem;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.18s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@media (max-width: 1024px) {
  .layout-shell {
    flex-direction: column;
  }

  .sidebar-panel {
    width: 100%;
    flex-direction: row;
    flex-wrap: wrap;
    align-items: center;
    padding: 1.5rem;
    gap: 1.2rem;
  }

  .nav-list {
    flex: 1;
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
    gap: 0.65rem;
  }

  .sidebar-footer {
    width: 100%;
    flex-direction: row;
    justify-content: space-between;
    border-top: none;
    padding-top: 0;
  }

  .content-region {
    flex: none;
  }
}

@media (max-width: 640px) {
  .layout-header-content {
    flex-direction: column;
    align-items: flex-start;
  }

  .layout-header-controls {
    width: 100%;
    justify-content: space-between;
  }

  .layout-main {
    padding: 1.5rem 1.6rem;
  }
}
</style>
