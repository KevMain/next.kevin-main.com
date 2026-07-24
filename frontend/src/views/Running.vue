<template>
  <div class="running-page">
    <div class="hero-section">
      <div class="hero-background-overlay"></div>
      <div class="hero-content">
        <h1 class="hero-title">Run Maino Run</h1>
        <p class="hero-subtitle">Documenting my running journey, one mile at a time</p>
        <a href="https://www.instagram.com/runmainorun/" target="_blank" rel="noopener noreferrer" class="instagram-link">
          <span class="instagram-icon">📷</span>
          Follow on Instagram
        </a>
      </div>
    </div>

    <div class="content-container">
      <!-- Personal Bests Section -->
      <section class="stats-section" v-if="pbs">
        <h2 class="section-title">Personal Bests</h2>
        <div class="stats-grid">
          <div class="stat-card" v-if="pbs.fastest5K">
            <div class="stat-icon">🏃‍♂️</div>
            <div class="stat-value">{{ formatDuration(pbs.fastest5K.duration) }}</div>
            <div class="stat-label">5K</div>
            <div class="stat-detail">{{ formatPace(pbs.fastest5K.pace) }} /km</div>
          </div>
          <div class="stat-card" v-if="pbs.fastest10K">
            <div class="stat-icon">🏃‍♂️</div>
            <div class="stat-value">{{ formatDuration(pbs.fastest10K.duration) }}</div>
            <div class="stat-label">10K</div>
            <div class="stat-detail">{{ formatPace(pbs.fastest10K.pace) }} /km</div>
          </div>
          <div class="stat-card" v-if="pbs.fastestHalfMarathon">
            <div class="stat-icon">🏃‍♂️</div>
            <div class="stat-value">{{ formatDuration(pbs.fastestHalfMarathon.duration) }}</div>
            <div class="stat-label">Half Marathon</div>
            <div class="stat-detail">{{ formatPace(pbs.fastestHalfMarathon.pace) }} /km</div>
          </div>
          <div class="stat-card" v-if="pbs.fastestMarathon">
            <div class="stat-icon">🏃‍♂️</div>
            <div class="stat-value">{{ formatDuration(pbs.fastestMarathon.duration) }}</div>
            <div class="stat-label">Marathon</div>
            <div class="stat-detail">{{ formatPace(pbs.fastestMarathon.pace) }} /km</div>
          </div>
        </div>
      </section>

      <!-- Gallery Section -->
      <section class="gallery-section">
        <h2 class="section-title">Recent Moments</h2>
        <div v-if="galleryImages.length > 0" class="gallery-grid">
          <div v-for="(image, index) in galleryImages" :key="index" class="gallery-item">
            <img :src="getImageUrl(image)" :alt="`Running moment ${index + 1}`" class="gallery-image" />
          </div>
        </div>
      </section>

      <!-- Activities Section -->
      <section class="activities-section">
        <h2 class="section-title">Recent Activities</h2>
        <div v-if="loading" class="loading">Loading activities...</div>
        <div v-else-if="error" class="error">{{ error }}</div>
        <div v-else class="activities-grid">
          <div v-for="activity in activities" :key="activity.id" class="activity-card">
            <div class="activity-header">
              <h3 class="activity-title">
                <a v-if="activity.stravaUrl" 
                   :href="activity.stravaUrl" 
                   target="_blank" 
                   rel="noopener noreferrer"
                   class="activity-title-link">
                  {{ activity.title }}
                </a>
                <span v-else>{{ activity.title }}</span>
              </h3>
              <span class="activity-date">{{ formatDate(activity.date) }}</span>
            </div>

            <div class="activity-metrics">
              <div class="metric">
                <span class="metric-icon">📏</span>
                <span class="metric-value">{{ activity.distance.toFixed(2) }} km</span>
              </div>
              <div class="metric">
                <span class="metric-icon">⏱️</span>
                <span class="metric-value">{{ formatDuration(activity.duration) }}</span>
              </div>
              <div class="metric" v-if="activity.averagePace">
                <span class="metric-icon">⚡</span>
                <span class="metric-value">{{ activity.averagePace.toFixed(2) }} min/km</span>
              </div>
              <div class="metric" v-if="activity.elevation">
                <span class="metric-icon">⛰️</span>
                <span class="metric-value">{{ activity.elevation }}m</span>
              </div>
            </div>

            <div class="activity-location" v-if="activity.location">
              <span class="location-icon">📍</span>
              {{ activity.location }}
            </div>

            <p class="activity-description" v-if="activity.description">
              {{ activity.description }}
            </p>

            <div class="activity-tags" v-if="activity.tags && activity.tags.length">
              <span v-for="tag in activity.tags" :key="tag" class="tag">
                #{{ tag }}
              </span>
            </div>

            <a v-if="activity.instagramPostUrl" 
               :href="activity.instagramPostUrl" 
               target="_blank" 
               rel="noopener noreferrer" 
               class="instagram-post-link">
              View on Instagram →
            </a>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script>
