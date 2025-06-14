<script setup>
import { ref, provide } from 'vue'
import NotificationBell from './components/NotificationBell.vue'
import InputModal from './components/InputModal.vue'
import ConfirmModal from './components/ConfirmModal.vue'

const notificationBell = ref()
const inputModal = ref()
const confirmModal = ref()

provide('notify', (msg, type = 'info') => {
  notificationBell.value?.addNotification(msg, type)
})

provide('openPrompt', (title, defaultValue) => {
  return inputModal.value?.openPrompt(title, defaultValue)
})

provide('openConfirm', (message) => {
  return confirmModal.value?.openConfirm(message)
})
</script>

<template>
  <div class="app">
    <div class="notification-bell-container">
      <NotificationBell ref="notificationBell" />
    </div>
    <router-view />
    <InputModal ref="inputModal" />
    <ConfirmModal ref="confirmModal" />
  </div>
</template>

<style>
.app {
  min-height: 100vh;
  position: relative;
}

.notification-bell-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 1000;
}
</style>
