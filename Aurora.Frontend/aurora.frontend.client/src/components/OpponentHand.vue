<template>
    <div class="opponent-hand">
      <div v-for="(card, index) in handCount" :key="index" class="card">Card</div>
    </div>
  </template>
  
  <script setup>
  const props = defineProps({
    handCount: {
      type: Number,
      required: true,
    },
  });
  </script>
  
  <style scoped>
  .opponent-hand {
    display: flex;
    justify-content: center;
    margin-bottom: 1rem;
  }
  
  .card {
    width: 80px;
    height: 120px;
    background-color: #e53935; /* Red color for opponent's hand */
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