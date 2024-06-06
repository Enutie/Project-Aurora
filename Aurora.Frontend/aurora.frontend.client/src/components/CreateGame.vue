<template>
    <div class="create-game">
        <h2>Create Game</h2>
        <form @submit.prevent="createGame">
            <label for="playerName">Enter Player Name:</label>
            <input type="text" id="playerName" v-model="playerName" required>
            <button type="submit">Create Game</button>
        </form>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { createGame as createGameAPI } from '../services/api'

const router = useRouter()
const playerName = ref('')

async function createGame() {
    try {
        const response = await createGameAPI(playerName.value)
        const gameId = response.data.gameId
        router.push({ name: 'Game', params: { id: gameId }})
    } catch (error)
    {
        console.error('Error creating game: ', error)
    }
}
</script>
<style scoped>
.create-game {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
}

form {
  display: flex;
  flex-direction: column;
}

label {
  margin-bottom: 10px;
}

input {
  padding: 8px;
  margin-bottom: 10px;
}

button {
  padding: 8px 16px;
  background-color: #007bff;
  color: #fff;
  border: none;
  cursor: pointer;
}
</style>