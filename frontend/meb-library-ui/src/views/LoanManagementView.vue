<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { apiClient } from '../stores/api'

interface Loan {
  id: string
  bookId: string
  bookTitle: string
  author: string
  userId: string
  userName: string
  borrowDate: string
  dueDate: string
  returnDate?: string
  status: 'Aktif' | 'Gecikmiş' | 'Teslim Edildi'
  lateFee?: number
}

interface LoanStats {
  activeLoans: number
  overdueLoans: number
  returnedToday: number
  totalLateFees: number
}

const loans = ref<Loan[]>([])
const stats = ref<LoanStats>({
  activeLoans: 0,
  overdueLoans: 0,
  returnedToday: 0,
  totalLateFees: 0
})

const loading = ref(true)
const activeTab = ref<'active' | 'overdue' | 'returned'>('active')

const filteredLoans = computed(() => {
  return loans.value.filter(loan => {
    switch (activeTab.value) {
      case 'active':
        return loan.status === 'Aktif' || loan.status === 'Gecikmiş'
      case 'overdue':
        return loan.status === 'Gecikmiş'
      case 'returned':
        return loan.status === 'Teslim Edildi'
      default:
        return true
    }
  })
})

const fetchLoans = async () => {
  try {
    loading.value = true

    // Backend'den ödünç işlemleri çekmek için API çağrısı
    // Şimdilik mock data kullanıyoruz
    loans.value = [
      {
        id: '1',
        bookId: 'book1',
        bookTitle: 'İnce Memed',
        author: 'Yaşar Kemal',
        userId: 'user1',
        userName: 'Ahmet Yılmaz',
        borrowDate: '2024-01-10',
        dueDate: '2024-02-10',
        status: 'Aktif',
        lateFee: 0
      },
      {
        id: '2',
        bookId: 'book2',
        bookTitle: 'Beyaz Diş',
        author: 'Jack London',
        userId: 'user2',
        userName: 'Ayşe Kaya',
        borrowDate: '2024-01-05',
        dueDate: '2024-02-05',
        status: 'Gecikmiş',
        lateFee: 5.0
      },
      {
        id: '3',
        bookId: 'book3',
        bookTitle: 'STEM Eğitimi',
        author: 'MEB',
        userId: 'user3',
        userName: 'Mehmet Demir',
        borrowDate: '2024-01-01',
        dueDate: '2024-02-01',
        returnDate: '2024-01-20',
        status: 'Teslim Edildi',
        lateFee: 0
      }
    ]

    // İstatistikleri hesapla
    stats.value = {
      activeLoans: loans.value.filter(l => l.status === 'Aktif').length,
      overdueLoans: loans.value.filter(l => l.status === 'Gecikmiş').length,
      returnedToday: loans.value.filter(l =>
        l.status === 'Teslim Edildi' &&
        l.returnDate === new Date().toISOString().split('T')[0]
      ).length,
      totalLateFees: loans.value.reduce((sum, l) => sum + (l.lateFee || 0), 0)
    }

  } catch (error) {
    console.error('Ödünç işlemleri alınamadı:', error)
  } finally {
    loading.value = false
  }
}

const returnBook = async (loanId: string) => {
  try {
    // Kitap iade API çağrısı
    console.log('Kitap iade ediliyor:', loanId)
    // await apiClient.post(`/odunc-islemleri/${loanId}/return`)
    alert('Kitap başarıyla iade edildi!')
    await fetchLoans() // Listeyi yenile
  } catch (error) {
    console.error('İade hatası:', error)
    alert('İade işlemi başarısız!')
  }
}

const extendLoan = async (loanId: string) => {
  try {
    // Ödünç uzatma API çağrısı
    console.log('Ödünç uzatılıyor:', loanId)
    // await apiClient.post(`/odunc-islemleri/${loanId}/extend`)
    alert('Ödünç süresi uzatıldı!')
    await fetchLoans() // Listeyi yenile
  } catch (error) {
    console.error('Uzatma hatası:', error)
    alert('Uzatma işlemi başarısız!')
  }
}

const getStatusBadgeClass = (status: string) => {
  switch (status) {
    case 'Aktif': return 'status-active'
    case 'Gecikmiş': return 'status-overdue'
    case 'Teslim Edildi': return 'status-returned'
    default: return 'status-default'
  }
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('tr-TR')
}

const isOverdue = (dueDate: string) => {
  return new Date(dueDate) < new Date()
}

onMounted(() => {
  fetchLoans()
})
</script>

