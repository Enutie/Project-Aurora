<template>
    <div class="zone">
        <h4>{{ title }}</h4>
        <ul class="card-list">
            <li v-for="(card, index) in cards" :key="card.id" >
                <Card 
                :card="card"
                :index="index"
                @click="playCard(card, index)"
                />
                 
            </li>
        </ul>
    </div>
</template>

<script setup>
import Card from '../components/Card.vue'
const emit = defineEmits(['play-card'])
defineProps({
    title: {
        type: String,
        required: true,
    },
    cards: {
        type: Array,
        required: true,
    }
})

function playCard(card, index) {
    emit('play-card', { card, index })
}

</script>

<style scoped>
.zone {
  margin-top: 10px;
}

.card-list {
  list-style-type: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  justify-content: center;
}

</style>