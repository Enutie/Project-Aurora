<template>
    <div class="land-area">
      <div class="player-lands">
        <div v-for="i in playerLandsCount" :key="i" class="land">Land</div>
      </div>
      <div class="opponent-lands">
        <div v-for="i in opponentLandsCount" :key="i" class="land">Land</div>
      </div>
    </div>
  </template>
  
  <script setup>
  const props = defineProps({
    playerLandsCount: {
      type: Number,
      required: true,
    },
    opponentLandsCount: {
      type: Number,
      required: true,
    },
  });
  </script>
  
  <style scoped>
  .land-area {
    display: flex;
    justify-content: space-between;
    margin-bottom: 1rem;
  }
  
  .player-lands,
  .opponent-lands {
    display: flex;
    flex-wrap: wrap;
    width: 45%;
  }
  
  .land {
    width: 60px;
    height: 60px;
    background-color: #8bc34a;
    border-radius: 4px;
    margin: 0.25rem;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: bold;
    color: #fff;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }
  </style>