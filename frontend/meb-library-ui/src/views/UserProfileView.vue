<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { apiClient } from '../stores/api'

interface UserProfile {
  id: string
  firstName: string
  lastName: string
  email: string
  phone?: string
  membershipDate: string
  membershipType: string
  isActive: boolean
}

interface UserStats {
  activeLoans: number
  totalLoans: number
  overdueLoans: number
  totalLateFees: number
  favoriteCategories: string[]
}

interface LoanHistory {
  id: string
  bookTitle: string
  author: string
  borrowDate: string
  returnDate?: string
  status: string
  lateFee?: number
}

const user = ref<UserProfile>({
  id: '',
  firstName: '',
  lastName: '',
  email: '',
  phone: '',
  membershipDate: '',
  membershipType: 'Standart',
  isActive: true
})

const stats = ref<UserStats>({
  activeLoans: 0,
  totalLoans: 0,
  overdueLoans: 0,
  totalLateFees: 0,
  favoriteCategories: []
})

const loanHistory = ref<LoanHistory[]>([])
const loading = ref(true)
const editing = ref(false)

const fetchUserProfile = async () => {
  try {
    loading.value = true

    // Backend'den kullanıcı profili çekmek için API çağrısı
    // Şimdilik mock data kullanıyoruz
    user.value = {
      id: 'user1',
      firstName: 'Ahmet',
      lastName: 'Yılmaz',
      email: 'ahmet.yilmaz@email.com',
      phone: '+90 555 123 4567',
      membershipDate: '2023-09-15',
      membershipType: 'Premium',
      isActive: true
    }

    stats.value = {
      activeLoans: 2,
      totalLoans: 45,
      overdueLoans: 0,
      totalLateFees: 15.50,
      favoriteCategories: ['Roman', 'Tarih', 'Bilim']
    }

    loanHistory.value = [
      {
        id: '1',
        bookTitle: 'İnce Memed',
        author: 'Yaşar Kemal',
        borrowDate: '2024-01-10',
        returnDate: '2024-01-25',
        status: 'Teslim Edildi',
        lateFee: 0
      },
      {
        id: '2',
        bookTitle: 'Beyaz Diş',
        author: 'Jack London',
        borrowDate: '2024-01-15',
        status: 'Aktif',
        lateFee: 0
      },
      {
        id: '3',
        bookTitle: '1984',
        author: 'George Orwell',
        borrowDate: '2024-01-01',
        returnDate: '2024-02-01',
        status: 'Teslim Edildi',
        lateFee: 5.50
      }
    ]

  } catch (error) {
    console.error('Kullanıcı profili alınamadı:', error)
  } finally {
    loading.value = false
  }
}

const saveProfile = async () => {
  try {
    // Profil güncelleme API çağrısı
    console.log('Profil güncelleniyor:', user.value)
    // await apiClient.put('/users/profile', user.value)
    alert('Profil başarıyla güncellendi!')
    editing.value = false
  } catch (error) {
    console.error('Profil güncelleme hatası:', error)
    alert('Profil güncellenirken hata oluştu!')
  }
}

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('tr-TR')
}

const getMembershipBadgeColor = (type: string) => {
  switch (type.toLowerCase()) {
    case 'premium': return 'premium'
    case 'gold': return 'gold'
    default: return 'standard'
  }
}

onMounted(() => {
  fetchUserProfile()
})
</script>

