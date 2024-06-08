<template>
  <div class="game-container">
    <div class="playmat top-playmat">
      <Deck position="top" />
      <Hand :cards="topHandCards" />
      <Battlefield :cards="topBattlefieldCards" />
      <PlayerInfo
        playerName="Player 2"
        :lifePoints="20"
        position="top"
      />
    </div>
    <div class="playmat bottom-playmat">
      <PlayerInfo
        playerName="Player 1"
        :lifePoints="20"
        position="bottom"
      />
      <Battlefield :cards="bottomBattlefieldCards" />
      <Hand :cards="bottomHandCards" />
      <Deck position="bottom" />
    </div>
    <div class="pass-turn-button">
      <button @click="passTheTurn">Pass Turn</button>
    </div>
  </div>
</template>
  
  <script setup>
  import { useGameState } from '../useGameState.js'
  import PlayerInfo from './PlayerInfo.vue'
  import Battlefield from './Battlefield.vue'
  import Hand from './Hand.vue'
  import Deck from './Deck.vue'
  
  const { topBattlefieldCards, topHandCards, bottomBattlefieldCards, bottomHandCards, passTheTurn } = useGameState()
  </script>

  <style scoped>
.game-container {
    display: flex;
    flex-direction: column;
    flex: 1;
    background-color: #1e1e1e;
    color: #fff;
    box-sizing: border-box;
    padding: 20px;
    height: 98svh;
  }
  
  .playmat-container {
    display: flex;
    flex-direction: column;
    flex: 1;
    justify-content: space-around;
  }
  
  .playmat {
    display: flex;
    flex-direction: column;
    flex: 1;
    justify-content: space-between;
    align-items: center;
    border: 2px solid #fff;
    border-radius: 10px;
    padding: 15px;
    box-sizing: border-box;
    position: relative;
  }
  
  .top-playmat {
    margin-bottom: 10px;
  }
  
  .bottom-playmat {
    margin-top: 10px;
  }
  
  .pass-turn-button {
    position: absolute;
    bottom: 20px;
    right: 20px;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 10px;
    border-radius: 5px;
  }
  
  .pass-turn-button button {
    background-color: #4CAF50;
    color: white;
    border: none;
    padding: 10px 20px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
    cursor: pointer;
  }</style>
  