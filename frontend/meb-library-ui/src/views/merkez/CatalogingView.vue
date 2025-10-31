<template>
  <div class="cataloging-layout">
    <header class="cataloging-hero">
      <div class="cataloging-hero__content">
        <p class="cataloging-hero__eyebrow">Kataloglama Stüdyosu</p>
        <h1>Merkez koleksiyonunuza yeni kayıt ekleyin</h1>
        <p>
          Tüm bibliyografik alanları tek form üzerinden doldurun, MARC yapısını yönetin ve kaydı merkezi veritabanına
          gönderin. Form, CreateKatalogKaydiCommand tarafından beklenen bütün alanları kapsar.
        </p>
        <ul class="cataloging-hero__checklist">
          <li>Temel ve yayın bilgileri</li>
          <li>Dewey sınıflaması ve içerik notları</li>
          <li>Yazar ilişkileri ve konu başlıkları</li>
          <li>MARC alanları ve RDA uyumluluğu</li>
        </ul>
      </div>
      <div class="cataloging-hero__aside">
        <article class="cataloging-card cataloging-card--stats">
          <h2>Adım adım ilerleyin</h2>
          <ol>
            <li>Destek verilerini yükleyin (kütüphane, Dewey, yazar).</li>
            <li>Zorunlu alanları doldurun (başlık, kütüphane, materyal türü).</li>
            <li>Yazar, konu ve MARC alanlarını isteğe bağlı ekleyin.</li>
            <li>Kayıt tarihini doğrulayın ve kaydedin.</li>
          </ol>
          <p class="cataloging-card__hint">
            Kayıt oluşturulduktan sonra yanıt içerisindeki ID ile detay görüntüleyebilirsiniz.
          </p>
        </article>
      </div>
    </header>

    <main class="cataloging-main">
      <create-catalog-record-form />
      <aside class="cataloging-sidebar">
        <section class="cataloging-card">
          <h3>Alan rehberi</h3>
          <dl>
            <dt>Zorunlu alanlar</dt>
            <dd>Bağlı kütüphane, başlık, materyal türü ve kayıt tarihi.</dd>
            <dt>Tarih alanı</dt>
            <dd>Varsayılan olarak mevcut tarih atanır. Gerekiyorsa düzenleyebilirsiniz.</dd>
            <dt>MARC alanları</dt>
            <dd>
              Kod (ör. 245) ve alt alan kodları (ör. a, b) girin. Alt alan değeri boş bırakılırsa gönderilmez.
            </dd>
            <dt>Yazar seçimi</dt>
            <dd>
              Listeden seçim yapabilir ya da manuel GUID girebilirsiniz. Sıra numarası, görüntüleme sırasını belirler.
            </dd>
          </dl>
        </section>
        <section class="cataloging-card">
          <h3>Materyal türü eşlemesi</h3>
          <ul class="cataloging-card__list">
            <li><strong>Kitap:</strong> Basılı monograf, ders kitabı.</li>
            <li><strong>Süreli yayın:</strong> Dergi, gazete, yıllık.</li>
            <li><strong>Görsel materyal:</strong> Harita, poster, görsel arşiv.</li>
            <li><strong>Multimedya:</strong> DVD, CD, video veya ses içerikleri.</li>
            <li><strong>E-Kitap:</strong> Dijital monograf.</li>
            <li><strong>El yazması:</strong> Tek nüsha el yazmaları.</li>
            <li><strong>Diğer:</strong> Yukarıdakilerin haricindeki materyaller.</li>
          </ul>
        </section>
      </aside>
    </main>
  </div>
</template>

<script setup lang="ts">
import CreateCatalogRecordForm from '@/components/catalog/CreateCatalogRecordForm.vue'
</script>

<style scoped>
.cataloging-layout {
  display: grid;
  gap: 2rem;
  padding: 2.5rem min(5vw, 4rem) 3rem;
  background: radial-gradient(circle at top, #eef2ff 0%, #fdf2f8 45%, #fff7ed 100%);
}

.cataloging-hero {
  display: grid;
  gap: 2rem;
  grid-template-columns: minmax(0, 2fr) minmax(0, 1fr);
  align-items: start;
}

.cataloging-hero__content {
  display: grid;
  gap: 1rem;
  background: rgba(255, 255, 255, 0.85);
  border-radius: 24px;
  padding: 2.5rem;
  box-shadow: 0 22px 40px rgba(79, 70, 229, 0.12);
  border: 1px solid rgba(224, 231, 255, 0.7);
}

.cataloging-hero__eyebrow {
  text-transform: uppercase;
  letter-spacing: 0.16em;
  font-size: 0.85rem;
  color: #6366f1;
  font-weight: 700;
}

.cataloging-hero__content h1 {
  font-size: clamp(2rem, 4vw, 2.8rem);
  margin: 0;
  color: #0f172a;
}

.cataloging-hero__content p {
  margin: 0;
  color: rgba(15, 23, 42, 0.75);
  line-height: 1.6;
}

.cataloging-hero__checklist {
  display: grid;
  gap: 0.45rem;
  padding: 0;
  margin: 0;
  list-style: none;
}

.cataloging-hero__checklist li {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.6rem 1rem;
  border-radius: 999px;
  background: rgba(59, 130, 246, 0.12);
  color: #1d4ed8;
  font-weight: 600;
  width: fit-content;
}

.cataloging-hero__aside {
  display: grid;
}

.cataloging-main {
  display: grid;
  gap: 2rem;
  grid-template-columns: minmax(0, 2fr) minmax(0, 1fr);
  align-items: start;
}

.cataloging-sidebar {
  display: grid;
  gap: 1.5rem;
}

.cataloging-card {
  background: rgba(255, 255, 255, 0.9);
  border-radius: 20px;
  border: 1px solid rgba(226, 232, 240, 0.8);
  padding: 1.5rem;
  display: grid;
  gap: 1rem;
}

.cataloging-card h3 {
  margin: 0;
  font-size: 1.15rem;
  color: #0f172a;
}

.cataloging-card dl {
  display: grid;
  gap: 0.75rem;
  margin: 0;
}

.cataloging-card dt {
  font-weight: 600;
  color: #1e3a8a;
}

.cataloging-card dd {
  margin: 0.15rem 0 0;
  color: #475569;
  font-size: 0.95rem;
}

.cataloging-card__list {
  margin: 0;
  padding-left: 1.25rem;
  display: grid;
  gap: 0.5rem;
  color: #475569;
}

.cataloging-card--stats {
  background: linear-gradient(135deg, rgba(79, 70, 229, 0.9), rgba(14, 165, 233, 0.85));
  color: #f9fafb;
  border: none;
  gap: 1rem;
}

.cataloging-card--stats h2 {
  margin: 0;
  font-size: 1.4rem;
}

.cataloging-card--stats ol {
  margin: 0;
  padding-left: 1.25rem;
  display: grid;
  gap: 0.4rem;
}

.cataloging-card--stats li {
  font-size: 0.95rem;
  line-height: 1.5;
}

.cataloging-card__hint {
  margin: 0;
  font-size: 0.9rem;
  color: rgba(255, 255, 255, 0.85);
}

@media (max-width: 1080px) {
  .cataloging-hero,
  .cataloging-main {
    grid-template-columns: 1fr;
  }

  .cataloging-hero__aside {
    order: -1;
  }
}

@media (max-width: 720px) {
  .cataloging-layout {
    padding: 1.8rem 1.25rem 2.5rem;
  }

  .cataloging-hero__content {
    padding: 2rem;
  }
}
</style>
