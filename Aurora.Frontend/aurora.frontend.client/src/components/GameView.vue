<template>
    <div class="game-view">
<div class="status-message">{{ statusMessage }}</div>

        <div class="opponent-hand">
  <div class="card not-allowed" v-for="(card, index) in opponentHand" :key="index"></div>
</div>
<div class="opponent-battlefield">
  <div class="card not-allowed" v-for="(card, index) in opponentBattlefield" :key="index"></div>
</div>
<div class="player-battlefield">
  <div class="card not-allowed" v-for="(card, index) in playerBattlefield" :key="index"></div>
</div>
      <div class="player-hand">
  <div
    class="card"
    v-for="(card, index) in playerHand"
    :key="index"
    @click="playLandFromHand(index)"
    :class="{ 'not-allowed': !isPlayerTurn }"
  ></div>
</div>
      <div class="controls">
  <button @click="triggerAIOpponentPlay" :disabled="!isAITurn">AI Opponent Play</button>
</div>
    </div>
  </template>
  <script setup>
  import { ref, onMounted, computed } from 'vue';
  import axios from 'axios';
  
  const props = defineProps({
  gameId: {
    type: String,
    required: true
  }
});

const statusMessage = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  const aiOpponent = players.value.find(p => p.name === 'AI');

  if (player && player.id === currentPlayer.value) {
    return "Your turn";
  } else if (aiOpponent && aiOpponent.id === currentPlayer.value) {
    return "AI's turn";
  } else {
    return "";
  }
});

  const players = ref([]);
  const currentPlayer = ref('');
  
  const playerHand = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  return player ? Array(player.handCount).fill(null) : [];
});

const isPlayerTurn = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  return player && player.id === currentPlayer.value;
});

const isAITurn = computed(() => {
  const aiOpponent = players.value.find(p => p.name === 'AI');
  return aiOpponent && aiOpponent.id === currentPlayer.value;
});

const playerBattlefield = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  return player ? Array(player.battlefieldCount).fill(null) : [];
});

const opponentHand = computed(() => {
  const opponent = players.value.find(p => p.name === 'AI');
  return opponent ? Array(opponent.handCount).fill(null) : [];
});

const opponentBattlefield = computed(() => {
  const opponent = players.value.find(p => p.name === 'AI');
  return opponent ? Array(opponent.battlefieldCount).fill(null) : [];
});
  
  onMounted(async () => {
    // Fetch initial game state
    const response = await axios.get(`/api/games/${props.gameId}`);
    const state = response.data;
    players.value = state.players;
    currentPlayer.value = state.currentPlayer;
  });
  
  const playLandFromHand = async (index) => {
    const player = players.value.find(p => p.name !== 'AI');
  if (player && player.id === currentPlayer.value) {
    try {

        await axios.post(`/api/games/${props.gameId}/play`, {
            playerId: player.id,
            landIndex: index
        });
        // Refresh game state after playing land
        const response = await axios.get(`/api/games/${props.gameId}`);
        const state = response.data;
        players.value = state.players;
        currentPlayer.value = state.currentPlayer;
        }
        catch (error) 
    {
        if(error.response && error.response.state === 400)
        {
            alert('You can only play one land per turn.')
        } else {
            console.error('Error playing land: ', error)
            alert('An error occurred while playing the land. Please try again.')
        }
    }
    } 
  };
  
  const triggerAIOpponentPlay = async () => {
      try {

          await axios.post(`/api/games/${props.gameId}/ai-play`);
          // Refresh game state after AI opponent's move
          const response = await axios.get(`/api/games/${props.gameId}`);
          const state = response.data;
          players.value = state.players;
          currentPlayer.value = state.currentPlayer;
        } catch (error) {
    if (error.response && error.response.status === 500) {
      alert('The AI opponent tried to play too many lands in one turn.');
    } else {
      console.error('Error triggering AI opponent play:', error);
      alert('An error occurred while triggering the AI opponent play. Please try again.');
    }
  }
  };
  </script>
  
  <style scoped>
  .game-view {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 20px;
    padding: 20px;
  }
  
  .opponent-hand,
.opponent-battlefield,
.player-battlefield,
.player-hand {
  display: flex;
  gap: 5px;
}

.card {
  width: 50px;
  height: 70px;
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 4px;
  cursor: pointer;
}

.card.not-allowed {
  cursor: not-allowed;
}
  
  .controls {
    display: flex;
    justify-content: center;
    gap: 10px;
    margin-top: 20px;
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

  button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.status-message {
  font-size: 18px;
  font-weight: bold;
  margin-bottom: 20px;
}

@media (max-width: 600px) {
  .game-view {
    padding: 10px;
  }

  .opponent-hand,
  .opponent-battlefield,
  .player-battlefield,
  .player-hand {
    max-width: 100%;
  }

  .card {
    width: 40px;
    height: 60px;
  }
}
  </style>