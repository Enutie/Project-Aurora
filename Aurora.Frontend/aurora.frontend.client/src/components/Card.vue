<template>
  <div :class="[ 'card', cardType, { 'tapped': isTapped, 'attacking': isAttackingClass } ]">
    <CardName :card="card" />
    <CardButtons
      :isLand="isLand"
      :isCreature="isCreature"
      :isCurrentPlayer="isCurrentPlayer"
      :isTapped="isTapped"
      :isAttacking="isAttacking"
      :isBlocked="isBlocked"
      :isInHand="isInHand"
      @toggleTap="toggleTap"
      @toggleAttack="toggleAttack"
      @playCreature="playCreature"
      @playLand="playLand"
    />
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useGameStore } from '@/stores/gameStore'
import CardButtons from './CardButtons.vue'
import CardName from './CardName.vue'

const props = defineProps({
  card: {
    type: Object,
    required: true
  },
  isInHand: {
    type: Boolean,
    default: false
  },
  playerId: {
    type: String,
    required: true
  }
})

const gameStore = useGameStore()

const isCurrentPlayer = computed(() => {
  return props.playerId === gameStore.currentPlayer.id
})

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

const isAttackingClass = computed(() => {
  return gameStore.attackingCreatureIds.includes(props.card.id)
})

function toggleTap() {
  // Implement tap/untap logic here if needed
}

function toggleAttack() {
  gameStore.toggleAttack(props.card.id)
}

function playCreature() {
  gameStore.castCreature(props.card.id)
}

function playLand() {
  gameStore.playLand(props.card.id)
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
  transition: transform 0.3s, box-shadow 0.3s;
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

.tapped {
  transform: rotate(90deg);
}

.tapped:hover {
  transform: rotate(90deg) translateY(-5px);
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

.tapped {
  transform: rotate(90deg);
}

.tapped:hover {
  transform: rotate(90deg) translateY(-5px);
}

.attacking {
  border: 2px solid red;
  box-shadow: 0 0 8px red;
}
</style>
