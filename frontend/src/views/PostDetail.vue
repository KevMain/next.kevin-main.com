<template>
  <div class="post-page">
    <!-- Header -->
    <section class="post-header hero">
      <div class="hero-background">
        <div class="gradient-orb orb-1"></div>
        <div class="gradient-orb orb-2"></div>
        <div class="gradient-orb orb-3"></div>
      </div>
      <div class="container">
        <div class="hero-content">
          <template v-if="loading">
            <h1 class="page-title">Loading&hellip;</h1>
          </template>
          <template v-else-if="post">
            <h1 class="page-title">{{ post.title }}</h1>
            <time class="post-date" :datetime="post.publishedAt">
              {{ formatDate(post.publishedAt) }}
            </time>
          </template>
          <template v-else-if="error">
            <h1 class="page-title">Something Went Wrong</h1>
            <p class="page-subtitle">
              Sorry, we couldn't load this post right now. Please try again later.
            </p>
          </template>
          <template v-else>
            <h1 class="page-title">Post Not Found</h1>
            <p class="page-subtitle">
              Sorry, we couldn't find the post you're looking for.
            </p>
          </template>
        </div>
      </div>
    </section>

    <!-- Post Content -->
    <section class="post-section">
      <div class="container">
        <article v-if="post" class="post-content-card">
          <div class="post-content" v-html="renderedContent"></div>
        </article>
        <div class="back-link-wrap">
          <router-link to="/blog" class="back-link">&larr; Back to Blog</router-link>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { marked } from 'marked'
import config from '@/config.js'

export default {
  name: 'PostDetail',
  data() {
    return {
      post: null,
      loading: true,
      error: ''
    }
  },
  computed: {
    renderedContent() {
      return this.post ? marked.parse(this.post.content) : ''
    }
  },
  async mounted() {
    await this.fetchPost();
  },
  watch: {
    '$route.params.slug'(newSlug) {
      if (newSlug) {
        this.fetchPost();
      }
    }
  },
  methods: {
    async fetchPost() {
      try {
        this.loading = true;
        this.error = '';
        this.post = null;
        const slug = this.$route.params.slug;
        const response = await fetch(`${config.apiBaseUrl}/api/blog/${encodeURIComponent(slug)}`);

        if (response.status === 404) {
          return;
        }

        if (!response.ok) {
          throw new Error('Failed to load blog post');
        }

        this.post = await response.json();
      } catch (err) {
        this.error = 'Error loading blog post: ' + err.message;
        console.error('Error:', err);
      } finally {
        this.loading = false;
      }
    },
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
.post-header.hero {
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

.post-header .container {
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
  font-size: 3rem;
  font-weight: 700;
  margin: 0;
  letter-spacing: -1px;
  line-height: 1.15;
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

.post-date {
  display: block;
  font-size: 0.85rem;
  color: rgba(255, 255, 255, 0.6);
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 1px;
}

/* Post Section */
.post-section {
  padding: 60px 0;
}

.post-content-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px) saturate(180%);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 24px;
  padding: 50px;
  max-width: 800px;
  margin: 0 auto;
  box-shadow:
    0 0 30px rgba(14, 165, 233, 0.1),
    0 30px 60px rgba(0,0,0,0.2);
}

.post-content :deep(p) {
  color: #94a3b8;
  line-height: 1.9;
  font-size: 1.1rem;
  font-weight: 300;
  margin: 0 0 24px;
}

.post-content :deep(p:last-child) {
  margin-bottom: 0;
}

.post-content :deep(h2) {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 1.6rem;
  font-weight: 700;
  letter-spacing: -0.5px;
  color: #e0e7ff;
  margin: 40px 0 16px;
}

.post-content :deep(h3) {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 1.3rem;
  font-weight: 700;
  color: #e0e7ff;
  margin: 32px 0 12px;
}

.post-content :deep(ul),
.post-content :deep(ol) {
  color: #94a3b8;
  line-height: 1.9;
  font-size: 1.1rem;
  font-weight: 300;
  margin: 0 0 24px;
  padding-left: 28px;
}

.post-content :deep(li) {
  margin-bottom: 6px;
}

.post-content :deep(li::marker) {
  color: #0ea5e9;
}

.post-content :deep(blockquote) {
  margin: 0 0 24px;
  padding: 16px 24px;
  border-left: 3px solid #0ea5e9;
  border-radius: 0 12px 12px 0;
  background: rgba(14, 165, 233, 0.08);
}

.post-content :deep(blockquote p) {
  margin: 0;
  color: #cbd5e1;
  font-style: italic;
}

.post-content :deep(strong) {
  color: #e0e7ff;
  font-weight: 600;
}

.post-content :deep(code) {
  font-family: 'Fira Code', 'Courier New', monospace;
  font-size: 0.95em;
  color: #00f5ff;
  background: rgba(14, 165, 233, 0.1);
  padding: 2px 6px;
  border-radius: 6px;
}

.back-link-wrap {
  max-width: 800px;
  margin: 40px auto 0;
  text-align: center;
}

.back-link {
  display: inline-block;
  color: #e0e7ff;
  text-decoration: none;
  font-weight: 600;
  padding: 12px 28px;
  border-radius: 12px;
  background: rgba(255,255,255,0.05);
  border: 1px solid rgba(255,255,255,0.1);
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

.back-link:hover {
  border-color: rgba(14, 165, 233, 0.4);
  color: #0ea5e9;
  transform: translateY(-2px);
}

@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }

  .page-title {
    font-size: 2rem;
  }

  .post-content-card {
    padding: 30px 24px;
  }
}
</style>
