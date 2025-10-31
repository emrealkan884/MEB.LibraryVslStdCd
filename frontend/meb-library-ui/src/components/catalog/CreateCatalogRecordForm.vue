<template>
  <div class="catalog-form">
    <section v-if="lookupState.error" class="catalog-form__alert catalog-form__alert--error">
      <strong>Veriler yüklenemedi.</strong>
      <span>{{ lookupState.error }}</span>
      <button type="button" @click="loadLookups" class="catalog-form__retry">Tekrar dene</button>
    </section>

    <section v-if="saveState.successMessage" class="catalog-form__alert catalog-form__alert--success">
      <strong>{{ saveState.successMessage }}</strong>
      <span v-if="saveState.createdId">Oluşturulan kayıt ID: {{ saveState.createdId }}</span>
    </section>

    <section v-if="saveState.errorMessage" class="catalog-form__alert catalog-form__alert--error">
      <strong>Kaydedilirken bir hata oluştu.</strong>
      <span>{{ saveState.errorMessage }}</span>
    </section>

    <form class="catalog-form__grid" @submit.prevent="handleSubmit">
      <fieldset class="catalog-card">
        <legend>Temel Bilgiler</legend>
        <div class="catalog-card__grid">
          <label class="catalog-field">
            <span>Bağlı Kütüphane *</span>
            <select v-model="form.kutuphaneId" required :disabled="lookupState.loading">
              <option value="" disabled>Kütüphane seçin</option>
              <option v-for="library in lookupState.libraries" :key="library.id" :value="library.id">
                {{ library.label }}
              </option>
            </select>
          </label>
          <label class="catalog-field catalog-field--half">
            <span>Başlık *</span>
            <input v-model="form.baslik" type="text" required placeholder="Başlık" />
          </label>
          <label class="catalog-field catalog-field--half">
            <span>Alt Başlık</span>
            <input v-model="form.altBaslik" type="text" placeholder="Alt başlık" />
          </label>
          <label class="catalog-field catalog-field--half">
            <span>Yeni Katalog Talebi ID</span>
            <input v-model="form.yeniKatalogTalebiId" type="text" placeholder="Opsiyonel talep ID" />
          </label>
          <label class="catalog-field">
            <span>Materyal Türü *</span>
            <select v-model.number="form.materyalTuru" required>
              <option value="" disabled>Tür seçin</option>
              <option v-for="option in materyalTuruOptions" :key="option.value" :value="option.value">
                {{ option.label }}
              </option>
            </select>
          </label>
          <label class="catalog-field">
            <span>Materyal Alt Türü</span>
            <input v-model="form.materyalAltTuru" type="text" placeholder="Alt tür" />
          </label>
          <label class="catalog-field">
            <span>RDA Uyumlu</span>
            <div class="catalog-inline">
              <input id="rda-checkbox" v-model="form.rdaUyumlu" type="checkbox" />
              <label for="rda-checkbox">RDA kurallarına uygun</label>
            </div>
          </label>
          <label class="catalog-field">
            <span>Kayıt Tarihi *</span>
            <input v-model="form.kayitTarihi" type="datetime-local" required />
          </label>
        </div>
      </fieldset>

      <fieldset class="catalog-card">
        <legend>Yayın Bilgileri</legend>
        <div class="catalog-card__grid">
          <label class="catalog-field">
            <span>ISBN</span>
            <input v-model="form.isbn" type="text" placeholder="978..." />
          </label>
          <label class="catalog-field">
            <span>Yayıncı</span>
            <input v-model="form.yayinevi" type="text" />
          </label>
          <label class="catalog-field">
            <span>Yayın Yeri</span>
            <input v-model="form.yayinYeri" type="text" />
          </label>
          <label class="catalog-field">
            <span>Yayın Yılı</span>
            <input v-model="form.yayinYili" type="number" min="1700" max="2100" />
          </label>
          <label class="catalog-field">
            <span>Sayfa Sayısı</span>
            <input v-model="form.sayfaSayisi" type="number" min="1" />
          </label>
          <label class="catalog-field">
            <span>Dil</span>
            <input v-model="form.dil" type="text" placeholder="Örn. Türkçe" />
          </label>
          <label class="catalog-field">
            <span>Dizi</span>
            <input v-model="form.dizi" type="text" />
          </label>
          <label class="catalog-field">
            <span>Baskı</span>
            <input v-model="form.baski" type="text" />
          </label>
        </div>
      </fieldset>

      <fieldset class="catalog-card">
        <legend>Sınıflama ve İçerik</legend>
        <div class="catalog-card__grid">
          <label class="catalog-field">
            <span>Dewey Sınıflaması</span>
            <select v-model="form.deweySiniflamaId" :disabled="lookupState.loading">
              <option value="">Seçim yapılmadı</option>
              <option v-for="item in lookupState.deweys" :key="item.id" :value="item.id">
                {{ item.label }}
              </option>
            </select>
          </label>
          <label class="catalog-field catalog-field--wide">
            <span>Özet</span>
            <textarea v-model="form.ozet" rows="3" placeholder="Kaydın kısa özeti"></textarea>
          </label>
          <label class="catalog-field catalog-field--wide">
            <span>Notlar</span>
            <textarea v-model="form.notlar" rows="3" placeholder="Ek açıklamalar"></textarea>
          </label>
          <label class="catalog-field catalog-field--wide">
            <span>Kapak Görseli URL</span>
            <input v-model="form.kapakResmiYolu" type="text" placeholder="https://..." />
          </label>
        </div>
      </fieldset>

      <fieldset class="catalog-card">
        <legend>Yazarlar</legend>
        <div class="catalog-collection">
          <header class="catalog-collection__header">
            <p>Kayıta bağlı yazar ve rollerini ekleyin.</p>
            <button type="button" class="catalog-button catalog-button--ghost" @click="addAuthor">Yazar ekle</button>
          </header>
          <p v-if="form.yazarlar.length === 0" class="catalog-empty">Henüz yazar eklenmedi.</p>
          <div v-for="(author, index) in form.yazarlar" :key="author.key" class="catalog-collection__item">
            <div class="catalog-collection__grid">
              <label class="catalog-field">
                <span>Yazar</span>
                <select v-model="author.yazarId">
                  <option value="">Yazar seçin veya ID girin</option>
                  <option v-for="person in lookupState.authors" :key="person.id" :value="person.id">
                    {{ person.name }}
                  </option>
                </select>
                <input
                  v-if="!author.yazarId"
                  v-model="author.manualId"
                  type="text"
                  placeholder="Manuel yazar ID gir"
                  class="catalog-field__stacked"
                />
              </label>
              <label class="catalog-field">
                <span>Rol</span>
                <select v-model.number="author.rol">
                  <option v-for="role in yazarRolOptions" :key="role.value" :value="role.value">
                    {{ role.label }}
                  </option>
                </select>
              </label>
              <label class="catalog-field">
                <span>Sıra</span>
                <input v-model.number="author.sira" type="number" min="1" />
              </label>
              <label class="catalog-field">
                <span>Otorite Kaydı ID</span>
                <input v-model="author.otoriteKaydiId" type="text" placeholder="Opsiyonel" />
              </label>
            </div>
            <button type="button" class="catalog-button catalog-button--danger" @click="removeAuthor(index)">
              Yazarı kaldır
            </button>
          </div>
        </div>
      </fieldset>

      <fieldset class="catalog-card">
        <legend>Konular</legend>
        <div class="catalog-collection">
          <header class="catalog-collection__header">
            <p>Konu başlıklarını girin.</p>
            <button type="button" class="catalog-button catalog-button--ghost" @click="addSubject">Konu ekle</button>
          </header>
          <p v-if="form.konular.length === 0" class="catalog-empty">Henüz konu eklenmedi.</p>
          <div v-for="(subject, index) in form.konular" :key="subject.key" class="catalog-collection__item">
            <div class="catalog-collection__grid">
              <label class="catalog-field catalog-field--wide">
                <span>Konu Başlığı *</span>
                <input v-model="subject.konuBasligi" type="text" placeholder="Örn. Türk romanı" />
              </label>
              <label class="catalog-field">
                <span>Otorite Kaydı ID</span>
                <input v-model="subject.otoriteKaydiId" type="text" placeholder="Opsiyonel" />
              </label>
            </div>
            <button type="button" class="catalog-button catalog-button--danger" @click="removeSubject(index)">
              Konuyu kaldır
            </button>
          </div>
        </div>
      </fieldset>

      <fieldset class="catalog-card">
        <legend>MARC Alanları</legend>
        <div class="catalog-collection">
          <header class="catalog-collection__header">
            <p>MARC21 alan ve alt alan yapılarını ekleyin.</p>
            <button type="button" class="catalog-button catalog-button--ghost" @click="addMarcField">
              MARC alanı ekle
            </button>
          </header>
          <p v-if="form.marcAlanlari.length === 0" class="catalog-empty">Henüz MARC alanı eklenmedi.</p>
          <div v-for="(field, fieldIndex) in form.marcAlanlari" :key="field.key" class="catalog-collection__item">
            <div class="catalog-collection__grid">
              <label class="catalog-field">
                <span>Kod *</span>
                <input v-model="field.kod" type="text" placeholder="Örn. 245" />
              </label>
              <label class="catalog-field">
                <span>İndikatör 1</span>
                <input v-model="field.indikator1" type="text" maxlength="1" />
              </label>
              <label class="catalog-field">
                <span>İndikatör 2</span>
                <input v-model="field.indikator2" type="text" maxlength="1" />
              </label>
            </div>
            <div class="catalog-subcollection">
              <header class="catalog-subcollection__header">
                <span>Alt Alanlar</span>
                <button type="button" class="catalog-button catalog-button--ghost" @click="addMarcSubField(fieldIndex)">
                  Alt alan ekle
                </button>
              </header>
              <p v-if="field.altAlanlar.length === 0" class="catalog-empty">Alt alan eklenmedi.</p>
              <div
                v-for="(subField, subIndex) in field.altAlanlar"
                :key="subField.key"
                class="catalog-subcollection__item"
              >
                <label class="catalog-field">
                  <span>Kod *</span>
                  <input v-model="subField.kod" type="text" maxlength="1" placeholder="Örn. a" />
                </label>
                <label class="catalog-field catalog-field--wide">
                  <span>Değer *</span>
                  <input v-model="subField.deger" type="text" placeholder="Alt alan değeri" />
                </label>
                <button
                  type="button"
                  class="catalog-button catalog-button--danger"
                  @click="removeMarcSubField(fieldIndex, subIndex)"
                >
                  Sil
                </button>
              </div>
            </div>
            <button type="button" class="catalog-button catalog-button--danger" @click="removeMarcField(fieldIndex)">
              MARC alanını kaldır
            </button>
          </div>
        </div>
        <label class="catalog-field catalog-field--wide">
          <span>MARC21 Serbest Metin</span>
          <textarea
            v-model="form.marc21Verisi"
            rows="4"
            placeholder="Serbest MARC21 JSON veya etiket metni"
          ></textarea>
        </label>
      </fieldset>

      <div class="catalog-actions">
        <button type="submit" class="catalog-button catalog-button--primary" :disabled="isSubmitDisabled">
          {{ saveState.saving ? 'Kaydediliyor...' : 'Kaydı oluştur' }}
        </button>
        <button type="button" class="catalog-button catalog-button--ghost" @click="resetForm" :disabled="saveState.saving">
          Formu temizle
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import apiClient from '@/stores/api'
import { useAuthStore } from '@/stores/auth'