<template>
  <div class="loan-management">
    <div class="header">
      <h1>Ödünç İşlemleri Yönetimi</h1>
      <p>Aktif ödünçleri, gecikmeleri ve iadeleri takip edin</p>
    </div>

    <!-- İstatistik Kartları -->
    <div class="stats-grid" v-if="!loading">
      <div class="stat-card">
        <div class="stat-icon">📚</div>
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

      <div class="stat-card success">
        <div class="stat-icon">✅</div>
        <div class="stat-content">
          <h3>{{ stats.returnedToday }}</h3>
          <p>Bugün İade</p>
        </div>
      </div>

      <div class="stat-card danger">
        <div class="stat-icon">💰</div>
        <div class="stat-content">
          <h3>{{ stats.totalLateFees.toFixed(2) }} ₺</h3>
          <p>Toplam Ceza</p>
        </div>
      </div>
    </div>

    <!-- Tab Navigation -->
    <div class="tabs">
      <button
        @click="activeTab = 'active'"
        :class="{ active: activeTab === 'active' }"
      >
        Aktif Ödünçler
      </button>
      <button
        @click="activeTab = 'overdue'"
        :class="{ active: activeTab === 'overdue' }"
      >
        Gecikmiş Ödünçler
      </button>
      <button
        @click="activeTab = 'returned'"
        :class="{ active: activeTab === 'returned' }"
      >
        İade Edilenler
      </button>
    </div>

    <!-- Ödünç Listesi -->
    <div class="loans-section">
      <div v-if="loading" class="loading">
        <div class="spinner"></div>
        <p>Ödünç işlemleri yükleniyor...</p>
      </div>

      <div v-else-if="filteredLoans.length === 0" class="no-loans">
        <p>Bu kategoride ödünç işlemi bulunmuyor.</p>
      </div>

      <div v-else class="loans-grid">
        <div
          class="loan-card"
          v-for="loan in filteredLoans"
          :key="loan.id"
        >
          <div class="loan-info">
            <h3>{{ loan.bookTitle }}</h3>
            <p class="author">{{ loan.author }}</p>

            <div class="loan-details">
              <p><strong>Ödünç Alan:</strong> {{ loan.userName }}</p>
              <p><strong>Ödünç Tarihi:</strong> {{ formatDate(loan.borrowDate) }}</p>
              <p><strong>Son Teslim:</strong> {{ formatDate(loan.dueDate) }}</p>
              <p v-if="loan.returnDate">
                <strong>İade Tarihi:</strong> {{ formatDate(loan.returnDate) }}
              </p>
              <p v-if="loan.lateFee && loan.lateFee > 0">
                <strong>Ceza:</strong> {{ loan.lateFee }} ₺
              </p>
            </div>
          </div>

          <div class="loan-status">
            <span
              class="status-badge"
              :class="getStatusBadgeClass(loan.status)"
            >
              {{ loan.status }}
            </span>

            <div class="loan-actions" v-if="activeTab !== 'returned'">
              <button
                v-if="loan.status === 'Aktif'"
                @click="returnBook(loan.id)"
                class="action-btn return-btn"
              >
                İade Al
              </button>

              <button
                v-if="loan.status === 'Aktif'"
                @click="extendLoan(loan.id)"
                class="action-btn extend-btn"
              >
                Uzat
              </button>

              <button
                v-if="loan.status === 'Gecikmiş'"
                @click="returnBook(loan.id)"
                class="action-btn return-btn urgent"
              >
                Acil İade
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.loan-management {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.header {
  text-align: center;
  margin-bottom: 2rem;
}

.header h1 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
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
  border-left: 4px solid #f39c12;
}

.stat-card.success {
  border-left: 4px solid #27ae60;
}

.stat-card.danger {
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

.tabs {
  background: white;
  border-radius: 12px;
  padding: 0.5rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  display: flex;
  margin-bottom: 2rem;
}

.tabs button {
  flex: 1;
  background: none;
  border: none;
  padding: 0.75rem 1rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  color: #7f8c8d;
  transition: all 0.2s;
}

.tabs button.active {
  background: #3498db;
  color: white;
}

.tabs button:hover:not(.active) {
  background: #ecf0f1;
}

.loans-section {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.loans-grid {
  display: grid;
  gap: 1rem;
}

.loan-card {
  border: 1px solid #e1e8ed;
  border-radius: 8px;
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  transition: box-shadow 0.2s;
}

.loan-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.loan-info h3 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
  font-size: 1.2rem;
}

.loan-info .author {
  color: #3498db;
  margin: 0 0 1rem 0;
  font-weight: 500;
}

.loan-details {
  margin-bottom: 1rem;
}

.loan-details p {
  margin: 0.25rem 0;
  font-size: 0.9rem;
  color: #555;
}

.loan-status {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 1rem;
  min-width: 150px;
}

.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
  text-transform: uppercase;
}

.status-badge.status-active {
  background: #d4edda;
  color: #155724;
}

.status-badge.status-overdue {
  background: #f8d7da;
  color: #721c24;
}

.status-badge.status-returned {
  background: #d1ecf1;
  color: #0c5460;
}

.loan-actions {
  display: flex;
  gap: 0.5rem;
}

.action-btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  font-weight: 500;
  transition: all 0.2s;
}

.return-btn {
  background: #27ae60;
  color: white;
}

.return-btn:hover {
  background: #219a52;
}

.return-btn.urgent {
  background: #e74c3c;
}

.return-btn.urgent:hover {
  background: #c0392b;
}

.extend-btn {
  background: #3498db;
  color: white;
}

.extend-btn:hover {
  background: #2980b9;
}

.loading,
.no-loans {
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
  .loan-management {
    padding: 1rem;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }

  .tabs {
    flex-direction: column;
  }

  .loan-card {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .loan-status {
    align-items: flex-start;
    width: 100%;
  }

  .loan-actions {
    justify-content: flex-start;
  }
}
</style>