<template>
    <div class="player-info">
      <div class="player-name">{{ playerName }}</div>
      <div class="player-life">Life: {{ playerLife }}</div>
    </div>
  </template>
  
  <script setup>
  import { computed } from 'vue';
  
  const props = defineProps({
    player: {
      type: Object,
      required: true
    }
  });
  
  const playerName = computed(() => props.player?.name || '');
  const playerLife = computed(() => props.player?.life || 0);
  </script>
  
  <style scoped>
  /* Add styles for player info */
  </style>