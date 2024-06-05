<template>
    <div class="player-hand">
      <div
        v-for="(card, index) in handCount"
        :key="index"
        class="card"
        @click="selectCard(index)"
      >
        Card
      </div>
    </div>
  </template>
  
  <script setup>
  const props = defineProps({
    handCount: {
      type: Number,
      required: true,
    },
  });

  const emit = defineEmits(['select-card']);

const selectCard = (index) => {
  emit('select-card', index);
};
  </script>
  
  <style scoped>
  .player-hand {
    display: flex;
    justify-content: center;
    margin-bottom: 1rem;
  }
  
  .card {
    width: 80px;
    height: 120px;
    background-color: #4caf50; /* Green color for player's hand */
    border-radius: 8px;
    margin: 0 0.5rem;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: bold;
    color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }
  </style>