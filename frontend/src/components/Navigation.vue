<template>
  <nav class="main-nav">
    <div class="nav-container">
      <router-link to="/" class="nav-logo">
        <span class="logo-text">Kevin<span class="logo-accent">.Main</span></span>
      </router-link>

      <div class="nav-links" :class="{ 'mobile-open': mobileMenuOpen }">
        <router-link to="/" class="nav-link" @click="closeMobileMenu">Home</router-link>
        <router-link to="/cv" class="nav-link" @click="closeMobileMenu">CV</router-link>
        <router-link to="/projects" class="nav-link" @click="closeMobileMenu">Projects</router-link>
        <router-link to="/contact" class="nav-link" @click="closeMobileMenu">Contact</router-link>
      </div>

      <button class="mobile-menu-toggle" @click="toggleMobileMenu" aria-label="Toggle menu">
        <span class="hamburger" :class="{ 'open': mobileMenuOpen }"></span>
      </button>
    </div>
  </nav>
</template>

<script>
export default {
  name: 'Navigation',
  data() {
    return {
      mobileMenuOpen: false
    }
  },
  methods: {
    toggleMobileMenu() {
      this.mobileMenuOpen = !this.mobileMenuOpen;
    },
    closeMobileMenu() {
      this.mobileMenuOpen = false;
    }
  }
}
</script>

<style scoped>
.main-nav {
  background: rgba(15, 23, 42, 0.95);
  backdrop-filter: blur(20px) saturate(180%);
  box-shadow: 
    0 8px 32px rgba(0,0,0,0.4),
    inset 0 1px 0 rgba(255,255,255,0.1);
  position: sticky;
  top: 0;
  z-index: 1000;
  border-bottom: 1px solid rgba(255,255,255,0.1);
}

.nav-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 20px 40px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.nav-logo {
  text-decoration: none;
  z-index: 1001;
}

.logo-text {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 1.8rem;
  font-weight: 700;
  color: #e0e7ff;
  letter-spacing: -1px;
  transition: all 0.3s;
}

.logo-accent {
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.nav-logo:hover .logo-text {
  text-shadow: 0 0 20px rgba(14, 165, 233, 0.5);
}

.nav-links {
  display: flex;
  gap: 10px;
  align-items: center;
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

.nav-link:hover,
.nav-link.router-link-active {
  color: #ffffff;
  background: linear-gradient(135deg, rgba(14, 165, 233, 0.3) 0%, rgba(168, 85, 247, 0.3) 100%);
  transform: translateY(-2px);
  box-shadow: 0 10px 30px rgba(14, 165, 233, 0.4);
  border-color: rgba(14, 165, 233, 0.5);
}

.nav-link.router-link-active {
  border-color: #0ea5e9;
}

/* Mobile Menu Toggle */
.mobile-menu-toggle {
  display: none;
  background: none;
  border: none;
  cursor: pointer;
  padding: 10px;
  z-index: 1001;
}

.hamburger {
  display: block;
  width: 28px;
  height: 2px;
  background: #e0e7ff;
  position: relative;
  transition: all 0.3s;
}

.hamburger::before,
.hamburger::after {
  content: '';
  position: absolute;
  width: 28px;
  height: 2px;
  background: #e0e7ff;
  transition: all 0.3s;
}

.hamburger::before {
  top: -8px;
}

.hamburger::after {
  bottom: -8px;
}

.hamburger.open {
  background: transparent;
}

.hamburger.open::before {
  transform: rotate(45deg);
  top: 0;
}

.hamburger.open::after {
  transform: rotate(-45deg);
  bottom: 0;
}

/* Responsive */
@media (max-width: 768px) {
  .nav-container {
    padding: 15px 20px;
  }

  .mobile-menu-toggle {
    display: block;
  }

  .nav-links {
    position: fixed;
    top: 0;
    right: -100%;
    height: 100vh;
    width: 80%;
    max-width: 300px;
    background: rgba(15, 23, 42, 0.98);
    backdrop-filter: blur(20px);
    flex-direction: column;
    justify-content: center;
    gap: 20px;
    padding: 40px;
    transition: right 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
    box-shadow: -10px 0 50px rgba(0,0,0,0.5);
    border-left: 1px solid rgba(255,255,255,0.1);
  }

  .nav-links.mobile-open {
    right: 0;
  }

  .nav-link {
    width: 100%;
    text-align: center;
    padding: 16px 24px;
    font-size: 1.1rem;
  }

  .logo-text {
    font-size: 1.5rem;
  }
}
</style>
