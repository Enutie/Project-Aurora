<template>
    <div class="game-board">
      <div v-if="!gameId">
        <input v-model="playerName" placeholder="Enter your name" />
        <button @click="createGame(playerName)">Start Game</button>
      </div>
      <div v-else>
        <PlayerHand
          :handCount="playerHandCount"
          @select-card="selectCard"
        />
        <OpponentHand :handCount="opponentHandCount" />
        <LandArea
          :playerLandsCount="playerLandsCount"
          :opponentLandsCount="opponentLandsCount"
        />
        <GameControls
          :handCount="playerHandCount"
          :currentPlayerId="currentPlayer"
          :selectedCardIndex="selectedCardIndex"
          @play-land="playLand"
          @ai-play="aiPlay"
        />
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, reactive, watch } from 'vue';
  import PlayerHand from './PlayerHand.vue';
  import OpponentHand from './OpponentHand.vue';
  import LandArea from './LandArea.vue';
  import GameControls from './GameControls.vue';
  
  const props = defineProps({
    gameId: {
      type: String,
      required: true,
    },
    players: {
      type: Array,
      required: true,
    },
    currentPlayer: {
      type: String,
      required: true,
    },
  });
  
  const playerName = ref('');
  const gameState = reactive({
    players: [],
    currentPlayer: '',
  });
  const selectedCardIndex = ref(null);
  
  watch(
    () => props.players,
    (newPlayers) => {
      gameState.players = newPlayers;
    }
  );
  
  watch(
    () => props.currentPlayer,
    (newCurrentPlayer) => {
      gameState.currentPlayer = newCurrentPlayer;
    }
  );
  
  const emit = defineEmits(['create-game', 'play-land', 'ai-play']);
  
  const createGame = (name) => {
    emit('create-game', name);
  };
  
  const selectCard = (index) => {
    selectedCardIndex.value = index;
  };
  
  const getPlayerById = (playerId) =>
  computed(() => gameState.players.find((p) => p.id === playerId));

const playerHandCount = computed(() =>
  getPlayerById(gameState.currentPlayer).value?.handCount || 0
);

const opponentHandCount = computed(() =>
  getPlayerById(
    gameState.players.find((p) => p.id !== gameState.currentPlayer)?.id
  ).value?.handCount || 0
);

const playerLandsCount = computed(() =>
  getPlayerById(gameState.currentPlayer).value?.battlefieldCount || 0
);

const opponentLandsCount = computed(() =>
  getPlayerById(
    gameState.players.find((p) => p.id !== gameState.currentPlayer)?.id
  ).value?.battlefieldCount || 0
);
  
  const playLand = async (playerId, landIndex) => {
    try {
      const response = await fetch(`/api/Games/${props.gameId}/play`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ playerId, landIndex }),
      });
  
      if (response.ok) {
        const data = await response.json();
        gameState.players = data.players;
        gameState.currentPlayer = data.currentPlayer;
        selectedCardIndex.value = null; // Reset the selected card index after playing
      } else {
        console.error('Error playing land:', response.status);
      }
    } catch (error) {
      console.error('Error playing land:', error);
    }
  };
  
  const aiPlay = async () => {
    try {
      const response = await fetch(`/api/Games/${props.gameId}/ai-play`, {
        method: 'POST',
      });
  
      if (response.ok) {
        const data = await response.json();
        gameState.players = data.players;
        gameState.currentPlayer = data.currentPlayer;
      } else {
        console.error('Error with AI play:', response.status);
      }
    } catch (error) {
      console.error('Error with AI play:', error);
    }
  };
  </script>