interface LibraryOption {
  id: string
  label: string
}

interface DeweyOption {
  id: string
  label: string
}

interface AuthorOption {
  id: string
  name: string
}

interface AuthorInput {
  key: string
  yazarId: string
  manualId: string
  rol: number
  sira: number
  otoriteKaydiId: string
}

interface SubjectInput {
  key: string
  konuBasligi: string
  otoriteKaydiId: string
}

interface MarcSubFieldInput {
  key: string
  kod: string
  deger: string
}

interface MarcFieldInput {
  key: string
  kod: string
  indikator1: string
  indikator2: string
  altAlanlar: MarcSubFieldInput[]
}

const materyalTuruOptions = [
  { value: 1, label: 'Kitap' },
  { value: 2, label: 'Süreli Yayın' },
  { value: 3, label: 'Görsel Materyal' },
  { value: 4, label: 'Multimedya' },
  { value: 5, label: 'E-Kitap' },
  { value: 6, label: 'Harita' },
  { value: 7, label: 'El Yazması' },
  { value: 99, label: 'Diğer' }
]

const yazarRolOptions = [
  { value: 1, label: 'Yazar' },
  { value: 2, label: 'Çevirmen' },
  { value: 3, label: 'Editör' },
  { value: 4, label: 'Hazırlayan' }
]

