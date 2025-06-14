<template>
  <div
      class="task-card"
      :class="{ followed: isFollowed }"
      @click="toggleFollow"
  >
    <div class="task-header">
      <h4 class="title">{{ task.title }}</h4>
      <div class="task-actions">
        <button
            v-if="isAuthor"
            class="action-btn edit-btn"
            @click.stop="$emit('edit', task)"
            title="Редактировать"
        >
          ✏️
        </button>
        <button
            class="action-btn menu-btn"
            @click.stop="toggleMenu"
            title="Действия"
        >
          ⋮
        </button>
      </div>
    </div>

    <p>{{ task.description || 'Нет описания' }}</p>
    <p><strong>Приоритет:</strong> {{ priorityLabel(task.priorityLevel) }}</p>
    <p><strong>Срок:</strong> {{ formatDate(task.dueDate) }}</p>
    
    <div class="user-info">
      <div class="user">
        <img v-if="getUserPhoto(task.author.id)" :src="getUserPhoto(task.author.id)" :alt="getUserDisplay(task.author.id)" class="user-avatar" />
        <div class="user-details">
          <strong>Автор:</strong> {{ getUserDisplay(task.author.id) }}
        </div>
      </div>
      <div class="user">
        <img v-if="getUserPhoto(task.leader.id)" :src="getUserPhoto(task.leader.id)" :alt="getUserDisplay(task.leader.id)" class="user-avatar" />
        <div class="user-details">
          <strong>Лидер:</strong> {{ getUserDisplay(task.leader.id) }}
        </div>
      </div>
    </div>
  </div>

  <!-- Выносим меню в портал -->
  <Teleport to="body">
    <div v-if="showMenu" class="task-menu-overlay" @click="closeMenu">
      <div class="task-menu" @click.stop>
        <button 
          v-if="!isFollowed" 
          @click="followTask" 
          class="menu-item"
        >
          👥 Подписаться
        </button>
        <button 
          v-else 
          @click="unfollowTask" 
          class="menu-item"
        >
          👥 Отписаться
        </button>

        <button 
          v-if="isLeader && task.status === 2" 
          @click="sendToApprove" 
          class="menu-item"
        >
          📤 Отправить на проверку
        </button>

        <template v-if="isAuthor && task.status === 3">
          <button 
            @click="approveTask" 
            class="menu-item success"
          >
            ✅ Принять
          </button>
          <button 
            @click="showDeclineOptions = true" 
            class="menu-item danger"
          >
            ❌ Отклонить
          </button>
        </template>

        <!-- Подменю для отклонения -->
        <div v-if="showDeclineOptions" class="decline-options">
          <button 
            @click="declineTask('InProgress')" 
            class="menu-item"
          >
            🔄 Отправить на доработку
          </button>
          <button 
            @click="declineTask('Failed')" 
            class="menu-item danger"
          >
            ❌ Отклонить
          </button>
        </div>

        <!-- Кнопки для работы над задачей -->
        <template v-if="isFollowed">
          <button 
            v-if="!currentWorker?.startedAt && currentWorker?.isConfirmed" 
            @click="startTask" 
            class="menu-item"
          >
            🚀 Начать работу
          </button>
          <button 
            v-if="currentWorker?.startedAt && !currentWorker?.finishedAt" 
            @click="finishTask" 
            class="menu-item"
          >
            ✅ Завершить работу
          </button>
          <button 
            v-if="currentWorker?.startedAt && currentWorker?.finishedAt" 
            @click="restartTask" 
            class="menu-item"
          >
            🔄 Переделать задачу
          </button>
        </template>

        <!-- Подтверждение пользователей (только для автора) -->
        <template v-if="isAuthor">
          <div v-if="unconfirmedWorkers.length > 0" class="confirm-section">
            <div class="confirm-header">Неподтвержденные работники:</div>
            <div v-for="worker in unconfirmedWorkers" :key="worker.id" class="confirm-item">
              <div class="user">
                <img v-if="getUserPhoto(worker.id)" :src="getUserPhoto(worker.id)" :alt="getUserDisplay(worker.id)" class="user-avatar" />
                <span>{{ getUserDisplay(worker.id) }}</span>
              </div>
              <button 
                @click="confirmUser(worker.id)" 
                class="menu-item success"
              >
                ✓ Подтвердить
              </button>
            </div>
          </div>
        </template>

        <!-- Кнопка удаления -->
        <button 
          v-if="canDeleteTask"
          @click="deleteTask" 
          class="menu-item danger"
        >
          🗑️ Удалить задачу
        </button>
      </div>
    </div>
  </Teleport>
