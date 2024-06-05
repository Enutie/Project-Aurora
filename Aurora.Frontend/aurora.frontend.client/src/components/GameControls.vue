<template>
    <div class="game-controls">
      <div class="buttons">
        <button
          class="play-land-button"
          @click="playLand"
          :disabled="selectedCardIndex === null"
        >
          Play Land
        </button>
        <button class="ai-play-button" @click="aiPlay">AI Play</button>
      </div>
    </div>
  </template>
  
  <script setup>
  const props = defineProps({
    currentPlayerId: {
      type: String,
      required: true,
    },
    selectedCardIndex: {
      type: Number,
      required: true,
    },
  });
  
  const emit = defineEmits(['play-land', 'ai-play']);
  
  const playLand = () => {
    if (props.selectedCardIndex !== null) {
      emit('play-land', props.currentPlayerId, props.selectedCardIndex);
    }
  };
  
  const aiPlay = () => {
    emit('ai-play');
  };
  </script>
  
  <!-- Styles remain the same -->
  
  <style scoped>
  .game-controls {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  
  .buttons {
    display: flex;
    justify-content: center;
  }
  
  button {
    padding: 0.5rem 1rem;
    font-size: 1rem;
    font-weight: bold;
    border: none;
    border-radius: 4px;
    color: #fff;
    cursor: pointer;
    margin: 0 0.5rem;
  }
  
  .play-land-button {
    background-color: #4caf50;
  }
  
  .play-land-button:disabled {
    background-color: #ccc;
    color: #333;
    cursor: not-allowed;
  }
  
  .ai-play-button {
    background-color: #2196f3;
  }
  </style>