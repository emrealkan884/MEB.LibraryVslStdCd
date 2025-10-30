<template>
  <div class="catalog-detail">
    <nav class="detail-nav">
      <button type="button" class="back-button" @click="goBack">
        ← Sonuçlara dön
      </button>
    </nav>

    <section v-if="state.loading" class="detail-state">
      <span class="detail-spinner" aria-hidden="true"></span>
      <p>Kayıt bilgileri yükleniyor...</p>
    </section>

    <section v-else-if="state.error" class="detail-state detail-state--error">
      <h2>Bilgi alınamadı</h2>
      <p>{{ state.error }}</p>
      <div class="detail-actions">
        <button type="button" class="primary" @click="retry">Tekrar dene</button>
        <button type="button" class="link" @click="goBack">Aramaya dön</button>
      </div>
    </section>

    <section v-else-if="detail" class="detail-content">
      <header class="detail-hero">
        <div class="detail-hero__text">
          <span class="detail-chip">{{ detail.materialType }}</span>
          <h1>{{ detail.title }}</h1>
          <p v-if="detail.subtitle" class="detail-subtitle">{{ detail.subtitle }}</p>
          <p v-if="authorList" class="detail-authors">{{ authorList }}</p>
        </div>

        <ul class="detail-meta">
          <li>
            <span class="detail-meta__label">Yayın yılı</span>
            <span class="detail-meta__value">{{ detail.publicationYear ?? 'Belirtilmemiş' }}</span>
          </li>
          <li>
            <span class="detail-meta__label">Dil</span>
            <span class="detail-meta__value">{{ detail.language ?? 'Belirtilmemiş' }}</span>
          </li>
          <li>
            <span class="detail-meta__label">ISBN</span>
            <span class="detail-meta__value">{{ detail.isbn ?? 'Belirtilmemiş' }}</span>
          </li>
          <li>
            <span class="detail-meta__label">Materyal kodu</span>
            <span class="detail-meta__value">{{ detail.materialCategory }}</span>
          </li>
        </ul>
      </header>

      <nav class="detail-tabs" role="tablist">
        <button
          type="button"
          :class="['detail-tab', { active: activeDetailTab === 'overview' }]"
          role="tab"
          :aria-selected="activeDetailTab === 'overview'"
          @click="setActiveTab('overview')"
        >
          Katalog özeti
        </button>
        <button
          type="button"
          :class="['detail-tab', { active: activeDetailTab === 'marc', disabled: !hasMarcRows }]"
          role="tab"
          :aria-selected="activeDetailTab === 'marc'"
          @click="setActiveTab('marc')"
        >
          Marc görünüm
        </button>
      </nav>

      <div
        v-if="activeDetailTab === 'overview' || !hasMarcRows"
        class="detail-overview"
        role="tabpanel"
        :aria-hidden="activeDetailTab !== 'overview' && hasMarcRows"
      >
        <section class="detail-section">
          <h2>Özet</h2>
          <p>{{ detail.summary ?? 'Bu kayıt için özet bilgisi eklenmemiş.' }}</p>
        </section>

        <section v-if="detail.notes" class="detail-section">
          <h2>Notlar</h2>
          <p>{{ detail.notes }}</p>
        </section>

        <section class="detail-section detail-section--grid">
          <div>
            <h3>Yayın bilgileri</h3>
            <dl class="detail-definition">
              <div>
                <dt>Yayınevi</dt>
                <dd>{{ detail.publisher ?? 'Belirtilmemiş' }}</dd>
              </div>
              <div>
                <dt>Yayın yeri</dt>
                <dd>{{ detail.publicationPlace ?? 'Belirtilmemiş' }}</dd>
              </div>
              <div>
                <dt>Güncelleme</dt>
                <dd>{{ formattedUpdatedAt }}</dd>
              </div>
            </dl>
          </div>

          <div v-if="detail.deweyCode || detail.deweyTitle">
            <h3>Dewey sınıflaması</h3>
            <dl class="detail-definition">
              <div>
                <dt>Kod</dt>
                <dd>{{ detail.deweyCode ?? '—' }}</dd>
              </div>
              <div>
                <dt>Başlık</dt>
                <dd>{{ detail.deweyTitle ?? '—' }}</dd>
              </div>
            </dl>
          </div>
        </section>

        <section class="detail-section">
          <header class="section-header">
            <h2>Dijital kaynaklar</h2>
            <small v-if="detail.digitalResources.length">
              {{ detail.digitalResources.length }} kayıt
            </small>
          </header>

          <p v-if="detail.digitalResources.length === 0" class="section-empty">
            Bu kayıt için dijital erişim bilgisi paylaşılmamış.
          </p>

          <ul v-else class="digital-list">
            <li v-for="resource in detail.digitalResources" :key="resource.key" class="digital-card">
              <header class="digital-card__header">
                <strong>{{ resource.format }}</strong>
                <span v-if="resource.language" class="digital-card__meta">• {{ resource.language }}</span>
              </header>
              <p v-if="resource.description" class="digital-card__description">
                {{ resource.description }}
              </p>
              <p class="digital-card__access">
                <a
                  v-if="resource.isLink"
                  :href="resource.accessInformation"
                  target="_blank"
                  rel="noopener"
                >
                  {{ resource.hostname ? `${resource.hostname} üzerinden aç` : 'Kaynağı aç / indir' }}
                </a>
                <span v-else>{{ resource.accessInformation }}</span>
              </p>
            </li>
          </ul>
        </section>

        <section class="detail-section">
          <header class="section-header">
            <h2>Kütüphanelerde durumu</h2>
            <div v-if="availabilitySummary" class="availability-summary">
              <span>{{ availabilitySummary.totalLibraries }} kütüphane</span>
              <span>
                Rafta {{ availabilitySummary.totalAvailable }} / {{ availabilitySummary.totalCopies }} kopya
              </span>
            </div>
          </header>

          <div v-if="detail.availability.length" class="availability-filters">
            <label>
              <span>İl</span>
              <select v-model="filters.city">
                <option value="all">Tümü</option>
                <option v-for="city in cityOptions" :key="city" :value="city">
                  {{ city }}
                </option>
              </select>
            </label>

            <label>
              <span>İlçe</span>
              <select v-model="filters.district" :disabled="cityOptions.length === 0">
                <option value="all">Tümü</option>
                <option v-for="district in districtOptions" :key="district" :value="district">
                  {{ district }}
                </option>
              </select>
            </label>

            <label class="availability-toggle">
              <input v-model="filters.onlyAvailable" type="checkbox" />
              <span>Sadece ödünç verilebilir</span>
            </label>
          </div>

          <p v-if="filteredAvailability.length === 0" class="section-empty">
            Seçili filtrelere uygun kütüphane bulunamadı.
          </p>

          <ul v-else class="availability-list">
            <li
              v-for="entry in filteredAvailability"
              :key="entry.libraryId"
              class="availability-card"
              :data-status="entry.status"
            >
              <header class="availability-card__header">
                <h3>{{ entry.libraryName }}</h3>
                <span class="availability-chip">{{ availabilityLabel(entry.status) }}</span>
              </header>
              <p class="availability-location">
                <span>{{ entry.city ?? 'İl bilgisi yok' }}</span>
                <span v-if="entry.district"> / {{ entry.district }}</span>
              </p>
              <p v-if="entry.address" class="availability-address">{{ entry.address }}</p>
              <p class="availability-counts">
                Toplam: <strong>{{ entry.totalCopies }}</strong> • Rafta:
                <strong>{{ entry.availableCopies }}</strong>
              </p>
              <p v-if="entry.sections.length" class="availability-sections">
                Bölümler: {{ entry.sections.join(' • ') }}
              </p>
              <p v-if="entry.isReservable" class="availability-reservable">
                Rezervasyona açıktır.
              </p>
              <p v-if="entry.notes" class="availability-notes">Not: {{ entry.notes }}</p>
            </li>
          </ul>
        </section>

        <section v-if="detail.marc21" class="detail-section">
          <details>
            <summary>Marc21 ham verisini göster</summary>
            <pre class="marc-block">{{ detail.marc21 }}</pre>
          </details>
        </section>
      </div>

      <div v-if="activeDetailTab === 'marc'" class="detail-marc" role="tabpanel" aria-label="Marc görünüm">
        <header class="marc-header">
          <div>
            <h2>Marc alanları</h2>
            <p>Kontrol alanları, göstergeler ve alt alan değerleri.</p>
          </div>
          <button type="button" class="detail-tab link" @click="setActiveTab('overview')">
            Genel özete dön
          </button>
        </header>

        <div v-if="hasMarcRows" class="marc-table-wrapper">
          <table class="marc-table">
            <thead>
              <tr>
                <th scope="col">Marc kodu</th>
                <th scope="col">1. gösterge</th>
                <th scope="col">2. gösterge</th>
                <th scope="col">Alt alan</th>
                <th scope="col">Veri</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(row, index) in marcRows" :key="`${row.tag}-${index}-${row.subfield}`">
                <th scope="row">{{ row.tag }}</th>
                <td>{{ row.ind1 }}</td>
                <td>{{ row.ind2 }}</td>
                <td>{{ row.subfield }}</td>
                <td>
                  <code class="marc-value">{{ row.value }}</code>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <p v-else class="section-empty">
          Bu katalog kaydı için Marc alanları henüz paylaşılmamış.
        </p>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { isAxiosError } from 'axios'
