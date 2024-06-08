<!-- PlayerInfo.vue -->
<template>
    <div :class="`player-info ${position}-player-info`">
      <h2>{{ player?.name || 'Unknown Player' }}</h2>
    <p>Life: {{ player?.life || 0 }}</p>
    </div>
  </template>
  
  <script setup>
  defineProps ({
      player: {
        type: Object,
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