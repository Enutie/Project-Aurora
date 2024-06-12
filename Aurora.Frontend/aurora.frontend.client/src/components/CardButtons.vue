<!-- CardButtons.vue -->
<!-- CardButtons.vue -->
<template>
    <div class="card-buttons">
      <button v-if="isLand && isCurrentPlayer && !isInHand" class="tap-button" @click="$emit('toggleTap')">
        {{ isTapped ? 'Untap' : 'Tap' }}
      </button>
      <button v-if="isCreature && isCurrentPlayer && !isInHand" class="attack-button" @click="$emit('toggleAttack')" :class="{ 'attacking': isAttacking }">
        {{ isAttacking ? 'Attacking' : 'Attack' }}
      </button>
      <button v-if="isCreature && isCurrentPlayer && !isInHand && isBlocked" class="block-button">Block</button>
      <button v-if="!isLand && isCurrentPlayer && isInHand" class="play-button" @click="$emit('playCreature')">Play</button>
      <button v-if="isLand && isCurrentPlayer && isInHand" class="play-button" @click="$emit('playLand')">Play</button>
    </div>
  </template>
  
  <script setup>
  defineProps({
    isLand: Boolean,
    isCreature: Boolean,
    isCurrentPlayer: Boolean,
    isTapped: Boolean,
    isAttacking: Boolean,
    isBlocked: Boolean,
    isInHand: Boolean,
  })
  </script>

  <style scoped>
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
  background-color: #388e3c;
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
}</style>