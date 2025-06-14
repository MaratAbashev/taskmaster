<template>
  <div class="taskboard">
    <button class="back-button" @click="goBack">← Назад</button>

    <div class="header">
      <input v-model="editableTitle" class="board-title" />
      <button @click="renameBoard">💾 Сохранить</button>
      <button @click="deleteBoard" class="danger">🗑️ Удалить доску</button>
    </div>

    <h2>Задачи</h2>
    <button @click="openCreateModal" class="create-task-button">➕ Создать задачу</button>
    <TaskModal
        v-if="showModal"
        :visible="showModal"
        :editingTask="editingTask"
        :boardId="boardId"
        :ownerId="board?.ownerId"
        :users="board?.users || []"
        @close="closeModal"
        @updated="onTaskUpdated"
    />
    <div v-if="loading">Загрузка...</div>

    <div v-else class="kanban">
      <div
          v-for="status in statuses"
          :key="status.value"
          class="kanban-column"
      >
        <h3 class="column-title">{{ status.label }}</h3>
        <div class="kanban-tasks">
          <TaskCard
              v-for="task in filteredTasks(status.value)"
              :key="task.id"
              :task="task"
              :users="board?.users || []"
              :current-user-id="board?.currentUserId"
              @edit="openEditModal"
              @updated="onTaskUpdated"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { authFetch } from '../utils/authFetch'
import { inject } from 'vue'
import TaskModal from '../components/TaskModal.vue'
import TaskCard from '../components/TaskCard.vue'

const notify = inject('notify')
const openPrompt = inject('openPrompt')
const openConfirm = inject('openConfirm')

const route = useRoute()
const router = useRouter()
const boardId = route.params.id

const board = ref({})
const editableTitle = ref('')
const loading = ref(true)

const showModal = ref(false)
const editingTask = ref(null)

function openCreateModal() {
  editingTask.value = null
  showModal.value = true
}

function openEditModal(task) {
  editingTask.value = task
  showModal.value = true
}

function closeModal() {
  showModal.value = false
}

async function onTaskUpdated() {
  await loadBoard()
}


const statuses = [
  { value: 1, label: '📋 To Do' },
  { value: 2, label: '🚧 In Progress' },
  { value: 3, label: '🔍 On Review' },
  { value: 4, label: '✅ Approved' },
  { value: 5, label: '❌ Failed' }
]

const loadBoard = async () => {
  loading.value = true
  const res = await authFetch(`/taskboards/${boardId}`)
  if (!res.ok) {
    notify('Ошибка загрузки доски', 'error')
    return router.push('/dashboards')
  }
  const data = await res.json()
  board.value = data
  editableTitle.value = data.title
  loading.value = false
}

const renameBoard = async () => {
  const res = await authFetch(`/taskboards/${boardId}/rename?newTitle=${encodeURIComponent(editableTitle.value)}`, {
    method: 'PUT'
  })
  if (!res.ok) return notify('Ошибка при переименовании', 'error')
  const data = await res.json()
  board.value = data
  notify('Название изменено', 'success')
}

const deleteBoard = async () => {
  const confirmed = await openConfirm('Удалить доску навсегда?')
  if (!confirmed) return
  const res = await authFetch(`/taskboards/${boardId}`, { method: 'DELETE' })
  if (res.status === 204) {
    notify('Доска удалена', 'success')
    router.push('/dashboards')
  } else {
    notify('Ошибка при удалении', 'error')
  }
}

function filteredTasks(status) {
  return board.value.tasks?.filter(t => t.status === status) || []
}

function priorityLabel(priority) {
  switch (priority) {
    case 0: return 'Низкий'
    case 1: return 'Средний'
    case 2: return 'Высокий'
    default: return ''
  }
}

function goBack() {
  router.push('/dashboards')
}

onMounted(loadBoard)
</script>

<style scoped>
.taskboard {
  padding: 2rem;
  min-height: 100vh;
  background: #f5f7fa;
  position: relative;
}

.header {
  display: flex;
  align-items: center;
  gap: 1rem;
  margin-bottom: 2rem;
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
}

