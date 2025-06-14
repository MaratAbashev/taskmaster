<template>
  <div class="login-container">
    <div class="login-card">
      <h1>Добро пожаловать</h1>
      <p class="subtitle">Войдите через Telegram для доступа к доскам задач</p>
      <div ref="telegramButton" class="telegram-button"></div>
      <button
          v-if="isDev"
          @click="devLogin"
          class="dev-login-button"
      >
        🚀 Войти как разработчик
      </button>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { inject } from 'vue'

const notify = inject('notify')
const telegramButton = ref(null)
const router = useRouter()
const isDev = process.env.NODE_ENV === 'development'

async function devLogin() {
  const payload = {
    id: 1840413780,
    firstName: 'Марат',
    lastName: 'Абашев',
    username: 'marat_abashev_1',
    photoUrl:
        'https://t.me/i/userpic/320/C_tyfaaIfONAj6fME46nIdye9FgBmkk_GlKFOpItWOM.jpg',
    authDate: 1749724413,
    hash: '4eb6be476e738451d2f4fc18060d768bc640db0f5af4965f7139af258a88440c'
  }

  try {
    const response = await fetch('/auth/telegram', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(payload),
      credentials: 'include'
    })

    if (!response.ok) throw new Error('Не удалось авторизоваться')

    const data = await response.json()
    localStorage.setItem('accessToken', data.token)
    router.push('/dashboards')
  } catch (err) {
    console.error('Dev login error:', err)
  }
}

function onTelegramAuth(user) {
  const payload = {
    id: user.id,
    firstName: user.first_name,
    lastName: user.last_name,
    username: user.username,
    photoUrl: user.photo_url,
    authDate: user.auth_date,
    hash: user.hash
  }

  fetch('/auth/telegram', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
    credentials: 'include'
  })
      .then(async res => {
        if (!res.ok) throw new Error(await res.text())
        const json = await res.json()

        if (json.token) {
          localStorage.setItem('accessToken', json.token)
        }

        router.push('/dashboards')
      })
      .catch(err => {
        console.error('Auth error:', err)
        notify('Ошибка авторизации', 'error')
      })
}

onMounted(() => {
  window.onTelegramAuth = onTelegramAuth

  const script = document.createElement('script')
  script.async = true
  script.src = 'https://telegram.org/js/telegram-widget.js?22'
  script.setAttribute('data-telegram-login', 'marat_task_master_bot')
  script.setAttribute('data-size', 'large')
  script.setAttribute('data-onauth', 'onTelegramAuth(user)')
  script.setAttribute('data-request-access', 'write')

  telegramButton.value?.appendChild(script)
})
</script>

<style scoped>
.login-container {
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: linear-gradient(135deg, var(--color-bg) 0%, #e9ecef 100%);
  padding: 1rem;
}

.login-card {
  background: var(--color-card);
  padding: 2.5rem;
  border-radius: 16px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
  text-align: center;
  max-width: 400px;
  width: 100%;
  animation: fadeIn 0.5s ease-out;
}

.login-card h1 {
  color: var(--color-text);
  margin-bottom: 0.5rem;
}

.subtitle {
  color: var(--color-text-light);
  margin-bottom: 2rem;
}

.telegram-button {
  margin: 1rem 0;
}

.dev-login-button {
  margin-top: 1.5rem;
  background-color: var(--color-text-light);
  color: white;
  width: 100%;
  padding: 0.8rem;
  border-radius: 8px;
  transition: all 0.2s ease;
}

.dev-login-button:hover {
  background-color: var(--color-text);
  transform: translateY(-1px);
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
