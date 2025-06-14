<template>
  <div class="dashboard">
    <h1 class="title">Мои доски</h1>

    <button @click="createBoard">Создать доску</button>

    <div class="board-list">
      <div v-for="board in taskBoards" :key="board.id" class="board">
        <h2>{{ board.title }}</h2>
        <a v-if="board.telegramGroupLink" :href="board.telegramGroupLink" target="_blank">Перейти в Telegram</a>

        <div class="users">
          <div v-for="user in board.users" :key="user.id" class="user">
            <img v-if="user.photoUrl" :src="user.photoUrl" />
            <span>{{ user.displayName }}</span>
          </div>
        </div>

        <div class="board-actions">
          <button @click="renameBoard(board.id)">Переименовать</button>
          <button @click="deleteBoard(board.id)">Удалить</button>
          <button @click="goToBoard(board.id)">Открыть</button>
        </div>
      </div>
    </div>

    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { authFetch } from '../utils/authFetch'
import { useRouter } from 'vue-router'
import { inject } from 'vue'

const notify = inject('notify')
const openPrompt = inject('openPrompt')
const openConfirm = inject('openConfirm')

const router = useRouter()

const taskBoards = ref([])
const error = ref(null)

async function fetchBoards() {
  try {
    const res = await authFetch('/taskboards')
    if (!res.ok) throw new Error('Не удалось загрузить доски')

    const data = await res.json()
    taskBoards.value = data
  } catch (e) {
    error.value = e.message
  }
}

async function createBoard() {
  const title = await openPrompt('Введите название новой доски')
  if (!title) return

  const res = await authFetch(`/taskboards/create?title=${encodeURIComponent(title)}`, {
    method: 'POST'
  })

  if (res.ok) {
    const newBoard = await res.json()
    taskBoards.value.push(newBoard)
  } else {
    notify('Ошибка при создании доски', 'error')
  }
}

async function renameBoard(id) {
  const newTitle = await openPrompt('Введите новое название доски')
  if (!newTitle) return

  const res = await authFetch(`/taskboards/${id}/rename?newTitle=${encodeURIComponent(newTitle)}`, {
    method: 'PUT'
  })

  if (res.ok) {
    const updated = await res.json()
    const index = taskBoards.value.findIndex(b => b.id === id)
    if (index !== -1) taskBoards.value[index] = updated
  } else {
    notify('Ошибка при переименовании доски', 'error')
  }
}

async function deleteBoard(id) {
  const confirmed = await openConfirm('Удалить доску?')
  if (!confirmed) return

  const res = await authFetch(`/taskboards/${id}`, {
    method: 'DELETE'
  })

  if (res.ok) {
    taskBoards.value = taskBoards.value.filter(b => b.id !== id)
  } else {
    notify('Ошибка при удалении доски', 'error')
  }
}
function goToBoard(id) {
  router.push(`/board/${id}`)
}

onMounted(fetchBoards)
</script>

<style scoped>
.dashboard {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  animation: fadeIn 0.3s ease-out;
}

.title {
  font-size: 2rem;
  margin-bottom: 2rem;
  color: var(--color-text);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.title button {
  background-color: var(--color-primary);
  color: white;
  padding: 0.8rem 1.5rem;
  border-radius: 8px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: all 0.2s ease;
}

.title button:hover {
  background-color: var(--color-primary-hover);
  transform: translateY(-1px);
}

.error {
  color: var(--color-danger);
  margin-top: 1rem;
  padding: 1rem;
  background-color: rgba(220, 53, 69, 0.1);
  border-radius: 8px;
  text-align: center;
}

.board-list {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
  margin-top: 2rem;
}

.board {
  background-color: var(--color-card);
  padding: 1.5rem;
  border-radius: 12px;
  text-align: left;
  box-shadow: 0 2px 8px var(--color-shadow);
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.board:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px var(--color-shadow-hover);
}

.board h2 {
  margin: 0;
  color: var(--color-text);
  font-size: 1.25rem;
}

.board a {
  color: var(--color-primary);
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  transition: color 0.2s ease;
}

.board a:hover {
  color: var(--color-primary-hover);
}

.users {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin: 0.5rem 0;
}

.user {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  background-color: var(--color-input-bg);
  padding: 0.4rem 0.8rem;
  border-radius: 8px;
  font-size: 0.9rem;
  color: var(--color-text);
  transition: all 0.2s ease;
}

.user:hover {
  background-color: var(--color-border);
}

.user img {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid var(--color-card);
}

.board-actions {
  margin-top: auto;
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.board-actions button {
  flex: 1;
  min-width: 120px;
  padding: 0.6rem;
  font-size: 0.9rem;
  white-space: nowrap;
}

.board-actions button:last-child {
  background-color: var(--color-primary);
}

@media (max-width: 768px) {
  .dashboard {
    padding: 1rem;
  }

  .title {
    flex-direction: column;
    gap: 1rem;
    align-items: stretch;
  }

  .board-list {
    grid-template-columns: 1fr;
  }

  .board-actions {
    flex-direction: column;
  }

  .board-actions button {
    width: 100%;
    min-width: unset;
  }
}
</style>
