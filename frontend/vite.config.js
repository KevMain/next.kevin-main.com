import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import basicSsl from '@vitejs/plugin-basic-ssl'
import path from 'path'

export default defineConfig({
  plugins: [vue(), basicSsl()],
  server: {
    port: 5173,
    https: true
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  }
})
