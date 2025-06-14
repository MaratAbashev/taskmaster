import './style.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router.js'
import { refreshToken } from './utils/authFetch'

const app = createApp(App)

async function init() {
    // Сначала монтируем приложение с роутером
    app.use(router)
    
    // Если мы на странице логина, не пытаемся обновлять токен
    if (router.currentRoute.value.path === '/login') {
        app.mount('#app')
        return
    }

    try {
        await refreshToken()
        console.log('Токен обновлён')
    } catch (err) {
        console.warn('Не удалось обновить токен')
        // Если не удалось обновить токен, роутер сам перенаправит на логин
        // благодаря глобальному guard'у
    }

    app.mount('#app')
}

init()
