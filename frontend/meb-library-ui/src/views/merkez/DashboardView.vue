<template>
  <div class="dashboard">
    <section class="hero">
      <div class="hero-content">
        <div class="hero-text">
          <h1>Merkez kütüphanenin nabzı</h1>
          <p>Gerçek zamanlı metrikler, son aktiviteler ve sık kullanılan işlemler tek ekranda.</p>
        </div>
        <div class="hero-pulse">
          <div class="pulse-circle"></div>
          <span>Son senkronizasyon · 18 sn önce</span>
        </div>
      </div>
      <div class="hero-wave"></div>
    </section>

    <section class="metrics">
      <article v-for="card in metricCards" :key="card.title" class="metric" :class="card.variant">
        <div class="metric-icon">{{ card.icon }}</div>
        <div class="metric-body">
          <span class="metric-title">{{ card.title }}</span>
          <strong>{{ card.value }}</strong>
          <small>{{ card.subtitle }}</small>
        </div>
      </article>
    </section>

    <section class="panels">
      <article class="panel activity">
        <header>
          <div>
            <h2>Son aktiviteler</h2>
            <p>Operasyonel akışın güncel özetleri</p>
          </div>
          <button type="button">Tümünü Gör</button>
        </header>
        <ul>
          <li v-for="(item, index) in activities" :key="index" :class="`state-${item.state}`">
            <div class="bullet"></div>
            <div>
              <strong>{{ item.title }}</strong>
              <p>{{ item.description }}</p>
              <time>{{ item.time }}</time>
            </div>
          </li>
        </ul>
      </article>

      <article class="panel quick-actions">
        <header>
          <div>
            <h2>Hızlı işlemler</h2>
            <p>En sık kullanılan ekranlara tek dokunuşla ulaşın</p>
          </div>
        </header>
        <div class="action-grid">
          <button v-for="(action, index) in actions" :key="index" type="button">
            <span class="action-icon">{{ action.icon }}</span>
            <div>
              <strong>{{ action.title }}</strong>
              <p>{{ action.subtitle }}</p>
            </div>
          </button>
        </div>
      </article>
    </section>

    <section class="panels status">
      <article class="panel">
        <header>
          <div>
            <h2>Servis durumu</h2>
            <p>Altyapı bileşenlerinin canlı izlenmesi</p>
          </div>
        </header>
        <div class="status-grid">
          <div v-for="(item, index) in statusCards" :key="index" class="status-card">
            <span class="status-icon">{{ item.icon }}</span>
            <div>
              <strong>{{ item.title }}</strong>
              <p>{{ item.description }}</p>
              <small>{{ item.detail }}</small>
            </div>
          </div>
        </div>
      </article>
    </section>
  </div>
</template>

<script setup lang="ts">
const metricCards = [
  { title: 'Toplam Kayıt', value: '12.543', subtitle: 'Bu ay +342', variant: 'rose', icon: '📚' },
  { title: 'Aktif Ödünç', value: '1.247', subtitle: 'Son 24 saatte 82 işlem', variant: 'emerald', icon: '🔄' },
  { title: 'Geciken Ödünç', value: '23', subtitle: 'Ortalama gecikme 3,1 gün', variant: 'amber', icon: '⏰' },
  { title: 'Aktif Kullanıcı', value: '8.901', subtitle: 'Son giriş: 3 dk önce', variant: 'indigo', icon: '🧑‍🤝‍🧑' }
]

const activities = [
  { title: 'Yeni kitap kataloglandı', description: '“Yapay Zeka ve Eğitim” kaydı tamamlandı.', time: '2 dakika önce', state: 'success' },
  { title: 'Ödünç işlem iadesi', description: 'Ümraniye Anadolu Lisesi, 4 materyal iade etti.', time: '8 dakika önce', state: 'info' },
  { title: 'Gecikme bildirimi', description: 'İstanbul Fen Lisesi için otomatik hatırlatma gönderildi.', time: '14 dakika önce', state: 'warn' }
]

