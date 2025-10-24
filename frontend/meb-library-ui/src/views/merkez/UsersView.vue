<template>
  <div class="users-page">
    <section class="hero">
      <div class="hero__content">
        <div class="hero__status">
          <span class="hero__dot"></span>
          Aktif kullanıcı sayısı: 2
        </div>
        <h1 class="hero__title">Kullanıcı Yönetimi</h1>
        <p class="hero__subtitle">
          Kullanıcı hesaplarını yönetin, yetkileri güncelleyin ve erişim seviyelerini anlık olarak izleyin.
        </p>
        <ul class="hero__highlights">
          <li class="hero__highlight">Rol bazlı yetkilendirme</li>
          <li class="hero__highlight">Kütüphane türüne göre filtre</li>
          <li class="hero__highlight">Son giriş aktiviteleri</li>
        </ul>
      </div>
      <button type="button" class="btn btn--primary hero__cta">
        <span class="btn__icon">+</span>
        Yeni kullanıcı
      </button>
    </section>

    <section class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">Filtreler</h2>
          <p class="module__subtitle">Rol, kütüphane türü ve T.C. numarasına göre arayın.</p>
        </div>
        <button type="button" class="btn btn--outline">Varsayılanı sıfırla</button>
      </header>
      <div class="filter-grid">
        <label class="field">
          <span class="field__label">Rol</span>
          <select class="select">
            <option>Tümü</option>
            <option>Bakanlık Yetkilisi</option>
            <option>İl Yetkilisi</option>
            <option>İlçe Yetkilisi</option>
            <option>Okul Kütüphane Yöneticisi</option>
            <option>Öğretmen</option>
            <option>Öğrenci</option>
          </select>
        </label>
        <label class="field">
          <span class="field__label">Kütüphane türü</span>
          <select class="select">
            <option>Tümü</option>
            <option>Merkez Kütüphane</option>
            <option>Okul Kütüphanesi</option>
          </select>
        </label>
        <label class="field field--search">
          <span class="field__label">Arama</span>
          <input type="text" class="input" placeholder="Ad, soyad veya T.C. No" />
        </label>
        <div class="field field--actions">
          <button type="button" class="btn btn--primary btn--block">Filtreleri uygula</button>
        </div>
      </div>
    </section>

    <section class="module">
      <header class="module__header">
        <div>
          <h2 class="module__title">Kullanıcı listesi</h2>
          <p class="module__subtitle">Son güncelleme: 1 saat önce</p>
        </div>
        <div class="legend">
          <span class="legend__item legend__item--active">Aktif</span>
          <span class="legend__item legend__item--pending">Davet gönderildi</span>
        </div>
      </header>
      <div class="table-wrapper">
        <table class="data-table">
          <thead>
            <tr>
              <th scope="col">Ad Soyad</th>
              <th scope="col">T.C. No</th>
              <th scope="col">Rol</th>
              <th scope="col">Kütüphane</th>
              <th scope="col">Durum</th>
              <th scope="col">Son giriş</th>
              <th scope="col" class="table-actions">İşlemler</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <div class="entity">
                  <span class="entity__title">Ahmet Yılmaz</span>
                  <span class="entity__meta">Bakanlık Yetkilisi</span>
                </div>
              </td>
              <td>12345678901</td>
              <td>
                <span class="badge badge--purple">Bakanlık Yetkilisi</span>
              </td>
              <td>
                <div class="entity">
                  <span class="entity__title">Merkez Kütüphane</span>
                  <span class="entity__meta">Ankara / Çankaya</span>
                </div>
              </td>
              <td><span class="badge badge--success">Aktif</span></td>
              <td>2 saat önce</td>
              <td class="table-actions">
                <button type="button" class="btn btn--ghost btn--small">Düzenle</button>
                <button type="button" class="btn btn--danger btn--small">Sil</button>
              </td>
            </tr>
            <tr>
              <td>
                <div class="entity">
                  <span class="entity__title">Ayşe Kara</span>
                  <span class="entity__meta">Okul Kütüphane Yöneticisi</span>
                </div>
              </td>
              <td>12345678902</td>
              <td>
                <span class="badge badge--indigo">Okul Kütüphane Yöneticisi</span>
              </td>
              <td>
                <div class="entity">
                  <span class="entity__title">Çankaya Lisesi</span>
                  <span class="entity__meta">İstanbul / Kadıköy</span>
                </div>
              </td>
              <td><span class="badge badge--success">Aktif</span></td>
              <td>1 gün önce</td>
              <td class="table-actions">
                <button type="button" class="btn btn--ghost btn--small">Düzenle</button>
                <button type="button" class="btn btn--danger btn--small">Sil</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>
  </div>
