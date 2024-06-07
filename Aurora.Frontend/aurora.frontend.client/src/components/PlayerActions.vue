<template>
    <div class="player-actions">
        <h3>CurrentPlayer: {{ player.name }}</h3>
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
  background-color: mediumturquoise;
  margin-top: 20px;
}

.action-button {
  padding: 8px 16px;
  font-size: 14px;
  background-color: #4caf50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>