const lookupState = reactive({
  loading: false,
  libraries: [] as LibraryOption[],
  deweys: [] as DeweyOption[],
  authors: [] as AuthorOption[],
  error: ''
})

const saveState = reactive({
  saving: false,
  successMessage: '',
  errorMessage: '',
  createdId: ''
})

const form = reactive({
  deweySiniflamaId: '',
  yeniKatalogTalebiId: '',
  kutuphaneId: '',
  baslik: '',
  altBaslik: '',
  isbn: '',
  yayinevi: '',
  yayinYeri: '',
  yayinYili: '',
  sayfaSayisi: '',
  dil: '',
  dizi: '',
  baski: '',
  ozet: '',
  notlar: '',
  kapakResmiYolu: '',
  materyalTuru: 1,
  materyalAltTuru: '',
  marc21Verisi: '',
  rdaUyumlu: true,
  kayitTarihi: new Date().toISOString().slice(0, 16),
  yazarlar: [] as AuthorInput[],
  konular: [] as SubjectInput[],
  marcAlanlari: [] as MarcFieldInput[]
})

function createKey(prefix: string) {
  const fallback = Math.random().toString(36).slice(2, 10)
  if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
    return `${prefix}-${crypto.randomUUID()}`
  }
  return `${prefix}-${fallback}`
}

