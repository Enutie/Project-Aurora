<!-- Deck.vue -->
<template>
    <div :class="`deck ${position}-deck`">Deck</div>
  </template>
  
  <script setup>
  
    defineProps( {
      position: {
        type: String,
        required: true,
        validator: value => ['top', 'bottom'].includes(value)
      }
    }
)
  </script>
  
  <style scoped>
  .deck {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 80px;
    height: 120px;
    background-color: #f0f0f0;
    border-radius: 5px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    color: #000;
    font-weight: bold;
    text-align: center;
    padding: 5px;
  }
  
  .bottom-deck {
    position: absolute;
    bottom: 1.5em;
    left: 1em;
  }
  
  .top-deck {
    position: absolute;
    top: 1.5em;
    left: 1em;
  }
  </style>