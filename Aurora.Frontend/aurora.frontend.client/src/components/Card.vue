<template>
  <div :class="[ 'card', cardType, { 'tapped': isTapped, 'attacking': isAttackingClass } ]">
    <span class="card-name">{{ cardName }}, {{ formattedManaCost }}</span>
    <div class="card-buttons" v-if="isCurrentPlayer && isInHand">
      <button class="play-button" v-if="!isLand" @click="playCreature">Play</button>
      <button class="play-button" v-if="isLand" @click="playLand">Play</button>
    </div>
    <div class="card-buttons" v-else-if="isCreature && isCurrentPlayer">
      <button
        class="attack-button"
        @click="toggleAttack"
        :class="{ 'attacking': isAttacking }"
      >
        {{ isAttacking ? 'Attacking' : 'Attack' }}
      </button>
      <button class="block-button" v-if="isBlocked">Block</button>
    </div>
    <div class="card-buttons" v-else-if="isLand && isCurrentPlayer">
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

const manaCost = computed(() => {
  return props.card.manaCost || []
})

const formattedManaCost = computed(() => {
  const manaCount = {
    Green: 0,
    Blue: 0,
    Red: 0,
    White: 0,
    Black: 0,
    Colorless: 0
  };

  manaCost.value.forEach(mana => {
    manaCount[mana]++;
  });

  let result = '';

  if (manaCount.Colorless > 0) {
    result += manaCount.Colorless;
  }
  if (manaCount.Green > 0) {
    result += manaCount.Green > 1 ? manaCount.Green + 'G' : 'G';
  }
  if (manaCount.Blue > 0) {
    result += manaCount.Blue > 1 ? manaCount.Blue + 'U' : 'U';
  }
  if (manaCount.Red > 0) {
    result += manaCount.Red > 1 ? manaCount.Red + 'R' : 'R';
  }
  if (manaCount.White > 0) {
    result += manaCount.White > 1 ? manaCount.White + 'W' : 'W';
  }
  if (manaCount.Black > 0) {
    result += manaCount.Black > 1 ? manaCount.Black + 'B' : 'B';
  }

  return result;
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
.attacking {
  border: 2px solid red;
  box-shadow: 0 0 8px red;
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
  margin: 0;
  transition: background-color 0.3s ease;
}
.tap-button {
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