import { apiClient } from '@/stores/api'

type AvailabilityState = 'available' | 'limited' | 'unavailable'

interface ServerDigitalResource {
  format?: string | null
  accessInformation?: string | null
  language?: string | null
  description?: string | null
}

interface ServerAvailability {
  libraryId?: string | null
  libraryName?: string | null
  city?: string | null
  district?: string | null
  address?: string | null
  sections?: string[] | null
  isReservable?: boolean | null
  totalCopies?: number | null
  availableCopies?: number | null
  notes?: string | null
}

interface ServerCatalogDetail {
  id?: string
  title?: string | null
  subtitle?: string | null
  authors?: string[] | null
  materialType?: string | null
  materialCategory?: string | null
  language?: string | null
  publicationYear?: number | null
  publisher?: string | null
  publicationPlace?: string | null
  isbn?: string | null
  summary?: string | null
  notes?: string | null
  deweyCode?: string | null
  deweyTitle?: string | null
  marc21?: string | null
  updatedAt?: string | null
  digitalResources?: ServerDigitalResource[] | null
  availability?: ServerAvailability[] | null
}

interface DigitalResource {
  key: string
  format: string
  accessInformation: string
  language?: string | null
  description?: string | null
  isLink: boolean
  hostname?: string | null
}

interface AvailabilityEntry {
  libraryId: string
  libraryName: string
  city?: string | null
  district?: string | null
  address?: string | null
  sections: string[]
  isReservable: boolean
  totalCopies: number
  availableCopies: number
  notes?: string | null
  status: AvailabilityState
}

interface CatalogDetail {
  id: string
  title: string
  subtitle?: string | null
  authors: string[]
  materialType: string
  materialCategory: string
  language?: string | null
  publicationYear?: number | null
  publisher?: string | null
  publicationPlace?: string | null
  isbn?: string | null
  summary?: string | null
  notes?: string | null
  deweyCode?: string | null
  deweyTitle?: string | null
  marc21?: string | null
  updatedAt: string
  digitalResources: DigitalResource[]
  availability: AvailabilityEntry[]
}

interface MarcTableRow {
  tag: string
  ind1: string
  ind2: string
  subfield: string
  value: string
}

const route = useRoute()
const router = useRouter()

const state = reactive({
  loading: true,
  error: '',
  detail: null as CatalogDetail | null
})

const filters = reactive({
  city: 'all',
  district: 'all',
  onlyAvailable: false
})

const lastRequestedId = ref('')
const activeDetailTab = ref<'overview' | 'marc'>('overview')

const detail = computed(() => state.detail)

