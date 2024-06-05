<script setup>
import { ref } from 'vue';
import axios from 'axios';
import GameBoard from './components/GameBoard.vue';

const gameId = ref('');
const players = ref([]);
const currentPlayer = ref('');

const createGame = async (playerName) => {
  try {
    const response = await axios.post('/api/Games', { playerName });
    gameId.value = response.data.gameId;
    players.value = response.data.players;
    currentPlayer.value = response.data.currentPlayer;
    console.log(gameId.value, players.value, currentPlayer.value)
  } catch (error) {
    console.error('Error creating game:', error);
  }
};

const getGameState = async () => {
  try {
    const response = await axios.get(`/api/Games/${gameId.value}`);
    players.value = response.data.players;
    currentPlayer.value = response.data.currentPlayer;
  } catch (error) {
    console.error('Error getting game state:', error);
  }
};

const playLand = async (playerId, landIndex) => {
  try {
    const response = await axios.post(`/api/Games/${gameId.value}/play`, {
      playerId,
      landIndex,
    });
    players.value = response.data.players;
    currentPlayer.value = response.data.currentPlayer;
  } catch (error) {
    console.error('Error playing land:', error);
  }
};

const aiPlay = async () => {
  try {
    const response = await axios.post(`/api/Games/${gameId.value}/ai-play`);
    players.value = response.data.players;
    currentPlayer.value = response.data.currentPlayer;
  } catch (error) {
    console.error('Error with AI play:', error);
  }
};
</script>

<template>
  <header>
    <div class="wrapper">
      <GameBoard
      :gameId="gameId"
      :players="players"
      :currentPlayer="currentPlayer"
      @create-game="createGame"
      @get-game-state="getGameState"
      @play-land="playLand"
      @ai-play="aiPlay"
    />
    </div>
  </header>

  <main>
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
