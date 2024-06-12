<template>
  <div class="battlefield">
    <Card
      v-for="(card, index) in cards"
      :key="index"
      :card="card"
      :player-id="playerId"
      @toggle-attack="toggleAttack"
    />
  </div>
  <button
    class="attack-button"
    @click="handleAttack"
    v-if="playerId === gameStore.currentPlayer.id"
    :disabled="gameStore.attackingCreatureIds.length === 0"
  >
    Attack
  </button>
</template>

<script setup>
import Card from './Card.vue'
import { useGameStore } from '@/stores/gameStore'

const props = defineProps({
  cards: {
    type: Array,
    required: true
  },
  playerId: {
    type: String,
    required: true
  },
  opponentId: {
    type: String,
    required: true
  }
})


const gameStore = useGameStore()

function toggleAttack(cardId) {
  gameStore.toggleAttack(cardId)
}

function handleAttack() {
  gameStore.attack(props.playerId, gameStore.attackingCreatureIds, props.opponentId)
}
</script>

<style scoped>
.battlefield {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
  width: 100%;
  min-height: 150px;
  background-color: #2e2e2e;
  border-radius: 5px;
  padding: 10px;
  box-sizing: border-box;
}
.attack-button {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 10px 20px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin-top: 10px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}

.attack-button:hover {
  background-color: #d32f2f;
}

.attack-button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
</style>