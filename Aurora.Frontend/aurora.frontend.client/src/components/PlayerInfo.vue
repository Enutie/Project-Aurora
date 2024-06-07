<!-- PlayerInfo.vue -->
<template>
    <div :class="`player-info ${position}-player-info`">
      <h2>{{ playerName }}</h2>
      <p>Life: {{ lifePoints }}</p>
    </div>
  </template>
  
  <script setup>
  
    defineProps ({
      playerName: {
        type: String,
        required: true
      },
      lifePoints: {
        type: Number,
        required: true
      },
      position: {
        type: String,
        required: true,
        validator: value => ['top', 'bottom'].includes(value)
      }
    })
  </script>
  
  <style scoped>
  .player-info {
    position: absolute;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 10px;
    border-radius: 5px;
  }
  
  .top-player-info {
    top: 20px;
    right: 20px;
  }
  
  .bottom-player-info {
    bottom: 20px;
    left: 20px;
  }
  </style>