<template>
  <div class="user-profile">
    <div class="profile-header">
      <div class="user-avatar">
        <div class="avatar-circle">
          {{ user.firstName.charAt(0) }}{{ user.lastName.charAt(0) }}
        </div>
      </div>
      <div class="user-info">
        <h1>{{ user.firstName }} {{ user.lastName }}</h1>
        <p class="email">{{ user.email }}</p>
        <span
          class="membership-badge"
          :class="getMembershipBadgeColor(user.membershipType)"
        >
          {{ user.membershipType }} Üye
        </span>
      </div>
    </div>

    <!-- Profil Düzenleme -->
    <div class="profile-section" v-if="!loading">
      <div class="section-header">
        <h2>Kişisel Bilgiler</h2>
        <button
          @click="editing = !editing"
          class="edit-btn"
        >
          {{ editing ? 'İptal' : 'Düzenle' }}
        </button>
      </div>

      <form v-if="editing" @submit.prevent="saveProfile" class="profile-form">
        <div class="form-row">
          <div class="form-group">
            <label>Ad</label>
            <input v-model="user.firstName" type="text" required />
          </div>
          <div class="form-group">
            <label>Soyad</label>
            <input v-model="user.lastName" type="text" required />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>E-posta</label>
            <input v-model="user.email" type="email" required />
          </div>
          <div class="form-group">
            <label>Telefon</label>
            <input v-model="user.phone" type="tel" />
          </div>
        </div>

        <div class="form-actions">
          <button type="submit" class="save-btn">Kaydet</button>
          <button type="button" @click="editing = false" class="cancel-btn">İptal</button>
        </div>
      </form>

      <div v-else class="profile-details">
        <div class="detail-row">
          <span class="label">Ad Soyad:</span>
          <span class="value">{{ user.firstName }} {{ user.lastName }}</span>
        </div>
        <div class="detail-row">
          <span class="label">E-posta:</span>
          <span class="value">{{ user.email }}</span>
        </div>
        <div class="detail-row" v-if="user.phone">
          <span class="label">Telefon:</span>
          <span class="value">{{ user.phone }}</span>
        </div>
        <div class="detail-row">
          <span class="label">Üyelik Tarihi:</span>
          <span class="value">{{ formatDate(user.membershipDate) }}</span>
        </div>
        <div class="detail-row">
          <span class="label">Üyelik Tipi:</span>
          <span class="value">{{ user.membershipType }}</span>
        </div>
      </div>
    </div>

    <!-- İstatistikler -->
    <div class="stats-section" v-if="!loading">
      <h2>Kütüphane İstatistikleri</h2>
      <div class="stats-grid">
        <div class="stat-card">
          <div class="stat-icon">📚</div>
          <div class="stat-content">
            <h3>{{ stats.activeLoans }}</h3>
            <p>Aktif Ödünç</p>
          </div>
        </div>

        <div class="stat-card">
          <div class="stat-icon">📖</div>
          <div class="stat-content">
            <h3>{{ stats.totalLoans }}</h3>
            <p>Toplam Ödünç</p>
          </div>
        </div>

        <div class="stat-card warning">
          <div class="stat-icon">⚠️</div>
          <div class="stat-content">
            <h3>{{ stats.overdueLoans }}</h3>
            <p>Gecikme</p>
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

      <!-- Favori Kategoriler -->
      <div class="favorite-categories" v-if="stats.favoriteCategories.length > 0">
        <h3>Favori Kategorileriniz</h3>
        <div class="categories-list">
          <span
            v-for="category in stats.favoriteCategories"
            :key="category"
            class="category-tag"
          >
            {{ category }}
          </span>
        </div>
      </div>
    </div>

    <!-- Ödünç Geçmişi -->
    <div class="loan-history-section" v-if="!loading">
      <h2>Ödünç Geçmişi</h2>
      <div class="loan-history">
        <div
          class="loan-item"
          v-for="loan in loanHistory"
          :key="loan.id"
        >
          <div class="loan-info">
            <h4>{{ loan.bookTitle }}</h4>
            <p class="author">{{ loan.author }}</p>
            <div class="loan-dates">
              <span>Ödünç: {{ formatDate(loan.borrowDate) }}</span>
              <span v-if="loan.returnDate">İade: {{ formatDate(loan.returnDate) }}</span>
            </div>
          </div>

          <div class="loan-status">
            <span
              class="status-badge"
              :class="{
                active: loan.status === 'Aktif',
                returned: loan.status === 'Teslim Edildi'
              }"
            >
              {{ loan.status }}
            </span>
            <span v-if="loan.lateFee && loan.lateFee > 0" class="late-fee">
              {{ loan.lateFee }} ₺ ceza
            </span>
          </div>
        </div>
      </div>
    </div>

    <!-- Yükleme Durumu -->
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Profil bilgileri yükleniyor...</p>
    </div>
  </div>
