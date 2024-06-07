<template>
    <div class="game-view">
        <h1>Project Aurora</h1>
        <GameInfo :gameId="gameId"/>
        <PlayerList :gameId="gameId" :players="players" @update-game-state="updateGameState" />
        <PlayerActions v-if="!isGameOver" :player="currentPlayer" :game-id="gameId" @update-game-state="updateGameState" />
        <GameOver v-else :winner="winner" />
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { getGameState } from '@/services/api'
import GameInfo from '@/components/GameInfo.vue'
import PlayerList from '@/components/PlayerList.vue'
import PlayerActions from '@/components/PlayerActions.vue'
import GameOver from '@/components/GameOver.vue'

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
    background-color: firebrick;
    display:flex;
    flex-direction: column;
    height: 100vh;
}
</style>