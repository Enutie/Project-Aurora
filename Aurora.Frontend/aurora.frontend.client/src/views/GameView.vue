<template>
    <div class="game-view">
        <h1>Project Aurora</h1>
        <GameInfo :gameId="gameId"/>
        <PlayerList :gameId="gameId" :players="players" />
        <PlayerActions v-if="!isGameOver" :player="currentPlayer" />
        <GameOver v-else :winner="winner" />>
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
    players.value = response.data.players
    currentPlayer.value = response.data.players.find(p => p.id === response.data.currentPlayer)
    isGameOver.value = response.data.isGameOver
    winner.value = response.data.winner
  } catch (error) {
    console.error('Error fetching game state:', error)
  }
})

</script>

<style scoped>
.game-view {
    background-color: firebrick;
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
}
</style>