<!-- Battlefield.vue -->
<template>
    <div class="battlefield">
      <Card
        v-for="(card, index) in cards"
        :key="index"
        :cardName="card.name"
        :cardType="card.type"
      />
    </div>
  </template>
  
  <script setup>
  import Card from './Card.vue'
  
  defineProps( {
      cards: {
        type: Array,
        required: true
      }
    }
)
  </script>
  
  <style scoped>
  .battlefield {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-wrap: wrap;
    gap: 10px;
    width: 100%;
    min-height: 150px;
    background-color: #2e2e2e;
    border-radius: 5px;
    padding: 10px;
    box-sizing: border-box;
  }
  </style>