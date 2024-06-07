<template>
    <div class="player-info">
        <div class="player-stats">
            <div class="stat-item">
                <h3>Name</h3>
                <p>{{ player.name }}</p>
            </div>
            <div class="stat-item">
                <h3>Life</h3>
                <p>{{ player.life }}</p>
            </div>
            <div class="stat-item">
                <h3>Deck Count</h3>
                <p>{{ player.deckCount }}</p>
            </div>
        </div>
        <div class="zones">
            <Zone :title="`Hand (${player.handCount})`" :cards="player.hand" @play-card="playCard"/>
            <Zone title="Battlefield" :cards="player.battlefield" />
        </div>
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
    background-color: #2e2e2e;
    border-radius: 1em;
    display: flex;
    flex-direction: column;
    height: 100%;
    padding: 1em;
    width: 94%;
}

.player-stats {
    background-color: darkolivegreen;
    border-radius: 1em;
    display: flex;
    justify-content: space-between;
    margin-bottom: 0.25em;
}

.stat-item {
    text-align: center;
}
</style>