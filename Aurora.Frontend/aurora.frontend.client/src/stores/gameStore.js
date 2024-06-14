import { defineStore } from 'pinia'
import {
  getGameState,
  advancePhase as advancePhaseAPI,
  playLand as playLandAPI,
  castCreature as castCreatureAPI,
  attack as attackAPI,
  assignBlockers as assignBlockersAPI,
} from '@/services/api'

export const useGameStore = defineStore('game', {
  state: () => ({
    gameId: '',
    players: [],
    currentPlayer: {},
    isGameOver: false,
    winner: {},
    currentPhase: "",
    showBlockerModal: false,
    attackingCreatures: [],
    defendingCreatures: [],
    attackingCreatureIds: [],
  }),
  actions: {
    async fetchGameState() {
      try {
        const response = await getGameState(this.gameId)
        this.updateGameState(response.data)
      } catch (error) {
        console.error('Error fetching game state:', error)
      }
    },
    updateGameState(gameState) {
      this.players = gameState.players
      this.currentPlayer = gameState.players[gameState.currentPlayerIndex]
      this.isGameOver = gameState.isGameOver
      this.winner = gameState.winner
      this.currentPhase = gameState.currentPhase
      console.log('Gamestate updated:', gameState)

      if (this.opponentIsAttacking) {
        this.promptForBlockers()
      }
    },
    clearAttackingCreatures() {
      this.attackingCreatureIds = []
      this.attackingCreatures = []
      this.defendingCreatures = []
    },
    async advancePhase() {
      try {
        const response = await advancePhaseAPI(this.gameId)
        this.updateGameState(response.data)
        this.clearAttackingCreatures()
        console.log('GOING TO NEXT PHASE')
      } catch (error) {
        console.error('Error advancing phase:', error)
      }
    },
    async castCreature(creatureId) {
      try {
        const response = await castCreatureAPI(this.gameId, this.currentPlayer.id, creatureId)
        this.updateGameState(response.data)
      } catch (error) {
        console.error('Error casting creature')
      }
    },
    async playLand(landId) {
      try {
        const response = await playLandAPI(this.gameId, this.currentPlayer.id, landId)
        this.updateGameState(response.data)
      } catch (error) {
        console.error('Error playing land:', error)
      }
    },
    async attack(attackingPlayerId, attackingCreatureIds, opponentId) {
      try {
        const response = await attackAPI(this.gameId, attackingPlayerId, attackingCreatureIds)
        this.updateGameState(response.data)
        const attackingPlayer = this.players.find((player) => player.id === attackingPlayerId)
        if (attackingPlayer) {
          this.attackingCreatures = attackingPlayer.battlefield.filter((creature) => creature.isAttacking)
        }
        const defendingPlayer = this.players.find((player) => player.id === opponentId)
        if (defendingPlayer) {
          this.defendingCreatures = defendingPlayer.battlefield.filter((creature) => !creature.isAttacking)
          console.log('Updated defendingCreatures:', this.defendingCreatures); // Log defendingCreatures
        }
      } catch (error) {
        console.error('Error attacking', error)
      }
    },
    
    promptForBlockers() {
      const aiPlayer = this.players.find((player) => player.name === 'AI');
      if (aiPlayer) {
        const opponentAttackingCreatures = aiPlayer.battlefield.filter((creature) => creature.isAttacking);

        if (opponentAttackingCreatures.length > 0) {
          this.attackingCreatures = opponentAttackingCreatures;
          this.defendingCreatures = this.players[0].battlefield.filter((creature) => !creature.isAttacking);
          console.log('Prompting for blockers with defendingCreatures:', this.defendingCreatures); // Log defendingCreatures
          this.showBlockerModal = true;
          this.fetchGameState(); // Fetch game state before showing the modal
        }
      }
    },

    async assignBlockers(defendingPlayerId, blockerAssignments) {
      try {
        const response = await assignBlockersAPI(this.gameId, defendingPlayerId, blockerAssignments);
        this.attackingCreatures = [];
        this.defendingCreatures = [];
        for (const player of response.data.players) {
          for (const creature of player.battlefield) {
            creature.isAttacking = false;
          }
        }
        this.updateGameState(response.data);
        this.fetchGameState(); // Ensure fresh data after assigning blockers
      } catch (error) {
        console.error('Error assigning blockers:', error)
      }
    },
    
    toggleAttack(cardId) {
      const creatureIndex = this.attackingCreatureIds.indexOf(cardId)
      if (creatureIndex === -1) {
        this.attackingCreatureIds.push(cardId)
      } else {
        this.attackingCreatureIds.splice(creatureIndex, 1)
      }
    },
  },
  getters: {
    opponentIsAttacking() {
      const aiPlayerId = this.players.find((player) => player.name === 'AI')?.id
      if (aiPlayerId) {
        const aiPlayer = this.players.find((player) => player.id === aiPlayerId)
        const isAttacking = aiPlayer.battlefield.some((creature) => creature.isAttacking)
        console.log('AI is attacking:', isAttacking)
        
        return isAttacking
      }
      console.log('AI not found or not attacking')
      return false
    },
    getNextPhase() {
      const phases = [
        'Beginning',
        'MainPhase1',
        'Combat',
        'MainPhase2',
        'Ending',
        'Passturn'
      ]
      return phases[phases.indexOf(this.currentPhase) + 1]
    }
  },
})