</template>

<script setup>
import { computed, inject, ref, watchEffect, onMounted, onUnmounted } from 'vue'
import { authFetch } from '../utils/authFetch'

const props = defineProps({
  task: Object,
  currentUserId: Number,
  users: Array,
  isBoardOwner: Boolean
})

const emit = defineEmits(['edit', 'updated'])
const notify = inject('notify')
const openConfirm = inject('openConfirm')

const isAuthor = computed(() => props.task.authorId === props.currentUserId)
const isLeader = computed(() => props.task.leaderId === props.currentUserId)
const isFollowed = computed(() => {
  const isFollowed = props.task.workers?.some(w => w.userId === props.currentUserId)
  console.log('Is followed:', isFollowed) // Отладочная информация
  return isFollowed
})
const showMenu = ref(false)
const showDeclineOptions = ref(false)
const isWorkingOnTask = ref(false)

// Получаем текущего работника
const currentWorker = computed(() => {
  const worker = props.task.workers?.find(w => w.userId === props.currentUserId)
  console.log('Current worker:', worker) // Отладочная информация
  return worker
})

// Проверяем, можно ли начать работу над задачей
const canStartTask = computed(() => {
  return [1, 2, 3].includes(props.task.status) // Todo, InProgress, OnReview
})

// Проверяем, может ли пользователь удалить задачу
const canDeleteTask = computed(() => {
  return isAuthor.value || props.isBoardOwner
})

watchEffect(() => {
  isFollowed.value = props.task.workers?.some(w => w.userId === props.currentUserId)
})

// Вычисляемое свойство для неподтвержденных работников
const unconfirmedWorkers = computed(() => {
  return props.task.workers?.filter(w => !w.isConfirmed) || []
})

function getUserDisplay(id) {
  const user = props.users.find(u => u.id === id)
  return user?.displayName || user?.username || `Пользователь ${id}`
}

function getUserPhoto(id) {
  const user = props.users.find(u => u.id === id)
  return user?.photoUrl
}

function priorityLabel(priority) {
  switch (priority) {
    case 0: return 'Низкий'
    case 1: return 'Средний'
    case 2: return 'Высокий'
    default: return ''
  }
}

function formatDate(date) {
  return new Date(date).toLocaleString()
}

function toggleMenu(event) {
  if (!showMenu.value) {
    const button = event.currentTarget
    const rect = button.getBoundingClientRect()
    const menuHeight = 400 // Примерная высота меню
    const windowHeight = window.innerHeight
    const spaceBelow = windowHeight - rect.bottom
    const spaceAbove = rect.top
    
    // Находим родительскую колонку канбан
    const kanbanColumn = button.closest('.kanban-column')
    const columnRect = kanbanColumn?.getBoundingClientRect()
    
    if (columnRect) {
      // Ограничиваем позицию меню пределами колонки
      const maxRight = columnRect.right - rect.right
      const maxLeft = rect.left - columnRect.left
      
      // Если меню не помещается справа, показываем слева
      if (maxRight < 200) { // 200px - минимальная ширина меню
        document.documentElement.style.setProperty('--menu-right', 'auto')
        document.documentElement.style.setProperty('--menu-left', `${maxLeft}px`)
      } else {
        document.documentElement.style.setProperty('--menu-right', `${maxRight}px`)
        document.documentElement.style.setProperty('--menu-left', 'auto')
      }
      
      // Проверяем, помещается ли меню снизу
      if (spaceBelow < menuHeight && spaceAbove > menuHeight) {
        // Если снизу не помещается, но сверху есть место - показываем сверху
        document.documentElement.style.setProperty('--menu-top', `${rect.top - menuHeight}px`)
        document.documentElement.style.setProperty('--menu-bottom', 'auto')
      } else if (spaceBelow < menuHeight) {
        // Если ни снизу, ни сверху не помещается - показываем по центру экрана
        const centerY = windowHeight / 2 - menuHeight / 2
        document.documentElement.style.setProperty('--menu-top', `${centerY}px`)
        document.documentElement.style.setProperty('--menu-bottom', 'auto')
      } else {
        // Если помещается снизу - показываем снизу
        document.documentElement.style.setProperty('--menu-top', `${rect.bottom + window.scrollY}px`)
        document.documentElement.style.setProperty('--menu-bottom', 'auto')
      }
    } else {
      // Если не нашли колонку, используем старую логику
      document.documentElement.style.setProperty('--menu-right', `${window.innerWidth - rect.right}px`)
      document.documentElement.style.setProperty('--menu-top', `${rect.bottom + window.scrollY}px`)
    }
  }
  
  showMenu.value = !showMenu.value
  if (!showMenu.value) {
    showDeclineOptions.value = false
  }
}

