import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import basicSsl from '@vitejs/plugin-basic-ssl'
import path from 'path'

export default defineConfig({
  plugins: [vue(), basicSsl()],
  server: {
    port: 5173,
    https: true,
    proxy: {
      // The server owns article pages and legacy redirects; /blog itself stays in the SPA.
      '^/blog/.+': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        secure: false
      },
      '^/post/.+': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        secure: false
      }
    }
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  }
})
