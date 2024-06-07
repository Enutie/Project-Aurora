import { ref } from 'vue'

export function useGameState() {
  const topBattlefieldCards = ref([
    { name: 'Creature 1', type: 'creature-card' },
    { name: 'Creature 2', type: 'creature-card' },
    { name: 'Land 1', type: 'land-card' }
  ])

  const topHandCards = ref([
    { name: 'Card 1', type: 'card' },
    { name: 'Card 2', type: 'card' },
    { name: 'Card 3', type: 'card' },
    { name: 'Card 4', type: 'card' },
    { name: 'Card 5', type: 'card' }
  ])

  const bottomBattlefieldCards = ref([
    { name: 'Creature 3', type: 'creature-card' },
    { name: 'Land 2', type: 'land-card' },
    { name: 'Land 3', type: 'land-card' }
  ])

  const bottomHandCards = ref([
    { name: 'Card 6', type: 'card' },
    { name: 'Card 7', type: 'card' },
    { name: 'Card 8', type: 'card' },
    { name: 'Card 9', type: 'card' }
  ])

  const passTheTurn = () => {
    // Handle the turn passing logic here
    console.log('Passing the turn...')
  }

  return {
    topBattlefieldCards,
    topHandCards,
    bottomBattlefieldCards,
    bottomHandCards,
    passTheTurn
  }
}