const actions = [
  { title: 'Yeni katalog kaydı', subtitle: 'MARC veya RDA giriş ekranını aç', icon: '➕' },
  { title: 'Ödünç ver', subtitle: 'Tek tıkla seri ödünç işlemine git', icon: '📥' },
  { title: 'Rapor indir', subtitle: 'Hazır rapor şablonlarını indir', icon: '⬇️' },
  { title: 'Kullanıcı davet et', subtitle: 'Yeni okul yöneticisi ekle', icon: '✉️' }
]

const statusCards = [
  { title: 'Veritabanı', description: 'Bağlı ve replikasyon aktif', detail: 'Son yedekleme: 17 dk önce', icon: '🗄️' },
  { title: 'API servisleri', description: '4 servis · 26 ms yanıt süresi', detail: 'Durum: sağlıklı', icon: '🌐' },
  { title: 'Depo kapasitesi', description: '%72 doluluk · log temizliği öneriliyor', detail: 'Planlanan bakım: 23:00', icon: '📦' }
]
</script>

<style scoped>
.dashboard {
  display: flex;
  flex-direction: column;
  gap: 1.8rem;
}

.hero {
  position: relative;
  border-radius: 24px;
  overflow: hidden;
  background: linear-gradient(135deg, rgba(255, 240, 245, 0.95), rgba(255, 228, 230, 0.7));
  padding: 2.4rem;
  box-shadow: 0 24px 45px rgba(248, 113, 113, 0.12);
}

.hero::after {
  content: '';
  position: absolute;
  inset: 0;
  background: radial-gradient(circle at 80% 20%, rgba(244, 114, 182, 0.3), transparent 55%);
}

.hero-content {
  position: relative;
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1.5rem;
  z-index: 1;
}

.hero-text h1 {
  margin: 0;
  font-size: 1.9rem;
  color: #1f2937;
}

.hero-text p {
  margin: 0.35rem 0 0;
  color: rgba(55, 65, 81, 0.75);
  max-width: 520px;
}

.hero-pulse {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  background: rgba(255, 255, 255, 0.85);
  padding: 0.6rem 0.9rem;
  border-radius: 999px;
  border: 1px solid rgba(248, 113, 113, 0.3);
  backdrop-filter: blur(6px);
  font-size: 0.85rem;
  color: rgba(220, 38, 38, 0.8);
}

@keyframes pulse-ring {
  0% {
    transform: scale(0.9);
    opacity: 0.7;
  }
  70% {
    transform: scale(1.1);
    opacity: 0;
  }
  100% {
    transform: scale(0.9);
    opacity: 0;
  }
}

.pulse-circle {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: #dc2626;
  position: relative;
}

.pulse-circle::after {
  content: '';
  position: absolute;
  inset: 0;
  border-radius: 50%;
  background: rgba(220, 38, 38, 0.35);
  animation: pulse-ring 2.2s infinite;
}

.hero-wave {
  position: absolute;
  inset: auto -40% -60px -40%;
  height: 200px;
  background: radial-gradient(circle at top, rgba(248, 113, 113, 0.28), transparent 65%);
  z-index: 0;
}

.metrics {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.2rem;
}

.metric {
  display: flex;
  align-items: center;
  gap: 0.9rem;
  padding: 1.05rem 1.25rem;
  border-radius: 18px;
  background: #ffffff;
  border: 1px solid rgba(226, 232, 240, 0.9);
  box-shadow: 0 16px 30px rgba(15, 23, 42, 0.05);
  animation: fade-up 0.45s ease forwards;
  opacity: 0;
}

.metric:nth-child(2) { animation-delay: 0.08s; }
.metric:nth-child(3) { animation-delay: 0.16s; }
.metric:nth-child(4) { animation-delay: 0.24s; }

