<template>
  <div class="game-view">
      <div class="playmat top-playmat">
        <Deck position="top" :cardCount="players[1]?.deckCount || 0"/>
        <Hand 
          :cards="players[1]?.hand || []" 
          :playerId="players[1]?.id || ''"
          @play-land="playLand"
          @cast-creature="castCreature"/>
        <Battlefield 
          :cards="players[1]?.battlefield || []"
          :playerId="players[1]?.id || ''"
          :opponentId="players[0]?.id || ''"
          @attack="attack"
           />
        <PlayerInfo
          :player="players[1] || {}"
          position="top"
        />
      </div>
      <div class="playmat bottom-playmat">
      <PlayerInfo
        :player="players[0] || {}"
        position="bottom"
      />
      <Battlefield 
        :cards="players[0]?.battlefield || []"
        :playerId="players[0]?.id || ''"
        :opponentId="players[1]?.id || ''"
        @attack="attack"
         />
      <Hand 
          :cards="players[0]?.hand || []" 
          playerId="players[0]?.id || ''"
          @play-land="playLand"
          @cast-creature="castCreature"/>
      <Deck position="bottom" :cardCount="players[0]?.deckCount || 0" />
    </div>
    <div class="pass-turn-button">
      <button @click="endTurn">Pass Turn</button>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { 
  getGameState, 
  endTurn as endTurnAPI, 
  playLand as playLandAPI,
  castCreature as castCreatureAPI,
  attack as attackAPI,
  assignBlockers as assignBlockersAPI
} from '@/services/api'
import PlayerInfo from '@/components/PlayerInfo.vue';
import Battlefield from '@/components/Battlefield.vue';
import Hand from '@/components/Hand.vue';
import Deck from '@/components/Deck.vue';

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
    currentPlayer.value = gameState.players[gameState.currentPlayerIndex]
    isGameOver.value = gameState.isGameOver
    winner.value = gameState.winner
}

async function endTurn() {
  try {
    const response = await endTurnAPI(gameId.value)
    updateGameState(response.data)
  } catch (error) {
    console.error('Error ending turn:', error)
  }
}

async function castCreature(creature) {
  try {
    console.log(gameId.value)
    console.log(currentPlayer.value.id)
    console.log(creature.id)
    console.log(creature.manaCost)
    const response = await castCreatureAPI(gameId.value, currentPlayer.value.id, creature.id, creature.manaCost)
    updateGameState(response.data)
  } catch (error) {
    console.error('Error casting creature')
  }
}

async function playLand(land) {
  try {
    const response = await playLandAPI(gameId.value, currentPlayer.value.id, land.id)
    updateGameState(response.data)
  } catch (error) {
    console.error('Error playing land:', error)
  }
}

async function attack(attackingPlayerId, attackingCreatureIds){
  try {
    const response = await attackAPI(gameId.value, attackingPlayerId, attackingCreatureIds)
    updateGameState(response.data)
  } catch(error) {
    console.err('Error attacking', error)
  }
}

async function assignBlockers(defendingPlayerId, blockerAssignments) {
  try {
    const response = await assignBlockersAPI(gameId.value, defendingPlayerId, blockerAssignments);
    updateGameState(response.data);
  } catch (error) {
    console.error('Error assigning blockers:', error);
  }
}

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