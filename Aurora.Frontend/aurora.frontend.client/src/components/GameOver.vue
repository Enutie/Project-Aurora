<template>
    <div class="game-over">
        <div class="overlay"></div>
        <div class="content">
            <h3>Game Over</h3>
            <p>Winner: {{ winner.name }}</p>
        </div>
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

<style scoped>
.game-over {
    align-items: center;
    display: flex;
    justify-content: center;
    left: 0;
    position: fixed;
    top: 0;
    height: 100%;
    width: 100%;
}

.overlay {
    background-color: rgba(0, 0, 0, 0.7);
    height: 100%;
    left: 0;
    position: absolute;
    top: 0;
    width: 100%;
    z-index: 1;
}

.content {
    background-color: #fff;
    border-radius: 8px;
    padding: 20px;
    text-align: center;
    z-index: 2;
}
</style>