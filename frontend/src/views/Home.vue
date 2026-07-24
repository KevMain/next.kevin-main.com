<template>
  <div class="home">
    <!-- Static Code Background -->
    <div class="code-background">
      <pre class="code-snippet code-snippet-1">
public class Developer {
    public string Name => "Kevin Main";
    public int Experience => 20;
    public string[] Skills => new[] {
        ".NET", "Azure", "AI/ML",
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
&lt;template&gt;
  &lt;div class="app"&gt;
    &lt;router-view /&gt;
  &lt;/div&gt;
&lt;/template&gt;</pre>
      <pre class="code-snippet code-snippet-5">
[HttpGet]
public async Task&lt;IActionResult&gt; Get()
{
    var result = await service.Execute();
    return Ok(result);
}</pre>
      <pre class="code-snippet code-snippet-6">
services.AddScoped&lt;IService, Service&gt;();
services.AddDbContext&lt;AppDbContext&gt;();
services.AddAutoMapper();</pre>
    </div>

    <!-- Hero Section -->
    <section class="hero-home">
      <div class="hero-content">
        <div class="profile-image-wrapper">
          <img 
            :src="profileImage" 
            alt="Kevin Main" 
            class="profile-image"
            @error="handleImageError"
          />
        </div>
        <h1 class="hero-title">Kevin Main</h1>
        <p class="hero-subtitle">Lead Developer | AI & Cloud Architect | Tech Innovator</p>
        <p class="hero-description">
          Crafting elegant solutions with over 20 years of experience in full-stack development, 
          cloud architecture, AI integration, and team leadership.
        </p>
        <div class="hero-buttons">
          <router-link to="/cv" class="btn-primary">View CV</router-link>
          <router-link to="/projects" class="btn-secondary">See Projects</router-link>
          <router-link to="/contact" class="btn-secondary">Get in Touch</router-link>
        </div>
      </div>
      <div class="hero-decoration">
        <div class="code-block">
          <span class="code-line"><span class="keyword">const</span> developer = {</span>
          <span class="code-line">  name: <span class="string">"Kevin Main"</span>,</span>
          <span class="code-line">  role: <span class="string">"Lead Developer"</span>,</span>
          <span class="code-line">  skills: [<span class="string">".NET"</span>, <span class="string">"Azure"</span>, <span class="string">"AI/ML"</span>],</span>
          <span class="code-line">  passion: <span class="string">"Building Amazing Software"</span></span>
          <span class="code-line">};</span>
        </div>
      </div>
    </section>

    <!-- Quick Stats Section -->
    <section class="stats-section">
      <div class="stat-card">
        <div class="stat-number">20+</div>
        <div class="stat-label">Years Experience</div>
      </div>
      <div class="stat-card">
        <div class="stat-number">100+</div>
        <div class="stat-label">Projects Completed</div>
      </div>
      <div class="stat-card">
        <div class="stat-number">5</div>
        <div class="stat-label">Team Members Led</div>
      </div>
    </section>

    <!-- Core Expertise Section -->
    <section class="expertise-section">
      <h2 class="section-title">Core Expertise</h2>
      <div class="expertise-grid">
        <div class="expertise-card">
          <div class="expertise-icon">💻</div>
          <h3>Full-Stack Development</h3>
          <p>.NET Core, ASP.NET, Vue.js, React, Angular - building scalable applications from database to UI</p>
        </div>
        <div class="expertise-card">
          <div class="expertise-icon">☁️</div>
          <h3>Cloud Architecture</h3>
          <p>Azure expertise in designing, deploying, and optimizing cloud-native solutions</p>
        </div>
        <div class="expertise-card highlight-card">
          <div class="expertise-icon">🤖</div>
          <h3>AI & Machine Learning</h3>
          <p>Azure OpenAI, GitHub Copilot, AI-assisted development, prompt engineering, and LLM integration for modern software solutions</p>
        </div>
        <div class="expertise-card">
          <div class="expertise-icon">👥</div>
          <h3>Team Leadership</h3>
          <p>Leading development teams through the full software lifecycle with agile methodologies</p>
        </div>
        <div class="expertise-card">
          <div class="expertise-icon">🎯</div>
          <h3>Software Architecture</h3>
          <p>SOLID principles, DRY, TDD, IoC, and design patterns for maintainable code</p>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
// Import both images explicitly for Vite using relative paths
import profilePhoto from '../assets/images/profile.jpg';
import placeholderImage from '../assets/images/profile-placeholder.svg';

export default {
  name: 'Home',
  data() {
    return {
      profileImage: profilePhoto,
      fallbackImage: placeholderImage,
      imageLoadError: false
    }
  },
  methods: {
    handleImageError(e) {
      if (!this.imageLoadError) {
        // First error - try fallback
        this.imageLoadError = true;
        e.target.src = this.fallbackImage;
      }
    }
  }
}
</script>

<style scoped>
.home {
  min-height: 100vh;
  position: relative;
}

/* Static Code Background */
.code-background {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  overflow: hidden;
  z-index: 0;
  pointer-events: none;
}

.code-snippet {
  position: absolute;
  font-family: 'Fira Code', 'Courier New', monospace;
  font-size: 0.9rem;
  line-height: 1.7;
  color: #0ea5e9;
  opacity: 0.6;
  white-space: pre;
  margin: 0;
  text-shadow: 0 0 15px rgba(14, 165, 233, 0.6);
  padding: 25px;
  background: linear-gradient(135deg, rgba(14, 165, 233, 0.08) 0%, rgba(168, 85, 247, 0.08) 100%);
  border-radius: 12px;
  border: 1px solid rgba(14, 165, 233, 0.3);
  backdrop-filter: blur(10px);
}

.code-snippet-1 {
  top: 10%;
  left: 5%;
  transform: rotate(-5deg);
}

.code-snippet-2 {
  top: 15%;
  right: 8%;
  transform: rotate(3deg);
}

.code-snippet-3 {
  top: 45%;
  left: 3%;
  transform: rotate(2deg);
}

.code-snippet-4 {
  top: 50%;
  right: 5%;
  transform: rotate(-3deg);
}

.code-snippet-5 {
  bottom: 15%;
  left: 10%;
  transform: rotate(4deg);
}

.code-snippet-6 {
  bottom: 20%;
  right: 7%;
  transform: rotate(-2deg);
}

/* Hero Section */
.hero-home {
  min-height: 90vh;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 60px;
  padding: 100px 40px;
  position: relative;
  z-index: 1;
  overflow: hidden;
  background:
    radial-gradient(circle at 10% 20%, rgba(14, 165, 233, 0.15) 0%, transparent 50%),
    radial-gradient(circle at 90% 80%, rgba(168, 85, 247, 0.15) 0%, transparent 50%);
}

.hero-home::before {
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

.hero-content {
  flex: 1;
  max-width: 700px;
  z-index: 1;
  text-align: center;
}

/* Profile Image */
.profile-image-wrapper {
  margin: 0 auto 2rem;
  width: 200px;
  height: 200px;
  position: relative;
  animation: float 6s ease-in-out infinite;
}

.profile-image {
  width: 100%;
  height: 100%;
  border-radius: 50%;
  object-fit: cover;
  border: 5px solid transparent;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  padding: 5px;
  box-shadow: 
    0 20px 60px rgba(14, 165, 233, 0.3),
    0 0 40px rgba(168, 85, 247, 0.2);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.profile-image:hover {
  transform: scale(1.05);
  box-shadow: 
    0 25px 80px rgba(14, 165, 233, 0.4),
    0 0 60px rgba(168, 85, 247, 0.3);
}

@keyframes float {
  0%, 100% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-15px);
  }
}

.hero-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 5rem;
  font-weight: 700;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 20px;
  letter-spacing: -3px;
  animation: fadeInUp 0.8s ease-out;
}

.hero-subtitle {
  font-size: 1.8rem;
  color: #cbd5e1;
  margin-bottom: 25px;
  font-weight: 300;
  animation: fadeInUp 0.8s ease-out 0.2s both;
}

.hero-description {
  font-size: 1.2rem;
  color: #94a3b8;
  line-height: 1.8;
  margin-bottom: 40px;
  font-weight: 300;
  animation: fadeInUp 0.8s ease-out 0.4s both;
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

.hero-buttons {
  display: flex;
  gap: 20px;
  flex-wrap: wrap;
  animation: fadeInUp 0.8s ease-out 0.6s both;
}

.btn-primary, .btn-secondary {
  padding: 16px 36px;
  border-radius: 12px;
  text-decoration: none;
  font-weight: 600;
  font-size: 1.05rem;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  border: 2px solid transparent;
  letter-spacing: 0.5px;
}

.btn-primary {
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  color: white;
  box-shadow: 0 0 30px rgba(14, 165, 233, 0.4);
}

.btn-primary:hover {
  transform: translateY(-3px) scale(1.05);
  box-shadow: 0 0 50px rgba(14, 165, 233, 0.6), 0 10px 30px rgba(0,0,0,0.3);
}

.btn-secondary {
  background: rgba(255,255,255,0.05);
  backdrop-filter: blur(10px);
  color: #cbd5e1;
  border-color: rgba(14, 165, 233, 0.3);
}

.btn-secondary:hover {
  background: rgba(14, 165, 233, 0.15);
  border-color: #0ea5e9;
  color: #ffffff;
  transform: translateY(-3px);
  box-shadow: 0 0 30px rgba(14, 165, 233, 0.4);
}

/* Code Block Decoration */
.hero-decoration {
  flex: 1;
  max-width: 500px;
  display: flex;
  align-items: center;
  animation: fadeInRight 1s ease-out 0.4s both;
}

@keyframes fadeInRight {
  from {
    opacity: 0;
    transform: translateX(50px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.code-block {
  background: rgba(15, 23, 42, 0.8);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(14, 165, 233, 0.3);
  border-radius: 16px;
  padding: 40px;
  font-family: 'Courier New', monospace;
  font-size: 1.1rem;
  box-shadow: 
    0 0 60px rgba(14, 165, 233, 0.3),
    inset 0 1px 0 rgba(255,255,255,0.1);
  position: relative;
  overflow: hidden;
}

.code-block::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, #0ea5e9, #a855f7);
}

.code-line {
  display: block;
  color: #cbd5e1;
  padding: 6px 0;
  animation: typeIn 0.5s ease-out backwards;
}

.code-line:nth-child(1) { animation-delay: 0.8s; }
.code-line:nth-child(2) { animation-delay: 1s; }
.code-line:nth-child(3) { animation-delay: 1.2s; }
.code-line:nth-child(4) { animation-delay: 1.4s; }
.code-line:nth-child(5) { animation-delay: 1.6s; }
.code-line:nth-child(6) { animation-delay: 1.8s; }

@keyframes typeIn {
  from {
    opacity: 0;
    transform: translateX(-10px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.keyword {
  color: #a855f7;
  font-weight: 600;
}

.string {
  color: #0ea5e9;
}

/* Stats Section */
.stats-section {
  display: flex;
  justify-content: center;
  gap: 40px;
  padding: 60px 40px;
  flex-wrap: wrap;
  background: rgba(255,255,255,0.02);
  backdrop-filter: blur(10px);
  border-top: 1px solid rgba(255,255,255,0.1);
  border-bottom: 1px solid rgba(255,255,255,0.1);
  position: relative;
  z-index: 1;
}

.stat-card {
  background: rgba(14, 165, 233, 0.05);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(14, 165, 233, 0.3);
  border-radius: 16px;
  padding: 40px 60px;
  text-align: center;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  box-shadow: 0 0 30px rgba(14, 165, 233, 0.2);
}

.stat-card:hover {
  transform: translateY(-10px) scale(1.05);
  box-shadow: 0 0 60px rgba(14, 165, 233, 0.4);
  border-color: #0ea5e9;
}

.stat-number {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 4rem;
  font-weight: 700;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  margin-bottom: 10px;
}

.stat-label {
  color: #cbd5e1;
  font-size: 1.1rem;
  font-weight: 600;
  letter-spacing: 0.5px;
}

/* Expertise Section */
.expertise-section {
  padding: 100px 40px;
  max-width: 1400px;
  margin: 0 auto;
  position: relative;
  z-index: 1;
}

.section-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 3.5rem;
  font-weight: 700;
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  text-align: center;
  margin-bottom: 70px;
  letter-spacing: -2px;
}

.expertise-grid {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 20px;
}

.expertise-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255,255,255,0.1);
  border-radius: 16px;
  padding: 30px 20px;
  text-align: center;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  box-shadow: 0 0 30px rgba(14, 165, 233, 0.1);
}

.expertise-card:hover {
  transform: translateY(-12px);
  box-shadow: 0 0 60px rgba(14, 165, 233, 0.3);
  border-color: rgba(14, 165, 233, 0.5);
}

.expertise-card.highlight-card {
  background: linear-gradient(135deg, rgba(168, 85, 247, 0.15) 0%, rgba(14, 165, 233, 0.15) 100%);
  border: 2px solid rgba(168, 85, 247, 0.4);
  box-shadow: 0 0 40px rgba(168, 85, 247, 0.3);
}

.expertise-card.highlight-card:hover {
  box-shadow: 0 0 80px rgba(168, 85, 247, 0.5);
  border-color: rgba(168, 85, 247, 0.7);
}

.expertise-icon {
  font-size: 3rem;
  margin-bottom: 15px;
  filter: drop-shadow(0 0 20px rgba(14, 165, 233, 0.5));
}

.expertise-card h3 {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 1.4rem;
  color: #e0e7ff;
  margin-bottom: 15px;
  font-weight: 700;
}

.expertise-card p {
  color: #94a3b8;
  line-height: 1.6;
  font-size: 0.95rem;
  font-weight: 300;
}

/* Responsive */
@media (max-width: 1024px) {
  .hero-home {
    flex-direction: column;
    text-align: center;
    padding: 80px 30px;
  }

  .hero-title {
    font-size: 4rem;
  }

  .hero-buttons {
    justify-content: center;
  }
}

@media (max-width: 768px) {
  .hero-title {
    font-size: 3rem;
  }

  .hero-subtitle {
    font-size: 1.4rem;
  }

  .hero-description {
    font-size: 1.1rem;
  }

  .code-block {
    font-size: 0.9rem;
    padding: 30px;
  }

  .stat-card {
    padding: 30px 40px;
  }

  .stat-number {
    font-size: 3rem;
  }

  .section-title {
    font-size: 2.5rem;
  }

  .expertise-grid {
    grid-template-columns: 1fr;
  }

  .btn-primary, .btn-secondary {
    width: 100%;
    text-align: center;
  }
}

/* Tablet: 2 columns */
@media (max-width: 1200px) and (min-width: 769px) {
  .expertise-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}
</style>
