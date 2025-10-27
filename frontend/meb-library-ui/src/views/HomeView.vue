<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { apiClient } from '../stores/api'

interface BookStats {
  totalBooks: number
  activeLoans: number
  overdueLoans: number
  totalUsers: number
}

interface RecentBook {
  id: string
  baslik: string
  yazarAdi: string
  materyalTuru: string
  kayitTarihi: string
}

const stats = ref<BookStats>({
  totalBooks: 0,
  activeLoans: 0,
  overdueLoans: 0,
  totalUsers: 0
})

const recentBooks = ref<RecentBook[]>([])
const loading = ref(true)

const fetchDashboardData = async () => {
  try {
    loading.value = true

    // Backend'den veri çekmek için API çağrıları
    // Şimdilik mock data kullanıyoruz - gerçek API'lere bağlanacak
    stats.value = {
      totalBooks: 1250,
      activeLoans: 89,
      overdueLoans: 12,
      totalUsers: 450
    }

    recentBooks.value = [
      {
        id: '1',
        baslik: 'İnce Memed',
        yazarAdi: 'Yaşar Kemal',
        materyalTuru: 'Kitap',
        kayitTarihi: '2024-01-15'
      },
      {
        id: '2',
        baslik: 'Beyaz Diş',
        yazarAdi: 'Jack London',
        materyalTuru: 'Kitap',
        kayitTarihi: '2024-01-14'
      },
      {
        id: '3',
        baslik: 'STEM Eğitimi',
        yazarAdi: 'Milli Eğitim Bakanlığı',
        materyalTuru: 'Kitap',
        kayitTarihi: '2024-01-13'
      }
    ]
  } catch (error) {
    console.error('Dashboard verisi alınamadı:', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchDashboardData()
})
</script>

<template>
  <div class="dashboard">
    <div class="dashboard-header">
      <h1>Kütüphane Yönetim Sistemi</h1>
      <p>Hoş geldiniz! Sistemin genel durumuna buradan göz atabilirsiniz.</p>
    </div>

    <!-- İstatistik Kartları -->
    <div class="stats-grid" v-if="!loading">
      <div class="stat-card">
        <div class="stat-icon">📚</div>
        <div class="stat-content">
          <h3>{{ stats.totalBooks.toLocaleString() }}</h3>
          <p>Toplam Kitap</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon">📖</div>
        <div class="stat-content">
          <h3>{{ stats.activeLoans }}</h3>
          <p>Aktif Ödünç</p>
        </div>
      </div>

      <div class="stat-card warning">
        <div class="stat-icon">⚠️</div>
        <div class="stat-content">
          <h3>{{ stats.overdueLoans }}</h3>
          <p>Gecikmiş Ödünç</p>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon">👥</div>
        <div class="stat-content">
          <h3>{{ stats.totalUsers }}</h3>
          <p>Toplam Kullanıcı</p>
        </div>
      </div>
    </div>

    <!-- Son Eklenen Kitaplar -->
    <div class="recent-books-section" v-if="!loading">
      <h2>Son Eklenen Kitaplar</h2>
      <div class="books-grid">
        <div class="book-card" v-for="book in recentBooks" :key="book.id">
          <div class="book-info">
            <h4>{{ book.baslik }}</h4>
            <p class="author">{{ book.yazarAdi }}</p>
            <p class="type">{{ book.materyalTuru }}</p>
            <p class="date">{{ new Date(book.kayitTarihi).toLocaleDateString('tr-TR') }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Yükleme Durumu -->
    <div class="loading" v-if="loading">
      <div class="spinner"></div>
      <p>Veriler yükleniyor...</p>
    </div>
  </div>
</template>

<style scoped>
.dashboard {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.dashboard-header {
  margin-bottom: 2rem;
  text-align: center;
}

.dashboard-header h1 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.stat-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  gap: 1rem;
  transition: transform 0.2s;
}

.stat-card:hover {
  transform: translateY(-2px);
}

.stat-card.warning {
  border-left: 4px solid #e74c3c;
}

.stat-icon {
  font-size: 2rem;
}

.stat-content h3 {
  font-size: 2rem;
  margin: 0;
  color: #2c3e50;
}

.stat-content p {
  margin: 0.5rem 0 0 0;
  color: #7f8c8d;
  font-weight: 500;
}

.recent-books-section h2 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
  text-align: center;
}

.books-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1rem;
}

.book-card {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s;
}

.book-card:hover {
  transform: translateY(-2px);
}

.book-info h4 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
  font-size: 1.1rem;
}

.book-info .author {
  color: #3498db;
  margin: 0 0 0.5rem 0;
  font-weight: 500;
}

.book-info .type {
  color: #27ae60;
  margin: 0 0 0.5rem 0;
  font-size: 0.9rem;
}

.book-info .date {
  color: #7f8c8d;
  margin: 0;
  font-size: 0.8rem;
}

.loading {
  text-align: center;
  padding: 3rem;
}

.spinner {
  border: 4px solid #f3f3f3;
  border-top: 4px solid #3498db;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 1s linear infinite;
  margin: 0 auto 1rem auto;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

@media (max-width: 768px) {
  .dashboard {
    padding: 1rem;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }

  .books-grid {
    grid-template-columns: 1fr;
  }
}
</style>
