import react from '@vitejs/plugin-react'
import { defineConfig, loadEnv } from 'vite'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  // Backend has no CORS configured (see ../Eraj/CLAUDE.md) - proxying /api and /Images through
  // Vite's own dev server keeps every request on the frontend's origin, so the browser never
  // needs CORS at all. VITE_API_TARGET overrides which port the backend is actually running on
  // (launchSettings.json's default "http" profile is 5056, but a session may run it elsewhere).
  const apiTarget = env.VITE_API_TARGET || 'http://localhost:5056'

  return {
    plugins: [react()],
    server: {
      proxy: {
        '/api': { target: apiTarget, changeOrigin: true },
        '/Images': { target: apiTarget, changeOrigin: true },
      },
    },
  }
})