const mockDetailData: Record<string, ServerCatalogDetail> = {
  'mock-1': {
    id: 'mock-1',
    title: 'Benim Adım Kırmızı',
    subtitle: 'Bir cinayet ve aşk hikayesi',
    authors: ['Orhan Pamuk'],
    materialType: 'Kitap • Roman',
    materialCategory: 'Kitap',
    language: 'Türkçe',
    publicationYear: 1998,
    publisher: 'İletişim Yayınları',
    publicationPlace: 'İstanbul',
    isbn: '9754707080',
    summary:
      '16. yüzyıl Osmanlısında geçen, nakkaşların dünyasına ve saray entrikalarına odaklanan polisiye roman.',
    notes: 'Edebiyat müfredatında yer alan seçkin eserlerden biridir.',
    deweyCode: '894.353',
    deweyTitle: 'Türk Romanı',
    updatedAt: new Date().toISOString(),
    digitalResources: [
      {
        format: 'PDF',
        accessInformation: 'https://cdn.meb.gov.tr/dijital-kutuphanem/benim-adim-kirmizi.pdf',
        language: 'Türkçe',
        description: 'Tam metin e-kitap (MEB hesabı ile erişim).'
      },
      {
        format: 'Sesli Kitap',
        accessInformation:
          'https://cdn.meb.gov.tr/dijital-kutuphanem/benim-adim-kirmizi-sesli.mp3',
        language: 'Türkçe',
        description: 'Seslendiren: Devlet Tiyatroları sanatçıları.'
      }
    ],
    availability: [
      {
        libraryId: 'ankara-merkez',
        libraryName: 'MEB Merkez Kütüphanesi',
        city: 'Ankara',
        district: 'Çankaya',
        address: 'Atatürk Bulvarı No:14, Çankaya',
        sections: ['Edebiyat', 'Türk Romanı'],
        isReservable: true,
        totalCopies: 8,
        availableCopies: 6,
        notes: 'Yeni baskı raf düzenlemesi yapıldı.'
      },
      {
        libraryId: 'istanbul-beyoglu',
        libraryName: 'İstanbul İl Halk Kütüphanesi',
        city: 'İstanbul',
        district: 'Beyoğlu',
        address: 'İstiklal Cad. No:201',
        sections: ['Roman', 'Özel Koleksiyon'],
        isReservable: true,
        totalCopies: 5,
        availableCopies: 2,
        notes: null
      }
    ]
  },
  'mock-2': {
    id: 'mock-2',
    title: 'İnce Memed',
    authors: ['Yaşar Kemal'],
    materialType: 'Kitap • Roman',
    materialCategory: 'Kitap',
    language: 'Türkçe',
    publicationYear: 1955,
    publisher: 'Yapı Kredi Yayınları',
    publicationPlace: 'İstanbul',
    isbn: '9789754707083',
    summary:
      'Çukurova topraklarında ağalara karşı direnişi anlatan destansı roman. Anadolu edebiyatının başyapıtlarından biri.',
    notes: '4 ciltlik serinin birinci kitabıdır.',
    deweyCode: '894.353',
    deweyTitle: 'Türk Romanı',
    updatedAt: new Date().toISOString(),
    digitalResources: [
      {
        format: 'PDF',
        accessInformation: 'https://cdn.meb.gov.tr/dijital-kutuphanem/ince-memed.pdf',
        language: 'Türkçe',
        description: 'Öğretmenler için sınıf içi kullanım izni verilmiştir.'
      }
    ],
    availability: [
      {
        libraryId: 'adana-merkez',
        libraryName: 'Adana İl Halk Kütüphanesi',
        city: 'Adana',
        district: 'Seyhan',
        address: 'Atatürk Cad. No:55, Seyhan',
        sections: ['Roman', 'Yerel Koleksiyon'],
        isReservable: true,
        totalCopies: 7,
        availableCopies: 4,
        notes: 'Bir kopya restorasyonda.'
      },
      {
        libraryId: 'ankara-merkez',
        libraryName: 'MEB Merkez Kütüphanesi',
        city: 'Ankara',
        district: 'Çankaya',
        address: 'Atatürk Bulvarı No:14, Çankaya',
        sections: ['Roman'],
        isReservable: true,
        totalCopies: 6,
        availableCopies: 5,
        notes: null
      }
    ]
  },
  'mock-3': {
    id: 'mock-3',
    title: 'Beyaz Diş',
    authors: ['Jack London'],
    materialType: 'Kitap • Macera',
    materialCategory: 'Kitap',
    language: 'Türkçe',
    publicationYear: 1906,
    publisher: 'Can Yayınları',
    publicationPlace: 'İstanbul',
    isbn: '9786059852564',
    summary:
      'Vahşi doğada geçen insan ve kurt-karışımı Beyaz Dişin hayatta kalma mücadelesini anlatan klasik roman.',
    notes: '8. sınıf okuma listesinde önerilen eser.',
    deweyCode: '813.4',
    deweyTitle: 'Amerikan Romanı',
    marc21: `=LDR  00000nam a2200000 a 4500
=008  198801s1987    tu a     g    000 0 tur
=020  __$$c550 TL
=035  __$$a0055285NLT
=041  0_$atur
=082  04$$a087.84
=099  __$$a1988 AD 243
=100  0_$aHugo, Victor,$d1802-1885.
=245  10$$aSefiller /$$cVictor Hugo
=260  __$$aİstanbul :$$bÜnlü,$c1987
=300  __$$a80 s. ;$$c19 cm.
=440  _0$$aÜnlü Kitabevi .$$pÜnlü çocuk klasikleri dizisi ;$$v16
=910  __$$aDM.4961-87
=947  __$$a1987`,
    updatedAt: new Date().toISOString(),
    digitalResources: [
      {
        format: 'PDF',
        accessInformation: 'https://cdn.meb.gov.tr/dijital-kutuphanem/beyaz-dis.pdf',
        language: 'Türkçe',
        description: 'Tam metin çeviri; sayfa numaraları basılı baskı ile eşleşir.'
      },
      {
        format: 'EPUB',
        accessInformation: 'https://cdn.meb.gov.tr/dijital-kutuphanem/beyaz-dis.epub',
        language: 'Türkçe',
        description: 'Mobil cihazlar için yeniden akışkan metin.'
      },
      {
        format: 'Marc21',
        accessInformation: 'Marc kaydı için merkez kütüphane katalogundan yararlanabilirsiniz.',
        language: null,
        description: 'Standart katalog kaydı numarası: MEB-00045872.'
      }
    ],
    availability: [
      {
        libraryId: 'hatay-merkez',
        libraryName: 'Hatay İl Halk Kütüphanesi',
        city: 'Hatay',
        district: 'Antakya',
        address: 'Ulus Meydanı No:8, Antakya',
        sections: ['Genel Koleksiyon', 'Gençlik Bölümü'],
        isReservable: true,
        totalCopies: 5,
        availableCopies: 3,
        notes: 'Bir kopya ödünçte, iade tarihi 12.05.2025.'
      },
      {
        libraryId: 'hatay-defne',
        libraryName: 'Defne İlçe Halk Kütüphanesi',
        city: 'Hatay',
        district: 'Defne',
        address: 'Harbiye Cad. No:20, Defne',
        sections: ['Roman', 'Çocuk Rafı'],
        isReservable: false,
        totalCopies: 2,
        availableCopies: 1,
        notes: 'Çocuk bölümünde yerinde okuma önerilir.'
      },
      {
        libraryId: 'ankara-merkez',
        libraryName: 'MEB Merkez Kütüphanesi',
        city: 'Ankara',
        district: 'Çankaya',
        address: 'Atatürk Bulvarı No:14, Çankaya',
        sections: ['Çeviri Roman'],
        isReservable: true,
        totalCopies: 4,
        availableCopies: 4,
        notes: null
      }
    ]
  },
  'mock-4': {
    id: 'mock-4',
    title: 'STEM Eğitimi',
    subtitle: 'Uygulamalı sınıf içi etkinlikler',
    authors: ['Milli Eğitim Bakanlığı'],
    materialType: 'Kitap • Eğitim',
    materialCategory: 'Kitap',
    language: 'Türkçe',
    publicationYear: 2023,
    publisher: 'MEB Yayınları',
    publicationPlace: 'Ankara',
    isbn: '9789751100000',
    summary:
      'Fen, teknoloji, mühendislik ve matematik disiplinlerini bütünleştiren 30 uygulamalı etkinlik sunar.',
    notes: 'EBA platformu ile entegre edilmiştir.',
    deweyCode: '507',
    deweyTitle: 'Eğitim Araştırmaları',
    updatedAt: new Date().toISOString(),
    digitalResources: [
      {
        format: 'PDF',
        accessInformation: 'https://cdn.meb.gov.tr/dijital-kutuphanem/stem-egitimi.pdf',
        language: 'Türkçe',
        description: 'Öğretmen kılavuzu.'
      },
      {
        format: 'Video',
        accessInformation: 'https://youtu.be/meb-stem-uygulama',
        language: 'Türkçe',
        description: 'Etkinliklerin sınıf içi uygulama kayıtları.'
      }
    ],
    availability: [
      {
        libraryId: 'ankara-merkez',
        libraryName: 'MEB Merkez Kütüphanesi',
        city: 'Ankara',
        district: 'Çankaya',
        address: 'Atatürk Bulvarı No:14, Çankaya',
        sections: ['Eğitim', 'Öğretmen Kaynakları'],
        isReservable: true,
        totalCopies: 12,
        availableCopies: 9,
        notes: null
      },
      {
        libraryId: 'izmir-bornova',
        libraryName: 'Bornova Öğretmen Akademisi Kütüphanesi',
        city: 'İzmir',
        district: 'Bornova',
        address: 'Kazım Dirik Mah. No:22',
        sections: ['Eğitim Teknolojileri'],
        isReservable: true,
        totalCopies: 6,
        availableCopies: 4,
        notes: null
      }
    ]
  },
  'mock-5': {
    id: 'mock-5',
    title: 'Bilim Dergisi - Özel Sayı',
    subtitle: 'Sürdürülebilir enerji dosyası',
    authors: ['TÜBİTAK'],
    materialType: 'Süreli Yayın',
    materialCategory: 'Süreli',
    language: 'Türkçe',
    publicationYear: 2024,
    publisher: 'TÜBİTAK Yayınları',
    publicationPlace: 'Ankara',
    summary:
      'Yenilenebilir enerji teknolojileri, yeşil hidrojen ve enerji depolama çözümlerine odaklanan özel sayı.',
    deweyCode: '621.044',
    deweyTitle: 'Yenilenebilir Enerji',
    updatedAt: new Date().toISOString(),
    digitalResources: [
      {
        format: 'PDF',
        accessInformation:
          'https://cdn.meb.gov.tr/dijital-kutuphanem/bilim-dergisi-enerji-ozel.pdf',
        language: 'Türkçe',
        description: 'Sadece yerinde erişim için hazırlanmış tarama.'
      }
    ],
    availability: [
      {
        libraryId: 'izmir-merkez',
        libraryName: 'İzmir Bölge Kütüphanesi',
        city: 'İzmir',
        district: 'Konak',
        address: 'Cumhuriyet Bulvarı No:48',
        sections: ['Süreli Yayınlar'],
        isReservable: false,
        totalCopies: 3,
        availableCopies: 0,
        notes: 'Sadece kütüphane içinde kullanılabilir.'
      }
    ]
  },
  'mock-6': {
    id: 'mock-6',
    title: 'Digital Literacy for Educators',
    authors: ['UNESCO'],
    materialType: 'Rapor',
    materialCategory: 'Rapor',
    language: 'İngilizce',
    publicationYear: 2021,
    publisher: 'UNESCO Publishing',
    publicationPlace: 'Paris',
    summary:
      'Öğretmenlerin dijital yetkinliklerini geliştirmeye yönelik politika önerileri ve değerlendirme araçları.',
    notes: 'Türkçe özet bölümü, sayfa 78.',
    deweyCode: '371.33',
    deweyTitle: 'Eğitim Teknolojileri',
    updatedAt: new Date().toISOString(),
    digitalResources: [
      {
        format: 'PDF',
        accessInformation: 'https://unesdoc.unesco.org/digital-literacy-educators.pdf',
        language: 'İngilizce',
        description: 'UNESCO açık erişim belgesi.'
      }
    ],
    availability: [
      {
        libraryId: 'ankara-merkez',
        libraryName: 'MEB Merkez Kütüphanesi',
        city: 'Ankara',
        district: 'Çankaya',
        address: 'Atatürk Bulvarı No:14, Çankaya',
        sections: ['Rapor', 'Eğitim Teknolojileri'],
        isReservable: true,
        totalCopies: 4,
        availableCopies: 4,
        notes: null
      },
      {
        libraryId: 'istanbul-bakirkoy',
        libraryName: 'Bakırköy Öğretmen Akademisi',
        city: 'İstanbul',
        district: 'Bakırköy',
        address: 'Ataköy 7-8-9-10 Mah. Eğitim Kampüsü',
        sections: ['Öğretmen Gelişim'],
        isReservable: true,
        totalCopies: 2,
        availableCopies: 2,
        notes: null
      }
    ]
  }
}

