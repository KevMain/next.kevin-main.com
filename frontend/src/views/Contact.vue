<template>
  <div class="contact-page">
    <!-- Header -->
    <section class="contact-header">
      <div class="container">
        <h1 class="page-title">Get In Touch</h1>
        <p class="page-subtitle">
          Let's discuss your next project or opportunity. I'm always open to interesting conversations.
        </p>
      </div>
    </section>

    <!-- Contact Content -->
    <section class="contact-section">
      <div class="container">
        <div class="contact-grid">
          <!-- Contact Info -->
          <div class="contact-info-card">
            <h2>Contact Information</h2>
            <div class="info-items">
              <a href="mailto:KevMain@gmail.com" class="info-item">
                <div class="info-icon">📧</div>
                <div class="info-content">
                  <h3>Email</h3>
                  <p>KevMain@gmail.com</p>
                </div>
              </a>
              <div class="info-item">
                <div class="info-icon">📱</div>
                <div class="info-content">
                  <h3>Phone</h3>
                  <p>07739688271</p>
                </div>
              </div>
              <div class="info-item">
                <div class="info-icon">📍</div>
                <div class="info-content">
                  <h3>Location</h3>
                  <p>Northwich, Cheshire, UK</p>
                </div>
              </div>
              <a href="https://github.com/KevMain" target="_blank" class="info-item">
                <div class="info-icon">💻</div>
                <div class="info-content">
                  <h3>GitHub</h3>
                  <p>github.com/KevMain</p>
                </div>
              </a>
            </div>

            <div class="availability-banner">
              <div class="status-indicator"></div>
              <div>
                <h3>Current Status</h3>
                <p>Available for consulting and freelance opportunities</p>
              </div>
            </div>
          </div>

          <!-- Contact Form -->
          <div class="contact-form-card">
            <h2>Send a Message</h2>
            <form @submit.prevent="handleSubmit" class="contact-form">
              <div class="form-group">
                <label for="name">Name</label>
                <input 
                  type="text" 
                  id="name" 
                  v-model="form.name" 
                  placeholder="Your name" 
                  required
                  class="form-input"
                />
              </div>

              <div class="form-group">
                <label for="email">Email</label>
                <input 
                  type="email" 
                  id="email" 
                  v-model="form.email" 
                  placeholder="your.email@example.com" 
                  required
                  class="form-input"
                />
              </div>

              <div class="form-group">
                <label for="subject">Subject</label>
                <input 
                  type="text" 
                  id="subject" 
                  v-model="form.subject" 
                  placeholder="What's this about?" 
                  required
                  class="form-input"
                />
              </div>

              <div class="form-group">
                <label for="message">Message</label>
                <textarea 
                  id="message" 
                  v-model="form.message" 
                  placeholder="Tell me about your project or opportunity..." 
                  rows="6" 
                  required
                  class="form-input"
                ></textarea>
              </div>

              <button type="submit" class="btn-submit" :disabled="isSubmitting">
                <span v-if="!isSubmitting">Send Message</span>
                <span v-else>Sending...</span>
              </button>

              <div v-if="submitMessage" :class="['submit-message', submitSuccess ? 'success' : 'error']">
                {{ submitMessage }}
              </div>
            </form>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
export default {
  name: 'Contact',
  data() {
    return {
      form: {
        name: '',
        email: '',
        subject: '',
        message: ''
      },
      isSubmitting: false,
      submitMessage: '',
      submitSuccess: false
    }
  },
  methods: {
    async handleSubmit() {
      this.isSubmitting = true;
      this.submitMessage = '';

      try {
        const response = await fetch('http://localhost:5000/api/contact', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            name: this.form.name,
            email: this.form.email,
            subject: this.form.subject,
            message: this.form.message
          })
        });

        const result = await response.json();

        if (response.ok && result.success) {
          this.submitSuccess = true;
          this.submitMessage = result.message;

          // Reset form on success
          this.form = {
            name: '',
            email: '',
            subject: '',
            message: ''
          };

          // Clear message after 5 seconds
          setTimeout(() => {
            this.submitMessage = '';
            this.submitSuccess = false;
          }, 5000);
        } else {
          this.submitSuccess = false;
          this.submitMessage = result.message || 'Failed to send message. Please try again.';
        }
      } catch (error) {
        console.error('Error submitting contact form:', error);
        this.submitSuccess = false;
        this.submitMessage = 'An error occurred. Please try again later.';
      } finally {
        this.isSubmitting = false;
      }
    }
  }
}
</script>

<style scoped>
.contact-page {
  min-height: 100vh;
  padding-bottom: 80px;
}

.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 0 40px;
}

/* Header */
.contact-header {
  padding: 120px 40px 80px;
  text-align: center;
  background: 
    radial-gradient(circle at 50% 50%, rgba(14, 165, 233, 0.15) 0%, transparent 70%),
    radial-gradient(circle at 80% 20%, rgba(168, 85, 247, 0.15) 0%, transparent 70%);
  position: relative;
  overflow: hidden;
}

.contact-header::before {
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
  max-width: 700px;
  margin: 0 auto;
  line-height: 1.8;
  font-weight: 300;
  animation: fadeInUp 0.8s ease-out 0.2s both;
  position: relative;
  z-index: 1;
}