</template>

<style scoped>
.user-profile {
  padding: 2rem;
  max-width: 1000px;
  margin: 0 auto;
}

.profile-header {
  display: flex;
  align-items: center;
  gap: 2rem;
  margin-bottom: 3rem;
  padding: 2rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.user-avatar {
  flex-shrink: 0;
}

.avatar-circle {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-size: 2rem;
  font-weight: bold;
}

.user-info h1 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
}

.email {
  color: #7f8c8d;
  margin: 0 0 1rem 0;
}

.membership-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
  text-transform: uppercase;
}

.membership-badge.premium {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.membership-badge.gold {
  background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
  color: white;
}

.membership-badge.standard {
  background: #ecf0f1;
  color: #2c3e50;
}

.profile-section {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.section-header h2 {
  color: #2c3e50;
  margin: 0;
}

.edit-btn {
  background: #3498db;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background-color 0.2s;
}

.edit-btn:hover {
  background: #2980b9;
}

.profile-form {
  max-width: 600px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-bottom: 1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group label {
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #2c3e50;
}

.form-group input {
  padding: 0.75rem;
  border: 2px solid #e1e8ed;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.form-group input:focus {
  outline: none;
  border-color: #3498db;
}

.form-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1.5rem;
}

.save-btn,
.cancel-btn {
  padding: 0.75rem 2rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.2s;
}

.save-btn {
  background: #27ae60;
  color: white;
}

.save-btn:hover {
  background: #219a52;
}

.cancel-btn {
  background: #95a5a6;
  color: white;
}

.cancel-btn:hover {
  background: #7f8c8d;
}

.profile-details {
  max-width: 600px;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 0;
  border-bottom: 1px solid #ecf0f1;
}

.detail-row:last-child {
  border-bottom: none;
}

.label {
  font-weight: 500;
  color: #2c3e50;
}

.value {
  color: #555;
}

.stats-section {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.stats-section h2 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: #f8f9fa;
  border-radius: 8px;
  padding: 1.5rem;
  display: flex;
  align-items: center;
  gap: 1rem;
}

.stat-card.warning {
  border-left: 4px solid #f39c12;
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

.favorite-categories h3 {
  color: #2c3e50;
  margin-bottom: 1rem;
}

.categories-list {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.category-tag {
  background: #3498db;
  color: white;
  padding: 0.25rem 0.75rem;
  border-radius: 15px;
  font-size: 0.8rem;
  font-weight: 500;
}

.loan-history-section {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.loan-history-section h2 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
}

.loan-history {
  display: grid;
  gap: 1rem;
}

.loan-item {
  border: 1px solid #e1e8ed;
  border-radius: 8px;
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  transition: box-shadow 0.2s;
}

.loan-item:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.loan-info h4 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
}

.loan-info .author {
  color: #3498db;
  margin: 0 0 1rem 0;
  font-weight: 500;
}

.loan-dates {
  display: flex;
  gap: 1rem;
  font-size: 0.9rem;
  color: #7f8c8d;
}

.loan-status {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 0.5rem;
  min-width: 120px;
}

.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
  text-transform: uppercase;
}

.status-badge.active {
  background: #d4edda;
  color: #155724;
}

.status-badge.returned {
  background: #d1ecf1;
  color: #0c5460;
}

.late-fee {
  color: #e74c3c;
  font-size: 0.8rem;
  font-weight: 500;
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
  .profile-header {
    flex-direction: column;
    text-align: center;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .stats-grid {
    grid-template-columns: 1fr;
  }

  .loan-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .loan-status {
    align-items: flex-start;
    width: 100%;
  }
}
</style>