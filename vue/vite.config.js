import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';

export default defineConfig({
  plugins: [vue()],
  base: '/', // Относительные пути для корректной работы внутри wwwroot
  build: {
    outDir: '../TaskMaster/Api/wwwroot',
    emptyOutDir: true, // Очищать dist перед сборкой
    assetsDir: '', // Файлы в корне dist (не в /assets)
  },
  server: {
    proxy: {
      // Проксируем все запросы, начинающиеся с `/auth`
      '/auth': {
        target: 'https://task-master.cloudpub.ru',
        changeOrigin: true,
        secure: false, // отключаем проверку SSL-сертификата
        // Удалим префикс если нужно (опционально)
        // rewrite: path => path.replace(/^\/auth/, '')
      },
      '/taskboards': {
        target: 'https://task-master.cloudpub.ru',
        changeOrigin: true,
        secure: false,
      },
      '/tasks': {
        target: 'https://task-master.cloudpub.ru',
        changeOrigin: true,
        secure: false,
      }
    }
  }
});