<template>
  <div class="game-view">
    <div class="status-message">{{ statusMessage }}</div>

    <HandZone :cards="opponentHand" />
    <BattlefieldZone :cards="opponentBattlefield" />

    <BattlefieldZone :cards="playerBattlefield" />
    <HandZone :cards="playerHand" :is-playable="isPlayerTurn" @card-click="handleCardClick" />

    <PlayerInfo :player="player" />

    <div class="controls">
      <button @click="triggerAIOpponentPlay" :disabled="!isAITurn">AI Opponent Play</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
import PlayerInfo from './PlayerInfo.vue';
import HandZone from './HandZone.vue';
import BattlefieldZone from './BattlefieldZone.vue';

const props = defineProps({
  gameId: {
    type: String,
    required: true
  }
});

const players = ref([]);
const currentPlayer = ref('');
const player = computed(() => players.value.find(p => p.name !== 'AI'));
const handleCardClick = (index) => {
  const card = playerHand.value[index];
  if (card.type === 'Creature') {
    castCreatureFromHand(index);
  } else {
    playLandFromHand(index);
  }
};

const playerHand = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  return player ? player.hand : [];
});

const playerBattlefield = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  return player ? player.battlefield : [];
});

const opponentHand = computed(() => {
  const opponent = players.value.find(p => p.name === 'AI');
  return opponent ? opponent.hand : [];
});

const opponentBattlefield = computed(() => {
  const opponent = players.value.find(p => p.name === 'AI');
  return opponent ? opponent.battlefield : [];
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

const isPlayerTurn = computed(() => {
  const player = players.value.find(p => p.name !== 'AI');
  return player && player.id === currentPlayer.value;
});

const isAITurn = computed(() => {
  const aiOpponent = players.value.find(p => p.name === 'AI');
  return aiOpponent && aiOpponent.id === currentPlayer.value;
});

onMounted(async () => {
  const response = await axios.get(`/api/games/${props.gameId}`);
  const state = response.data;
  players.value = state.players;
  currentPlayer.value = state.currentPlayer;
});

const playLandFromHand = async (index) => {
  const player = players.value.find(p => p.name !== 'AI');
  if (player && player.id === currentPlayer.value) {
    try {
      const response = await axios.post(`/api/games/${props.gameId}/play`, {
        playerId: player.id,
        landIndex: index
      });
      const state = response.data;
      players.value = state.players;
      currentPlayer.value = state.currentPlayer;
    } catch (error) {
      if (error.response && error.response.status === 400) {
        alert('You can only play one land per turn.');
      } else {
        console.error('Error playing land:', error);
        alert('An error occurred while playing the land. Please try again.');
      }
    }
  }
};

const castCreatureFromHand = async (index) => {
  const player = players.value.find(p => p.name !== 'AI');
  if (player && player.id === currentPlayer.value) {
    try {
      const response = await axios.post(`/api/games/${props.gameId}/cast-creature`, {
        playerId: player.id,
        creatureIndex: index
      });
      const state = response.data;
      players.value = state.players;
      currentPlayer.value = state.currentPlayer;
    } catch (error) {
      console.error('Error casting creature:', error);
      alert('An error occurred while casting the creature. Please try again.');
    }
  }
};

const triggerAIOpponentPlay = async () => {
  try {
    const response = await axios.post(`/api/games/${props.gameId}/ai-play`);
    const state = response.data;
    players.value = state.players;
    currentPlayer.value = state.currentPlayer;
  } catch (error) {
    if (error.response && error.response.status === 400) {
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

 
}
  </style>