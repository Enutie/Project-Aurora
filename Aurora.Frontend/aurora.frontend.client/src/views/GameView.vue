<template>
  
</template>
<!-- <template>
  <div class="game-view">
      <h1>Project Aurora</h1>
      <GameInfo :gameId="gameId"/>
      <PlayerList class="player-list" :gameId="gameId" :players="players" @update-game-state="updateGameState" />
      <PlayerActions v-if="!isGameOver" :player="currentPlayer" :game-id="gameId" @update-game-state="updateGameState" />
      <GameOver v-else :winner="winner" />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getGameState } from '@/services/api'
import GameInfo from '@/backup/GameInfo.vue'
import PlayerList from '@/backup/PlayerList.vue'
import PlayerActions from '@/backup/PlayerActions.vue'
import GameOver from '@/backup/GameOver.vue'

const route = useRoute()
const gameId = ref('')
const players = ref([])
const currentPlayer = ref({})
const isGameOver = ref(false)
const winner = ref({})

onMounted(async () => {
  gameId.value = route.params.id
  try {
    const response = await getGameState(gameId.value)
    updateGameState(response.data)
  } catch (error) {
    console.error('Error fetching game state:', error)
  }
})

function updateGameState(gameState) {
    players.value = gameState.players
    currentPlayer.value = gameState.players.find(p => p.id === gameState.currentPlayer)
    isGameOver.value = gameState.isGameOver
    winner.value = gameState.winner
}

</script>

<style scoped>
.game-view {
    background-color: #1e1e1e;
    color: #fff;
    display: flex;
    flex-direction: column;
    height: 94vh;
    padding: 20px;
}

h1 {
  text-align: center;
}

.player-list {
    flex: 1;
    overflow-y: auto;
}
</style> -->