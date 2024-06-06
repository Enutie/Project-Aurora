<script setup>
import { ref } from 'vue';
import axios from 'axios';
import GameView from './components/GameView.vue';

const gameId = ref('');
const playerName = ref('');

const createGame = async () => {
  try {
    const response = await axios.post('/api/games', {
      playerName: playerName.value
    });
    gameId.value = response.data.gameId;
    console.log(gameId.value)
  } catch (error) {
    console.error('Error creating game:', error);
  }
};
</script>

<template>
  <div id="app">
    <header>
      <h1>Project Aurora</h1>
    </header>
    <main>
      <GameView v-if="gameId" :gameId="gameId" />
      <div v-else class="create-game">
        <input type="text" v-model="playerName" placeholder="Enter your name" />
        <button @click="createGame">Create Game</button>
      </div>
    </main>
  </div>
</template>

<style>
#app {
  font-family: Arial, sans-serif;
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

header {
  text-align: center;
  margin-bottom: 20px;
}

main {
  background-color: #f5f5f5;
  padding: 20px;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.create-game {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
}

input {
  padding: 10px;
  font-size: 16px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

button {
  padding: 10px 20px;
  font-size: 16px;
  background-color: #007bff;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>