</template>
<script setup lang="ts">
// UI prototipi - gerçek veri ve işlemler entegrasyon için hazır.
</script>
<style scoped>
.users-page {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  padding: 2.5rem 3rem 4rem;
  background: linear-gradient(180deg, #f8fafc 0%, #e8edff 100%);
  color: #0f172a;
}

.hero {
  display: flex;
  justify-content: space-between;
  gap: 2rem;
  padding: 2.8rem 3rem;
  border-radius: 28px;
  background: linear-gradient(135deg, #2563eb 0%, #7c3aed 100%);
  color: #fff;
  box-shadow: 0 28px 60px -40px rgba(76, 29, 149, 0.65);
}

.hero__content {
  display: flex;
  flex-direction: column;
  gap: 1.35rem;
  max-width: 640px;
}

.hero__status {
  display: inline-flex;
  align-items: center;
  gap: 0.65rem;
  padding: 0.45rem 1.1rem;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.18);
  font-size: 0.9rem;
  font-weight: 500;
}

.hero__dot {
  width: 0.6rem;
  height: 0.6rem;
  border-radius: 50%;
  background: #38bdf8;
  box-shadow: 0 0 0 0 rgba(56, 189, 248, 0.6);
  animation: pulse 2s infinite;
}

.hero__title {
  margin: 0;
  font-size: 2.4rem;
  font-weight: 600;
  letter-spacing: -0.02em;
}

.hero__subtitle {
  margin: 0;
  font-size: 1.02rem;
  line-height: 1.6;
  color: rgba(255, 255, 255, 0.86);
}

.hero__highlights {
  display: flex;
  flex-wrap: wrap;
  gap: 0.8rem;
  list-style: none;
  padding: 0;
  margin: 0;
}

.hero__highlight {
  padding: 0.4rem 1rem;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.18);
  font-size: 0.88rem;
  font-weight: 500;
}

.hero__cta {
  align-self: flex-start;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  border: none;
  border-radius: 16px;
  padding: 0.8rem 1.6rem;
  background: rgba(255, 255, 255, 0.18);
  color: #fff;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.2s ease, background 0.2s ease;
  box-shadow: 0 18px 30px -26px rgba(15, 23, 42, 0.55);
}

.hero__cta:hover {
  transform: translateY(-2px);
  background: rgba(255, 255, 255, 0.26);
}

.btn__icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 1.1rem;
}

.module {
  background: #fff;
  border-radius: 24px;
  border: 1px solid rgba(148, 163, 184, 0.18);
  box-shadow: 0 22px 60px -45px rgba(15, 23, 42, 0.45);
  padding: 1.8rem;
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

.module__header {
  display: flex;
  justify-content: space-between;
  gap: 1rem;
  flex-wrap: wrap;
  align-items: center;
}

.module__title {
  margin: 0;
  font-size: 1.35rem;
  font-weight: 600;
  color: #0f172a;
}

.module__subtitle {
  margin: 0.25rem 0 0;
  font-size: 0.95rem;
  color: #64748b;
}

.filter-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.field {
  display: flex;
  flex-direction: column;
  gap: 0.45rem;
}

.field__label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #475569;
}

.select,
.input {
  border-radius: 12px;
  border: 1px solid rgba(148, 163, 184, 0.55);
  padding: 0.65rem 0.85rem;
  font-size: 0.95rem;
  color: #1f2937;
  background: #fff;
  transition: border 0.18s ease, box-shadow 0.18s ease;
  font-family: inherit;
}

