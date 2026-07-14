<template>
  <div id="app">
    <div v-if="loading" class="loading">
      <p>Loading...</p>
    </div>

    <div v-else-if="error" class="error-container">
      <p class="error">{{ error }}</p>
    </div>

    <div v-else class="portfolio">
      <!-- Hero Section -->
      <header class="hero">
        <div class="container">
          <h1 class="name">{{ cvData.personalInfo.name }}</h1>
          <p class="title">{{ cvData.personalInfo.title }}</p>
          <div class="contact-info">
            <a :href="`mailto:${cvData.personalInfo.email}`" class="contact-link">
              <span class="icon">✉</span> {{ cvData.personalInfo.email }}
            </a>
            <span class="contact-item">
              <span class="icon">📱</span> {{ cvData.personalInfo.phone }}
            </span>
            <span class="contact-item">
              <span class="icon">📍</span> {{ cvData.personalInfo.location }}
            </span>
          </div>
        </div>
      </header>

      <!-- Navigation -->
      <nav class="nav">
        <div class="container">
          <a href="#profile" class="nav-link">Profile</a>
          <a href="#skills" class="nav-link">Skills</a>
          <a href="#experience" class="nav-link">Experience</a>
          <a href="#education" class="nav-link">Education</a>
          <a href="#leisure" class="nav-link">Leisure</a>
        </div>
      </nav>

      <div class="container">
        <!-- Profile Section -->
        <section id="profile" class="section">
          <h2 class="section-title">Personal Profile</h2>
          <div class="profile-content">
            <p v-for="(paragraph, index) in cvData.profile.split('\n\n')" :key="index">
              {{ paragraph }}
            </p>
          </div>
        </section>

        <!-- Skills Section -->
        <section id="skills" class="section">
          <h2 class="section-title">Key Skills & Tools</h2>
          <div class="skills-grid">
            <div class="skills-column">
              <h3>Core Skills</h3>
              <ul class="skills-list">
                <li v-for="(skill, index) in cvData.keySkills" :key="index">{{ skill }}</li>
              </ul>
            </div>
            <div class="skills-column">
              <h3>Tools & Technologies</h3>
              <ul class="skills-list">
                <li v-for="(tool, index) in cvData.tools" :key="index">{{ tool }}</li>
              </ul>
            </div>
          </div>
        </section>

        <!-- Work Experience Section -->
        <section id="experience" class="section">
          <h2 class="section-title">Work Experience</h2>
          <div class="timeline">
            <div v-for="(job, index) in cvData.workExperience" :key="index" class="timeline-item">
              <div class="timeline-marker"></div>
              <div class="timeline-content">
                <div class="job-header">
                  <h3 class="job-title">{{ job.position }}</h3>
                  <span class="job-date">{{ job.startDate }} - {{ job.endDate }}</span>
                </div>
                <p class="job-company">{{ job.company }} | {{ job.location }}</p>
                <p class="job-description">{{ job.description }}</p>
                <ul class="job-highlights" v-if="job.highlights.length > 0">
                  <li v-for="(highlight, hIndex) in job.highlights" :key="hIndex">{{ highlight }}</li>
                </ul>
                <div class="tech-stack" v-if="job.techStack">
                  <strong>Tech Stack:</strong> {{ job.techStack }}
                </div>
              </div>
            </div>
          </div>
        </section>

        <!-- Education Section -->
        <section id="education" class="section">
          <h2 class="section-title">Education</h2>
          <div class="education-content">
            <div class="education-item">
              <h3>{{ cvData.education.higher.course }}</h3>
              <p class="education-institution">{{ cvData.education.higher.university }}</p>
              <p class="education-grade">{{ cvData.education.higher.grade }} | {{ cvData.education.higher.dates }}</p>
            </div>
            <div v-for="(edu, index) in cvData.education.secondary" :key="index" class="education-item">
              <h3>{{ edu.qualification }}</h3>
              <p class="education-institution">{{ edu.institution }}</p>
              <p class="education-grade">{{ edu.grade }} | {{ edu.date }}</p>
            </div>
          </div>
        </section>

        <!-- Leisure Section -->
        <section id="leisure" class="section">
          <h2 class="section-title">Leisure Activities</h2>
          <div class="leisure-content">
            <p v-for="(paragraph, index) in cvData.leisureActivities.split('\n\n')" :key="index">
              {{ paragraph }}
            </p>
          </div>
        </section>
      </div>

      <!-- Footer -->
      <footer class="footer">
        <div class="container">
          <p>&copy; {{ new Date().getFullYear() }} {{ cvData.personalInfo.name }}. All rights reserved.</p>
        </div>
      </footer>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      cvData: {
        personalInfo: {},
        profile: '',
        keySkills: [],
        tools: [],
        workExperience: [],
        education: { higher: {}, secondary: [] },
        leisureActivities: ''
      },
      loading: true,
      error: ''
    }
  },
  async mounted() {
    await this.fetchCV();
  },
  methods: {
    async fetchCV() {
      try {
        this.loading = true;
        this.error = '';
        const response = await fetch('http://localhost:5000/api/cv');

        if (!response.ok) {
          throw new Error('Failed to load CV data');
        }

        this.cvData = await response.json();
      } catch (err) {
        this.error = 'Error loading CV data: ' + err.message;
        console.error('Error:', err);
      } finally {
        this.loading = false;
      }
    }
  }
}
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap');

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  line-height: 1.7;
  color: #1a202c;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  background-attachment: fixed;
}

