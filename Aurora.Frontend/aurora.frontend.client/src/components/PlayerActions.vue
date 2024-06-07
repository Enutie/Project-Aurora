<template>
  <div class="player-actions">
      <h3>Current Player: {{ player.name }}</h3>
      <button @click="endTurn" class="action-button">Pass Turn</button>
  </div>
</template>

<script setup>
import { endTurn as endTurnAPI } from '@/services/api';

const props = defineProps({
    player: {
        type: Object,
        required: true,
    },
    gameId: {
      type: String,
      required: true,
    }
})

const emit = defineEmits(['update-game-state']);

async function endTurn() {
  try {
    const response = await endTurnAPI(props.gameId);
    emit('update-game-state', response.data);
  } catch (error) {
    console.error('Error ending turn:', error);
  }
}


</script>

<style scoped>
.player-actions {
    align-items: center;
    display: flex;
    justify-content: space-between;
    margin-top: 20px;
}

.action-button {
    background-color: #4c51bf;
    border: none;
    border-radius: 4px;
    color: #fff;
    cursor: pointer;
    font-size: 16px;
    padding: 10px 20px;
    transition: background-color 0.3s ease;
}

.action-button:hover {
    background-color: #434190;
}
</style>