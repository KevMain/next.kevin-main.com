// Environment configuration
const config = {
  development: {
    apiBaseUrl: 'https://localhost:5001'
  },
  production: {
    apiBaseUrl: import.meta.env.VITE_API_URL || 'https://your-api.azurecontainerapps.io'
  }
}

const env = import.meta.env.MODE || 'development'

export default {
  apiBaseUrl: config[env].apiBaseUrl
}
