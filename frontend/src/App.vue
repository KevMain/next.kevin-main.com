<template>
  <div id="app">
    <h1>{{ title }}</h1>
    <button @click="fetchMessage">Get Message from API</button>
    <p v-if="apiMessage" class="message">{{ apiMessage }}</p>
    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script>
export default {
  data() {
    return {
      title: 'Hello from Vue.js!',
      apiMessage: '',
      error: ''
    }
  },
  methods: {
    async fetchMessage() {
      try {
        this.error = '';
        this.apiMessage = '';
        const response = await fetch('http://localhost:5000/api/hello');
        const data = await response.json();
        this.apiMessage = data.message;
      } catch (err) {
        this.error = 'Error connecting to API: ' + err.message;
      }
    }
  }
}
</script>

<style>
#app {
  font-family: Arial, sans-serif;
  text-align: center;
  margin-top: 60px;
}

h1 {
  color: #42b983;
}

button {
  background-color: #42b983;
  color: white;
  border: none;
  padding: 10px 20px;
  font-size: 16px;
  cursor: pointer;
  border-radius: 4px;
  margin: 20px 0;
}

button:hover {
  background-color: #35a372;
}

.message {
  color: #2c3e50;
  font-size: 18px;
  font-weight: bold;
}

.error {
  color: #e74c3c;
}
</style>
