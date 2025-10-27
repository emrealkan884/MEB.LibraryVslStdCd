<script setup lang="ts">
import { ref, computed } from 'vue'
import { apiClient } from '../stores/api'

interface Book {
  id: string
  baslik: string
  altBaslik?: string
  yazarAdi: string
  isbn?: string
  yayinevi?: string
  yayinYili?: number
  materyalTuru: string
  materyalAltTuru?: string
  durum: string
  kapakResmiYolu?: string
}

interface SearchFilters {
  query: string
  materyalTuru: string
  yazar: string
  yayinevi: string
  yayinYili: string
}

const books = ref<Book[]>([])
const loading = ref(false)
const searchPerformed = ref(false)

const filters = ref<SearchFilters>({
  query: '',
  materyalTuru: '',
  yazar: '',
  yayinevi: '',
  yayinYili: ''
})

const filteredBooks = computed(() => {
  return books.value.filter(book => {
    const matchesQuery = !filters.value.query ||
      book.baslik.toLowerCase().includes(filters.value.query.toLowerCase()) ||
      book.yazarAdi.toLowerCase().includes(filters.value.query.toLowerCase()) ||
      (book.isbn && book.isbn.includes(filters.value.query))

    const matchesMateryal = !filters.value.materyalTuru ||
      book.materyalTuru === filters.value.materyalTuru

    const matchesYazar = !filters.value.yazar ||
      book.yazarAdi.toLowerCase().includes(filters.value.yazar.toLowerCase())

    return matchesQuery && matchesMateryal && matchesYazar
  })
})

const searchBooks = async () => {
  try {
    loading.value = true
    searchPerformed.value = true

    // Backend'den kitap listesi çekmek için API çağrısı
    // Şimdilik mock data kullanıyoruz
    books.value = [
      {
        id: '1',
        baslik: 'İnce Memed',
        altBaslik: 'Çukurova Romanı',
        yazarAdi: 'Yaşar Kemal',
        isbn: '9789754707083',
        yayinevi: 'Yapı Kredi Yayınları',
        yayinYili: 1955,
        materyalTuru: 'Kitap',
        materyalAltTuru: 'Roman',
        durum: 'Müsait',
        kapakResmiYolu: '/covers/ince-memed.jpg'
      },
      {
        id: '2',
        baslik: 'Beyaz Diş',
        yazarAdi: 'Jack London',
        isbn: '9786059852564',
        yayinevi: 'İletişim Yayınları',
        yayinYili: 1906,
        materyalTuru: 'Kitap',
        materyalAltTuru: 'Macera',
        durum: 'Ödünç Verildi',
        kapakResmiYolu: '/covers/beyaz-dis.jpg'
      },
      {
        id: '3',
        baslik: 'STEM Eğitimi',
        yazarAdi: 'Milli Eğitim Bakanlığı',
        isbn: '9789751100000',
        yayinevi: 'MEB Yayınları',
        yayinYili: 2023,
        materyalTuru: 'Kitap',
        materyalAltTuru: 'Eğitim',
        durum: 'Müsait',
        kapakResmiYolu: '/covers/stem-egitimi.jpg'
      }
    ]
  } catch (error) {
    console.error('Kitap arama hatası:', error)
  } finally {
    loading.value = false
  }
}

const clearFilters = () => {
  filters.value = {
    query: '',
    materyalTuru: '',
    yazar: '',
    yayinevi: '',
    yayinYili: ''
  }
}

const borrowBook = async (bookId: string) => {
  try {
    // Ödünç alma API çağrısı
    console.log('Kitap ödünç alınıyor:', bookId)
    // await apiClient.post(`/odunc-islemleri`, { bookId })
    alert('Kitap ödünç alma işlemi başlatıldı!')
  } catch (error) {
    console.error('Ödünç alma hatası:', error)
    alert('Ödünç alma işlemi başarısız!')
  }
}

const handleImageError = (event: Event) => {
  const img = event.target as HTMLImageElement
  img.src = '/placeholder-book.png'
}
</script>

