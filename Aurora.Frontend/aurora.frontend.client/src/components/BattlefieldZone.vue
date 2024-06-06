<template>
    <div class="battlefield-zone">
      <Card
        v-for="(card, index) in cards"
        :key="index"
        :card="card"
        :index="index"
      />
    </div>
  </template>
  
  <script setup>
  import Card from './Card.vue';
  
  const props = defineProps({
    cards: {
      type: Array,
      required: true
    }
  });
  </script>
  
  <style scoped>
  /* Add styles for battlefield zone */
  </style>