<template>
  <div class="services-page">
    <div v-if="loading" class="loading">
      <p>Loading Services...</p>
    </div>

    <div v-else-if="error" class="error-container">
      <p class="error">{{ error }}</p>
    </div>

    <div v-else class="services-content">
      <!-- Hero Section -->
      <header class="hero">
        <div class="hero-background">
          <!-- Code snippets background -->
          <div class="code-background">
            <pre class="code-snippet code-snippet-1">
public class ServiceProvider {
    public async Task&lt;Result&gt; Deliver() {
        return await Analyze()
            .Design()
            .Implement()
            .Deploy();
    }
}</pre>
            <pre class="code-snippet code-snippet-2">
const services = {
  leadership: true,
  cloudArchitecture: true,
  aiIntegration: true,
  modernization: true
};</pre>
            <pre class="code-snippet code-snippet-3">
app.UseModernization();
app.UseAI();
app.UseExcellence();
app.Run();</pre>
            <pre class="code-snippet code-snippet-4">
[HttpPost("transform")]
public async Task&lt;IActionResult&gt; Transform()
{
    var result = await modernize.Execute();
    return Ok(result);
}</pre>
          </div>

          <div class="gradient-orb orb-1"></div>
          <div class="gradient-orb orb-2"></div>
          <div class="gradient-orb orb-3"></div>
        </div>
        <div class="container">
          <div class="hero-content">
            <h1 class="page-title">Services</h1>
            <p class="page-subtitle">
              Expert consulting in technical leadership, cloud architecture, AI integration, and modern .NET development
            </p>
          </div>
        </div>
      </header>

      <div class="container">
        <!-- Service Categories -->
        <div 
          v-for="(category, categoryIndex) in servicesData.categories" 
          :key="categoryIndex"
          :id="slugify(category.categoryName)"
          class="section"
        >
          <div class="category-header">
            <h2 class="category-title">
              <span class="category-icon">{{ category.icon }}</span>
              {{ category.categoryName }}
            </h2>
            <p class="category-description">{{ category.categoryDescription }}</p>
          </div>

          <div class="services-grid">
            <div 
              v-for="(service, serviceIndex) in category.services" 
              :key="serviceIndex"
              class="service-card"
            >
              <h3 class="service-name">{{ service.name }}</h3>
              <p class="service-description">{{ service.description }}</p>

              <div class="service-section" v-if="service.keyFeatures.length > 0">
                <h4 class="service-section-title">Key Features</h4>
                <ul class="service-list">
                  <li v-for="(feature, fIndex) in service.keyFeatures" :key="fIndex">
                    <svg class="check-icon" viewBox="0 0 20 20" fill="currentColor">
                      <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd" />
                    </svg>
                    {{ feature }}
                  </li>
                </ul>
              </div>

              <div class="service-section" v-if="service.technologies.length > 0">
                <h4 class="service-section-title">Technologies</h4>
                <div class="tech-badges">
                  <span 
                    v-for="(tech, tIndex) in service.technologies" 
                    :key="tIndex"
                    class="tech-badge"
                  >
                    {{ tech }}
                  </span>
                </div>
              </div>

              <div class="service-section" v-if="service.deliverables.length > 0">
                <h4 class="service-section-title">Deliverables</h4>
                <ul class="service-list deliverables-list">
                  <li v-for="(deliverable, dIndex) in service.deliverables" :key="dIndex">
                    <svg class="package-icon" viewBox="0 0 20 20" fill="currentColor">
                      <path d="M3 4a1 1 0 011-1h12a1 1 0 011 1v2a1 1 0 01-1 1H4a1 1 0 01-1-1V4zM3 10a1 1 0 011-1h6a1 1 0 011 1v6a1 1 0 01-1 1H4a1 1 0 01-1-1v-6zM14 9a1 1 0 00-1 1v6a1 1 0 001 1h2a1 1 0 001-1v-6a1 1 0 00-1-1h-2z" />
                    </svg>
                    {{ deliverable }}
                  </li>
                </ul>
              </div>

              <div class="ideal-for" v-if="service.idealFor">
                <strong>Ideal for:</strong> {{ service.idealFor }}
              </div>
            </div>
          </div>
        </div>

        <!-- CTA Section -->
        <section class="cta-section">
          <div class="cta-card">
            <h2>Ready to Get Started?</h2>
            <p>Let's discuss how I can help transform your project into a success story.</p>
            <router-link to="/contact" class="cta-button">Get in Touch</router-link>
          </div>
        </section>
      </div>
    </div>
  </div>
