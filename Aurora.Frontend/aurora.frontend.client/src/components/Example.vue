<template>
    <div class="game-container">
      <div class="playmat top-playmat">
        <div class="player-info top-player-info">
          <h2>Player 2</h2>
          <p>Life: 20</p>
        </div>
        <div class="battlefield">
          <div class="card creature-card">
            <span class="card-name">Creature 1</span>
            <div class="card-buttons">
              <button class="attack-button">Attack</button>
              <button class="block-button">Block</button>
            </div>
          </div>
          <div class="card creature-card">
            <span class="card-name">Creature 2</span>
            <div class="card-buttons">
              <button class="attack-button">Attack</button>
              <button class="block-button">Block</button>
            </div>
          </div>
          <div class="card land-card">Land 1</div>
        </div>
        <div class="hand">
          <div class="card">Card 1</div>
          <div class="card">Card 2</div>
          <div class="card">Card 3</div>
          <div class="card">Card 4</div>
          <div class="card">Card 5</div>
        </div>
        <div class="deck bottom-right-deck">Deck</div>
      </div>
      <div class="playmat bottom-playmat">
        <div class="player-info bottom-player-info">
          <h2>Player 1</h2>
          <p>Life: 20</p>
        </div>
        <div class="battlefield">
          <div class="card creature-card">
            <span class="card-name">Creature 3</span>
            <div class="card-buttons">
              <button class="attack-button">Attack</button>
              <button class="block-button">Block</button>
            </div>
          </div>
          <div class="card land-card">Land 2</div>
          <div class="card land-card">Land 3</div>
        </div>
        <div class="hand">
          <div class="card">Card 6</div>
          <div class="card">Card 7</div>
          <div class="card">Card 8</div>
          <div class="card">Card 9</div>
        </div>
        <div class="deck top-right-deck">Deck</div>
        <div class="pass-turn-button">
      <button >Pass Turn</button>
    </div>
      </div>
    </div>
  </template>

  
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
}
.top-playmat {
  margin-bottom: 10px;
}

.bottom-playmat {
  margin-top: 10px;
}
  .player-info {
    position: absolute;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 10px;
    border-radius: 5px;
  }
  
  .top-player-info {
    top: 20px;
    right: 20px;
  }
  
  .bottom-player-info {
    bottom: 20px;
    left: 20px;
  }
  
 .battlefield {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
  width: 100%;
  min-height: 150px;
  background-color: #2e2e2e;
  border-radius: 5px;
  padding: 10px;
  box-sizing: border-box;
}

.hand {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-wrap: wrap;
  gap: 10px;
  width: 100%;
  min-height: 100px;
  background-color: #2e2e2e;
  border-radius: 5px;
  padding: 10px;
  box-sizing: border-box;
}

.card {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 80px;
  height: 120px;
  background-color: #f0f0f0;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  color: #000;
  font-weight: bold;
  text-align: center;
  padding: 5px;
  position: relative;
    overflow: hidden;
    transition: transform 0.3s;
}

.card:hover {
    transform: translateY(-5px);
  }

  .card-name {
    display: block;
    font-weight: bold;
  }

  .card-buttons {
    display: none;
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.7);
    padding: 5px;
    text-align: center;
  }
  .card:hover .card-buttons {
    display: flex;
    justify-content: center;
    gap: 10px;
  }

  .attack-button,
  .block-button {
    background-color: #4CAF50;
    color: white;
    border: none;
    padding: 5px 10px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 14px;
    cursor: pointer;
  }

  .block-button {
    background-color: #2196F3;
  }

.creature-card {
  background-color: #8bc34a;
}

.land-card {
  background-color: #795548;
}

.deck {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 80px;
    height: 120px;
    background-color: #f0f0f0;
    border-radius: 5px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    color: #000;
    font-weight: bold;
    text-align: center;
    padding: 5px;
  }

  .bottom-right-deck {
    position: absolute;
    top: 31.75em;
    left: 2em;
  }

  .top-right-deck {
    position: absolute;
    top: 19.25em;
    left: 2em;
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
  }
  </style>