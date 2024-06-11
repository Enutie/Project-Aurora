<template>
  <div class="battlefield">
    <Card
      v-for="(card, index) in cards"
      :key="index"
      :cardName="card.name"
      :cardType="card.type"
      :index="index"
      :isAttacking="attackingCreatureIds.includes(card.id)"
      @toggle-attack="toggleAttack(index)"
    />
  </div>
  <button class="attack-button" @click="handleAttack" v-if="attackingCreatureIds.length > 0">Attack</button>
</template>

<script setup>
import Card from './Card.vue'
import { ref } from 'vue'

const props = defineProps({
  cards: {
    type: Array,
    required: true
  },
  playerId: {
    type: String,
    required: true
  },
  opponentId: {
    type: String,
    reqruired: true
  }
})

const emit = defineEmits(['attack'])

function handleAttack() {
  emit('attack', props.playerId, attackingCreatureIds.value);
}

const attackingCreatureIds = ref([])

function toggleAttack(index) {
  const creatureId = props.cards[index].id
  const creatureIndex = attackingCreatureIds.value.indexOf(creatureId)
  if (creatureIndex === -1) {
    attackingCreatureIds.value.push(creatureId)
  } else {
    attackingCreatureIds.value.splice(creatureIndex, 1)
  }
}

</script>

<style scoped>
.battlefield {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
  width: 100%;
  min-height: 150px;
  background-color: #2e2e2e;
  border-radius: 5px;
  padding: 10px;
  box-sizing: border-box;
}
.attack-button {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 10px 20px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin-top: 10px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}

.attack-button:hover {
  background-color: #d32f2f;
}

.attack-button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}
</style>