<template>
  <div class="blog-page">
    <!-- Header -->
    <section class="blog-header hero">
      <div class="hero-background">
        <div class="gradient-orb orb-1"></div>
        <div class="gradient-orb orb-2"></div>
        <div class="gradient-orb orb-3"></div>
      </div>
      <div class="container">
        <div class="hero-content">
          <h1 class="page-title">Blog</h1>
          <p class="page-subtitle">
            Thoughts on software development, cloud architecture, and technology
          </p>
        </div>
      </div>
    </section>

    <!-- Posts List -->
    <section class="blog-section">
      <div class="container">
        <div class="posts-list">
          <article v-for="post in posts" :key="post.id" class="post-card">
            <h2 class="post-title">
              <router-link :to="'/post/' + post.slug" class="post-link">
                {{ post.title }}
              </router-link>
            </h2>
            <time class="post-date" :datetime="post.publishedAt">
              {{ formatDate(post.publishedAt) }}
            </time>
            <p class="post-preview">{{ post.contentPreview }}</p>
          </article>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { getPosts } from '../data/mockPosts.js'

export default {
  name: 'Blog',
  data() {
    return {
      posts: getPosts()
    }
  },
  methods: {
    formatDate(isoString) {
      return new Date(isoString).toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'long',
        year: 'numeric'
      });
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 40px;
}

/* Header */
.blog-header.hero {
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

.gradient-orb {
  position: absolute;
  border-radius: 50%;
  filter: blur(80px);
  opacity: 0.6;
  animation: float-orb 20s ease-in-out infinite;
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

@keyframes float-orb {
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

.blog-header .container {
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

/* Posts Section */
.blog-section {
  padding: 60px 0;
}

.posts-list {
  display: flex;
  flex-direction: column;
  gap: 40px;
  max-width: 800px;
  margin: 0 auto;
}

.post-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px) saturate(180%);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 24px;
  padding: 40px;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  box-shadow:
    0 0 30px rgba(14, 165, 233, 0.1),
    0 30px 60px rgba(0,0,0,0.2);
  position: relative;
  overflow: hidden;
}

.post-card::before {
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

.post-card:hover::before {
  transform: scaleX(1);
}

.post-card:hover {
  transform: translateY(-8px);
  box-shadow:
    0 0 60px rgba(14, 165, 233, 0.3),
    0 40px 80px rgba(0,0,0,0.3);
  border-color: rgba(14, 165, 233, 0.4);
}

.post-title {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 1.8rem;
  margin: 0 0 8px;
  font-weight: 700;
  letter-spacing: -0.5px;
}

.post-link {
  color: #e0e7ff;
  text-decoration: none;
  transition: color 0.3s;
}

.post-link:hover {
  color: #0ea5e9;
}

.post-date {
  display: block;
  font-size: 0.85rem;
  color: #64748b;
  margin-bottom: 18px;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.post-preview {
  color: #94a3b8;
  line-height: 1.8;
  font-size: 1.05rem;
  margin: 0;
  font-weight: 300;
}

@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }

  .page-title {
    font-size: 2.5rem;
  }

  .post-card {
    padding: 30px 24px;
  }
}
</style>
