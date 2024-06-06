<template>
    <div class="zone">
        <h4>{{ title }}</h4>
        <ul class="card-list">
            <li v-for="(card, index) in cards" :key="card.id" @click="playCard(card, index)" >
                <div class="card">
                    <div class="card-name">{{ card.name }}</div>
                    <div class="card-stats" v-if="card.power !== null && card.toughness !== null" >
                        {{ card.power }}/ {{ card.toughness }}
                    </div>
                </div>
                 
            </li>
        </ul>
    </div>
</template>

<script setup>
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
}

.card {
  background-color: white;
  border: 1px solid #ccc;
  border-radius: 4px;
  padding: 10px;
  cursor: pointer;
  width: 120px;
  height: 160px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.card-name {
  font-weight: bold;
}

.card-stats {
  text-align: right;
}
</style>