const currentRouteId = computed(() => {
  const raw = route.params.id
  if (Array.isArray(raw)) {
    return raw[0] ?? ''
  }
  return typeof raw === 'string' ? raw : ''
})

const authorList = computed(() => {
  const authors = detail.value?.authors ?? []
  return authors.length ? authors.join(', ') : ''
})

const formattedUpdatedAt = computed(() => {
  const value = detail.value?.updatedAt
  if (!value) {
    return 'Güncelleme bilgisi yok'
  }

  const parsed = new Date(value)
  if (Number.isNaN(parsed.getTime())) {
    return value
  }

  return new Intl.DateTimeFormat('tr-TR', { dateStyle: 'medium' }).format(parsed)
})

const cityOptions = computed(() => {
  const entries = detail.value?.availability ?? []
  const cities = new Set<string>()

  entries.forEach(entry => {
    if (entry.city) {
      cities.add(entry.city)
    }
  })

  return Array.from(cities).sort((a, b) => a.localeCompare(b, 'tr'))
})

const districtOptions = computed(() => {
  const entries = detail.value?.availability ?? []
  const districts = new Set<string>()

  entries.forEach(entry => {
    if (filters.city !== 'all' && entry.city !== filters.city) {
      return
    }
    if (entry.district) {
      districts.add(entry.district)
    }
  })

  return Array.from(districts).sort((a, b) => a.localeCompare(b, 'tr'))
})