#app {
  min-height: 100vh;
}

/* Loading & Error States */
.loading, .error-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  font-size: 1.2rem;
}

.loading {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  font-weight: 600;
}

.error {
  color: #e53e3e;
  padding: 30px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 20px 60px rgba(0,0,0,0.3);
  border-left: 5px solid #e53e3e;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

/* Hero Section - Modern Gradient with Background Image */
.hero {
  background: 
    linear-gradient(135deg, rgba(102, 126, 234, 0.9) 0%, rgba(118, 75, 162, 0.9) 100%),
    url('https://images.unsplash.com/photo-1517694712202-14dd9538aa97?w=1920&q=80') center/cover;
  color: white;
  padding: 120px 20px 80px;
  text-align: center;
  position: relative;
  overflow: hidden;
  background-attachment: fixed;
}

.hero::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: 
    radial-gradient(circle at 20% 50%, rgba(255,255,255,0.15) 0%, transparent 50%),
    radial-gradient(circle at 80% 80%, rgba(255,255,255,0.15) 0%, transparent 50%);
  animation: float 20s ease-in-out infinite;
  pointer-events: none;
}

@keyframes float {
  0%, 100% { transform: translateY(0px); }
  50% { transform: translateY(-20px); }
}

.hero .container {
  position: relative;
  z-index: 1;
}

.name {
  font-size: 4rem;
  font-weight: 800;
  margin-bottom: 15px;
  text-shadow: 2px 4px 8px rgba(0,0,0,0.3);
  animation: fadeInUp 0.8s ease-out;
  letter-spacing: -2px;
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

.title {
  font-size: 1.6rem;
  font-weight: 400;
  margin-bottom: 40px;
  opacity: 0.95;
  animation: fadeInUp 0.8s ease-out 0.2s both;
}

.contact-info {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 35px;
  font-size: 1rem;
  animation: fadeInUp 0.8s ease-out 0.4s both;
}

.contact-link, .contact-item {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  color: white;
  text-decoration: none;
  transition: all 0.3s;
  padding: 8px 16px;
  border-radius: 8px;
  background: rgba(255,255,255,0.1);
  backdrop-filter: blur(10px);
}

.contact-link:hover {
  background: rgba(255,255,255,0.2);
  transform: translateY(-3px);
}

.icon {
  font-size: 1.3rem;
}

/* Navigation - Glassmorphism Style */
.nav {
  background: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(10px);
  box-shadow: 0 8px 32px rgba(0,0,0,0.1);
  position: sticky;
  top: 0;
  z-index: 100;
  border-bottom: 1px solid rgba(255,255,255,0.5);
}

.nav .container {
  display: flex;
  justify-content: center;
  gap: 50px;
  padding: 20px;
}

.nav-link {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s;
  padding: 8px 16px;
  border-radius: 8px;
  position: relative;
}

.nav-link:hover {
  color: #764ba2;
  background: rgba(102, 126, 234, 0.1);
  transform: translateY(-2px);
}

.nav-link::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 0;
  height: 2px;
  background: #764ba2;
  transition: all 0.3s;
  transform: translateX(-50%);
}

