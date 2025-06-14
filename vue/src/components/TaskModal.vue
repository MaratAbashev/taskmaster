<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal">
      <h2>{{ editingTask ? 'Редактировать задачу' : 'Создать задачу' }}</h2>

      <form @submit.prevent="submit">
        <label>Название</label>
        <input v-model="form.title" required />

        <label>Описание</label>
        <textarea v-model="form.description" />

        <label>Приоритет</label>
        <select v-model="form.priorityLevel">
          <option :value="0">Низкий</option>
          <option :value="1">Средний</option>
          <option :value="2">Высокий</option>
        </select>

        <label>Срок</label>
        <input type="datetime-local" v-model="form.dueDate" />

        <!-- 👇 Новый селект для лидера -->
        <label v-if="!editingTask">Лидер</label>
        <select v-if="!editingTask" v-model="form.leaderId" required>
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.displayName || user.username || 'Пользователь ' + user.id }}
          </option>
        </select>

        <div class="modal-actions">
          <button type="submit">💾 Сохранить</button>
          <button type="button" @click="$emit('close')">Отмена</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { authFetch } from '../utils/authFetch'
import { inject } from 'vue'

const props = defineProps({
  visible: Boolean,
  editingTask: Object,
  boardId: [String, Number],
  users: Array, // 👈 добавили список пользователей
  ownerId: [String, Number]
})

const emit = defineEmits(['close', 'updated'])
const notify = inject('notify')

const form = ref({
  title: '',
  description: '',
  priorityLevel: 0,
  dueDate: '',
  leaderId: null
})

watch(() => props.editingTask, (task) => {
  if (task) {
    form.value = {
      title: task.title,
      description: task.description,
      priorityLevel: task.priorityLevel,
      dueDate: task.dueDate ? task.dueDate.slice(0, 16) : '',
      leaderId: task.leaderId
    }
  } else {
    form.value = {
      title: '',
      description: '',
      priorityLevel: 0,
      dueDate: '',
      leaderId: props.ownerId || null
    }
  }
}, { immediate: true })

async function submit() {
  const payload = {
    boardId: props.boardId,
    title: form.value.title,
    description: form.value.description,
    priorityLevel: form.value.priorityLevel,
    dueDate: form.value.dueDate ? new Date(form.value.dueDate).toISOString() : null,
    leaderId: form.value.leaderId
  }

  let res
  if (props.editingTask) {
    res = await authFetch(`/tasks/update`, {
      method: 'PUT',
      body: JSON.stringify({
        ...payload,
        id: props.editingTask.id
      }),
      headers: { 'Content-Type': 'application/json' }
    })
  } else {
    res = await authFetch(`/tasks/create`, {
      method: 'POST',
      body: JSON.stringify(payload),
      headers: { 'Content-Type': 'application/json' }
    })
  }

  if (res.ok) {
    notify('Задача сохранена', 'success')
    emit('updated')
    emit('close')
  } else {
    notify('Ошибка при сохранении задачи', 'error')
  }
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.4);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 999;
  padding: 1rem;
  animation: fadeIn 0.2s ease-out;
}

.modal {
  background: var(--color-card);
  padding: 2rem;
  border-radius: 12px;
  width: min(500px, 90%);
  color: var(--color-text);
  display: grid;
  gap: 1.25rem;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
  animation: slideIn 0.3s ease-out;
}

.modal h2 {
  margin: 0;
  font-size: 1.25rem;
  text-align: center;
  color: var(--color-text);
}

.modal form {
  display: grid;
  gap: 1.25rem;
}

.modal label {
  display: block;
  margin-bottom: 0.5rem;
  font-size: 0.9rem;
  color: var(--color-text-secondary);
}

.modal input,
.modal textarea,
.modal select {
  width: 100%;
  padding: 0.8rem 1rem;
  border-radius: 8px;
  border: 2px solid var(--color-input-border);
  background: var(--color-input-bg);
  color: var(--color-text);
  font-size: 1rem;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

.modal input:focus,
.modal textarea:focus,
.modal select:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px var(--color-input-focus);
}

.modal textarea {
  min-height: 100px;
  resize: vertical;
}

.modal-actions {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
  margin-top: 0.5rem;
}

.modal-actions button {
  padding: 0.8rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.2s ease;
}

.modal-actions button[type="submit"] {
  background: var(--color-primary);
  color: white;
}

.modal-actions button[type="submit"]:hover {
  background: var(--color-primary-hover);
  transform: translateY(-1px);
}

.modal-actions button[type="button"] {
  background: var(--color-input-bg);
  color: var(--color-text);
}

.modal-actions button[type="button"]:hover {
  background: var(--color-border);
  transform: translateY(-1px);
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 480px) {
  .modal {
    padding: 1.5rem;
  }
  
  .modal-actions {
    grid-template-columns: 1fr;
  }
}
</style>
