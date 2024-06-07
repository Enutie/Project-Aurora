<template>
    <div class="player-info">
        <h3>{{ player.name }}</h3>
        <p>Life: {{ player.life }}</p>
        <p>Deck Count: {{ player.deckCount }}</p>
        <Zone :title="`Hand (${player.handCount})`" :cards="player.hand" @play-card="playCard"/>
        <Zone title="Battlefield" :cards="player.battlefield" />
    </div>
</template>

<script setup>
import { playLand, castCreature, getGameState } from '@/services/api';
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

const emit = defineEmits(['update-game-state'])

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
        const response = await getGameState(props.gameId)
        emit('update-game-state', response.data)
    } catch (error) {
        console.error('Error playing card: ', error)
    }
}


</script>

<style scoped>
.player-info {
    background-color: crimson;
    padding: 10px;
    border-radius: 4px;
}
</style>