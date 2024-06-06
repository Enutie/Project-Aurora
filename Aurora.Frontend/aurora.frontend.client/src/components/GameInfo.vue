<template>
    <div class="game-info">
        <h2>Game ID: {{ gameId }}</h2>
    </div>
</template>

<script setup>
defineProps({
    gameId: {
        type: String,
        required: true,
    }
})
</script>