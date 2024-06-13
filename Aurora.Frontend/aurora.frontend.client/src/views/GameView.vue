<template>
  <div class="game-view">
    <div class="playmat top-playmat">
      <Deck position="top" :cardCount="gameStore.players[1]?.deckCount || 0" />
      <Hand
        :cards="gameStore.players[1]?.hand || []"
        :playerId="gameStore.players[1]?.id || ''"
        @play-land="gameStore.playLand"
        @cast-creature="gameStore.castCreature"
      />
      <Battlefield
    :cards="gameStore.players[1]?.battlefield || []"
    :playerId="gameStore.players[1]?.id || ''"
    :opponentId="gameStore.players[0]?.id || ''"
  />
      <PlayerInfo :player="gameStore.players[1] || {}" position="top" />
    </div>
    <div class="playmat bottom-playmat">
      <PlayerInfo :player="gameStore.players[0] || {}" position="bottom" />
      <Battlefield
    :cards="gameStore.players[0]?.battlefield || []"
    :playerId="gameStore.players[0]?.id || ''"
    :opponentId="gameStore.players[1]?.id || ''"
  />
      <Hand
        :cards="gameStore.players[0]?.hand || []"
        :playerId="gameStore.players[0]?.id || ''"
        @play-land="gameStore.playLand"
        @cast-creature="gameStore.castCreature"
      />
      <Deck position="bottom" :cardCount="gameStore.players[0]?.deckCount || 0" />
    </div>
    <div class="pass-turn-button">
      <button @click="gameStore.endTurn">Pass Turn</button>
    </div>
    <BlockerModal
    />
  </div>
</template>

<script setup>
import { onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useGameStore } from '@/stores/gameStore'
import PlayerInfo from '@/components/PlayerInfo.vue'
import Battlefield from '@/components/Battlefield.vue'
import Hand from '@/components/Hand.vue'
import Deck from '@/components/Deck.vue'
import BlockerModal from '@/components/BlockerModal.vue'

const route = useRoute()
const router = useRouter()
const gameStore = useGameStore()

onMounted(async () => {
  gameStore.gameId = route.params.id
  await gameStore.fetchGameState()
})

watch(
  () => gameStore.isGameOver, (isGameOver) => {
    if(isGameOver) {
      router.push({name: 'GameOver', params: { winner:gameStore.winner.name}})
    }
  }
)

watch(
      () => gameStore.opponentIsAttacking,
      (newValue, oldValue) => {
        if (newValue && !oldValue) {
          console.log('AI IS ATTACKING')
          gameStore.promptForBlockers()
        }
      }
    )
    
</script>

<style scoped>
.game-view {
  display: flex;
    flex-direction: column;
    flex: 1;
    background-color: #1e1e1e;
    color: #fff;
    box-sizing: border-box;
    padding: 20px;
    height: 98svh;
}

.playmat {
    display: flex;
    flex-direction: column;
    flex: 1;
    justify-content: space-between;
    align-items: center;
    border: 2px solid #fff;
    border-radius: 10px;
    padding: 15px;
    box-sizing: border-box;
    position: relative;
  }

  .top-playmat {
    margin-bottom: 10px;
  }
  
  .bottom-playmat {
    margin-top: 10px;
  }
  
  .pass-turn-button {
    position: absolute;
    bottom: 20px;
    right: 20px;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 10px;
    border-radius: 5px;
  }
  
  .pass-turn-button button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 10px 20px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.pass-turn-button button:hover {
  background-color: #45a049;
}

</style>