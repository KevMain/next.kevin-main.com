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
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  line-height: 1.6;
  color: #333;
  background-color: #f5f5f5;
}

#app {
  min-height: 100vh;
}

.loading, .error-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  font-size: 1.2rem;
}

.error {
  color: #e74c3c;
  padding: 20px;
  background: #ffe6e6;
  border-radius: 4px;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

/* Hero Section */
.hero {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 80px 20px 60px;
  text-align: center;
}

.name {
  font-size: 3rem;
  font-weight: 700;
  margin-bottom: 10px;
  text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
}

.title {
  font-size: 1.5rem;
  font-weight: 300;
  margin-bottom: 30px;
  opacity: 0.95;
}

.contact-info {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 30px;
  font-size: 0.95rem;
}

.contact-link, .contact-item {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  color: white;
  text-decoration: none;
  transition: opacity 0.3s;
}

.contact-link:hover {
  opacity: 0.8;
}

.icon {
  font-size: 1.2rem;
}

/* Navigation */
.nav {
  background: white;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
  position: sticky;
  top: 0;
  z-index: 100;
}

.nav .container {
  display: flex;
  justify-content: center;
  gap: 40px;
  padding: 15px 20px;
}

.nav-link {
  color: #667eea;
  text-decoration: none;
  font-weight: 600;
  transition: color 0.3s;
  padding: 5px 0;
  border-bottom: 2px solid transparent;
}

.nav-link:hover {
  color: #764ba2;
  border-bottom-color: #764ba2;
}

/* Sections */
.section {
  background: white;
  margin: 40px auto;
  padding: 50px 40px;
  border-radius: 8px;
  box-shadow: 0 2px 20px rgba(0,0,0,0.08);
}

.section-title {
  font-size: 2rem;
  color: #667eea;
  margin-bottom: 30px;
  padding-bottom: 15px;
  border-bottom: 3px solid #667eea;
}

/* Profile Section */
.profile-content p {
  margin-bottom: 15px;
  text-align: justify;
}

/* Skills Section */
.skills-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 40px;
}

.skills-column h3 {
  color: #764ba2;
  margin-bottom: 20px;
  font-size: 1.3rem;
}

.skills-list {
  list-style: none;
}

.skills-list li {
  padding: 10px 0;
  padding-left: 25px;
  position: relative;
  border-bottom: 1px solid #eee;
}

.skills-list li:before {
  content: '▸';
  position: absolute;
  left: 0;
  color: #667eea;
  font-weight: bold;
}

/* Timeline */
.timeline {
  position: relative;
  padding-left: 40px;
}

.timeline:before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 3px;
  background: linear-gradient(180deg, #667eea 0%, #764ba2 100%);
}

.timeline-item {
  position: relative;
  margin-bottom: 50px;
}

.timeline-marker {
  position: absolute;
  left: -46px;
  top: 0;
  width: 15px;
  height: 15px;
  border-radius: 50%;
  background: #667eea;
  border: 3px solid white;
  box-shadow: 0 0 0 3px #667eea;
}

.timeline-content {
  padding-left: 20px;
}

.job-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 10px;
  flex-wrap: wrap;
  gap: 10px;
}

.job-title {
  font-size: 1.4rem;
  color: #333;
}

.job-date {
  color: #667eea;
  font-weight: 600;
  font-size: 0.95rem;
}

.job-company {
  color: #666;
  font-weight: 600;
  margin-bottom: 15px;
}

.job-description {
  margin-bottom: 15px;
}

.job-highlights {
  margin: 20px 0;
  padding-left: 20px;
}

.job-highlights li {
  margin-bottom: 10px;
  padding-left: 10px;
}

.tech-stack {
  margin-top: 15px;
  padding: 15px;
  background: #f8f9ff;
  border-left: 4px solid #667eea;
  font-size: 0.9rem;
}

/* Education Section */
.education-content {
  display: flex;
  flex-direction: column;
  gap: 30px;
}

.education-item {
  padding: 20px;
  background: #f8f9ff;
  border-radius: 6px;
  border-left: 4px solid #764ba2;
}

.education-item h3 {
  color: #333;
  margin-bottom: 10px;
}

.education-institution {
  color: #666;
  font-weight: 600;
}

.education-grade {
  color: #667eea;
  font-weight: 500;
  margin-top: 5px;
}

/* Leisure Section */
.leisure-content p {
  margin-bottom: 15px;
  text-align: justify;
}

/* Footer */
.footer {
  background: #2c3e50;
  color: white;
  text-align: center;
  padding: 30px 20px;
  margin-top: 60px;
}

/* Responsive Design */
@media (max-width: 768px) {
  .name {
    font-size: 2rem;
  }

  .title {
    font-size: 1.2rem;
  }

  .contact-info {
    flex-direction: column;
    gap: 15px;
  }

  .nav .container {
    gap: 20px;
    flex-wrap: wrap;
  }

  .section {
    padding: 30px 20px;
    margin: 20px 10px;
  }

  .section-title {
    font-size: 1.5rem;
  }

  .job-header {
    flex-direction: column;
  }

  .timeline {
    padding-left: 30px;
  }

  .timeline-marker {
    left: -36px;
  }
}
</style>
