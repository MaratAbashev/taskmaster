<template>
  <div class="notification-bell">
    <button class="bell-button" @click="toggleNotifications">
      <span class="bell-icon">🔔</span>
      <span v-if="unreadCount > 0" class="notification-badge">{{ unreadCount }}</span>
    </button>

    <div v-if="isOpen" class="notifications-panel">
      <div class="notifications-header">
        <h3>Уведомления ({{ notifications.length }})</h3>
        <button v-if="notifications.length" @click="markAllAsRead" class="mark-read">
          Отметить все как прочитанные
        </button>
      </div>

      <div class="notifications-list">
        <div v-if="notifications.length === 0" class="no-notifications">
          Нет новых уведомлений
        </div>
        <div
          v-for="notification in notifications"
          :key="notification.id"
          class="notification-item"
          :class="{ unread: !notification.read, [notification.type]: true }"
          @click="markAsRead(notification.id)"
        >
          <p class="notification-message">{{ notification.message }}</p>
          <span class="notification-time">{{ formatTime(notification.timestamp) }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'

const isOpen = ref(false)
const notifications = ref([])

const unreadCount = computed(() => {
  return notifications.value.filter(n => !n.read).length
})

function toggleNotifications() {
  isOpen.value = !isOpen.value
  console.log('Current notifications:', notifications.value) // Отладочная информация
}

function addNotification(message, type = 'info') {
  const id = Date.now() + Math.random()
  const newNotification = {
    id,
    message,
    type,
    timestamp: new Date(),
    read: false
  }
  notifications.value.unshift(newNotification)
  console.log('Added notification:', newNotification) // Отладочная информация
  console.log('All notifications:', notifications.value) // Отладочная информация
}

function markAsRead(id) {
  const notification = notifications.value.find(n => n.id === id)
  if (notification) {
    notification.read = true
    console.log('Marked as read:', id) // Отладочная информация
  }
}

function markAllAsRead() {
  notifications.value.forEach(n => n.read = true)
  console.log('All marked as read') // Отладочная информация
}

function formatTime(timestamp) {
  const now = new Date()
  const diff = now - timestamp
  
  if (diff < 60000) { // меньше минуты
    return 'только что'
  } else if (diff < 3600000) { // меньше часа
    const minutes = Math.floor(diff / 60000)
    return `${minutes} ${minutes === 1 ? 'минуту' : 'минут'} назад`
  } else if (diff < 86400000) { // меньше суток
    const hours = Math.floor(diff / 3600000)
    return `${hours} ${hours === 1 ? 'час' : 'часов'} назад`
  } else {
    return timestamp.toLocaleDateString()
  }
}

// Закрываем панель при клике вне её
function handleClickOutside(event) {
  if (isOpen.value && !event.target.closest('.notification-bell')) {
    isOpen.value = false
  }
}

// Добавляем и удаляем обработчик клика вне панели
onMounted(() => {
  document.addEventListener('click', handleClickOutside)
  console.log('NotificationBell mounted') // Отладочная информация
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
  console.log('NotificationBell unmounted') // Отладочная информация
})

defineExpose({ addNotification })
</script>

<style scoped>
.notification-bell {
  position: relative;
}

.bell-button {
  background: none;
  border: none;
  cursor: pointer;
  padding: 8px;
  position: relative;
  color: var(--color-text);
  transition: transform 0.2s ease;
}

.bell-button:hover {
  transform: scale(1.1);
}

.bell-icon {
  font-size: 1.5rem;
}

.notification-badge {
  position: absolute;
  top: 0;
  right: 0;
  background: var(--color-primary);
  color: white;
  border-radius: 50%;
  width: 20px;
  height: 20px;
  font-size: 0.75rem;
  display: flex;
  align-items: center;
  justify-content: center;
  transform: translate(25%, -25%);
}

.notifications-panel {
  position: fixed;
  top: 0;
  right: 0;
  width: 360px;
  height: 100vh;
  background: var(--color-card);
  box-shadow: -4px 0 24px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  display: flex;
  flex-direction: column;
  animation: slideIn 0.3s ease-out;
}

.notifications-header {
  padding: 1rem;
  border-bottom: 1px solid var(--color-border);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.notifications-header h3 {
  margin: 0;
  font-size: 1.1rem;
  color: var(--color-text);
}

.mark-read {
  background: none;
  border: none;
  color: var(--color-primary);
  cursor: pointer;
  font-size: 0.9rem;
  padding: 4px 8px;
  border-radius: 4px;
  transition: background-color 0.2s ease;
}

.mark-read:hover {
  background: var(--color-input-bg);
}

.notifications-list {
  flex: 1;
  overflow-y: auto;
  padding: 1rem;
}

.no-notifications {
  color: var(--color-text-secondary);
  text-align: center;
  padding: 2rem 0;
}

.notification-item {
  padding: 1rem;
  border-radius: 8px;
  background: var(--color-input-bg);
  margin-bottom: 0.75rem;
  cursor: pointer;
  transition: all 0.2s ease;
  border-left: 4px solid transparent;
}

.notification-item:hover {
  background: var(--color-border);
}

.notification-item.success {
  border-left-color: var(--color-success);
}

.notification-item.error {
  border-left-color: var(--color-danger);
}

.notification-item.info {
  border-left-color: var(--color-primary);
}

.notification-item.unread {
  background: var(--color-primary-light);
}

.notification-message {
  margin: 0 0 0.5rem 0;
  color: var(--color-text);
  font-size: 0.95rem;
  line-height: 1.4;
}

.notification-time {
  color: var(--color-text-secondary);
  font-size: 0.8rem;
}

@keyframes slideIn {
  from {
    transform: translateX(100%);
  }
  to {
    transform: translateX(0);
  }
}

@media (max-width: 480px) {
  .notifications-panel {
    width: 100%;
  }
}
</style> 