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
    background-color: #f0f0f0;
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    cursor: pointer;
    display: flex;
    flex-direction: column;
    height: 120px;
    justify-content: space-between;
    padding: 10px;
    transition: transform 0.3s ease;
  }

  .card:hover {
    transform: scale(1.05);
  }

  .creature-card {
    background-color: #c7d2fe;
  }
  </style>