.select:focus,
.input:focus {
  outline: none;
  border-color: rgba(79, 70, 229, 0.6);
  box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.18);
}

.field--actions {
  display: flex;
  align-items: flex-end;
}

.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.35rem;
  border-radius: 12px;
  border: none;
  padding: 0.65rem 1.3rem;
  font-size: 0.95rem;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.16s ease, box-shadow 0.16s ease, background 0.16s ease;
}

.btn--primary {
  background: linear-gradient(135deg, #4f46e5, #7c3aed);
  color: #fff;
  box-shadow: 0 18px 30px -24px rgba(79, 70, 229, 0.6);
}

.btn--primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 24px 36px -24px rgba(79, 70, 229, 0.55);
}

.btn--outline {
  border: 1px solid rgba(148, 163, 184, 0.6);
  background: #fff;
  color: #1f2937;
}

.btn--outline:hover {
  border-color: rgba(79, 70, 229, 0.6);
  color: #4f46e5;
}

.btn--ghost {
  background: rgba(15, 23, 42, 0.05);
  color: #1f2937;
}

.btn--ghost:hover {
  background: rgba(15, 23, 42, 0.12);
}

.btn--danger {
  background: rgba(239, 68, 68, 0.12);
  color: #b91c1c;
}

.btn--danger:hover {
  background: rgba(239, 68, 68, 0.2);
}

.btn--small {
  padding: 0.4rem 0.9rem;
  font-size: 0.85rem;
}

.btn--block {
  width: 100%;
}

.table-wrapper {
  overflow-x: auto;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.92rem;
}

.data-table thead {
  background: rgba(99, 102, 241, 0.08);
}

.data-table th,
.data-table td {
  padding: 0.85rem 1rem;
  text-align: left;
  border-bottom: 1px solid rgba(226, 232, 240, 0.9);
}

.table-actions {
  display: flex;
  gap: 0.5rem;
}

.entity {
  display: flex;
  flex-direction: column;
  gap: 0.2rem;
}

.entity__title {
  font-weight: 600;
  color: #0f172a;
}

.entity__meta {
  font-size: 0.78rem;
  color: #94a3b8;
}

.badge {
  display: inline-flex;
  align-items: center;
  padding: 0.32rem 0.75rem;
  border-radius: 999px;
  font-size: 0.82rem;
  font-weight: 600;
}

.badge--purple {
  background: rgba(196, 181, 253, 0.4);
  color: #6d28d9;
}

.badge--indigo {
  background: rgba(165, 180, 252, 0.35);
  color: #4338ca;
}

.badge--success {
  background: rgba(134, 239, 172, 0.35);
  color: #15803d;
}

.legend {
  display: flex;
  gap: 0.6rem;
  align-items: center;
}

.legend__item {
  display: inline-flex;
  align-items: center;
  padding: 0.25rem 0.6rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
}

.legend__item--active {
  background: rgba(134, 239, 172, 0.3);
  color: #166534;
}

.legend__item--pending {
  background: rgba(253, 230, 138, 0.3);
  color: #b45309;
}

@keyframes pulse {
  0% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(56, 189, 248, 0.6);
  }
  70% {
    transform: scale(1.05);
    box-shadow: 0 0 0 8px rgba(56, 189, 248, 0);
  }
  100% {
    transform: scale(1);
    box-shadow: 0 0 0 0 rgba(56, 189, 248, 0);
  }
}

@media (max-width: 960px) {
  .hero {
    flex-direction: column;
  }

  .hero__cta {
    align-self: flex-end;
  }
}

@media (max-width: 720px) {
  .users-page {
    padding: 1.6rem;
  }

  .hero {
    padding: 2.2rem;
  }

  .module__header {
    flex-direction: column;
    align-items: flex-start;
  }

  .table-actions {
    flex-direction: column;
    align-items: flex-start;
  }
}
</style>