/* Contact Section */
.contact-section {
  padding: 60px 0;
}

.contact-grid {
  display: grid;
  grid-template-columns: 1fr 1.3fr;
  gap: 40px;
  align-items: start;
}

.contact-info-card, .contact-form-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(20px) saturate(180%);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 24px;
  padding: 50px 40px;
  box-shadow: 
    0 0 30px rgba(14, 165, 233, 0.1),
    0 30px 60px rgba(0,0,0,0.2);
}

.contact-info-card h2, .contact-form-card h2 {
  font-family: 'Space Grotesk', sans-serif;
  font-size: 2rem;
  color: #e0e7ff;
  margin-bottom: 35px;
  font-weight: 700;
}

/* Contact Info */
.info-items {
  display: flex;
  flex-direction: column;
  gap: 25px;
  margin-bottom: 40px;
}

.info-item {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 20px;
  background: rgba(14, 165, 233, 0.05);
  border-radius: 16px;
  transition: all 0.3s;
  border: 1px solid rgba(14, 165, 233, 0.2);
  text-decoration: none;
  color: inherit;
}

.info-item:hover {
  background: rgba(14, 165, 233, 0.1);
  transform: translateX(8px);
  box-shadow: 0 0 20px rgba(14, 165, 233, 0.3);
}

.info-icon {
  font-size: 2.5rem;
  filter: drop-shadow(0 0 10px rgba(14, 165, 233, 0.5));
}

.info-content h3 {
  color: #0ea5e9;
  font-size: 0.9rem;
  font-weight: 600;
  margin-bottom: 5px;
  letter-spacing: 0.5px;
  text-transform: uppercase;
}

.info-content p {
  color: #cbd5e1;
  font-size: 1.1rem;
  font-weight: 400;
}

/* Availability Banner */
.availability-banner {
  display: flex;
  align-items: center;
  gap: 20px;
  padding: 25px;
  background: rgba(168, 85, 247, 0.1);
  border-radius: 16px;
  border: 1px solid rgba(168, 85, 247, 0.3);
  margin-top: 30px;
}

.status-indicator {
  width: 16px;
  height: 16px;
  background: #10b981;
  border-radius: 50%;
  box-shadow: 0 0 20px #10b981;
  animation: pulse 2s ease-in-out infinite;
  flex-shrink: 0;
}

@keyframes pulse {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.5; }
}

.availability-banner h3 {
  color: #a855f7;
  font-size: 1rem;
  font-weight: 600;
  margin-bottom: 5px;
}

.availability-banner p {
  color: #cbd5e1;
  font-size: 0.95rem;
  font-weight: 300;
}

/* Contact Form */
.contact-form {
  display: flex;
  flex-direction: column;
  gap: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.form-group label {
  color: #94a3b8;
  font-weight: 600;
  font-size: 0.95rem;
  letter-spacing: 0.5px;
}

.form-input {
  background: rgba(255,255,255,0.05);
  border: 1px solid rgba(255,255,255,0.15);
  border-radius: 12px;
  padding: 16px 20px;
  color: #e0e7ff;
  font-size: 1rem;
  font-family: inherit;
  transition: all 0.3s;
  resize: vertical;
}

.form-input:focus {
  outline: none;
  border-color: #0ea5e9;
  box-shadow: 0 0 20px rgba(14, 165, 233, 0.3);
  background: rgba(14, 165, 233, 0.05);
}

.form-input::placeholder {
  color: #64748b;
}

textarea.form-input {
  min-height: 150px;
}

.btn-submit {
  background: linear-gradient(135deg, #0ea5e9 0%, #a855f7 100%);
  color: white;
  border: none;
  padding: 18px 40px;
  border-radius: 12px;
  font-size: 1.1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275);
  letter-spacing: 0.5px;
  box-shadow: 0 0 30px rgba(14, 165, 233, 0.4);
  margin-top: 10px;
}

.btn-submit:hover:not(:disabled) {
  transform: translateY(-3px);
  box-shadow: 0 0 50px rgba(14, 165, 233, 0.6), 0 10px 30px rgba(0,0,0,0.3);
}

.btn-submit:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.submit-message {
  padding: 16px 20px;
  border-radius: 12px;
  text-align: center;
  font-weight: 500;
  animation: fadeIn 0.3s ease-out;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}

.submit-message.success {
  background: rgba(16, 185, 129, 0.15);
  color: #10b981;
  border: 1px solid rgba(16, 185, 129, 0.3);
}

.submit-message.error {
  background: rgba(239, 68, 68, 0.15);
  color: #ef4444;
  border: 1px solid rgba(239, 68, 68, 0.3);
}

/* Responsive */
@media (max-width: 1024px) {
  .contact-grid {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .container {
    padding: 0 20px;
  }

  .contact-header {
    padding: 80px 20px 60px;
  }

  .page-title {
    font-size: 3rem;
  }

  .page-subtitle {
    font-size: 1.2rem;
  }

  .contact-info-card, .contact-form-card {
    padding: 40px 30px;
  }

  .info-item {
    flex-direction: column;
    text-align: center;
  }
}
</style>
