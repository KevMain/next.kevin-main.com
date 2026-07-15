<template>
  <div class="contact-page">
    <!-- Header with Code Background -->
    <section class="contact-header hero">
      <div class="hero-background">
        <!-- Code snippets background -->
        <div class="code-background">
          <pre class="code-snippet code-snippet-1">
const contact = {
  email: "KevMain@gmail.com",
  phone: "07739688271",
  location: "Cheshire, UK"
};</pre>
          <pre class="code-snippet code-snippet-2">
[HttpPost("contact")]
public async Task SendMessage()
{
    await emailService.Send();
    return Ok();
}</pre>
          <pre class="code-snippet code-snippet-3">
await fetch('/api/contact', {
  method: 'POST',
  body: JSON.stringify(data)
});</pre>
          <pre class="code-snippet code-snippet-4">
services.AddSmtp(options =>
{
    options.Host = "smtp.gmail.com";
    options.Port = 587;
});</pre>
        </div>

        <div class="gradient-orb orb-1"></div>
        <div class="gradient-orb orb-2"></div>
        <div class="gradient-orb orb-3"></div>
      </div>
      <div class="container">
        <div class="hero-content">
          <h1 class="page-title">Get In Touch</h1>
          <p class="page-subtitle">
            Let's discuss your next project or opportunity. I'm always open to interesting conversations.
          </p>
        </div>
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
        console.log('Submitting contact form...', this.form);

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

        console.log('Response status:', response.status);
        console.log('Response ok:', response.ok);

        const result = await response.json();
        console.log('Result:', result);

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
          console.error('Submission failed:', result);
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

/* Header with Code Background */
.contact-header.hero {
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

.contact-header .container {
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
