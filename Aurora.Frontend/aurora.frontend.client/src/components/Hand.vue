<!-- Hand.vue -->
<template>
    <div class="hand">
      <Card
        v-for="(card, index) in cards"
        :key="index"
        :cardName="card.name"
        :cardType="card.type"
        :isInHand="true"
        @play-card="cardClicked(card)"
      />
    </div>
  </template>
  
  <script setup>
  import Card from './Card.vue'

  const emit = defineEmits(['play-land', 'cast-creature'])

  function cardClicked(card) {
    if(card.type === 'Land') {
      emit('play-land', card)
    } else if(card.type === 'Creature') {
      emit('cast-creature', card)
    }
  }
  
  defineProps({
      cards: {
        type: Array,
        required: true
      },
      })

  </script>
  
  <style scoped>
  .hand {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-wrap: wrap;
    gap: 10px;
    width: 100%;
    min-height: 100px;
    background-color: #2e2e2e;
    border-radius: 5px;
    padding: 10px;
    box-sizing: border-box;
  }
  </style>