const filteredAvailability = computed(() => {
  const entries = detail.value?.availability ?? []

  return entries.filter(entry => {
    if (filters.onlyAvailable && entry.availableCopies <= 0) {
      return false
    }

    if (filters.city !== 'all' && entry.city !== filters.city) {
      return false
    }

    if (filters.district !== 'all' && entry.district !== filters.district) {
      return false
    }

    return true
  })
})

const availabilitySummary = computed(() => {
  const entries = detail.value?.availability ?? []
  if (!entries.length) {
    return null
  }

  return {
    totalLibraries: entries.length,
    totalCopies: entries.reduce((sum, entry) => sum + entry.totalCopies, 0),
    totalAvailable: entries.reduce((sum, entry) => sum + entry.availableCopies, 0)
  }
})

const marcRows = computed(() =>
  detail.value?.marc21 ? parseMarc21(detail.value.marc21) : []
)

const hasMarcRows = computed(() => marcRows.value.length > 0)

const setActiveTab = (tab: 'overview' | 'marc') => {
  activeDetailTab.value = tab
}

const availabilityLabel = (status: AvailabilityState) => {
  switch (status) {
    case 'available':
      return 'Ödünç verilebilir'
    case 'limited':
      return 'Sınırlı erişim'
    default:
      return 'Sadece yerinde kullanım'
  }
}

const mapDigitalResource = (resource: ServerDigitalResource, index: number): DigitalResource => {
  const format = resource.format?.trim() || 'Dijital kaynak'
  const access = resource.accessInformation?.trim() || 'Erişim bilgisi paylaşılmadı'
  const language = resource.language?.trim() || null
  const description = resource.description?.trim() || null

  let isLink = false
  let hostname: string | null = null

  try {
    const url = new URL(access)
    isLink = Boolean(url.protocol.startsWith('http'))
    hostname = url.hostname.replace(/^www\./, '')
  } catch {
    isLink = false
    hostname = null
  }

  return {
    key: `${format}-${index}`,
    format,
    accessInformation: access,
    language,
    description,
    isLink,
    hostname
  }
}

const mapAvailability = (entry: ServerAvailability, index: number): AvailabilityEntry => {
  const totalCopies = entry.totalCopies ?? 0
  const availableCopies = entry.availableCopies ?? 0
  const status: AvailabilityState =
    availableCopies > 0 ? 'available' : totalCopies > 0 ? 'limited' : 'unavailable'

  return {
    libraryId: entry.libraryId?.trim() || `library-${index}`,
    libraryName: entry.libraryName?.trim() || 'Belirtilmemiş kütüphane',
    city: entry.city?.trim() || null,
    district: entry.district?.trim() || null,
    address: entry.address?.trim() || null,
    sections: (entry.sections ?? []).map(section => section.trim()).filter(Boolean),
    isReservable: Boolean(entry.isReservable),
    totalCopies,
    availableCopies,
    notes: entry.notes?.trim() || null,
    status
  }
}

const mapDetail = (payload: ServerCatalogDetail): CatalogDetail => {
  const availability = (payload.availability ?? []).map(mapAvailability)

  return {
    id: payload.id ?? '',
    title: payload.title?.trim() || 'Başlıksız kayıt',
    subtitle: payload.subtitle?.trim() || null,
    authors: (payload.authors ?? []).map(author => author.trim()).filter(Boolean),
    materialType:
      payload.materialType?.trim() || payload.materialCategory?.trim() || 'Materyal',
    materialCategory: payload.materialCategory?.trim() || 'Materyal',
    language: payload.language?.trim() || null,
    publicationYear: payload.publicationYear ?? null,
    publisher: payload.publisher?.trim() || null,
    publicationPlace: payload.publicationPlace?.trim() || null,
    isbn: payload.isbn?.trim() || null,
    summary: payload.summary?.trim() || null,
    notes: payload.notes?.trim() || null,
    deweyCode: payload.deweyCode?.trim() || null,
    deweyTitle: payload.deweyTitle?.trim() || null,
    marc21: payload.marc21?.trim() || null,
    updatedAt: payload.updatedAt ? new Date(payload.updatedAt).toISOString() : new Date().toISOString(),
    digitalResources: (payload.digitalResources ?? []).map(mapDigitalResource),
    availability
  }
}

function parseMarc21(raw: string): MarcTableRow[] {
  if (!raw) {
    return []
  }

  const trimmed = raw.trim()
  if (!trimmed) {
    return []
  }

  const jsonRows = tryParseMarcJson(trimmed)
  if (jsonRows.length > 0) {
    return jsonRows
  }

  return parseMarcText(trimmed)
}

function tryParseMarcJson(raw: string): MarcTableRow[] {
  try {
    const parsed = JSON.parse(raw)
    return flattenMarcJson(parsed)
  } catch {
    return []
  }
}

function flattenMarcJson(data: unknown): MarcTableRow[] {
  const rows: MarcTableRow[] = []

  const processTag = (tagKey: string, value: unknown) => {
    const tag = tagKey.toUpperCase()

    if (Array.isArray(value)) {
      value.forEach(item => processTag(tag, item))
      return
    }

    if (value === null || value === undefined) {
      return
    }

    if (typeof value !== 'object') {
      const text = normaliseMarcValue(value)
      if (text) {
        rows.push({
          tag,
          ind1: isControlField(tag) ? '' : ' ',
          ind2: isControlField(tag) ? '' : ' ',
          subfield: '—',
          value: text
        })
      }
      return
    }

    const entry = value as Record<string, unknown>
    const usedKeys = new Set<string>()
    let ind1 = ' '
    let ind2 = ' '

    if (!isControlField(tag)) {
      ind1 = pickIndicator(entry, ['ind1', 'indicator1', 'i1', '1', 'indicator_1'], usedKeys)
      ind2 = pickIndicator(entry, ['ind2', 'indicator2', 'i2', '2', 'indicator_2'], usedKeys)
    } else {
      ind1 = ''
      ind2 = ''
    }

    usedKeys.add('subfields')
    let pushed = false

    const pushRow = (code: string, input: unknown) => {
      const text = normaliseMarcValue(input)
      if (!text) {
        return
      }

      rows.push({
        tag,
        ind1,
        ind2,
        subfield: code ? code.trim() : '—',
        value: text
      })
      pushed = true
    }

    const subfields = entry.subfields
    if (Array.isArray(subfields)) {
      subfields.forEach(item => {
        if (typeof item === 'object' && item !== null) {
          Object.entries(item as Record<string, unknown>).forEach(([code, val]) => pushRow(code, val))
        }
      })
    } else if (subfields && typeof subfields === 'object') {
      Object.entries(subfields as Record<string, unknown>).forEach(([code, val]) => pushRow(code, val))
    }

    Object.entries(entry).forEach(([key, val]) => {
      if (usedKeys.has(key)) {
        return
      }

      if (Array.isArray(val)) {
        val.forEach(item => pushRow(key, item))
        return
      }

      if (typeof val === 'object' && val !== null) {
        const nested = val as Record<string, unknown>
        if ('value' in nested) {
          pushRow(key, nested.value)
        } else if ('subfields' in nested) {
          pushed = true
          const nestedSubfields = nested.subfields
          if (Array.isArray(nestedSubfields)) {
            nestedSubfields.forEach(item => {
              if (typeof item === 'object' && item !== null) {
                Object.entries(item as Record<string, unknown>).forEach(([code, fieldValue]) =>
                  pushRow(code, fieldValue)
                )
              }
            })
          } else if (nestedSubfields && typeof nestedSubfields === 'object') {
            Object.entries(nestedSubfields as Record<string, unknown>).forEach(([code, fieldValue]) =>
              pushRow(code, fieldValue)
            )
          }
        } else {
          pushRow(key, nested)
        }
        return
      }

      pushRow(key, val)
    })

    if (!pushed) {
      const fallback = normaliseMarcValue(entry)
      if (fallback) {
        rows.push({
          tag,
          ind1,
          ind2,
          subfield: '—',
          value: fallback
        })
      }
    }
  }

  if (Array.isArray(data)) {
    data.forEach((entry, index) => processTag(String(index), entry))
  } else if (data && typeof data === 'object') {
    Object.entries(data as Record<string, unknown>).forEach(([tag, value]) => processTag(tag, value))
  }

  return rows
}