.board-title {
  font-size: 1.5rem;
  padding: 0.5em 0.8em;
  border-radius: 8px;
  border: 2px solid #e0e0e0;
  flex-grow: 1;
  transition: all 0.3s ease;
  background: #f8f9fa;
}

.board-title:focus {
  border-color: #2c7be5;
  outline: none;
  box-shadow: 0 0 0 3px rgba(44, 123, 229, 0.1);
}

.header button {
  padding: 0.6rem 1.2rem;
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.header button:not(.danger) {
  background-color: #2c7be5;
  color: white;
  border: none;
}

.header button:not(.danger):hover {
  background-color: #1a5fc0;
  transform: translateY(-1px);
}

.danger {
  background-color: #dc3545;
  color: white;
  border: none;
}

.danger:hover {
  background-color: #c82333;
  transform: translateY(-1px);
}

.back-button {
  position: fixed;
  top: 1.5rem;
  left: 1.5rem;
  background-color: white;
  color: #2c7be5;
  border: 2px solid #2c7be5;
  padding: 0.6rem 1.2rem;
  font-size: 1rem;
  border-radius: 8px;
  cursor: pointer;
  z-index: 1000;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.back-button:hover {
  background-color: #2c7be5;
  color: white;
  transform: translateY(-1px);
}

.kanban {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
  padding: 0.5rem;
  height: calc(100vh - 200px);
  overflow: hidden;
}

.kanban-column {
  background-color: white;
  border-radius: 12px;
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
  transition: all 0.3s ease;
  height: 100%;
  overflow: hidden;
}

.kanban-column:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.column-title {
  font-size: 1.1rem;
  margin-bottom: 1.5rem;
  color: #495057;
  font-weight: 600;
  padding-bottom: 0.8rem;
  border-bottom: 2px solid #e9ecef;
  flex-shrink: 0;
}

.kanban-tasks {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  flex-grow: 1;
  overflow-y: auto;
  padding-right: 0.5rem;
  margin-right: -0.5rem;
  min-height: 0;
}

/* Стилизация скроллбара */
.kanban-tasks::-webkit-scrollbar {
  width: 6px;
}

.kanban-tasks::-webkit-scrollbar-track {
  background: transparent;
}

.kanban-tasks::-webkit-scrollbar-thumb {
  background-color: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}

.kanban-tasks::-webkit-scrollbar-thumb:hover {
  background-color: rgba(0, 0, 0, 0.3);
}

.create-task-button {
  background-color: #2c7be5;
  color: white;
  border: none;
  padding: 0.8rem 1.5rem;
  border-radius: 8px;
  margin-bottom: 1.5rem;
  cursor: pointer;
  font-size: 1rem;
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.2s ease;
  box-shadow: 0 2px 8px rgba(44, 123, 229, 0.2);
}

.create-task-button:hover {
  background-color: #1a5fc0;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(44, 123, 229, 0.3);
}

/* Анимация загрузки */
@keyframes shimmer {
  0% {
    background-position: -1000px 0;
  }
  100% {
    background-position: 1000px 0;
  }
}

.loading {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 200px;
  color: #6c757d;
  font-size: 1.1rem;
  position: relative;
}

.loading::after {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  background-size: 1000px 100%;
  animation: shimmer 2s infinite linear;
  border-radius: 8px;
}

/* Адаптивность */
@media (max-width: 768px) {
  .taskboard {
    padding: 1rem;
  }

  .header {
    flex-direction: column;
    gap: 1rem;
    padding: 1rem;
  }

  .board-title {
    width: 100%;
  }

  .header button {
    width: 100%;
    justify-content: center;
  }

  .back-button {
    position: static;
    margin-bottom: 1rem;
    width: 100%;
    justify-content: center;
  }

  .kanban {
    grid-template-columns: 1fr;
    gap: 1rem;
    height: auto;
  }

  .kanban-column {
    min-width: 0;
    height: 70vh;
  }

  .kanban-tasks {
    max-height: none;
  }
}

/* Анимации для карточек */
.kanban-tasks {
  animation: fadeIn 0.3s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
