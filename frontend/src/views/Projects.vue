<template>
  <div class="projects-page">
    <!-- Header -->
    <section class="projects-header">
      <div class="container">
        <h1 class="page-title">Featured Projects</h1>
        <p class="page-subtitle">
          A showcase of my recent work in full-stack development, cloud architecture, and team leadership
        </p>
      </div>
    </section>

    <!-- Projects Grid -->
    <section class="projects-section">
      <div class="container">
        <div class="projects-grid">
          <div v-for="(project, index) in projects" :key="index" class="project-card">
            <div class="project-icon">{{ project.icon }}</div>
            <h3 class="project-title">{{ project.title }}</h3>
            <p class="project-description">{{ project.description }}</p>
            <div class="project-tech">
              <span v-for="(tech, tIndex) in project.technologies" :key="tIndex" class="tech-tag">
                {{ tech }}
              </span>
            </div>
            <div class="project-highlights">
              <div v-for="(highlight, hIndex) in project.highlights" :key="hIndex" class="highlight-item">
                <span class="highlight-icon">✓</span>
                <span>{{ highlight }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  name: 'Projects',
  data() {
    return {
      projects: [
        {
          icon: '🏫',
          title: 'School Management System',
          description: 'Enterprise-level management platform for independent schools with 50,000+ active users',
          technologies: ['.NET Core', 'Angular', 'Azure', 'SQL Server', 'Redis'],
          highlights: [
            'Led team of 5 developers through full SDLC',
            'Architected microservices on Azure with 99.9% uptime',
            'Reduced page load times by 60% through optimization'
          ]
        },
        {
          icon: '☁️',
          title: 'Cloud Migration Initiative',
          description: 'Successfully migrated legacy on-premises applications to Azure cloud infrastructure',
          technologies: ['Azure', 'Docker', 'Kubernetes', 'CI/CD', 'DevOps'],
          highlights: [
            'Reduced infrastructure costs by 40%',
            'Implemented automated deployment pipelines',
            'Zero downtime migration strategy'
          ]
        },
        {
          icon: '📊',
          title: 'Real-Time Analytics Dashboard',
          description: 'Interactive data visualization platform for business intelligence and reporting',
          technologies: ['React', 'D3.js', 'ASP.NET Core', 'SignalR', 'MongoDB'],
          highlights: [
            'Real-time data updates via WebSockets',
            'Custom chart components for complex visualizations',
            'Export functionality for multiple formats'
          ]
        },
        {
          icon: '🔐',
          title: 'Authentication & Identity Platform',
          description: 'Secure SSO system with OAuth 2.0 and SAML integration for enterprise clients',
          technologies: ['Identity Server', 'OAuth 2.0', 'SAML', 'Azure AD', '.NET'],
          highlights: [
            'Integrated with multiple identity providers',
            'Implemented MFA and conditional access',
            'GDPR and security compliance'
          ]
        },
        {
          icon: '📱',
          title: 'Mobile-First Progressive Web App',
          description: 'Cross-platform PWA for field service management with offline capabilities',
          technologies: ['Vue.js', 'PWA', 'IndexedDB', 'Web Workers', 'Azure'],
          highlights: [
            'Works offline with local data sync',
            'Native app-like experience',
            'Push notifications integration'
          ]
        },
        {
          icon: '🤖',
          title: 'Automated Testing Framework',
          description: 'Comprehensive testing solution reducing manual QA time by 70%',
          technologies: ['xUnit', 'Selenium', 'SpecFlow', 'Azure DevOps', 'C#'],
          highlights: [
            'Automated regression test suite',
            'Integrated into CI/CD pipeline',
            'Reduced bug escape rate by 50%'
          ]
        }
      ]
    }
  }
}
</script>

<style scoped>
.projects-page {
  min-height: 100vh;
  padding-bottom: 80px;
}

.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 40px;
}