function parseMarcText(raw: string): MarcTableRow[] {
  const rows: MarcTableRow[] = []
  const lines = raw.replace(/\r\n/g, '\n').split('\n')

  lines.forEach(line => {
    const trimmed = line.trim()
    if (!trimmed) {
      return
    }

    let working = trimmed
    let tag = ''

    if (working.startsWith('=')) {
      tag = working.substring(1, 4).trim().toUpperCase()
      working = working.substring(4)
    } else {
      tag = working.substring(0, 3).trim().toUpperCase()
      working = working.substring(3)
    }

    if (!tag) {
      return
    }

    working = working.replace(/^\s+/, '')
    const control = isControlField(tag)
    let ind1 = control ? '' : ' '
    let ind2 = control ? '' : ' '

    if (!control && working.length >= 2) {
      ind1 = normaliseIndicatorCharacter(working.charAt(0))
      ind2 = normaliseIndicatorCharacter(working.charAt(1))
      working = working.substring(2)
    }

    working = working.trim()
    if (!working) {
      rows.push({
        tag,
        ind1,
        ind2,
        subfield: '—',
        value: ''
      })
      return
    }

    const delimiter = detectSubfieldDelimiter(working)
    if (delimiter) {
      const parts = working.split(delimiter).filter(Boolean)

      if (parts.length === 0) {
        rows.push({
          tag,
          ind1,
          ind2,
          subfield: '—',
          value: working
        })
        return
      }

      parts.forEach(part => {
        const cleaned = part.trim()
        if (!cleaned) {
          return
        }
        const code = cleaned[0]
        const value = cleaned.substring(1).trim()
        rows.push({
          tag,
          ind1,
          ind2,
          subfield: code ? code : '—',
          value
        })
      })
    } else {
      rows.push({
        tag,
        ind1,
        ind2,
        subfield: '—',
        value: working
      })
    }
  })

  return rows
}

function detectSubfieldDelimiter(input: string): string | null {
  const delimiters = ['\u001f', '‡', 'ǂ', '$', '|']
  return delimiters.find(delimiter => input.includes(delimiter)) ?? null
}

function normaliseMarcValue(value: unknown): string {
  if (value === null || value === undefined) {
    return ''
  }

  if (typeof value === 'string') {
    return value.trim()
  }

  if (typeof value === 'number' || typeof value === 'boolean') {
    return String(value)
  }

  if (Array.isArray(value)) {
    return value
      .map(item => normaliseMarcValue(item))
      .filter(Boolean)
      .join(' ; ')
  }

  if (typeof value === 'object') {
    return Object.entries(value as Record<string, unknown>)
      .map(([key, val]) => {
        const resolved = normaliseMarcValue(val)
        if (!resolved) {
          return ''
        }
        return key ? `${key}: ${resolved}` : resolved
      })
      .filter(Boolean)
      .join(' • ')
  }

  return ''
}

function pickIndicator(
  entry: Record<string, unknown>,
  keys: string[],
  usedKeys: Set<string>
): string {
  for (const key of keys) {
    if (Object.prototype.hasOwnProperty.call(entry, key)) {
      usedKeys.add(key)
      return normaliseIndicatorValue(entry[key])
    }
  }

  return ' '
}

function normaliseIndicatorCharacter(char: string): string {
  if (!char) {
    return ' '
  }

  const trimmed = char.trim()
  if (
    !trimmed ||
    trimmed === '\\' ||
    trimmed === '#' ||
    trimmed === '_' ||
    trimmed === '|' ||
    trimmed === '0'
  ) {
    return trimmed === '0' ? '0' : ' '
  }

  return trimmed
}

function normaliseIndicatorValue(value: unknown): string {
  if (value === null || value === undefined) {
    return ' '
  }

  if (typeof value === 'number') {
    return value === 0 ? '0' : String(value)
  }

  if (typeof value === 'string') {
    const trimmed = value.trim()
    if (!trimmed || trimmed === '\\' || trimmed === '#' || trimmed === '_' || trimmed === '|') {
      return ' '
    }
    return trimmed.charAt(0)
  }

  return ' '
}

function isControlField(tag: string): boolean {
  return tag === 'LDR' || /^00/.test(tag)
}

