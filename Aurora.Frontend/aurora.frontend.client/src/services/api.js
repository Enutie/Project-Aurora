import axios from 'axios'

const API_BASE_URL = '/api'

export function createGame(playerName) {
    return axios.post(`${API_BASE_URL}/Games/create-game`, { playerName });
  }
  
  export function getGameState(gameId) {
    return axios.get(`${API_BASE_URL}/games/${gameId}`);
  }
  
  export function playLand(gameId, playerId, landId) {
    return axios.post(`${API_BASE_URL}/Games/${gameId}/play-land`, { playerId, landId });
  }
  
  export function castCreature(gameId, playerId, creatureId, manaCost) {
    const requestBody = {
      playerId: playerId,
      creatureId: creatureId,
      manaCost: manaCost
    }
    return axios.post(`${API_BASE_URL}/Games/${gameId}/cast-creature`, requestBody);
  }
  
  export function attack(gameId, attackingPlayerId, attackingCreatureIds) {
    return axios.post(`${API_BASE_URL}/Games/${gameId}/attack`, { attackingPlayerId, attackingCreatureIds });
  }
  
  export function assignBlockers(gameId, defendingPlayerId, blockerAssignments) {
    return axios.post(`${API_BASE_URL}/Games/${gameId}/assign-blockers`, { defendingPlayerId, blockerAssignments });
  }
  
  export function endTurn(gameId) {
    return axios.post(`${API_BASE_URL}/Games/${gameId}/end-turn`);
  }