<template>
  <div v-if="visible" class="modal-overlay">
    <div class="modal">
      <h3>{{ title }}</h3>
      <input
          v-model="inputValue"
          type="text"
          class="modal-input"
          @keyup.enter="confirm"
          placeholder="Введите значение"
      />
      <div class="modal-actions">
        <button @click="confirm">OK</button>
        <button @click="cancel">Отмена</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const visible = ref(false)
const title = ref('')
const inputValue = ref('')
let resolveFn = null

function openPrompt(promptTitle, defaultValue = '') {
  title.value = promptTitle
  inputValue.value = defaultValue
  visible.value = true

  return new Promise(resolve => {
    resolveFn = resolve
  })
}

function confirm() {
  visible.value = false
  resolveFn(inputValue.value || null)
}

function cancel() {
  visible.value = false
  resolveFn(null)
}

defineExpose({ openPrompt })
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(0, 0, 0, 0.4);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  animation: fadeIn 0.2s ease-out;
}

.modal {
  background: var(--color-card);
  padding: 2rem;
  border-radius: 12px;
  min-width: 320px;
  max-width: 90%;
  width: 400px;
  text-align: center;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.15);
  animation: slideIn 0.3s ease-out;
}

.modal h3 {
  margin: 0 0 1.5rem 0;
  color: var(--color-text);
  font-size: 1.25rem;
}

.modal-input {
  width: 100%;
  padding: 0.8rem 1rem;
  margin-top: 1rem;
  border-radius: 8px;
  border: 2px solid var(--color-input-border);
  background-color: var(--color-input-bg);
  color: var(--color-text);
  font-size: 1rem;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

.modal-input:focus {
  outline: none;
  border-color: var(--color-primary);
  box-shadow: 0 0 0 3px var(--color-input-focus);
}

.modal-actions {
  margin-top: 1.5rem;
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.modal-actions button {
  min-width: 100px;
  padding: 0.8rem 1.5rem;
  font-size: 1rem;
  border-radius: 8px;
  transition: all 0.2s ease;
}

.modal-actions button:first-child {
  background-color: var(--color-primary);
  color: white;
}

.modal-actions button:first-child:hover {
  background-color: var(--color-primary-hover);
  transform: translateY(-1px);
}

.modal-actions button:last-child {
  background-color: var(--color-input-bg);
  color: var(--color-text);
}

.modal-actions button:last-child:hover {
  background-color: var(--color-border);
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
    flex-direction: column;
  }

  .modal-actions button {
    width: 100%;
  }
}
</style>
