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
    background-color: cyan;
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}
</style>