</template>

<script>
import config from '../config';

export default {
  name: 'Services',
  data() {
    return {
      servicesData: {
        categories: []
      },
      loading: true,
      error: ''
    }
  },
  async mounted() {
    await this.fetchServices();
  },
  methods: {
    async fetchServices() {
      try {
        this.loading = true;
        this.error = '';
        const response = await fetch(`${config.apiBaseUrl}/api/services`);

        if (!response.ok) {
          throw new Error('Failed to load services data');
        }

        this.servicesData = await response.json();
      } catch (err) {
        this.error = 'Error loading services data: ' + err.message;
        console.error('Error:', err);
      } finally {
        this.loading = false;
      }
    },
    slugify(text) {
      return text
        .toLowerCase()
        .replace(/[^\w\s-]/g, '')
        .replace(/[\s_-]+/g, '-')
        .replace(/^-+|-+$/g, '');
    }
  }
}
</script>

<style scoped>
/* Base Styles */
.services-page {
  min-height: 100vh;
  padding-bottom: 80px;
}

.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 40px;
  position: relative;
  z-index: 1;
}

/* Loading & Error States */
.loading, .error-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 60vh;
  font-size: 1.25rem;
  color: #94a3b8;
}

.error {
  color: #ef4444;
  background: rgba(239, 68, 68, 0.1);
  padding: 1rem 2rem;
  border-radius: 8px;
  border: 1px solid rgba(239, 68, 68, 0.2);
}

/* Hero Section */
.hero {
  background: linear-gradient(135deg, #0a0e27 0%, #1a1f3a 100%);
  color: white;
  padding: 100px 20px 80px;
  position: relative;
  overflow: hidden;
}

.hero-background {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  overflow: hidden;
}
/* Code Background */
.code-background {
  position: absolute;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 0;
}

.code-snippet {
  position: absolute;
  font-family: 'Fira Code', 'Courier New', monospace;
  font-size: 0.85rem;
  line-height: 1.6;
  color: rgba(14, 165, 233, 0.3);
  white-space: pre;
  pointer-events: none;
}

.code-snippet-1 {
  top: 10%;
  left: 5%;
  transform: rotate(-2deg);
}

.code-snippet-2 {
  top: 15%;
  right: 8%;
  transform: rotate(3deg);
  color: rgba(168, 85, 247, 0.3);
}

.code-snippet-3 {
  bottom: 20%;
  left: 8%;
  transform: rotate(2deg);
}

.code-snippet-4 {
  bottom: 15%;
  right: 10%;
  transform: rotate(-3deg);
  color: rgba(0, 245, 255, 0.25);
}

.gradient-orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  opacity: 0.6;
  animation: float 20s ease-in-out infinite;
}

.orb-1 {
  width: 400px;
  height: 400px;
  background: radial-gradient(circle, rgba(14, 165, 233, 0.4) 0%, transparent 70%);
  top: -200px;
  left: -100px;
  animation-delay: 0s;
}

.orb-2 {
  width: 500px;
  height: 500px;
  background: radial-gradient(circle, rgba(168, 85, 247, 0.3) 0%, transparent 70%);
  top: -150px;
  right: -150px;
  animation-delay: -7s;
}

.orb-3 {
  width: 350px;
  height: 350px;
  background: radial-gradient(circle, rgba(0, 245, 255, 0.25) 0%, transparent 70%);
  bottom: -100px;
  left: 50%;
  transform: translateX(-50%);
  animation-delay: -14s;
}

@keyframes float {
  0%, 100% {
    transform: translate(0, 0) scale(1);
  }
  33% {
    transform: translate(30px, -30px) scale(1.1);
  }
  66% {
    transform: translate(-20px, 20px) scale(0.9);
  }
}

.hero-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
  text-align: center;
}

.page-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 4rem;
  font-weight: 700;
  margin: 0;
  letter-spacing: -2px;
  line-height: 1;
  background: linear-gradient(135deg, #ffffff 0%, #00f5ff 50%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  animation: gradient-shift 8s ease infinite;
  background-size: 200% 200%;
}

@keyframes gradient-shift {
  0%, 100% {
    background-position: 0% 50%;
  }
  50% {
    background-position: 100% 50%;
  }
}

.page-subtitle {
  font-size: 1.3rem;
  color: rgba(255, 255, 255, 0.8);
  max-width: 700px;
  margin: 0;
  line-height: 1.6;
  font-weight: 300;
}

