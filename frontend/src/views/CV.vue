<template>
  <div class="cv-page">
    <div v-if="loading" class="loading">
      <p>Loading CV...</p>
    </div>

    <div v-else-if="error" class="error-container">
      <p class="error">{{ error }}</p>
    </div>

    <div v-else class="cv-content">
      <!-- Hero Section -->
      <header class="hero">
        <div class="hero-background">
          <!-- Code snippets background -->
          <div class="code-background">
            <pre class="code-snippet code-snippet-1">
public class Developer {
    public string Name => "Kevin Main";
    public int Experience => 20;
    public string[] Skills => new[] {
        ".NET", "Azure", "Vue.js",
        "React", "Microservices"
    };
}</pre>
            <pre class="code-snippet code-snippet-2">
const architect = {
  async design() {
    return await this
      .analyze()
      .plan()
      .implement();
  }
};</pre>
            <pre class="code-snippet code-snippet-3">
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();</pre>
            <pre class="code-snippet code-snippet-4">
[HttpGet]
public async Task&lt;IActionResult&gt; Get()
{
    var result = await service.Execute();
    return Ok(result);
}</pre>
          </div>

          <div class="gradient-orb orb-1"></div>
          <div class="gradient-orb orb-2"></div>
          <div class="gradient-orb orb-3"></div>
        </div>
        <div class="container">
          <div class="hero-content">
            <div class="hero-badge">Available for Hire</div>
            <h1 class="name">{{ cvData.personalInfo.name }}</h1>
            <h2 class="role">{{ cvData.personalInfo.title }}</h2>
            <p class="tagline">Optimizationeer. Abstractionist. Algorithmist. Recursionist</p>

            <div class="contact-grid">
              <a :href="`mailto:${cvData.personalInfo.email}`" class="contact-item">
                <svg class="contact-icon" viewBox="0 0 24 24" fill="none">
                  <path d="M3 8L10.89 13.26C11.2187 13.4793 11.6049 13.5963 12 13.5963C12.3951 13.5963 12.7813 13.4793 13.11 13.26L21 8M5 19H19C19.5304 19 20.0391 18.7893 20.4142 18.4142C20.7893 18.0391 21 17.5304 21 17V7C21 6.46957 20.7893 5.96086 20.4142 5.58579C20.0391 5.21071 19.5304 5 19 5H5C4.46957 5 3.96086 5.21071 3.58579 5.58579C3.21071 5.96086 3 6.46957 3 7V17C3 17.5304 3.21071 18.0391 3.58579 18.4142C3.96086 18.7893 4.46957 19 5 19Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                <span>{{ cvData.personalInfo.email }}</span>
              </a>

              <div class="contact-item">
                <svg class="contact-icon" viewBox="0 0 24 24" fill="none">
                  <path d="M3 5C3 3.89543 3.89543 3 5 3H8.27924C8.70967 3 9.09181 3.27543 9.22792 3.68377L10.7257 8.17721C10.8831 8.64932 10.6694 9.16531 10.2243 9.38787L7.96701 10.5165C9.06925 12.9612 11.0388 14.9308 13.4835 16.033L14.6121 13.7757C14.8347 13.3306 15.3507 13.1169 15.8228 13.2743L20.3162 14.7721C20.7246 14.9082 21 15.2903 21 15.7208V19C21 20.1046 20.1046 21 19 21H18C9.71573 21 3 14.2843 3 6V5Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                <span>{{ cvData.personalInfo.phone }}</span>
              </div>

              <div class="contact-item">
                <svg class="contact-icon" viewBox="0 0 24 24" fill="none">
                  <path d="M17.657 16.657L13.414 20.9C13.039 21.275 12.5306 21.4854 12 21.4854C11.4694 21.4854 10.961 21.275 10.586 20.9L6.343 16.657C5.22422 15.5381 4.46234 14.1127 4.15369 12.5608C3.84504 11.009 4.00349 9.40047 4.60901 7.93868C5.21452 6.4769 6.2399 5.22749 7.55548 4.34846C8.87107 3.46943 10.4178 3.00024 12 3.00024C13.5822 3.00024 15.1289 3.46943 16.4445 4.34846C17.7601 5.22749 18.7855 6.4769 19.391 7.93868C19.9965 9.40047 20.155 11.009 19.8463 12.5608C19.5377 14.1127 18.7758 15.5381 17.657 16.657Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                  <path d="M12 13C13.1046 13 14 12.1046 14 11C14 9.89543 13.1046 9 12 9C10.8954 9 10 9.89543 10 11C10 12.1046 10.8954 13 12 13Z" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                <span>{{ cvData.personalInfo.location }}</span>
              </div>
            </div>
          </div>
        </div>
      </header>

      <!-- Sub Navigation -->
      <nav class="sub-nav">
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
    </div>
  </div>
</template>

