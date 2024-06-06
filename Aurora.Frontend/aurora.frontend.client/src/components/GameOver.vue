<template>
    <div class="game-over">
        <h3>Game Over</h3>
        <p>Winner: {{ winner.name }}</p>
    </div>
</template>

<script setup>
defineProps({
    winner: {
        type: Object,
        required: true,
    }
})
</script>