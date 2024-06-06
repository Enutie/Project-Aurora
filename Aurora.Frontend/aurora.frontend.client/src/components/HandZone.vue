<template>
    <div class="hand-zone">
      <Card
        v-for="(card, index) in cards"
        :key="index"
        :card="card"
        :index="index"
        :is-playable="isPlayable"
        @card-click="$emit('card-click', $event)"
      />
    </div>
  </template>
  
  <script setup>
  import Card from './Card.vue';
  
  const props = defineProps({
    cards: {
      type: Array,
      required: true
    },
    isPlayable: {
      type: Boolean,
      default: false
    }
  });
  </script>
  
  <style scoped>
  /* Add styles for hand zone */
  </style>