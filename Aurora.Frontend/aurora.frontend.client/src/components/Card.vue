<!-- Card.vue -->
<template>
    <div :class="['card', cardType]">
      <span class="card-name">{{ cardName }}</span>
      <div class="card-buttons" v-if="isCreature">
        <button class="attack-button">Attack</button>
        <button class="block-button">Block</button>
      </div>
    </div>
  </template>
  
  <script setup>
  import { computed} from 'vue'
    const props = defineProps( {
      cardName: {
        type: String,
        required: true
      },
      cardType: {
        type: String,
        required: true,
        validator: value => ['creature-card', 'land-card'].includes(value)
      }
    })

    const isCreature =  computed( () => {
       
        return props.cardType === 'creature-card'
      
    })
  </script>
  
  <style scoped>
  .card {
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
    position: relative;
    overflow: hidden;
    transition: transform 0.3s;
  }
  
  .card:hover {
    transform: translateY(-5px);
  }
  
  .creature-card {
    background-color: #8bc34a;
  }
  
  .land-card {
    background-color: #795548;
  }
  
  .card-name {
    display: block;
    font-weight: bold;
  }
  
  .card-buttons {
    display: none;
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 5px;
    text-align: center;
  }
  
  .card:hover .card-buttons {
    display: flex;
    justify-content: center;
    gap: 10px;
  }
  
  .attack-button,
  .block-button {
    background-color: #4CAF50;
    color: white;
    border: none;
    padding: 5px 10px;
    text-decoration: none;
    font-size: 14px;
    cursor: pointer;
  }
  
  .block-button {
    background-color: #2196F3;
  }
  </style>