<script>
export default {
  name: 'CV',
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

<style scoped>
.cv-page {
  min-height: 100vh;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
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
  color: #cbd5e1;
  font-weight: 600;
  letter-spacing: 0.5px;
}

.loading::after {
  content: '';
  width: 50px;
  height: 50px;
  border: 4px solid rgba(255,255,255,0.3);
  border-top-color: #0ea5e9;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin-left: 20px;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.error {
  color: #ef4444;
  padding: 40px;
  background: rgba(255,255,255,0.1);
  backdrop-filter: blur(20px);
  border-radius: 20px;
  box-shadow: 
    0 0 60px rgba(239, 68, 68, 0.3),
    0 20px 60px rgba(0,0,0,0.4);
  border: 1px solid rgba(239, 68, 68, 0.3);
}

/* Hero Section - Clean Minimal Style */
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

.hero .container {
  position: relative;
  z-index: 1;
  max-width: 900px;
}

.hero-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
  text-align: center;
}

.hero-badge {
  display: inline-block;
  padding: 8px 20px;
  background: linear-gradient(135deg, rgba(14, 165, 233, 0.2), rgba(168, 85, 247, 0.2));
  border: 1px solid rgba(14, 165, 233, 0.4);
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 500;
  letter-spacing: 1px;
  text-transform: uppercase;
  color: #00f5ff;
  backdrop-filter: blur(10px);
  animation: pulse-badge 3s ease-in-out infinite;
}

@keyframes pulse-badge {
  0%, 100% {
    box-shadow: 0 0 20px rgba(14, 165, 233, 0.3);
  }
  50% {
    box-shadow: 0 0 30px rgba(14, 165, 233, 0.6);
  }
}

.name {
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

.role {
  font-size: 1.5rem;
  font-weight: 300;
  margin: 10px 0 0;
  color: rgba(255, 255, 255, 0.85);
  letter-spacing: 0.5px;
}

.tagline {
  font-size: 1.15rem;
  font-weight: 400;
  margin: 10px 0 30px;
  color: rgba(255, 255, 255, 0.6);
  letter-spacing: 0.5px;
  font-style: italic;
  max-width: 700px;
}

.contact-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  width: 100%;
  max-width: 850px;
  margin-top: 20px;
}

.contact-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 16px 24px;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
  color: rgba(255, 255, 255, 0.85);
  text-decoration: none;
  font-size: 0.95rem;
}

.contact-item:hover {
  background: rgba(255, 255, 255, 0.1);
  border-color: rgba(14, 165, 233, 0.5);
  transform: translateY(-2px);
  box-shadow: 0 8px 24px rgba(14, 165, 233, 0.2);
}

.contact-icon {
  width: 20px;
  height: 20px;
  flex-shrink: 0;
  color: #00f5ff;
}

/* Sub Navigation */
.sub-nav {
  background: rgba(15, 23, 42, 0.8);
  backdrop-filter: blur(20px) saturate(180%);
  box-shadow: 
    0 8px 32px rgba(0,0,0,0.4),
    inset 0 1px 0 rgba(255,255,255,0.1);
  position: sticky;
  top: 0;
  z-index: 100;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.sub-nav .container {
  display: flex;
  justify-content: center;
  gap: 10px;
  padding: 20px;
}

.nav-link {
  color: #e0e7ff;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  padding: 12px 28px;
  border-radius: 12px;
  position: relative;
  background: rgba(255,255,255,0.05);
  border: 1px solid rgba(255,255,255,0.1);
  letter-spacing: 0.5px;
  font-size: 0.95rem;
}

.nav-link:hover {
  color: #ffffff;
  background: linear-gradient(135deg, rgba(14, 165, 233, 0.3) 0%, rgba(168, 85, 247, 0.3) 100%);
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(14, 165, 233, 0.4);
  border-color: rgba(14, 165, 233, 0.5);
}

/* Sections */
.section {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px) saturate(180%);
  margin: 60px auto;
  padding: 70px 60px;
  border-radius: 24px;
  box-shadow: 
    0 0 60px rgba(14, 165, 233, 0.2),
    0 30px 80px rgba(0,0,0,0.3),
    inset 0 1px 0 rgba(255,255,255,0.1);
  border: 1px solid rgba(255,255,255,0.15);
  position: relative;
  overflow: hidden;
}

.section::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, #0ea5e9, #a855f7, #0ea5e9);
  background-size: 200% 100%;
  animation: shimmer 3s linear infinite;
}

@keyframes shimmer {
  0% { background-position: 200% 0; }
  100% { background-position: -200% 0; }
}

.section-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 3rem;
  font-weight: 700;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 50px;
  padding-bottom: 20px;
  position: relative;
  letter-spacing: -2px;
}

