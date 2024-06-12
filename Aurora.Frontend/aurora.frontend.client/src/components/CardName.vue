<!-- CardName.vue -->
<template>
    <span class="card-name">{{ cardName }} {{ formattedManaCost }}</span>
  </template>
  
  <script setup>
  import { computed } from 'vue'
  
  const props = defineProps({
    card: {
      type: Object,
      required: true
    }
  })
  
  const cardName = computed(() => {
    return props.card.name
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
    result += 'G'.repeat(manaCount.Green);
  }
  if (manaCount.Blue > 0) {
    result += 'U'.repeat(manaCount.Blue);
  }
  if (manaCount.Red > 0) {
    result += 'R'.repeat(manaCount.Red);
  }
  if (manaCount.White > 0) {
    result += 'W'.repeat(manaCount.White);
  }
  if (manaCount.Black > 0) {
    result += 'B'.repeat(manaCount.Black);
  }
  return result;
})
  </script>

  <style scoped>
.card-name {
  display: block;
  font-weight: bold;
}</style>