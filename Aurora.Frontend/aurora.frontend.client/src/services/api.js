import axios from 'axios'

const API_BASE_URL = '/api'

export function createGame(playerName) {
    return axios.post(`${API_BASE_URL}/games`, { playerName });
  }
  
  export function getGameState(gameId) {
    return axios.get(`${API_BASE_URL}/games/${gameId}`);
  }
  
  export function playLand(gameId, playerId, landIndex) {
    return axios.post(`${API_BASE_URL}/games/${gameId}/play-land`, { playerId, landIndex });
  }
  
  export function castCreature(gameId, playerId, creatureIndex) {
    return axios.post(`${API_BASE_URL}/games/${gameId}/cast-creature`, { playerId, creatureIndex });
  }
  
  export function attack(gameId, attackerId, defenderId, attackingCreatureIds) {
    return axios.post(`${API_BASE_URL}/games/${gameId}/attack`, { attackerId, defenderId, attackingCreatureIds });
  }
  
  export function endTurn(gameId) {
    return axios.post(`${API_BASE_URL}/games/${gameId}/end-turn`);
  }