<template>
  <div class="book-search">
    <div class="search-header">
      <h1>Kitap Arama</h1>
      <p>Kütüphane koleksiyonunda arama yapın</p>
    </div>

    <!-- Arama Formu -->
    <div class="search-form">
      <div class="search-inputs">
        <div class="input-group">
          <input
            v-model="filters.query"
            type="text"
            placeholder="Kitap adı, yazar veya ISBN ara..."
            @keyup.enter="searchBooks"
          />
          <button @click="searchBooks" :disabled="loading" class="search-btn">
            <span v-if="loading">🔍</span>
            <span v-else>🔍</span>
            Ara
          </button>
        </div>
      </div>

      <!-- Filtreler -->
      <div class="filters">
        <select v-model="filters.materyalTuru">
          <option value="">Tüm Materyal Türleri</option>
          <option value="Kitap">Kitap</option>
          <option value="Dergi">Dergi</option>
          <option value="DVD">DVD</option>
          <option value="CD">CD</option>
        </select>

        <input
          v-model="filters.yazar"
          type="text"
          placeholder="Yazar adı"
        />

        <input
          v-model="filters.yayinevi"
          type="text"
          placeholder="Yayınevi"
        />

        <input
          v-model="filters.yayinYili"
          type="number"
          placeholder="Yayın yılı"
        />

        <button @click="clearFilters" class="clear-btn">Temizle</button>
      </div>
    </div>

    <!-- Sonuçlar -->
    <div class="search-results" v-if="searchPerformed">
      <div class="results-header">
        <h2>
          Arama Sonuçları
          <span class="result-count">({{ filteredBooks.length }} kitap bulundu)</span>
        </h2>
      </div>

      <div v-if="loading" class="loading">
        <div class="spinner"></div>
        <p>Arama yapılıyor...</p>
      </div>

      <div v-else-if="filteredBooks.length === 0" class="no-results">
        <p>Arama kriterlerinize uygun kitap bulunamadı.</p>
      </div>

      <div v-else class="books-grid">
        <div
          class="book-card"
          v-for="book in filteredBooks"
          :key="book.id"
        >
          <div class="book-cover">
            <img
              :src="book.kapakResmiYolu || '/placeholder-book.png'"
              :alt="book.baslik"
              @error="handleImageError"
            />
          </div>

          <div class="book-info">
            <h3>{{ book.baslik }}</h3>
            <p v-if="book.altBaslik" class="subtitle">{{ book.altBaslik }}</p>

            <div class="book-details">
              <p><strong>Yazar:</strong> {{ book.yazarAdi }}</p>
              <p v-if="book.isbn"><strong>ISBN:</strong> {{ book.isbn }}</p>
              <p v-if="book.yayinevi"><strong>Yayınevi:</strong> {{ book.yayinevi }}</p>
              <p v-if="book.yayinYili"><strong>Yıl:</strong> {{ book.yayinYili }}</p>
              <p><strong>Tür:</strong> {{ book.materyalTuru }}</p>
              <p v-if="book.materyalAltTuru"><strong>Alt Tür:</strong> {{ book.materyalAltTuru }}</p>
            </div>

            <div class="book-status">
              <span
                class="status-badge"
                :class="{ available: book.durum === 'Müsait', borrowed: book.durum === 'Ödünç Verildi' }"
              >
                {{ book.durum }}
              </span>
            </div>

            <button
              v-if="book.durum === 'Müsait'"
              @click="borrowBook(book.id)"
              class="borrow-btn"
            >
              Ödünç Al
            </button>
            <button
              v-else
              disabled
              class="borrow-btn disabled"
            >
              Müsait Değil
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.book-search {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.search-header {
  text-align: center;
  margin-bottom: 2rem;
}

.search-header h1 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.search-form {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.search-inputs {
  margin-bottom: 1.5rem;
}

.input-group {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.input-group input {
  flex: 1;
  padding: 0.75rem 1rem;
  border: 2px solid #e1e8ed;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.input-group input:focus {
  outline: none;
  border-color: #3498db;
}

.search-btn {
  background: #3498db;
  color: white;
  border: none;
  padding: 0.75rem 2rem;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: background-color 0.2s;
}

.search-btn:hover:not(:disabled) {
  background: #2980b9;
}

.search-btn:disabled {
  background: #bdc3c7;
  cursor: not-allowed;
}

.filters {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  align-items: center;
}

.filters select,
.filters input {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 0.9rem;
}

.clear-btn {
  background: #95a5a6;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.clear-btn:hover {
  background: #7f8c8d;
}

.search-results {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.results-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.results-header h2 {
  color: #2c3e50;
  margin: 0;
}

.result-count {
  color: #7f8c8d;
  font-size: 0.9rem;
  font-weight: normal;
}

.books-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 1.5rem;
}

.book-card {
  border: 1px solid #e1e8ed;
  border-radius: 8px;
  padding: 1.5rem;
  display: flex;
  gap: 1rem;
  transition: box-shadow 0.2s;
}

.book-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.book-cover {
  flex-shrink: 0;
  width: 100px;
  height: 140px;
  border-radius: 4px;
  overflow: hidden;
  background: #f8f9fa;
}

.book-cover img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.book-info {
  flex: 1;
}

.book-info h3 {
  margin: 0 0 0.5rem 0;
  color: #2c3e50;
  font-size: 1.2rem;
}

.subtitle {
  color: #7f8c8d;
  margin: 0 0 1rem 0;
  font-style: italic;
}

.book-details {
  margin-bottom: 1rem;
}

.book-details p {
  margin: 0.25rem 0;
  font-size: 0.9rem;
  color: #555;
}

.book-status {
  margin-bottom: 1rem;
}

.status-badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 500;
  text-transform: uppercase;
}

.status-badge.available {
  background: #d4edda;
  color: #155724;
}

.status-badge.borrowed {
  background: #f8d7da;
  color: #721c24;
}

.borrow-btn {
  background: #27ae60;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background-color 0.2s;
}

.borrow-btn:hover:not(.disabled) {
  background: #219a52;
}

.borrow-btn.disabled {
  background: #bdc3c7;
  cursor: not-allowed;
}

.loading,
.no-results {
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
  .book-search {
    padding: 1rem;
  }

  .search-form {
    padding: 1rem;
  }

  .input-group {
    flex-direction: column;
  }

  .input-group input {
    width: 100%;
  }

  .filters {
    grid-template-columns: 1fr;
  }

  .books-grid {
    grid-template-columns: 1fr;
  }

  .book-card {
    flex-direction: column;
    text-align: center;
  }
}
</style>
