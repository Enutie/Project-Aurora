<template>
    <div class="game-over-view">
      <div class="game-over-message">
        <h2>Game Over</h2>
        <p v-if="winner">{{ winner }} wins!</p>
        <p v-else>It's a draw!</p>
        <button @click="resetGame">Play Again</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { useRoute } from 'vue-router'
  import { useRouter } from 'vue-router';
  import { ref, onMounted } from 'vue'
  
  const router = useRouter();
  const route = useRoute();
  const winner = ref('')

  onMounted(async () => {
  winner.value = route.params.winner
  
})
  
  function resetGame() {
    router.push({ name: 'CreateGame' });
  }
  </script>
  
  <style scoped>
  .game-over-view {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background-color: #1e1e1e;
  }
  
  .game-over-message {
    background-color: #2e2e2e;
    padding: 20px;
    border-radius: 8px;
    max-width: 400px;
    width: 100%;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
    color: #fff;
    text-align: center;
  }
  
  .game-over-message h2 {
    margin-top: 0;
    margin-bottom: 10px;
    font-size: 24px;
  }
  
  .game-over-message p {
    margin-bottom: 20px;
  }
  
  .game-over-message button {
    background-color: #4CAF50;
    color: white;
    border: none;
    padding: 10px 20px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    cursor: pointer;
    border-radius: 4px;
    transition: background-color 0.3s ease;
  }
  
  .game-over-message button:hover {
    background-color: #45a049;
  }
  </style>