.section-title::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 0;
  width: 100px;
  height: 4px;
  background: linear-gradient(90deg, #0ea5e9, #a855f7);
  border-radius: 2px;
  box-shadow: 0 0 20px rgba(14, 165, 233, 0.8);
}

/* Profile Section */
.profile-content p {
  margin-bottom: 25px;
  font-size: 1.15rem;
  line-height: 2;
  color: #cbd5e1;
  font-weight: 300;
}

/* Skills Section */
.skills-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
  gap: 30px;
}

.skills-column {
  background: rgba(14, 165, 233, 0.05);
  backdrop-filter: blur(10px);
  padding: 40px;
  border-radius: 20px;
  box-shadow: 
    0 0 30px rgba(14, 165, 233, 0.3),
    inset 0 1px 0 rgba(255,255,255,0.1);
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  border: 1px solid rgba(14, 165, 233, 0.3);
  position: relative;
  overflow: hidden;
}

.skills-column::before {
  content: '';
  position: absolute;
  top: -2px;
  left: -2px;
  right: -2px;
  bottom: -2px;
  background: linear-gradient(135deg, #0ea5e9, #a855f7);
  border-radius: 20px;
  opacity: 0;
  transition: opacity 0.4s;
  z-index: -1;
}

.skills-column:hover {
  transform: translateY(-12px) scale(1.02);
  box-shadow: 
    0 0 60px rgba(14, 165, 233, 0.6),
    0 30px 60px rgba(0,0,0,0.4);
}

.skills-column:hover::before {
  opacity: 0.5;
}

.skills-column h3 {
  color: #0ea5e9;
  margin-bottom: 30px;
  font-size: 1.6rem;
  font-weight: 700;
  font-family: 'Space Grotesk', sans-serif;
  letter-spacing: -0.5px;
  text-shadow: 0 0 20px rgba(14, 165, 233, 0.5);
}

.skills-list {
  list-style: none;
  padding: 0;
}

.skills-list li {
  padding: 16px 0;
  padding-left: 35px;
  position: relative;
  border-bottom: 1px solid rgba(255,255,255,0.1);
  transition: all 0.3s;
  font-weight: 400;
  color: #cbd5e1;
}

.skills-list li:hover {
  padding-left: 45px;
  color: #0ea5e9;
  text-shadow: 0 0 10px rgba(14, 165, 233, 0.5);
}

.skills-list li:before {
  content: '◆';
  position: absolute;
  left: 0;
  color: #0ea5e9;
  font-weight: bold;
  font-size: 1.3rem;
  filter: drop-shadow(0 0 5px rgba(14, 165, 233, 0.8));
  transition: all 0.3s;
}

.skills-list li:hover:before {
  transform: scale(1.2) rotate(45deg);
}

/* Timeline */
.timeline {
  position: relative;
  padding-left: 60px;
}

.timeline:before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 3px;
  background: linear-gradient(180deg, #0ea5e9 0%, #a855f7 100%);
  border-radius: 2px;
  box-shadow: 0 0 20px rgba(14, 165, 233, 0.8);
}

.timeline-item {
  position: relative;
  margin-bottom: 70px;
  transition: all 0.4s;
}

.timeline-item:hover .timeline-marker {
  transform: scale(1.5) rotate(180deg);
  box-shadow: 0 0 0 6px rgba(14, 165, 233, 0.4);
}

.timeline-marker {
  position: absolute;
  left: -68px;
  top: 5px;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  border: 3px solid rgba(15, 23, 42, 1);
  box-shadow: 
    0 0 0 4px rgba(14, 165, 233, 0.3),
    0 0 20px rgba(14, 165, 233, 0.8);
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  z-index: 1;
}

.timeline-content {
  padding: 35px 40px;
  background: rgba(14, 165, 233, 0.05);
  backdrop-filter: blur(10px);
  border-radius: 20px;
  box-shadow: 
    0 0 30px rgba(14, 165, 233, 0.2),
    inset 0 1px 0 rgba(255,255,255,0.1);
  border-left: 4px solid #0ea5e9;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  border: 1px solid rgba(14, 165, 233, 0.3);
  position: relative;
  overflow: hidden;
}

.timeline-content::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(14, 165, 233, 0.1), transparent);
  transition: left 0.6s;
}

.timeline-content:hover::before {
  left: 100%;
}

.timeline-content:hover {
  transform: translateX(15px);
  box-shadow: 
    0 0 60px rgba(14, 165, 233, 0.4),
    0 20px 50px rgba(0,0,0,0.4);
  border-left-width: 6px;
}

.job-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 20px;
  flex-wrap: wrap;
  gap: 15px;
}

.job-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 1.8rem;
  font-weight: 700;
  color: #e0e7ff;
  letter-spacing: -0.5px;
}