const fetchDetail = async (id: string) => {
  if (!id) {
    state.error = 'Geçerli bir kayıt seçilmedi.'
    state.detail = null
    state.loading = false
    return
  }

  const requestId = id
  lastRequestedId.value = requestId

  state.loading = true
  state.error = ''

  const applyDetailResult = (catalogDetail: CatalogDetail) => {
    state.detail = catalogDetail
    filters.city = 'all'
    filters.district = 'all'
    filters.onlyAvailable = false

    if (typeof document !== 'undefined' && catalogDetail.title) {
      document.title = `${catalogDetail.title} • MEB Dijital Kütüphane`
    }
  }

  const resolveMockDetail = (mockId: string): CatalogDetail | null => {
    const payload = mockDetailData[mockId]
    if (!payload) {
      return null
    }

    return mapDetail({
      ...payload,
      updatedAt: payload.updatedAt ?? new Date().toISOString()
    })
  }

  if (id.startsWith('mock-')) {
    const mockDetail = resolveMockDetail(id)
    if (mockDetail) {
      applyDetailResult(mockDetail)
      state.error = ''
    } else {
      state.detail = null
      state.error = 'Bu demo kaydının detay bilgisi henüz hazırlanmadı.'
    }
    state.loading = false
    return
  }

  try {
    const { data } = await apiClient.get<ServerCatalogDetail>(`/public-catalog/${encodeURIComponent(id)}`)

    if (lastRequestedId.value !== requestId) {
      return
    }

    if (!data || !data.id) {
      throw new Error('Kayıt bulunamadı.')
    }

    const mappedDetail = mapDetail(data)
    applyDetailResult(mappedDetail)
  } catch (error) {
    if (lastRequestedId.value !== requestId) {
      return
    }

    state.detail = null
    const fallback =
      isAxiosError(error) && typeof error?.message === 'string'
        ? error.message
        : error instanceof Error
        ? error.message
        : 'Kayıt detayları alınırken beklenmeyen bir hata oluştu.'
    state.error = fallback
  } finally {
    if (lastRequestedId.value === requestId) {
      state.loading = false
    }
  }
}

const goBack = () => {
  if (typeof window !== 'undefined' && window.history.length > 1) {
    router.back()
  } else {
    router.push({ name: 'public-catalog' })
  }
}

const retry = () => {
  fetchDetail(currentRouteId.value)
}

watch(
  () => filters.city,
  () => {
    filters.district = 'all'
  }
)

watch(
  detail,
  () => {
    activeDetailTab.value = 'overview'
  }
)

watch(
  hasMarcRows,
  value => {
    if (!value) {
      activeDetailTab.value = 'overview'
    }
  }
)

watch(
  currentRouteId,
  id => {
    fetchDetail(id)
  },
  { immediate: true }
)
</script>

<style scoped>
.catalog-detail {
  max-width: 960px;
  margin: 0 auto;
  padding: 2rem 1.5rem 4rem;
  display: grid;
  gap: 1.5rem;
}

.detail-nav {
  display: flex;
  justify-content: flex-start;
}

.back-button {
  background: none;
  border: none;
  color: #dc2626;
  font-weight: 600;
  cursor: pointer;
  padding: 0;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.95rem;
}

.detail-state {
  background: #fff;
  border-radius: 16px;
  padding: 3rem 2rem;
  border: 1px solid rgba(248, 113, 113, 0.25);
  text-align: center;
  display: grid;
  gap: 1rem;
  justify-items: center;
}

.detail-state--error h2 {
  color: #b91c1c;
  margin: 0;
}

.detail-spinner {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  border: 4px solid rgba(220, 38, 38, 0.2);
  border-top-color: #dc2626;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.detail-actions {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
  justify-content: center;
}

.detail-actions .primary {
  background: linear-gradient(135deg, #dc2626, #f97316);
  color: #fff;
  border: none;
  border-radius: 10px;
  padding: 0.7rem 1.4rem;
  font-weight: 600;
  cursor: pointer;
}

.detail-actions .link {
  background: none;
  border: none;
  color: #7f1d1d;
  cursor: pointer;
  text-decoration: underline;
}

.detail-content {
  background: #fff;
  border-radius: 18px;
  border: 1px solid rgba(226, 232, 240, 0.6);
  padding: 2rem 2.5rem;
  display: grid;
  gap: 2rem;
  box-shadow: 0 24px 40px rgba(15, 23, 42, 0.08);
}

.detail-hero {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  align-items: flex-end;
  justify-content: space-between;
}

.detail-hero__text h1 {
  font-size: 2.1rem;
  line-height: 1.25;
  margin: 0.35rem 0;
  color: #111827;
}

.detail-chip {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.35rem 0.75rem;
  border-radius: 999px;
  background: rgba(248, 113, 113, 0.12);
  color: #b91c1c;
  font-weight: 600;
  font-size: 0.85rem;
}

.detail-subtitle {
  margin: 0;
  font-size: 1rem;
  color: rgba(55, 65, 81, 0.85);
}

.detail-authors {
  margin: 0.75rem 0 0;
  color: rgba(30, 64, 175, 0.85);
  font-weight: 500;
}

.detail-meta {
  list-style: none;
  margin: 0;
  padding: 0;
  display: grid;
  gap: 0.75rem;
  min-width: 220px;
}

.detail-meta__label {
  display: block;
  font-size: 0.75rem;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: rgba(107, 114, 128, 0.9);
}

.detail-meta__value {
  font-weight: 600;
  color: #111827;
}

.detail-tabs {
  position: relative;
  display: inline-flex;
  padding: 0.4rem;
  border-radius: 999px;
  background: linear-gradient(135deg, rgba(254, 226, 226, 0.92), rgba(254, 215, 170, 0.92));
  box-shadow: 0 8px 24px rgba(220, 38, 38, 0.12);
  gap: 0.25rem;
}

.detail-tab {
  position: relative;
  border: none;
  background: transparent;
  padding: 0.6rem 1.35rem;
  border-radius: 999px;
  font-weight: 600;
  font-size: 0.92rem;
  color: rgba(185, 28, 28, 0.9);
  cursor: pointer;
  transition: color 0.2s ease, transform 0.2s ease;
  z-index: 1;
}

.detail-tab:hover {
  color: #b91c1c;
  transform: translateY(-1px);
}

.detail-tab.active {
  color: #fff;
}

.detail-tab.active::before {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: 999px;
  background: linear-gradient(135deg, #dc2626, #f97316);
  box-shadow: 0 18px 28px rgba(220, 38, 38, 0.28);
  z-index: -1;
}

.detail-tab.disabled {
  color: rgba(220, 38, 38, 0.45);
  cursor: default;
}

.detail-tab.disabled:hover {
  transform: none;
  color: rgba(220, 38, 38, 0.45);
}

.detail-tab.link {
  align-self: center;
  background: none;
  color: #2563eb;
  padding: 0.4rem 0.2rem;
  box-shadow: none;
}

.detail-tab.link::before {
  display: none;
}

.detail-tab.link:hover {
  text-decoration: underline;
  background: none;
  transform: none;
}

.detail-overview {
  display: grid;
  gap: 1.75rem;
}

.detail-marc {
  display: grid;
  gap: 1.5rem;
}

.marc-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.marc-header h2 {
  margin: 0;
  color: #0f172a;
}

.marc-header p {
  margin: 0;
  color: rgba(71, 85, 105, 0.9);
  font-size: 0.9rem;
}

.marc-table-wrapper {
  overflow-x: auto;
  border-radius: 14px;
  border: 1px solid rgba(226, 232, 240, 0.8);
  background: rgba(248, 250, 252, 0.9);
}

.marc-table {
  width: 100%;
  border-collapse: collapse;
  min-width: 640px;
}

.marc-table th,
.marc-table td {
  padding: 0.65rem 0.9rem;
  text-align: left;
  border-bottom: 1px solid rgba(226, 232, 240, 0.7);
  font-size: 0.92rem;
}

.marc-table thead {
  background: rgba(99, 102, 241, 0.12);
}

.marc-table tbody tr:nth-child(even) {
  background: rgba(248, 250, 252, 0.7);
}

.marc-table th {
  font-weight: 700;
  color: #1f2937;
  white-space: nowrap;
}

.marc-table td:first-of-type,
.marc-table th:first-of-type {
  font-weight: 600;
  color: #111827;
}

.marc-value {
  font-family: 'Fira Code', 'Courier New', monospace;
  font-size: 0.9rem;
  color: #0f172a;
}

.detail-section {
  display: grid;
  gap: 0.75rem;
}

.detail-section h2,
.detail-section h3 {
  margin: 0;
  color: #0f172a;
}

.detail-section--grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
  gap: 1.5rem;
}

.detail-definition {
  margin: 0;
  padding: 0;
  display: grid;
  gap: 0.6rem;
}

.detail-definition div {
  display: grid;
  gap: 0.2rem;
}

.detail-definition dt {
  font-size: 0.8rem;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  color: rgba(107, 114, 128, 0.9);
}

.detail-definition dd {
  margin: 0;
  font-weight: 600;
  color: #1f2937;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
  gap: 1rem;
}

.section-header small {
  color: rgba(79, 70, 229, 0.75);
  font-weight: 600;
}

.section-empty {
  background: rgba(248, 250, 252, 0.8);
  border-radius: 12px;
  padding: 1rem 1.25rem;
  color: rgba(71, 85, 105, 0.85);
}

.digital-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: grid;
  gap: 1rem;
}

