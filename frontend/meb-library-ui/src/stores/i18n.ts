import { ref, computed } from 'vue'
import { defineStore } from 'pinia'

type Language = 'tr' | 'en'

export const useI18nStore = defineStore('i18n', () => {
  const currentLanguage = ref<Language>('tr')

  const messages = {
    tr: {
      // Genel
      'common.save': 'Kaydet',
      'common.cancel': 'İptal',
      'common.delete': 'Sil',
      'common.edit': 'Düzenle',
      'common.add': 'Ekle',
      'common.search': 'Ara',
      'common.filter': 'Filtrele',
      'common.export': 'Dışa Aktar',
      'common.import': 'İçe Aktar',
      'common.loading': 'Yükleniyor...',
      'common.error': 'Hata',
      'common.success': 'Başarılı',
      'common.warning': 'Uyarı',
      'common.info': 'Bilgi',

      // Navigation
      'nav.dashboard': 'Ana Sayfa',
      'nav.cataloging': 'Kataloglama',
      'nav.circulation': 'Dolaşım',
      'nav.users': 'Kullanıcılar',
      'nav.lists': 'Listeler',
      'nav.authorities': 'Otoriteler',
      'nav.serials': 'Süreli Yayınlar',
      'nav.acquisition': 'Sağlama',
      'nav.reports': 'Raporlar',
      'nav.management': 'Yönetim',
      'nav.events': 'Etkinlikler',
      'nav.opac': 'OPAC',
      'nav.profile': 'Profil',
      'nav.logout': 'Çıkış',

      // Login
      'login.title': 'MEB Kütüphane Otomasyon Sistemi',
      'login.tcNo': 'T.C. Kimlik Numarası',
      'login.password': 'Şifre',
      'login.rememberMe': 'Beni Hatırla',
      'login.submit': 'Giriş Yap',
      'login.meb': 'MEB Girişi',
      'login.eokul': 'e-Okul Girişi',

      // Dashboard
      'dashboard.totalBooks': 'Toplam Kitap',
      'dashboard.activeLoans': 'Aktif Ödünç',
      'dashboard.overdueLoans': 'Geciken Ödünç',
      'dashboard.totalUsers': 'Toplam Kullanıcı',
      'dashboard.recentActivities': 'Son Aktiviteler',
      'dashboard.quickActions': 'Hızlı İşlemler',

      // Library Types
      'library.type.central': 'Merkez Kütüphane',
      'library.type.school': 'Okul Kütüphanesi',
      'library.central': 'Merkez',
      'library.school': 'Okul',

      // User Roles
      'role.ministry': 'Bakanlık Yetkilisi',
      'role.province': 'İl Yetkilisi',
      'role.district': 'İlçe Yetkilisi',
      'role.school': 'Okul Kütüphane Yöneticisi',
      'role.teacher': 'Öğretmen',
      'role.student': 'Öğrenci',

      // Book Status
      'book.status.available': 'Mevcut',
      'book.status.loaned': 'Ödünç Verildi',
      'book.status.overdue': 'Gecikmiş',
      'book.status.lost': 'Kayıp',
      'book.status.damaged': 'Hasarlı',

      // Loan Status
      'loan.status.active': 'Aktif',
      'loan.status.returned': 'İade Edildi',
      'loan.status.overdue': 'Gecikmiş',
      'loan.status.cancelled': 'İptal Edildi',

      // Messages
      'message.loginSuccess': 'Başarıyla giriş yapıldı',
      'message.loginError': 'Giriş yapılırken hata oluştu',
      'message.logoutSuccess': 'Başarıyla çıkış yapıldı',
      'message.saveSuccess': 'Başarıyla kaydedildi',
      'message.saveError': 'Kaydetme sırasında hata oluştu',
      'message.deleteConfirm': 'Bu öğeyi silmek istediğinizden emin misiniz?',
      'message.deleteSuccess': 'Başarıyla silindi',
      'message.deleteError': 'Silme sırasında hata oluştu',
    },
    en: {
      // General
      'common.save': 'Save',
      'common.cancel': 'Cancel',
      'common.delete': 'Delete',
      'common.edit': 'Edit',
      'common.add': 'Add',
      'common.search': 'Search',
      'common.filter': 'Filter',
      'common.export': 'Export',
      'common.import': 'Import',
      'common.loading': 'Loading...',
      'common.error': 'Error',
      'common.success': 'Success',
      'common.warning': 'Warning',
      'common.info': 'Info',

      // Navigation
      'nav.dashboard': 'Dashboard',
      'nav.cataloging': 'Cataloging',
      'nav.circulation': 'Circulation',
      'nav.users': 'Users',
      'nav.lists': 'Lists',
      'nav.authorities': 'Authorities',
      'nav.serials': 'Serials',
      'nav.acquisition': 'Acquisition',
      'nav.reports': 'Reports',
      'nav.management': 'Management',
      'nav.events': 'Events',
      'nav.opac': 'OPAC',
      'nav.profile': 'Profile',
      'nav.logout': 'Logout',

      // Login
      'login.title': 'MEB Library Automation System',
      'login.tcNo': 'T.C. ID Number',
      'login.password': 'Password',
      'login.rememberMe': 'Remember Me',
      'login.submit': 'Sign In',
      'login.meb': 'MEB Login',
      'login.eokul': 'e-Okul Login',

      // Dashboard
      'dashboard.totalBooks': 'Total Books',
      'dashboard.activeLoans': 'Active Loans',
      'dashboard.overdueLoans': 'Overdue Loans',
      'dashboard.totalUsers': 'Total Users',
      'dashboard.recentActivities': 'Recent Activities',
      'dashboard.quickActions': 'Quick Actions',

      // Library Types
      'library.type.central': 'Central Library',
      'library.type.school': 'School Library',
      'library.central': 'Central',
      'library.school': 'School',

      // User Roles
      'role.ministry': 'Ministry Officer',
      'role.province': 'Province Officer',
      'role.district': 'District Officer',
      'role.school': 'School Librarian',
      'role.teacher': 'Teacher',
      'role.student': 'Student',

      // Book Status
      'book.status.available': 'Available',
      'book.status.loaned': 'Loaned',
      'book.status.overdue': 'Overdue',
      'book.status.lost': 'Lost',
      'book.status.damaged': 'Damaged',

      // Loan Status
      'loan.status.active': 'Active',
      'loan.status.returned': 'Returned',
      'loan.status.overdue': 'Overdue',
      'loan.status.cancelled': 'Cancelled',

      // Messages
      'message.loginSuccess': 'Successfully logged in',
      'message.loginError': 'An error occurred during login',
      'message.logoutSuccess': 'Successfully logged out',
      'message.saveSuccess': 'Successfully saved',
      'message.saveError': 'An error occurred while saving',
      'message.deleteConfirm': 'Are you sure you want to delete this item?',
      'message.deleteSuccess': 'Successfully deleted',
      'message.deleteError': 'An error occurred while deleting',
    }
  }

  const t = (key: string): string => {
    return messages[currentLanguage.value][key as keyof typeof messages.tr] || key
  }

  const setLanguage = (lang: Language) => {
    currentLanguage.value = lang
    localStorage.setItem('meb_library_language', lang)
  }

  const initializeLanguage = () => {
    const savedLang = localStorage.getItem('meb_library_language') as Language
    if (savedLang && (savedLang === 'tr' || savedLang === 'en')) {
      currentLanguage.value = savedLang
    }
  }

  return {
    currentLanguage,
    messages,
    t,
    setLanguage,
    initializeLanguage
  }
})