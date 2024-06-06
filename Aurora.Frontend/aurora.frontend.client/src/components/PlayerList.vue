<template>
    <div class="player-list">
        <PlayerInfo
            v-for="player in players"
            :key="player.id"
            :player="player"
            :gameId="gameId"
        />
    </div>
</template>

<script setup>
import PlayerInfo from './PlayerInfo.vue';

defineProps({
    players: {
        type: Array,
        required: true,
    },
    gameId: {
        type: String,
        required: true
    }
})
</script>

<style scoped>
.player-list {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
}
</style>