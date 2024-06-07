<template>
    <div class="game-container">
      <div class="playmat top-playmat">
        <div class="player-info top-player-info">
          <h2>Player 2</h2>
          <p>Life: 20</p>
        </div>
        <div class="battlefield">
          <div class="card creature-card">Creature 1</div>
          <div class="card creature-card">Creature 2</div>
          <div class="card land-card">Land 1</div>
        </div>
        <div class="hand">
          <div class="card">Card 1</div>
          <div class="card">Card 2</div>
          <div class="card">Card 3</div>
          <div class="card">Card 4</div>
          <div class="card">Card 5</div>
        </div>
      </div>
      <div class="playmat bottom-playmat">
        <div class="player-info bottom-player-info">
          <h2>Player 1</h2>
          <p>Life: 20</p>
        </div>
        <div class="battlefield">
          <div class="card creature-card">Creature 3</div>
          <div class="card land-card">Land 2</div>
          <div class="card land-card">Land 3</div>
        </div>
        <div class="hand">
          <div class="card">Card 6</div>
          <div class="card">Card 7</div>
          <div class="card">Card 8</div>
          <div class="card">Card 9</div>
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
}

.creature-card {
  background-color: #8bc34a;
}

.land-card {
  background-color: #795548;
}
  </style>