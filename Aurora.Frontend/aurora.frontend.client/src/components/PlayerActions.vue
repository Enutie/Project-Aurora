<template>
    <div class="player-actions">
        <h3>CurrentPlayer: {{ player.name }}</h3>
    </div>
</template>

<script setup>
defineProps({
    player: {
        type: Object,
        required: true,
    }
})
</script>