@keyframes fade-up {
  from {
    transform: translateY(12px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.metric-icon {
  width: 38px;
  height: 38px;
  border-radius: 14px;
  display: grid;
  place-items: center;
  font-size: 1.05rem;
  color: #fff;
}

.metric-body span {
  display: block;
  font-size: 0.85rem;
  color: rgba(55, 65, 81, 0.7);
}

.metric-body strong {
  display: block;
  font-size: 1.35rem;
  color: #0f172a;
}

.metric-body small {
  color: rgba(100, 116, 139, 0.85);
  font-size: 0.78rem;
}

.metric.rose .metric-icon { background: linear-gradient(135deg, #f43f5e, #fb7185); }
.metric.emerald .metric-icon { background: linear-gradient(135deg, #10b981, #34d399); }
.metric.amber .metric-icon { background: linear-gradient(135deg, #f59e0b, #fbbf24); }
.metric.indigo .metric-icon { background: linear-gradient(135deg, #6366f1, #60a5fa); }

.panels {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 1.4rem;
}

.panel {
  background: #ffffff;
  border-radius: 22px;
  border: 1px solid rgba(226, 232, 240, 0.85);
  box-shadow: 0 20px 44px rgba(15, 23, 42, 0.07);
  padding: 1.7rem;
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

.panel header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}

.panel header h2 {
  margin: 0;
  font-size: 1.25rem;
  color: #0f172a;
}

.panel header p {
  margin: 0.25rem 0 0;
  color: rgba(71, 85, 105, 0.75);
  font-size: 0.88rem;
}

.panel header button {
  border: none;
  background: none;
  color: #dc2626;
  font-weight: 600;
  cursor: pointer;
  transition: color 0.18s ease;
}

.panel header button:hover {
  color: #be123c;
}

.activity ul {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  gap: 1.1rem;
}

.activity li {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 0.85rem;
  align-items: start;
}

.activity li .bullet {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  margin-top: 0.35rem;
}

.activity li.state-success .bullet { background: #16a34a; }
.activity li.state-info .bullet { background: #2563eb; }
.activity li.state-warn .bullet { background: #f97316; }

.activity li strong {
  display: block;
  margin-bottom: 0.2rem;
  color: #0f172a;
}

.activity li p {
  margin: 0;
  color: rgba(71, 85, 105, 0.85);
  font-size: 0.9rem;
}

.activity li time {
  margin-top: 0.25rem;
  display: block;
  color: rgba(148, 163, 184, 0.95);
  font-size: 0.78rem;
}

.quick-actions .action-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}

.quick-actions button {
  border: 1px solid rgba(226, 232, 240, 0.85);
  border-radius: 16px;
  padding: 0.9rem 1rem;
  background: #ffffff;
  display: flex;
  align-items: center;
  gap: 0.8rem;
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease, border 0.2s ease;
}

.quick-actions button:hover {
  transform: translateY(-3px);
  box-shadow: 0 18px 32px rgba(15, 23, 42, 0.1);
  border-color: rgba(248, 113, 113, 0.4);
}

.action-icon {
  width: 28px;
  height: 28px;
  border-radius: 10px;
  background: rgba(248, 113, 113, 0.12);
  display: grid;
  place-items: center;
  font-size: 0.95rem;
  color: #dc2626;
}

.quick-actions strong {
  display: block;
  color: #0f172a;
  font-weight: 600;
}

.quick-actions p {
  margin: 0.25rem 0 0;
  color: rgba(71, 85, 105, 0.8);
  font-size: 0.85rem;
}

.status-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}

.status-card {
  display: flex;
  align-items: center;
  gap: 0.9rem;
  padding: 1rem 1.1rem;
  border-radius: 16px;
  background: rgba(248, 250, 252, 0.95);
  border: 1px solid rgba(226, 232, 240, 0.85);
}

.status-icon {
  width: 32px;
  height: 32px;
  border-radius: 12px;
  display: grid;
  place-items: center;
  background: rgba(248, 113, 113, 0.12);
  color: #dc2626;
  font-size: 0.95rem;
}

.status-card strong {
  display: block;
  color: #0f172a;
}

.status-card p {
  margin: 0.2rem 0;
  color: rgba(71, 85, 105, 0.85);
}

.status-card small {
  color: rgba(148, 163, 184, 0.95);
  font-size: 0.78rem;
}

@media (max-width: 960px) {
  .hero {
    padding: 2rem;
  }

  .hero-content {
    flex-direction: column;
    align-items: flex-start;
  }
}

@media (max-width: 640px) {
  .quick-actions .action-grid {
    grid-template-columns: 1fr;
  }

  .metrics {
    grid-template-columns: 1fr 1fr;
  }
}
</style>
