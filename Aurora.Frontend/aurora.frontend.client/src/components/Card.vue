<template>
    <div 
    class="card" 
    :class="{ 'creature-card': isCreature }"
    @click="$emit('play', card, index)">
      <div class="card-name">{{ card.name }}</div>
      <div class="card-stats" v-if="isCreature">
        {{ card.power }}/ {{ card.toughness }}
      </div>
    </div>
  </template>
  
  <script setup>
  import { computed } from 'vue'
  const props = defineProps({
    card: {
      type: Object,
      required: true,
    },
    index: {
      type: Number,
      required: true,
    },
  });

  const isCreature = computed(() => props.card.power !== null && props.card.toughness !== null)
  </script>
  
  <style scoped>
  .card {
    background-color: white;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    padding: 10px;
    cursor: pointer;
    width: 120px;
    height: 160px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    transition: transform 0.3s ease-in-out;
  }
  .card:hover {
  transform: scale(1.05);
}
  .card-name {
    font-weight: bold;
    margin-bottom: 5px;
  }
  .card-stats {
    text-align: right;
  }
  .creature-card {
  background-color: #f0f0f0;
}
  </style>