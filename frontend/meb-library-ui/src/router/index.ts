import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/',
      name: 'public-catalog',
      component: () => import('../views/PublicCatalogSearchView.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/kayit/:id',
      name: 'public-catalog-detail',
      component: () => import('../views/PublicCatalogDetailView.vue'),
      meta: { requiresAuth: false }
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: () => import('../views/HomeView.vue'),
      meta: { requiresAuth: true }
    },
    // Merkez Kütüphane Routes
    {
      path: '/merkez',
      name: 'merkez-layout',
      component: () => import('../layouts/MerkezLayout.vue'),
      meta: {
        requiresAuth: true,
        libraryType: 'Merkez',
        requiredRoles: ['Role.BakanlikYetkilisi', 'Role.SistemYoneticisi']
      },
      children: [
        {
          path: 'dashboard',
          name: 'merkez-dashboard',
          component: () => import('../views/merkez/DashboardView.vue')
        },
        {
          path: 'book-search',
          name: 'book-search',
          component: () => import('../views/BookSearchView.vue')
        },
        {
          path: 'loan-management',
          name: 'loan-management',
          component: () => import('../views/LoanManagementView.vue')
        },
        {
          path: 'profile',
          name: 'user-profile',
          component: () => import('../views/UserProfileView.vue')
        },
        {
          path: 'kataloglama',
          name: 'merkez-cataloging',
          component: () => import('../views/merkez/CatalogingView.vue')
        },
        {
          path: 'katalog-arama',
          name: 'merkez-catalog-search',
          component: () => import('../views/merkez/CatalogSearchView.vue')
        },
        {
          path: 'dolasim',
          name: 'merkez-circulation',
          component: () => import('../views/merkez/CirculationView.vue')
        },
        {
          path: 'kutuphaneler',
          name: 'merkez-libraries',
          component: () => import('../views/merkez/LibrariesView.vue')
        },
        {
          path: 'kullanicilar',
          name: 'merkez-users',
          component: () => import('../views/merkez/UsersView.vue')
        },
        {
          path: 'listeler',
          name: 'merkez-lists',
          component: () => import('../views/merkez/ListsView.vue')
        },
        {
          path: 'otoriteler',
          name: 'merkez-authorities',
          component: () => import('../views/merkez/AuthoritiesView.vue')
        },
        {
          path: 'sureli-yayinlar',
          name: 'merkez-serials',
          component: () => import('../views/merkez/SerialsView.vue')
        },
        {
          path: 'saglama',
          name: 'merkez-acquisition',
          component: () => import('../views/merkez/AcquisitionView.vue')
        },
        {
          path: 'raporlar',
          name: 'merkez-reports',
          component: () => import('../views/merkez/ReportsView.vue')
        },
        {
          path: 'yonetim',
          name: 'merkez-management',
          component: () => import('../views/merkez/ManagementView.vue')
        }
      ]
    },
    // Okul Kütüphanesi Routes
    {
      path: '/okul',
      name: 'okul-layout',
      component: () => import('../layouts/OkulLayout.vue'),
      meta: { requiresAuth: true, libraryType: 'Okul' },
      children: [
        {
          path: 'dashboard',
          name: 'okul-dashboard',
          component: () => import('../views/okul/DashboardView.vue')
        },
        {
          path: 'opac',
          name: 'okul-opac',
          component: () => import('../views/okul/OpacView.vue')
        },
        {
          path: 'odunc-islemleri',
          name: 'okul-circulation',
          component: () => import('../views/okul/CirculationView.vue')
        },
        {
          path: 'yeni-katalog-talebi',
          name: 'okul-catalog-request',
          component: () => import('../views/okul/CatalogRequestView.vue')
        },
        {
          path: 'etkinlikler',
          name: 'okul-events',
          component: () => import('../views/okul/EventsView.vue')
        },
        {
          path: 'raporlar',
          name: 'okul-reports',
          component: () => import('../views/okul/ReportsView.vue')
        }
      ]
    },
    // 404 Page
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('../views/NotFoundView.vue')
    }
  ],
})

// Navigation Guard
router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore()

  // Kullanıcı henüz yüklenmediyse auth bilgisini yenile
  if (!authStore.user) {
    await authStore.checkAuth()
  }

  // Login sayfası için auth kontrolü yapma
  if (to.meta.requiresAuth === false) {
    next()
    return
  }

  // Auth gerektiren sayfa için token kontrolü
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
    return
  }

  // Kullanıcının türüne göre uygun dashboard'a yönlendir
  if (to.path === '/dashboard' || to.name === 'dashboard') {
    if (authStore.isMerkezKutuphane) {
      next('/merkez/dashboard')
      return
    }
    if (authStore.isOkulKutuphane) {
      next('/okul/dashboard')
      return
    }
  }

  // Rol kontrolü
  const requiredRoles = Array.isArray(to.meta.requiredRoles) ? (to.meta.requiredRoles as string[]) : []
  if (requiredRoles.length > 0) {
    const hasAllowedRole = requiredRoles.some(role => authStore.hasRole(role))
    if (!hasAllowedRole) {
      // Yetkisi yoksa uygun dashboard'a ya da giriş sayfasına yönlendir
      if (authStore.isMerkezKutuphane) {
        next('/merkez/dashboard')
      } else if (authStore.isOkulKutuphane) {
        next('/okul/dashboard')
      } else {
        next('/login')
      }
      return
    }
  }

  // Kütüphane türü kontrolü
  if (to.meta.libraryType) {
    if (to.meta.libraryType === 'Merkez' && authStore.isMerkezKutuphane) {
      next()
      return
    }
    if (to.meta.libraryType === 'Okul' && authStore.isOkulKutuphane) {
      next()
      return
    }

    if (authStore.user?.libraryType !== to.meta.libraryType) {
      if (authStore.isMerkezKutuphane) {
        next('/merkez/dashboard')
      } else if (authStore.isOkulKutuphane) {
        next('/okul/dashboard')
      } else {
        next('/login')
      }
      return
    }
  }

  next()
})
export default router