.digital-card {
  border: 1px solid rgba(226, 232, 240, 0.8);
  border-radius: 14px;
  padding: 1rem 1.25rem;
  background: rgba(249, 250, 251, 0.8);
  display: grid;
  gap: 0.5rem;
}

.digital-card__header {
  display: flex;
  align-items: baseline;
  gap: 0.5rem;
  font-size: 1rem;
}

.digital-card__meta {
  color: rgba(30, 64, 175, 0.8);
  font-weight: 500;
}

.digital-card__access a {
  color: #2563eb;
  text-decoration: none;
  font-weight: 600;
}

.digital-card__access a:hover {
  text-decoration: underline;
}

.availability-summary {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
  color: rgba(55, 65, 81, 0.8);
  font-size: 0.9rem;
}

.availability-filters {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  align-items: flex-end;
}

.availability-filters label {
  display: grid;
  gap: 0.3rem;
  font-size: 0.85rem;
  color: rgba(55, 65, 81, 0.85);
}

.availability-filters select {
  min-width: 160px;
  border-radius: 10px;
  border: 1px solid rgba(209, 213, 219, 0.9);
  padding: 0.5rem 0.75rem;
}

.availability-toggle {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-weight: 500;
  color: rgba(30, 64, 175, 0.9);
}

.availability-list {
  list-style: none;
  margin: 0;
  padding: 0;
  display: grid;
  gap: 1rem;
}

.availability-card {
  border-radius: 16px;
  border: 1px solid rgba(226, 232, 240, 0.9);
  padding: 1.25rem 1.5rem;
  background: #fff;
  display: grid;
  gap: 0.6rem;
  box-shadow: 0 18px 28px rgba(15, 23, 42, 0.06);
}

.availability-card[data-status='available'] {
  border-color: rgba(52, 211, 153, 0.4);
}

.availability-card[data-status='limited'] {
  border-color: rgba(251, 191, 36, 0.4);
}

.availability-card[data-status='unavailable'] {
  border-color: rgba(248, 113, 113, 0.4);
}

.availability-card__header {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
  gap: 1rem;
}

.availability-card__header h3 {
  margin: 0;
  font-size: 1.1rem;
  color: #111827;
}

.availability-chip {
  border-radius: 999px;
  padding: 0.3rem 0.7rem;
  font-size: 0.78rem;
  font-weight: 600;
  background: rgba(37, 99, 235, 0.1);
  color: #1d4ed8;
}

.availability-card[data-status='available'] .availability-chip {
  background: rgba(16, 185, 129, 0.18);
  color: #047857;
}

.availability-card[data-status='limited'] .availability-chip {
  background: rgba(251, 191, 36, 0.18);
  color: #b45309;
}

.availability-card[data-status='unavailable'] .availability-chip {
  background: rgba(248, 113, 113, 0.18);
  color: #b91c1c;
}

.availability-location {
  margin: 0;
  font-weight: 500;
  color: rgba(30, 64, 175, 0.85);
}

.availability-address {
  margin: 0;
  color: rgba(55, 65, 81, 0.85);
}

.availability-counts {
  margin: 0;
  color: rgba(55, 65, 81, 0.9);
}

.availability-sections,
.availability-reservable,
.availability-notes {
  margin: 0;
  color: rgba(75, 85, 99, 0.9);
}

.availability-reservable {
  font-weight: 600;
  color: #047857;
}

.availability-notes {
  font-style: italic;
}

details {
  border-radius: 12px;
  border: 1px solid rgba(226, 232, 240, 0.9);
  background: rgba(248, 250, 252, 0.8);
  overflow: hidden;
}

details summary {
  padding: 0.85rem 1.1rem;
  cursor: pointer;
  font-weight: 600;
  color: #1d4ed8;
}

.marc-block {
  margin: 0;
  padding: 1.25rem 1.5rem;
  font-family: 'Fira Code', 'Courier New', monospace;
  font-size: 0.9rem;
  line-height: 1.5;
  white-space: pre-wrap;
  color: #0f172a;
}

@media (max-width: 768px) {
  .catalog-detail {
    padding: 1.5rem 1.25rem 3rem;
  }

  .detail-content {
    padding: 1.5rem 1.75rem;
  }

  .detail-hero {
    flex-direction: column;
    align-items: flex-start;
  }

  .detail-meta {
    width: 100%;
    grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
  }

  .detail-tabs {
    width: 100%;
    justify-content: space-between;
    flex-wrap: wrap;
  }

  .detail-tab {
    flex: 1 1 45%;
    text-align: center;
  }

  .detail-tab.link {
    flex: 0 0 auto;
    width: auto;
    text-align: left;
  }

  .marc-header {
    flex-direction: column;
    align-items: flex-start;
  }

  .availability-filters select {
    min-width: 140px;
  }
}
</style>