function closeMenu() {
  showMenu.value = false
  showDeclineOptions.value = false
}

async function followTask() {
  const res = await authFetch(`/tasks/${props.task.id}/follow`, { method: 'POST' })
  if (res.ok) {
    isFollowed.value = true
    notify('Вы подписались на задачу', 'success')
    emit('updated')
  } else {
    notify('Не удалось подписаться на задачу', 'error')
  }
  showMenu.value = false
}

async function unfollowTask() {
  const res = await authFetch(`/tasks/${props.task.id}/unfollow`, { method: 'POST' })
  if (res.ok) {
    isFollowed.value = false
    notify('Вы отписались от задачи', 'success')
    emit('updated')
  } else {
    notify('Не удалось отписаться от задачи', 'error')
  }
  showMenu.value = false
}

async function sendToApprove() {
  const res = await authFetch(`/tasks/${props.task.id}/send-to-approve`, { method: 'POST' })
  if (res.ok) {
    notify('Задача отправлена на проверку', 'success')
    emit('updated')
  } else {
    notify('Не удалось отправить задачу на проверку', 'error')
  }
  showMenu.value = false
}

async function approveTask() {
  const res = await authFetch(`/tasks/${props.task.id}/approve`, { method: 'POST' })
  if (res.ok) {
    notify('Задача принята', 'success')
    emit('updated')
  } else {
    notify('Не удалось принять задачу', 'error')
  }
  showMenu.value = false
}

async function declineTask(status) {
  const res = await authFetch(`/tasks/${props.task.id}/decline?taskStatus=${status}`, { method: 'POST' })
  if (res.ok) {
    notify(status === 'InProgress' ? 'Задача отправлена на доработку' : 'Задача отклонена', 'success')
    emit('updated')
  } else {
    notify('Не удалось отклонить задачу', 'error')
  }
  showMenu.value = false
  showDeclineOptions.value = false
}

async function startTask() {
  const res = await authFetch(`/tasks/${props.task.id}/start`, { method: 'POST' })
  if (res.ok) {
    isWorkingOnTask.value = true
    notify('Вы начали работу над задачей', 'success')
    emit('updated')
  } else {
    notify('Не удалось начать работу над задачей', 'error')
  }
  showMenu.value = false
}

async function finishTask() {
  const res = await authFetch(`/tasks/${props.task.id}/finish`, { method: 'POST' })
  if (res.ok) {
    isWorkingOnTask.value = false
    notify('Вы завершили работу над задачей', 'success')
    emit('updated')
  } else {
    notify('Не удалось завершить работу над задачей', 'error')
  }
  showMenu.value = false
}

async function restartTask() {
  const res = await authFetch(`/tasks/${props.task.id}/start`, { method: 'POST' })
  if (res.ok) {
    isWorkingOnTask.value = true
    notify('Вы начали работу над задачей заново', 'success')
    emit('updated')
  } else {
    notify('Не удалось начать работу над задачей', 'error')
  }
  showMenu.value = false
}

