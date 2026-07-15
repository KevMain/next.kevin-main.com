<template>
  <footer class="main-footer">
    <div class="footer-container">
      <div class="footer-content">
        <div class="footer-section">
          <h3 class="footer-title">
            <span class="footer-logo">
              <span class="logo-bracket">&lt;</span>KM<span class="logo-bracket">/&gt;</span>
            </span>
          </h3>
          <p class="footer-text">
            Lead Developer & Software Engineer with 20+ years of experience in building 
            scalable, modern applications.
          </p>
        </div>

        <div class="footer-section">
          <h4 class="footer-heading">Quick Links</h4>
          <div class="footer-links">
            <router-link to="/" class="footer-link">Home</router-link>
            <router-link to="/cv" class="footer-link">CV</router-link>
            <router-link to="/projects" class="footer-link">Projects</router-link>
            <router-link to="/contact" class="footer-link">Contact</router-link>
          </div>
        </div>

        <div class="footer-section">
          <h4 class="footer-heading">Connect</h4>
          <div class="footer-links">
            <a href="mailto:KevMain@gmail.com" class="footer-link">Email</a>
            <a href="https://github.com/KevMain" target="_blank" class="footer-link">GitHub</a>
          </div>
        </div>
      </div>

      <div class="footer-bottom">
        <p>&copy; {{ new Date().getFullYear() }} Kevin Main. All rights reserved.</p>
        <p class="footer-tagline">Built with Vue.js & .NET 10</p>
      </div>
    </div>
  </footer>
</template>

<script>
export default {
  name: 'Footer'
}
</script>

<style scoped>
.main-footer {
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  color: #cbd5e1;
  margin-top: 100px;
  box-shadow: 
    0 -10px 40px rgba(0,0,0,0.5),
    inset 0 1px 0 rgba(255,255,255,0.1);
  border-top: 1px solid rgba(255,255,255,0.1);
}

.footer-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 60px 40px 30px;
}

.footer-content {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr;
  gap: 60px;
  margin-bottom: 50px;
  padding-bottom: 40px;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.footer-section {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.footer-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 2rem;
  font-weight: 700;
  margin-bottom: 10px;
}

.footer-logo {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-family: 'Fira Code', 'Courier New', monospace;
}

.footer-logo .logo-bracket {
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  font-weight: 700;
}

.footer-text {
  color: #94a3b8;
  line-height: 1.8;
  font-size: 0.95rem;
  font-weight: 300;
}

.footer-heading {
  color: #e0e7ff;
  font-size: 1.2rem;
  font-weight: 600;
  margin-bottom: 5px;
}

.footer-links {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.footer-link {
  color: #94a3b8;
  text-decoration: none;
  transition: all 0.3s;
  font-weight: 400;
  width: fit-content;
}

.footer-link:hover {
  color: #0ea5e9;
  transform: translateX(5px);
  text-shadow: 0 0 10px rgba(14, 165, 233, 0.5);
}

.footer-bottom {
  text-align: center;
  padding-top: 30px;
  color: #64748b;
  font-size: 0.9rem;
  font-weight: 300;
}

.footer-tagline {
  margin-top: 8px;
  color: #475569;
}

/* Responsive */
@media (max-width: 1024px) {
  .footer-content {
    grid-template-columns: 1fr 1fr;
    gap: 40px;
  }

  .footer-section:first-child {
    grid-column: 1 / -1;
  }
}

@media (max-width: 768px) {
  .footer-container {
    padding: 40px 20px 20px;
  }

  .footer-content {
    grid-template-columns: 1fr;
    gap: 30px;
    margin-bottom: 30px;
  }

  .footer-section:first-child {
    grid-column: 1;
  }

  .footer-title {
    font-size: 1.6rem;
  }
}
</style>
