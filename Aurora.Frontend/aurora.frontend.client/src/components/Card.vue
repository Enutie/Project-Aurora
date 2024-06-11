<template>
  <div :class="['card', cardType, { 'tapped': isTapped, 'attacking': isAttacking }]">
    <span class="card-name">{{ cardName }}, {{ manaCost }}</span>
    <div class="card-buttons" v-if="isInHand">
      <button class="play-button" v-if="!isLand" @click="gameStore.castCreature(card.id)">Play</button>
      <button class="play-button" v-if="isLand" @click="gameStore.playLand(card.id)">Play</button>
    </div>
    <div class="card-buttons" v-else-if="isCreature">
      <button
        class="attack-button"
        @click="toggleAttack"
        :class="{ 'attacking': isAttacking }"
      >
        {{ isAttacking ? 'Attacking' : 'Attack' }}
      </button>
      <button class="block-button" v-if="isBlocked">Block</button>
    </div>
    <div class="card-buttons" v-else-if="isLand">
      <button class="tap-button" @click="toggleTap">{{ isTapped ? 'Untap' : 'Tap' }}</button>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useGameStore } from '@/stores/gameStore'

const props = defineProps({
  card: {
    type: Object,
    required: true
  },
  isInHand: {
    type: Boolean,
    default: false
  }
})

const gameStore = useGameStore()

const isCreature = computed(() => {
  return props.card.type === 'Creature'
})

const isLand = computed(() => {
  return props.card.type === 'Land'
})

const cardName = computed(() => {
  return props.card.name
})

const cardType = computed(() => {
  return props.card.type.toLowerCase()
})

const isTapped = computed(() => {
  return props.card.isTapped
})

const isAttacking = computed(() => {
  return props.card.isAttacking
})

const isBlocked = computed(() => {
  return props.card.isBlocked
})

const manaCost = computed(() => {
  return props.card.manaCost
})

function toggleTap() {
  // Implement tap/untap logic here if needed
}

function toggleAttack() {
  // Implement attack logic here if needed
}
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
  .attacking {
  border: 2px solid red;
}
  
  .card-name {
    display: block;
    font-weight: bold;
  }

  .tapped {
  transform: rotate(90deg);
}
.tapped:hover {
  transform: rotate(90deg) translateY(-5px);
}
  
  .card-buttons {
    display: none;
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 0;
    text-align: center;
  }
  
  .card:hover .card-buttons {
    display: flex;
    justify-content: center;
  }
  
  .tap-button,
  .play-button,
  .attack-button,
  .block-button {
    background-color: #4CAF50;
    color: white;
    border: none;
    padding: 10px;
    text-decoration: none;
    font-size: 14px;
    cursor: pointer;
    flex: 1;
    margin:0;
    transition: background-color 0.3s ease;
  }.tap-button {
  background-color: #FFC107;
}

.tap-button:hover {
  background-color: #FFA000;
}
  .attack-button:hover,
.block-button:hover {
  background-color: #388e3c; /* Darker shade on hover */
}
  .block-button {
    background-color: #2196F3;
  }
 .card:hover .play-button {
  display: block; 
}

.play-button:hover {
  background-color: #388e3c;
}

.attacking {
  border: 2px solid red;
  box-shadow: 0 0 8px red;
}
  </style>