function addAuthor() {
  form.yazarlar.push({
    key: createKey('author'),
    yazarId: '',
    manualId: '',
    rol: 1,
    sira: form.yazarlar.length + 1,
    otoriteKaydiId: ''
  })
}

function removeAuthor(index: number) {
  form.yazarlar.splice(index, 1)
}

function addSubject() {
  form.konular.push({
    key: createKey('subject'),
    konuBasligi: '',
    otoriteKaydiId: ''
  })
}

function removeSubject(index: number) {
  form.konular.splice(index, 1)
}

function addMarcField() {
  form.marcAlanlari.push({
    key: createKey('marc'),
    kod: '',
    indikator1: '',
    indikator2: '',
    altAlanlar: []
  })
}

function removeMarcField(index: number) {
  form.marcAlanlari.splice(index, 1)
}

function addMarcSubField(fieldIndex: number) {
  const field = form.marcAlanlari[fieldIndex]
  if (!field) return
  field.altAlanlar.push({
    key: createKey('subfield'),
    kod: '',
    deger: ''
  })
}

function removeMarcSubField(fieldIndex: number, subIndex: number) {
  const field = form.marcAlanlari[fieldIndex]
  if (!field) return
  field.altAlanlar.splice(subIndex, 1)
}

async function loadLookups() {
  lookupState.loading = true
  lookupState.error = ''

  try {
    const params = { PageIndex: 0, PageSize: 200 }
    const [libraryRes, deweyRes, authorRes] = await Promise.all([
      apiClient.get('/Kutuphaneler', { params }),
      apiClient.get('/DeweySiniflamalar', { params }),
      apiClient.get('/Yazarlar', { params })
    ])

    lookupState.libraries =
      libraryRes.data?.items?.map((item: any) => ({
        id: item.id,
        label: `[${item.kod}] ${item.ad}`
      })) ?? []

    lookupState.deweys =
      deweyRes.data?.items?.map((item: any) => ({
        id: item.id,
        label: `${item.kod} - ${item.baslik}`
      })) ?? []

    lookupState.authors =
      authorRes.data?.items?.map((item: any) => ({
        id: item.id,
        name: item.adSoyad || 'İsimsiz yazar'
      })) ?? []

    if (!form.kutuphaneId && lookupState.libraries.length > 0) {
      form.kutuphaneId = lookupState.libraries[0].id
    }

    if (!form.deweySiniflamaId && lookupState.deweys.length > 0) {
      form.deweySiniflamaId = lookupState.deweys[0].id
    }
  } catch (error: any) {
    lookupState.error = error?.response?.data?.message || 'Destek verileri alınamadı.'
  } finally {
    lookupState.loading = false
  }
}

function resetForm() {
  form.deweySiniflamaId = lookupState.deweys[0]?.id ?? ''
  form.yeniKatalogTalebiId = ''
  form.baslik = ''
  form.altBaslik = ''
  form.isbn = ''
  form.yayinevi = ''
  form.yayinYeri = ''
  form.yayinYili = ''
  form.sayfaSayisi = ''
  form.dil = ''
  form.dizi = ''
  form.baski = ''
  form.ozet = ''
  form.notlar = ''
  form.kapakResmiYolu = ''
  form.materyalTuru = 1
  form.materyalAltTuru = ''
  form.marc21Verisi = ''
  form.rdaUyumlu = true
  form.kayitTarihi = new Date().toISOString().slice(0, 16)
  form.yazarlar = []
  form.konular = []
  form.marcAlanlari = []
  saveState.successMessage = ''
  saveState.errorMessage = ''
  saveState.createdId = ''
}

