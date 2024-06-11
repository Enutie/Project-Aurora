<!-- CreateGame.vue -->
<template>
    <div class="create-game-container">
      <h2>Create Game</h2>
      <form @submit.prevent="createGame">
        <div class="form-group">
          <label for="playerName">Player Name:</label>
          <input type="text" id="playerName" v-model="playerName" required>
        </div>
        <button type="submit">Create Game</button>
      </form>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import axios from 'axios'
import router from '@/router';
  
  const playerName = ref('')
  
  const createGame = async () => {
  try {
    const response = await axios.post('/api/Games/create-game', playerName.value, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
    const gameId = response.data.id;
    router.push({ name: 'Game', params: { id: gameId } });
  } catch (error) {
    console.error(error);
  }
};
  </script>
  
  <style scoped>
  .create-game-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 98svh;
    background-color: #1e1e1e;
    color: #fff;
  }
  
  h2 {
    font-size: 24px;
    margin-bottom: 20px;
  }
  
  .form-group {
    margin-bottom: 20px;
  }
  
  label {
    display: block;
    font-size: 18px;
    margin-bottom: 5px;
  }
  
  input[type="text"] {
    width: 300px;
    padding: 10px;
    font-size: 16px;
    border: none;
    border-radius: 5px;
    background-color: #2c2c2c;
    color: #fff;
  }
  
  button[type="submit"] {
    padding: 10px 20px;
    font-size: 18px;
    background-color: #4CAF50;
    color: #fff;
    border: none;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s ease;
  }
  
  button[type="submit"]:hover {
    background-color: #45a049;
  }
  </style>