async function confirmUser(userId) {
  const res = await authFetch(`/tasks/${props.task.id}/confirm/${userId}`, { method: 'POST' })
  if (res.ok) {
    notify('Пользователь подтвержден', 'success')
    emit('updated')
  } else {
    notify('Не удалось подтвердить пользователя', 'error')
  }
  showMenu.value = false
}

async function deleteTask() {
  const confirmed = await openConfirm('Вы уверены, что хотите удалить эту задачу?')
  if (!confirmed) return

  const res = await authFetch(`/tasks/${props.task.id}`, { method: 'DELETE' })
  if (res.ok) {
    notify('Задача удалена', 'success')
    emit('updated')
  } else {
    notify('Не удалось удалить задачу', 'error')
  }
  showMenu.value = false
}
</script>

<style scoped>
.task-card {
  background-color: var(--color-card);
  color: var(--color-text);
  border-radius: 12px;
  padding: 1.25rem;
  border: 1px solid var(--color-border);
  box-shadow: 0 2px 8px var(--color-shadow);
  cursor: pointer;
  position: relative;
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.task-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px var(--color-shadow-hover);
}

.task-card.followed {
  border-left: 4px solid var(--color-success);
}

.task-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
}

.task-header .title {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--color-text);
  flex: 1;
}

.task-actions {
  display: flex;
  gap: 0.5rem;
}

.action-btn {
  background: none;
  border: none;
  color: var(--color-text-light);
  font-size: 1.1rem;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 4px;
  transition: all 0.2s ease;
  flex-shrink: 0;
}

.action-btn:hover {
  color: var(--color-primary);
  background-color: var(--color-input-bg);
}

.task-card p {
  margin: 0;
  font-size: 0.95rem;
  color: var(--color-text-light);
  line-height: 1.4;
}

.task-card p strong {
  color: var(--color-text);
  font-weight: 500;
}

.task-menu-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 999;
}

.task-menu {
  position: fixed;
  top: var(--menu-top);
  right: var(--menu-right);
  left: var(--menu-left);
  bottom: var(--menu-bottom);
  background: var(--color-card);
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  padding: 0.5rem;
  z-index: 1000;
  min-width: 200px;
  max-height: 80vh;
  overflow-y: auto;
  animation: slideIn 0.2s ease-out;
  transform-origin: top right;
}

.menu-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  width: 100%;
  padding: 0.75rem 1rem;
  border: none;
  background: none;
  color: var(--color-text);
  font-size: 0.95rem;
  text-align: left;
  cursor: pointer;
  border-radius: 4px;
  transition: all 0.2s ease;
}

.menu-item:hover {
  background: var(--color-input-bg);
}

.menu-item.success {
  color: var(--color-success);
}

.menu-item.danger {
  color: var(--color-danger);
}

.decline-options {
  margin-top: 0.5rem;
  padding-top: 0.5rem;
  border-top: 1px solid var(--color-border);
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.confirm-section {
  margin-top: 0.5rem;
  padding-top: 0.5rem;
  border-top: 1px solid var(--color-border);
}

.confirm-header {
  font-size: 0.9rem;
  color: var(--color-text-secondary);
  margin-bottom: 0.5rem;
}

.confirm-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.5rem;
  padding: 0.5rem;
  border-radius: 4px;
  background: var(--color-input-bg);
  margin-bottom: 0.5rem;
}

.confirm-item:last-child {
  margin-bottom: 0;
}

.confirm-item .user {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  flex: 1;
}

.confirm-item .user-avatar {
  width: 20px;
  height: 20px;
}

.user-info {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.user {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.user-avatar {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  object-fit: cover;
}

.user-details {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

@media (max-width: 480px) {
  .task-card {
    padding: 1rem;
  }

  .task-header .title {
    font-size: 1rem;
  }

  .task-menu {
    position: fixed;
    top: auto;
    bottom: 0;
    left: 0;
    right: 0;
    border-radius: 12px 12px 0 0;
    padding: 1rem;
    max-height: 90vh;
    transform-origin: bottom center;
  }
}
</style>