const isSubmitDisabled = computed(() => {
  if (saveState.saving) return true
  return !form.baslik.trim() || !form.kutuphaneId
})

async function handleSubmit() {
  if (isSubmitDisabled.value) {
    return
  }

  saveState.saving = true
  saveState.successMessage = ''
  saveState.errorMessage = ''
  saveState.createdId = ''

  try {
    const authorsPayload = form.yazarlar
      .map(author => ({
        yazarId: (author.yazarId || author.manualId || '').trim(),
        rol: author.rol,
        sira: author.sira || 1,
        otoriteKaydiId: author.otoriteKaydiId?.trim() || undefined
      }))
      .filter(author => author.yazarId)

    const subjectsPayload = form.konular
      .map(subject => ({
        konuBasligi: subject.konuBasligi.trim(),
        otoriteKaydiId: subject.otoriteKaydiId?.trim() || undefined
      }))
      .filter(subject => subject.konuBasligi.length > 0)

    const marcPayload = form.marcAlanlari
      .map(field => ({
        kod: field.kod.trim(),
        indikator1: field.indikator1?.trim() || null,
        indikator2: field.indikator2?.trim() || null,
        altAlanlar: field.altAlanlar
          .map(sub => ({
            kod: sub.kod.trim(),
            deger: sub.deger.trim()
          }))
          .filter(sub => sub.kod.length > 0 && sub.deger.length > 0)
      }))
      .filter(field => field.kod.length > 0)

    const kayitDate = form.kayitTarihi ? new Date(form.kayitTarihi) : new Date()
    const kayitTarihiIso = Number.isNaN(kayitDate.getTime()) ? new Date().toISOString() : kayitDate.toISOString()

    const payload = {
      deweySiniflamaId: form.deweySiniflamaId || null,
      yeniKatalogTalebiId: form.yeniKatalogTalebiId || null,
      kutuphaneId: form.kutuphaneId,
      baslik: form.baslik.trim(),
      altBaslik: form.altBaslik?.trim() || null,
      isbn: form.isbn?.trim() || null,
      yayinevi: form.yayinevi?.trim() || null,
      yayinYeri: form.yayinYeri?.trim() || null,
      yayinYili: form.yayinYili ? Number(form.yayinYili) : null,
      sayfaSayisi: form.sayfaSayisi ? Number(form.sayfaSayisi) : null,
      dil: form.dil?.trim() || null,
      dizi: form.dizi?.trim() || null,
      baski: form.baski?.trim() || null,
      ozet: form.ozet?.trim() || null,
      notlar: form.notlar?.trim() || null,
      kapakResmiYolu: form.kapakResmiYolu?.trim() || null,
      materyalTuru: form.materyalTuru,
      materyalAltTuru: form.materyalAltTuru?.trim() || null,
      marc21Verisi: form.marc21Verisi?.trim() || null,
      marcAlanlari: marcPayload.length > 0 ? marcPayload : null,
      rdaUyumlu: form.rdaUyumlu,
      kayitTarihi: kayitTarihiIso,
      yazarlar: authorsPayload.length > 0 ? authorsPayload : null,
      konular: subjectsPayload.length > 0 ? subjectsPayload : null
    }

    const response = await apiClient.post('/KatalogKayitlari', payload)

    const createdId: string | undefined = response.data?.id ?? response.data?.Id
    saveState.successMessage = 'Katalog kaydı başarıyla oluşturuldu.'
    if (createdId) {
      saveState.createdId = createdId
    }
  } catch (error: any) {
    console.error('Create catalog record failed', error)
    saveState.errorMessage = error?.response?.data?.message || 'Katalog kaydı kaydedilemedi.'
  } finally {
    saveState.saving = false
  }
}

onMounted(() => {
  const authStore = useAuthStore()
  if (authStore.user?.isAuthenticated && authStore.user?.libraryType === 'Merkez') {
    form.kayitTarihi = new Date().toISOString().slice(0, 16)
  }
  loadLookups()
})
</script>

<style scoped>
.catalog-form {
  display: grid;
  gap: 1.5rem;
}

.catalog-form__alert {
  border-radius: 12px;
  padding: 1rem 1.25rem;
  display: grid;
  gap: 0.35rem;
  font-size: 0.95rem;
}

.catalog-form__alert strong {
  font-weight: 600;
}

