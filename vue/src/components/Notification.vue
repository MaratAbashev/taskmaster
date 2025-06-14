<template>
  <div class="notification-container">
    <transition-group name="fade" tag="div">
      <div
          v-for="note in notifications"
          :key="note.id"
          class="notification"
          :class="note.type"
      >
        <p>{{ note.message }}</p>
      </div>
    </transition-group>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const notifications = ref([])

function addNotification(message, type = 'info') {
  const id = Date.now() + Math.random()
  notifications.value.push({ id, message, type })
  setTimeout(() => {
    notifications.value = notifications.value.filter(n => n.id !== id)
  }, 4000)
}

defineExpose({ addNotification })
</script>

<style scoped>
.notification-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-width: 90vw;
}

.notification {
  background: rgba(255, 255, 255, 0.95);
  color: #333;
  padding: 1rem 1.5rem;
  border-radius: 8px;
  min-width: 280px;
  max-width: 400px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  backdrop-filter: blur(8px);
  transform-origin: right;
  display: flex;
  align-items: center;
  gap: 12px;
}

.notification::before {
  content: '';
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
  opacity: 0.7;
  flex-shrink: 0;
}

.notification p {
  margin: 0;
  font-size: 0.95rem;
  line-height: 1.4;
  flex-grow: 1;
}

.notification.success {
  background: rgba(76, 175, 80, 0.95);
  color: white;
}

.notification.error {
  background: rgba(244, 67, 54, 0.95);
  color: white;
}

.notification.info {
  background: rgba(33, 150, 243, 0.95);
  color: white;
}

/* Анимации */
.fade-enter-active {
  animation: slideIn 0.3s ease-out;
}

.fade-leave-active {
  animation: slideOut 0.3s ease-in;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(30px) scale(0.9);
  }
  to {
    opacity: 1;
    transform: translateX(0) scale(1);
  }
}

@keyframes slideOut {
  from {
    opacity: 1;
    transform: translateX(0) scale(1);
  }
  to {
    opacity: 0;
    transform: translateX(30px) scale(0.9);
  }
}

/* Анимация пульсации для точки */
.notification::before {
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% {
    transform: scale(1);
    opacity: 0.5;
  }
  50% {
    transform: scale(1.2);
    opacity: 0.8;
  }
  100% {
    transform: scale(1);
    opacity: 0.5;
  }
}

/* Адаптивность для мобильных устройств */
@media (max-width: 480px) {
  .notification-container {
    left: 20px;
    right: 20px;
  }
  
  .notification {
    min-width: 0;
    width: 100%;
  }
}
</style>