import config from '../config.js';

export default {
  name: 'Running',
  data() {
    return {
      activities: [],
      pbs: null,
      galleryImages: [],
      loading: true,
      error: null
    };
  },
  async mounted() {
    await this.loadData();
  },
  methods: {
    async loadData() {
      try {
        this.loading = true;
        this.error = null;

        const [activitiesRes, pbsRes, galleryRes] = await Promise.all([
          fetch(`${config.apiBaseUrl}/api/running/recent?count=10`),
          fetch(`${config.apiBaseUrl}/api/running/pbs`),
          fetch(`${config.apiBaseUrl}/api/running/gallery?count=6`)
        ]);

        if (!activitiesRes.ok || !pbsRes.ok || !galleryRes.ok) {
          throw new Error('Failed to load running data');
        }

        this.activities = await activitiesRes.json();
        this.pbs = await pbsRes.json();
        this.galleryImages = await galleryRes.json();
      } catch (err) {
        console.error('Error loading running data:', err);
        this.error = 'Failed to load running data. Please try again later.';
      } finally {
        this.loading = false;
      }
    },
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('en-US', { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
      });
    },
    formatDuration(duration) {
      if (typeof duration === 'string') {
        const parts = duration.split(':');
        const hours = parseInt(parts[0]);
        const minutes = parseInt(parts[1]);
        const seconds = parts[2] ? parseInt(parts[2].split('.')[0]) : 0;

        if (hours > 0) {
          return `${hours}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
        }
        return `${minutes}:${seconds.toString().padStart(2, '0')}`;
      }
      return duration;
    },
    formatPace(pace) {
      if (!pace) return '';
      const minutes = Math.floor(pace);
      const seconds = Math.round((pace - minutes) * 60);
      return `${minutes}:${seconds.toString().padStart(2, '0')}`;
    },
    getImageUrl(image) {
      // If it's already a full URL (starts with http), return as-is
      if (image.startsWith('http')) {
        return image;
      }
      // Otherwise, prepend the API base URL
      return `${config.apiBaseUrl}${image}`;
    }
  }
};
</script>

<style scoped>
.running-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
}

.hero-section {
  position: relative;
  background: 
    linear-gradient(135deg, rgba(14, 165, 233, 0.85) 0%, rgba(168, 85, 247, 0.85) 100%),
    url('https://images.unsplash.com/photo-1552674605-db6ffd4facb5?q=80&w=2070&auto=format&fit=crop') center/cover;
  padding: 120px 20px;
  text-align: center;
  box-shadow: 0 10px 40px rgba(0,0,0,0.3);
  overflow: hidden;
}

.hero-background-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: 
    radial-gradient(circle at 20% 50%, rgba(14, 165, 233, 0.3) 0%, transparent 50%),
    radial-gradient(circle at 80% 80%, rgba(168, 85, 247, 0.3) 0%, transparent 50%);
  animation: pulse 8s ease-in-out infinite;
  pointer-events: none;
}

@keyframes pulse {
  0%, 100% {
    opacity: 0.5;
  }
  50% {
    opacity: 0.8;
  }
}

.hero-content {
  max-width: 800px;
  margin: 0 auto;
  position: relative;
  z-index: 1;
}

.hero-title {
  font-size: 3.5rem;
  font-weight: 800;
  color: white;
  margin-bottom: 15px;
  text-shadow: 
    0 2px 10px rgba(0,0,0,0.3),
    0 4px 20px rgba(0,0,0,0.2);
  letter-spacing: 1px;
}

.hero-subtitle {
  font-size: 1.4rem;
  color: rgba(255, 255, 255, 0.95);
  margin-bottom: 35px;
  text-shadow: 0 2px 8px rgba(0,0,0,0.3);
  font-weight: 500;
}

.instagram-link {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  padding: 16px 35px;
  background: rgba(255, 255, 255, 0.98);
  color: #0ea5e9;
  text-decoration: none;
  border-radius: 50px;
  font-weight: 700;
  font-size: 1.15rem;
  transition: all 0.3s ease;
  box-shadow: 
    0 4px 15px rgba(0,0,0,0.3),
    0 0 0 0 rgba(255,255,255,0.5);
  backdrop-filter: blur(10px);
}

.instagram-link:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 
    0 8px 30px rgba(0,0,0,0.4),
    0 0 0 8px rgba(255,255,255,0.2);
  background: white;
}

.instagram-icon {
  font-size: 1.6rem;
  animation: rotate 3s ease-in-out infinite;
}

@keyframes rotate {
  0%, 100% {
    transform: rotate(0deg);
  }
  25% {
    transform: rotate(-10deg);
  }
  75% {
    transform: rotate(10deg);
  }
}

.content-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 60px 40px;
}

.section-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #e0e7ff;
  margin-bottom: 40px;
  text-align: center;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.stats-section {
  margin-bottom: 80px;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 40px;
  max-width: 1400px;
  margin-left: auto;
  margin-right: auto;
}

@media (max-width: 1200px) {
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 768px) {
  .stats-grid {
    grid-template-columns: 1fr;
  }
}

.stat-card {
  background: rgba(30, 41, 59, 0.8);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(14, 165, 233, 0.2);
  border-radius: 20px;
  padding: 30px;
  text-align: center;
  transition: all 0.3s;
}

.stat-card:hover {
  transform: translateY(-5px);
  border-color: rgba(14, 165, 233, 0.5);
  box-shadow: 0 10px 30px rgba(14, 165, 233, 0.2);
}

.stat-icon {
  font-size: 3rem;
  margin-bottom: 15px;
}

.stat-value {
  font-size: 2rem;
  font-weight: 700;
  color: #0ea5e9;
  margin-bottom: 10px;
}

.stat-label {
  font-size: 1rem;
  color: #94a3b8;
  text-transform: uppercase;
  letter-spacing: 1px;
  margin-bottom: 5px;
}

.stat-detail {
  font-size: 0.9rem;
  color: #64748b;
  margin-top: 8px;
}

.gallery-section {
  margin-bottom: 60px;
}

.gallery-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: 15px;
  margin-top: 20px;
}

.gallery-item {
  position: relative;
  overflow: hidden;
  border-radius: 12px;
  aspect-ratio: 1;
  border: 1px solid rgba(14, 165, 233, 0.2);
  transition: all 0.3s;
}

.gallery-item:hover {
  transform: scale(1.05);
  border-color: rgba(14, 165, 233, 0.5);
  box-shadow: 0 8px 20px rgba(14, 165, 233, 0.3);
}

.gallery-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  display: block;
}

.activities-section {
  margin-bottom: 60px;
}

.loading, .error {
  text-align: center;
  font-size: 1.2rem;
  color: #94a3b8;
  padding: 40px;
}

.error {
  color: #ef4444;
}

.activities-grid {
  display: grid;
  gap: 30px;
}

.activity-card {
  background: rgba(30, 41, 59, 0.8);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(14, 165, 233, 0.2);
  border-radius: 20px;
  padding: 30px;
  transition: all 0.3s;
}

.activity-card:hover {
  transform: translateX(5px);
  border-color: rgba(14, 165, 233, 0.5);
  box-shadow: 0 10px 30px rgba(14, 165, 233, 0.2);
}

.activity-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  flex-wrap: wrap;
  gap: 10px;
}

.activity-title {
  font-size: 1.8rem;
  font-weight: 700;
  color: #e0e7ff;
  margin: 0;
}

.activity-title-link {
  color: #e0e7ff;
  text-decoration: none;
  transition: color 0.3s;
}

.activity-title-link:hover {
  color: #fc4c02;
}

.activity-date {
  font-size: 0.95rem;
  color: #94a3b8;
}

.activity-metrics {
  display: flex;
  gap: 20px;
  flex-wrap: wrap;
  margin-bottom: 20px;
  padding-bottom: 20px;
  border-bottom: 1px solid rgba(148, 163, 184, 0.2);
}

.metric {
  display: flex;
  align-items: center;
  gap: 8px;
}

.metric-icon {
  font-size: 1.2rem;
}

.metric-value {
  font-size: 1.1rem;
  font-weight: 600;
  color: #0ea5e9;
}

.activity-location {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 1rem;
  color: #94a3b8;
  margin-bottom: 15px;
}

.location-icon {
  font-size: 1.2rem;
}

.activity-description {
  font-size: 1.05rem;
  line-height: 1.7;
  color: #cbd5e1;
  margin-bottom: 20px;
}

.activity-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 20px;
}

.tag {
  background: rgba(14, 165, 233, 0.1);
  color: #0ea5e9;
  padding: 6px 15px;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: 500;
  border: 1px solid rgba(14, 165, 233, 0.3);
}

.instagram-post-link {
  display: inline-flex;
  align-items: center;
  gap: 5px;
  color: #a855f7;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s;
}

.instagram-post-link:hover {
  color: #c084fc;
  gap: 10px;
}

@media (max-width: 768px) {
  .hero-section {
    padding: 80px 20px;
  }

  .hero-title {
    font-size: 2.2rem;
  }

  .hero-subtitle {
    font-size: 1.1rem;
  }

  .instagram-link {
    font-size: 1rem;
    padding: 14px 28px;
  }

  .content-container {
    padding: 40px 20px;
  }

  .stats-grid {
    grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
    gap: 15px;
  }

  .stat-card {
    padding: 20px;
  }

  .activity-card {
    padding: 20px;
  }

  .activity-metrics {
    gap: 15px;
  }

  .gallery-grid {
    grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
    gap: 10px;
  }
}
</style>