.catalog-form__alert--error {
  background: rgba(254, 226, 226, 0.7);
  border: 1px solid rgba(248, 113, 113, 0.4);
  color: #991b1b;
}

.catalog-form__alert--success {
  background: rgba(220, 252, 231, 0.7);
  border: 1px solid rgba(74, 222, 128, 0.4);
  color: #166534;
}

.catalog-form__retry {
  justify-self: start;
  background: none;
  border: 1px solid currentColor;
  padding: 0.35rem 0.8rem;
  border-radius: 20px;
  font-weight: 600;
  cursor: pointer;
}

.catalog-form__grid {
  display: grid;
  gap: 1.5rem;
}

.catalog-card {
  border: 1px solid rgba(226, 232, 240, 0.9);
  border-radius: 16px;
  padding: 1.5rem;
  background: #ffffff;
  display: grid;
  gap: 1rem;
}

.catalog-card legend {
  font-size: 1.1rem;
  font-weight: 600;
  color: #0f172a;
  padding: 0 0.5rem;
}

.catalog-card__grid {
  display: grid;
  grid-template-columns: repeat(12, minmax(0, 1fr));
  gap: 1.4rem 1.5rem;
}

.catalog-field {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  font-size: 0.9rem;
  min-width: 0;
  grid-column: span 12;
}

.catalog-field span {
  font-weight: 600;
  color: #1f2937;
}

.catalog-field input,
.catalog-field select,
.catalog-field textarea {
  border: 1px solid rgba(209, 213, 219, 0.9);
  border-radius: 10px;
  padding: 0.6rem 0.75rem;
  font-size: 0.92rem;
  transition: border-color 0.2s ease, box-shadow 0.2s ease;
  width: 100%;
  min-height: 2.75rem;
}

.catalog-field textarea {
  resize: vertical;
  min-height: 90px;
}

.catalog-field input:focus,
.catalog-field select:focus,
.catalog-field textarea:focus {
  outline: none;
  border-color: rgba(99, 102, 241, 0.6);
  box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.15);
}

.catalog-field--wide {
  grid-column: 1 / -1;
}

.catalog-field--half {
  grid-column: span 6;
}

.catalog-field--quarter {
  grid-column: span 3;
}

.catalog-field__stacked {
  margin-top: 0.35rem;
}

.catalog-inline {
  display: flex;
  align-items: center;
  gap: 0.45rem;
  flex-wrap: wrap;
  font-size: 0.9rem;
}

.catalog-field--inline {
  align-items: flex-start;
}

.catalog-inline label {
  margin: 0;
  font-weight: 500;
}

.catalog-inline input[type='checkbox'] {
  width: auto;
  min-height: auto;
}

.catalog-field--date input[type='datetime-local'] {
  min-height: 2.85rem;
}

.catalog-collection,
.catalog-subcollection {
  display: grid;
  gap: 1rem;
}

.catalog-collection__header,
.catalog-subcollection__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.catalog-collection__item,
.catalog-subcollection__item {
  border: 1px dashed rgba(148, 163, 184, 0.6);
  border-radius: 14px;
  padding: 1rem;
  display: grid;
  gap: 0.75rem;
  background: rgba(248, 250, 252, 0.6);
}

.catalog-collection__grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 0.75rem;
}

.catalog-empty {
  font-size: 0.88rem;
  color: #64748b;
}

.catalog-button {
  border: none;
  border-radius: 999px;
  padding: 0.5rem 1.1rem;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
}

.catalog-button--primary {
  background: linear-gradient(135deg, #4f46e5, #f97316);
  color: white;
  box-shadow: 0 10px 20px rgba(79, 70, 229, 0.2);
}

.catalog-button--ghost {
  background: rgba(226, 232, 240, 0.4);
  color: #0f172a;
  border: 1px solid rgba(209, 213, 219, 0.8);
}

.catalog-button--danger {
  background: rgba(248, 113, 113, 0.15);
  color: #b91c1c;
  border: 1px solid rgba(248, 113, 113, 0.35);
}

.catalog-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.catalog-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  padding-top: 0.5rem;
}

@media (max-width: 720px) {
  .catalog-actions {
    flex-direction: column;
    align-items: stretch;
  }

  .catalog-button {
    justify-content: center;
  }

  .catalog-card__grid {
    grid-template-columns: repeat(1, minmax(0, 1fr));
  }

  .catalog-field,
  .catalog-field--half,
  .catalog-field--quarter {
    grid-column: span 1;
  }
}
</style>
