/* Header */
.projects-header {
  padding: 120px 40px 80px;
  text-align: center;
  background: 
    radial-gradient(circle at 50% 50%, rgba(14, 165, 233, 0.15) 0%, transparent 70%),
    radial-gradient(circle at 80% 20%, rgba(168, 85, 247, 0.15) 0%, transparent 70%);
  position: relative;
  overflow: hidden;
}

.projects-header::before {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: 
    radial-gradient(circle, rgba(255,255,255,0.03) 2px, transparent 2px);
  background-size: 50px 50px;
  animation: gridMove 20s linear infinite;
}

@keyframes gridMove {
  0% { transform: translate(0, 0); }
  100% { transform: translate(50px, 50px); }
}

.page-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 4.5rem;
  font-weight: 700;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 25px;
  letter-spacing: -3px;
  animation: fadeInUp 0.8s ease-out;
  position: relative;
  z-index: 1;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.page-subtitle {
  font-size: 1.4rem;
  color: #94a3b8;
  max-width: 800px;
  margin: 0 auto;
  line-height: 1.8;
  font-weight: 300;
  animation: fadeInUp 0.8s ease-out 0.2s both;
  position: relative;
  z-index: 1;
}

/* Projects Section */
.projects-section {
  padding: 60px 0;
}

.projects-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 40px;
}

.project-card {
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

.project-card::before {
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

.project-card:hover::before {
  transform: scaleX(1);
}

.project-card:hover {
  transform: translateY(-12px) scale(1.02);
  box-shadow: 
    0 0 60px rgba(14, 165, 233, 0.3),
    0 40px 80px rgba(0,0,0,0.3);
  border-color: rgba(14, 165, 233, 0.4);
}

.project-icon {
  font-size: 4rem;
  margin-bottom: 25px;
  filter: drop-shadow(0 0 20px rgba(14, 165, 233, 0.5));
  animation: float 3s ease-in-out infinite;
}

@keyframes float {
  0%, 100% { transform: translateY(0px); }
  50% { transform: translateY(-10px); }
}

.project-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 2rem;
  color: #e0e7ff;
  margin-bottom: 20px;
  font-weight: 700;
  letter-spacing: -0.5px;
}

.project-description {
  color: #94a3b8;
  line-height: 1.8;
  font-size: 1.05rem;
  margin-bottom: 25px;
  font-weight: 300;
}

.project-tech {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 30px;
}

.tech-tag {
  background: rgba(14, 165, 233, 0.1);
  backdrop-filter: blur(10px);
  color: #0ea5e9;
  padding: 8px 18px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 600;
  border: 1px solid rgba(14, 165, 233, 0.3);
  transition: all 0.3s;
  letter-spacing: 0.5px;
}

.tech-tag:hover {
  background: rgba(14, 165, 233, 0.2);
  transform: scale(1.05);
  box-shadow: 0 0 15px rgba(14, 165, 233, 0.4);
}

.project-highlights {
  display: flex;
  flex-direction: column;
  gap: 15px;
  padding-top: 25px;
  border-top: 1px solid rgba(255,255,255,0.1);
}

.highlight-item {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  color: #cbd5e1;
  font-size: 0.95rem;
  line-height: 1.6;
  font-weight: 300;
}

.highlight-icon {
  color: #0ea5e9;
  font-weight: bold;
  font-size: 1.1rem;
  background: rgba(14, 165, 233, 0.15);
  width: 22px;
  height: 22px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  font-size: 0.8rem;
  box-shadow: 0 0 15px rgba(14, 165, 233, 0.4);
  border: 1px solid rgba(14, 165, 233, 0.3);
}

/* Responsive */
@media (max-width: 1200px) {
  .projects-grid {
    grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  }
}

@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }

  .projects-header {
    padding: 80px 20px 60px;
  }

  .page-title {
    font-size: 3rem;
  }

  .page-subtitle {
    font-size: 1.2rem;
  }

  .projects-grid {
    grid-template-columns: 1fr;
    gap: 30px;
  }

  .project-card {
    padding: 40px 30px;
  }

  .project-title {
    font-size: 1.7rem;
  }
}
</style>
