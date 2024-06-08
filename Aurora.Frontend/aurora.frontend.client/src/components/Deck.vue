<!-- Deck.vue -->
<template>
  <div :class="`deck ${position}-deck`" @mouseover="showCount" @mouseleave="hideCount">
    <div class="deck-label">Deck</div>
    <div class="card-count" v-if="isHovering">
      {{ cardCount }} cards
    </div>
  </div>
</template>
  
  <script setup>
import { ref } from 'vue'

defineProps({
  position: {
    type: String,
    required: true,
    validator: value => ['top', 'bottom'].includes(value)
  },
  cardCount: {
    type: Number,
    required: true
  }
})

const isHovering = ref(false)

function showCount() {
  isHovering.value = true
}

function hideCount() {
  isHovering.value = false
}
  </script>
  
  <style scoped>
 .deck {
  position: relative;
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

  .card-count {
  position: absolute;
  top: 100%;
  left: 50%;
  transform: translateX(-50%);
  background-color: rgba(0, 0, 0, 0.8);
  color: #fff;
  padding: 5px 10px;
  border-radius: 3px;
  font-size: 12px;
  white-space: nowrap;
}
  </style>