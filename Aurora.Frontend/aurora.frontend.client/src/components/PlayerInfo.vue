<template>
    <div class="player-info">
        <h3>{{ player.name }}</h3>
        <p>Life: {{ player.life }}</p>
        <p>Hand Count: {{ player.handCount }}</p>
        <p>Deck Count: {{ player.deckCount }}</p>
        <Zone title="Hand" :cards="player.hand" @play-card="playCard"/>
        <Zone title="Battlefield" :cards="player.battlefield" />
    </div>
</template>

<script setup>
import { playLand, castCreature } from '@/services/api';
import Zone from './Zone.vue';
const props = defineProps({
    player: {
        type: Object,
        required: true,
    },
    gameId: {
        type: String,
        required: true,
    }
})

async function playCard({card, index})
{
    try {
        if(card.power === null && card.toughness === null)
        {
            await playLand(props.gameId, props.player.id, index)
        } else {
            await castCreature(props.gameId, props.player.id, index)
        }
        // Refresh the game state after playing the card
    } catch (error) {
        console.error('Error playing card: ', error)
    }
}

</script>

<style scoped>
.player-info {
    background-color: #f5f5f5;
    padding: 10px;
    border-radius: 4px;
}
</style>