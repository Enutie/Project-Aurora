<template>
    <div class="card" :class="{ 'not-allowed': !isPlayable }" @click="$emit('card-click', index)">
      <div class="card-name">{{ card.name }}</div>
      <div class="card-type">{{ card.type }}</div>
      <div v-if="card.type === 'Creature'">
        <div class="card-power">Power: {{ card.power }}</div>
        <div class="card-toughness">Toughness: {{ card.toughness }}</div>
      </div>
    </div>
  </template>
  
  <script setup>
  const props = defineProps({
    card: {
      type: Object,
      required: true
    },
    index: {
      type: Number,
      required: true
    },
    isPlayable: {
      type: Boolean,
      default: false
    }
  });
  </script>
  
  <style scoped>
 .card {
  width: 50px;
  height: 70px;
  background-color: #fff;
  border: 1px solid #ccc;
  border-radius: 4px;
  cursor: pointer;
}

.card.not-allowed {
  cursor: not-allowed;
}

.card {
    width: 40px;
    height: 60px;
  }
  </style>