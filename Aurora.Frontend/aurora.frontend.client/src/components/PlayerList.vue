<template>
    <div class="player-list">
        <PlayerInfo
            v-for="player in players"
            :key="player.id"
            :player="player"
            :gameId="gameId"
            @update-game-state="$emit('update-game-state', $event)"
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

defineEmits(['update-game-state'])
</script>

<style scoped>
.player-list {
    display: flex;
    justify-content: space-around;
    width: 100%;
}
</style>