.job-date {
  color: white;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  padding: 8px 20px;
  border-radius: 25px;
  font-weight: 600;
  font-size: 0.9rem;
  box-shadow: 
    0 0 20px rgba(14, 165, 233, 0.6),
    0 4px 15px rgba(0,0,0,0.3);
  border: 1px solid rgba(255,255,255,0.2);
}

.job-company {
  color: #94a3b8;
  font-weight: 600;
  margin-bottom: 15px;
  font-size: 1.1rem;
}

.job-description {
  margin-bottom: 25px;
  color: #cbd5e1;
  line-height: 2;
  font-weight: 300;
}

.job-highlights {
  margin: 25px 0;
  padding-left: 0;
  list-style: none;
}

.job-highlights li {
  margin-bottom: 15px;
  padding-left: 35px;
  position: relative;
  color: #cbd5e1;
  line-height: 1.9;
  font-weight: 300;
}

.job-highlights li:before {
  content: '✓';
  position: absolute;
  left: 0;
  color: #0ea5e9;
  font-weight: bold;
  font-size: 1.2rem;
  background: rgba(14, 165, 233, 0.15);
  width: 24px;
  height: 24px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.9rem;
  box-shadow: 0 0 15px rgba(14, 165, 233, 0.5);
  border: 1px solid rgba(14, 165, 233, 0.3);
}

.tech-stack {
  margin-top: 25px;
  padding: 20px 24px;
  background: rgba(168, 85, 247, 0.1);
  backdrop-filter: blur(10px);
  border-radius: 12px;
  border-left: 4px solid #a855f7;
  font-size: 0.95rem;
  color: #cbd5e1;
  font-weight: 400;
  box-shadow: 
    0 0 20px rgba(168, 85, 247, 0.3),
    inset 0 1px 0 rgba(255,255,255,0.1);
  border: 1px solid rgba(168, 85, 247, 0.3);
}

.tech-stack strong {
  color: #a855f7;
  font-weight: 600;
  text-shadow: 0 0 10px rgba(168, 85, 247, 0.5);
}

/* Education Section */
.education-content {
  display: flex;
  flex-direction: column;
  gap: 30px;
}

.education-item {
  padding: 35px;
  background: rgba(168, 85, 247, 0.05);
  backdrop-filter: blur(10px);
  border-radius: 20px;
  border-left: 5px solid #a855f7;
  box-shadow: 
    0 0 30px rgba(168, 85, 247, 0.3),
    inset 0 1px 0 rgba(255,255,255,0.1);
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  border: 1px solid rgba(168, 85, 247, 0.3);
}

.education-item:hover {
  transform: translateX(15px) scale(1.02);
  box-shadow: 
    0 0 60px rgba(168, 85, 247, 0.5),
    0 20px 50px rgba(0,0,0,0.4);
  border-left-width: 8px;
}

.education-item h3 {
  color: #e0e7ff;
  margin-bottom: 15px;
  font-size: 1.5rem;
  font-weight: 700;
  font-family: 'Space Grotesk', sans-serif;
}

.education-institution {
  color: #94a3b8;
  font-weight: 600;
  font-size: 1.05rem;
}

.education-grade {
  color: #a855f7;
  font-weight: 600;
  margin-top: 10px;
  font-size: 1.05rem;
  text-shadow: 0 0 10px rgba(168, 85, 247, 0.5);
}

/* Leisure Section */
.leisure-content p {
  margin-bottom: 25px;
  font-size: 1.15rem;
  line-height: 2;
  color: #cbd5e1;
  font-weight: 300;
}

/* Responsive */
@media (max-width: 768px) {
  .hero {
    padding: 60px 20px 50px;
  }

  .name {
    font-size: 2.5rem;
    letter-spacing: -1px;
  }

  .role {
    font-size: 1.2rem;
  }

  .tagline {
    font-size: 1rem;
  }

  .contact-grid {
    grid-template-columns: 1fr;
    gap: 12px;
  }

  .contact-item {
    padding: 14px 20px;
    font-size: 0.9rem;
  }

  .gradient-orb {
    filter: blur(60px);
  }

  .orb-1 {
    width: 300px;
    height: 300px;
  }

  .orb-2 {
    width: 350px;
    height: 350px;
  }

  .orb-3 {
    width: 250px;
    height: 250px;
  }

  .sub-nav .container {
    gap: 8px;
    flex-wrap: wrap;
    padding: 15px;
  }

  .nav-link {
    font-size: 0.85rem;
    padding: 10px 18px;
  }

  .section {
    padding: 40px 30px;
    margin: 40px 15px;
    border-radius: 20px;
  }

  .section-title {
    font-size: 2.2rem;
  }

  .job-header {
    flex-direction: column;
  }

  .timeline {
    padding-left: 40px;
  }

  .timeline-marker {
    left: -50px;
  }

  .timeline-content {
    padding: 25px;
  }

  .skills-grid {
    grid-template-columns: 1fr;
  }
}
</style>