/* Section Styles */
.section {
  padding: 60px 0;
  margin-bottom: 0;
}

.category-header {
  text-align: center;
  margin-bottom: 3rem;
}

.category-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #f1f5f9;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
}

.category-icon {
  font-size: 3rem;
  display: inline-block;
}

.category-description {
  font-size: 1.25rem;
  color: #94a3b8;
  max-width: 700px;
  margin: 0 auto;
}

/* Services Grid */
.services-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 2rem;
}

.service-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px) saturate(180%);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 24px;
  padding: 50px 40px;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  box-shadow: 
    0 0 30px rgba(14, 165, 233, 0.1),
    0 30px 60px rgba(0,0,0,0.2);
  position: relative;
  overflow: hidden;
}

.service-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 4px;
  background: linear-gradient(90deg, #0ea5e9, #a855f7);
  transform: scaleX(0);
  transition: transform 0.4s;
  transform-origin: left;
}

.service-card:hover::before {
  transform: scaleX(1);
}

.service-card:hover {
  transform: translateY(-12px) scale(1.02);
  box-shadow: 
    0 0 60px rgba(14, 165, 233, 0.3),
    0 40px 80px rgba(0,0,0,0.3);
  border-color: rgba(14, 165, 233, 0.4);
}

.service-name {
  font-size: 1.5rem;
  font-weight: 700;
  color: #f1f5f9;
  margin-bottom: 1rem;
}

.service-description {
  color: #cbd5e1;
  line-height: 1.7;
  margin-bottom: 1.5rem;
}

.service-section {
  margin-bottom: 1.5rem;
}

.service-section-title {
  font-size: 1rem;
  font-weight: 600;
  color: #60a5fa;
  margin-bottom: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.service-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.service-list li {
  display: flex;
  align-items: flex-start;
  gap: 0.75rem;
  color: #cbd5e1;
  margin-bottom: 0.5rem;
  line-height: 1.6;
}

.check-icon, .package-icon {
  width: 1.25rem;
  height: 1.25rem;
  flex-shrink: 0;
  color: #60a5fa;
  margin-top: 0.15rem;
}

.tech-badges {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.tech-badge {
  background: rgba(59, 130, 246, 0.15);
  border: 1px solid rgba(59, 130, 246, 0.3);
  color: #93c5fd;
  padding: 0.375rem 0.875rem;
  border-radius: 0.375rem;
  font-size: 0.875rem;
  font-weight: 500;
  white-space: nowrap;
}

.ideal-for {
  margin-top: 1.5rem;
  padding-top: 1.5rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  color: #cbd5e1;
  line-height: 1.6;
}

.ideal-for strong {
  color: #60a5fa;
  font-weight: 600;
}

/* CTA Section */
.cta-section {
  padding: 60px 0;
}

.cta-card {
  background: linear-gradient(135deg, rgba(59, 130, 246, 0.15), rgba(139, 92, 246, 0.15));
  border: 1px solid rgba(59, 130, 246, 0.3);
  border-radius: 1rem;
  padding: 3rem 2rem;
  text-align: center;
  backdrop-filter: blur(10px);
}

.cta-card h2 {
  font-size: 2rem;
  font-weight: 700;
  color: #f1f5f9;
  margin-bottom: 1rem;
}

.cta-card p {
  font-size: 1.25rem;
  color: #94a3b8;
  margin-bottom: 2rem;
  max-width: 600px;
  margin-left: auto;
  margin-right: auto;
}

.cta-button {
  display: inline-block;
  background: linear-gradient(135deg, #3b82f6, #8b5cf6);
  color: white;
  padding: 1rem 2.5rem;
  border-radius: 0.5rem;
  font-weight: 600;
  font-size: 1.125rem;
  text-decoration: none;
  transition: all 0.3s ease;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.cta-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 16px rgba(59, 130, 246, 0.3);
}

/* Responsive Design */
@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }

  .hero {
    padding: 80px 20px 60px;
  }

  .page-title {
    font-size: 2.5rem;
  }

  .page-subtitle {
    font-size: 1rem;
  }

  .category-title {
    font-size: 2rem;
  }

  .services-grid {
    grid-template-columns: 1fr;
  }

  .service-card {
    padding: 1.5rem;
  }

  .cta-card {
    padding: 2rem 1.5rem;
  }

  .cta-card h2 {
    font-size: 1.5rem;
  }

  .cta-card p {
    font-size: 1rem;
  }
}
</style>