.nav-link:hover::after {
  width: 80%;
}

/* Sections - Modern Card Style */
.section {
  background: white;
  margin: 50px auto;
  padding: 60px 50px;
  border-radius: 20px;
  box-shadow: 0 20px 60px rgba(0,0,0,0.1);
  animation: fadeIn 0.6s ease-out;
  border: 1px solid rgba(255,255,255,0.5);
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.section-title {
  font-size: 2.5rem;
  font-weight: 800;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 40px;
  padding-bottom: 20px;
  border-bottom: 3px solid;
  border-image: linear-gradient(90deg, #667eea 0%, #764ba2 100%) 1;
  letter-spacing: -1px;
}

/* Profile Section */
.profile-content p {
  margin-bottom: 20px;
  font-size: 1.1rem;
  line-height: 1.8;
  color: #4a5568;
}

/* Skills Section - Modern Card Grid */
.skills-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 30px;
}

.skills-column {
  background: linear-gradient(135deg, #f8f9ff 0%, #e8ebff 100%);
  padding: 30px;
  border-radius: 16px;
  box-shadow: 0 10px 30px rgba(102, 126, 234, 0.1);
  transition: all 0.3s;
  border: 1px solid rgba(102, 126, 234, 0.2);
}

.skills-column:hover {
  transform: translateY(-8px);
  box-shadow: 0 20px 40px rgba(102, 126, 234, 0.2);
}

.skills-column h3 {
  color: #764ba2;
  margin-bottom: 25px;
  font-size: 1.5rem;
  font-weight: 700;
}

.skills-list {
  list-style: none;
}

.skills-list li {
  padding: 14px 0;
  padding-left: 30px;
  position: relative;
  border-bottom: 1px solid rgba(102, 126, 234, 0.2);
  transition: all 0.3s;
  font-weight: 500;
  color: #2d3748;
}

.skills-list li:hover {
  padding-left: 35px;
  color: #667eea;
}

.skills-list li:before {
  content: '◆';
  position: absolute;
  left: 0;
  color: #667eea;
  font-weight: bold;
  font-size: 1.2rem;
}

/* Timeline - Modern Design */
.timeline {
  position: relative;
  padding-left: 50px;
}

.timeline:before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 4px;
  background: linear-gradient(180deg, #667eea 0%, #764ba2 100%);
  border-radius: 2px;
  box-shadow: 0 0 10px rgba(102, 126, 234, 0.5);
}

.timeline-item {
  position: relative;
  margin-bottom: 60px;
  transition: all 0.3s;
}

.timeline-item:hover .timeline-marker {
  transform: scale(1.3);
}

.timeline-marker {
  position: absolute;
  left: -57px;
  top: 5px;
  width: 18px;
  height: 18px;
  border-radius: 50%;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: 4px solid white;
  box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.3);
  transition: all 0.3s;
  z-index: 1;
}

.timeline-content {
  padding: 25px 30px;
  background: linear-gradient(135deg, #ffffff 0%, #f8f9ff 100%);
  border-radius: 16px;
  box-shadow: 0 10px 30px rgba(0,0,0,0.08);
  border-left: 4px solid #667eea;
  transition: all 0.3s;
}

.timeline-content:hover {
  transform: translateX(10px);
  box-shadow: 0 15px 40px rgba(0,0,0,0.12);
}

.job-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 15px;
  flex-wrap: wrap;
  gap: 15px;
}

.job-title {
  font-size: 1.6rem;
  font-weight: 700;
  color: #2d3748;
  letter-spacing: -0.5px;
}

.job-date {
  color: white;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 6px 16px;
  border-radius: 20px;
  font-weight: 600;
  font-size: 0.9rem;
  box-shadow: 0 4px 10px rgba(102, 126, 234, 0.3);
}

.job-company {
  color: #718096;
  font-weight: 600;
  margin-bottom: 15px;
  font-size: 1.1rem;
}

.job-description {
  margin-bottom: 20px;
  color: #4a5568;
  line-height: 1.8;
}

.job-highlights {
  margin: 20px 0;
  padding-left: 0;
  list-style: none;
}

.job-highlights li {
  margin-bottom: 12px;
  padding-left: 30px;
  position: relative;
  color: #4a5568;
  line-height: 1.7;
}

.job-highlights li:before {
  content: '✓';
  position: absolute;
  left: 0;
  color: #667eea;
  font-weight: bold;
  font-size: 1.2rem;
  background: rgba(102, 126, 234, 0.1);
  width: 22px;
  height: 22px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.9rem;
}

.tech-stack {
  margin-top: 20px;
  padding: 18px 20px;
  background: linear-gradient(135deg, #e8ebff 0%, #d4d9ff 100%);
  border-radius: 10px;
  border-left: 4px solid #764ba2;
  font-size: 0.95rem;
  color: #4a5568;
  font-weight: 500;
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.15);
}

.tech-stack strong {
  color: #764ba2;
}

/* Education Section - Card Style */
.education-content {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.education-item {
  padding: 30px;
  background: linear-gradient(135deg, #f8f9ff 0%, #e8ebff 100%);
  border-radius: 16px;
  border-left: 5px solid #764ba2;
  box-shadow: 0 10px 30px rgba(118, 75, 162, 0.1);
  transition: all 0.3s;
}

.education-item:hover {
  transform: translateX(10px);
  box-shadow: 0 15px 40px rgba(118, 75, 162, 0.2);
}

.education-item h3 {
  color: #2d3748;
  margin-bottom: 12px;
  font-size: 1.4rem;
  font-weight: 700;
}

.education-institution {
  color: #718096;
  font-weight: 600;
  font-size: 1.05rem;
}

.education-grade {
  color: #667eea;
  font-weight: 600;
  margin-top: 8px;
  font-size: 1.05rem;
}

/* Leisure Section */
.leisure-content p {
  margin-bottom: 20px;
  font-size: 1.1rem;
  line-height: 1.8;
  color: #4a5568;
}

/* Footer - Modern Style */
.footer {
  background: linear-gradient(135deg, #2d3748 0%, #1a202c 100%);
  color: white;
  text-align: center;
  padding: 40px 20px;
  margin-top: 80px;
  box-shadow: 0 -10px 30px rgba(0,0,0,0.1);
}

/* Responsive Design */
@media (max-width: 768px) {
  .name {
    font-size: 2.5rem;
    letter-spacing: -1px;
  }

  .title {
    font-size: 1.3rem;
  }

  .hero {
    padding: 80px 20px 60px;
  }

  .contact-info {
    flex-direction: column;
    gap: 15px;
  }

  .nav .container {
    gap: 15px;
    flex-wrap: wrap;
    padding: 15px;
  }

  .nav-link {
    font-size: 0.9rem;
    padding: 6px 12px;
  }

  .section {
    padding: 40px 25px;
    margin: 30px 15px;
    border-radius: 16px;
  }

  .section-title {
    font-size: 2rem;
  }

  .job-header {
    flex-direction: column;
  }

  .timeline {
    padding-left: 35px;
  }

  .timeline-marker {
    left: -43px;
  }

  .timeline-content {
    padding: 20px;
  }

  .skills-grid {
    grid-template-columns: 1fr;
  }
}
</style>
