<template>
  <div v-if="gameStore.showBlockerModal" class="modal">
    <div class="modal-content">
      <h3>Assign Blockers</h3>
      <div v-for="(creature, index) in availableBlockers" :key="index" class="blocker-option">
        <label>
          <input type="checkbox" v-model="selectedBlockers" :value="creature.id" @change="onBlockerSelected" />
          {{ creature.name }}
        </label>
      </div>
      <button @click="confirmBlockers" class="confirm-button">Confirm</button>
      <button @click="cancelBlockers" class="cancel-button">Cancel</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue';
import { useGameStore } from '../stores/gameStore'

const gameStore = useGameStore();

const selectedBlockers = ref([]);

const availableBlockers = computed(() => {
  const result = gameStore.defendingCreatures.filter(creature => !creature.isTapped);
  console.log('availableBlockers:', result);
  return result;
});

// Watch for changes in defendingCreatures to re-compute availableBlockers
watch(
  () => gameStore.defendingCreatures,
  (newDefenders) => {
    console.log('Defending creatures changed:', newDefenders);
  }
);

watch(
  () => gameStore.showBlockerModal,
  async (newValue) => {
    if (newValue) {
      console.log('Blocker modal is now visible');
      await gameStore.fetchGameState(); // Ensure fresh game state when modal becomes visible
      console.log('Game state fetched for blocker modal:', gameStore.defendingCreatures);
    }
  }
);

function confirmBlockers() {
  const blockerAssignments = getSelectedBlockers();
  gameStore.assignBlockers(gameStore.players[0].id, blockerAssignments);
  gameStore.showBlockerModal = false;
}

function cancelBlockers() {
  gameStore.showBlockerModal = false;
}

function onBlockerSelected() {
  console.log('Selected blockers:', selectedBlockers.value);
}

function getSelectedBlockers() {
  const assignments = {};
  gameStore.attackingCreatures.forEach(attacker => {
    const blocker = gameStore.defendingCreatures.find(creature => selectedBlockers.value.includes(creature.id));
    if (blocker) {
      assignments[attacker.id] = blocker.id;
    }
  });
  return assignments;
}
</script>
  
<style scoped>
.modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 9999;
}

.modal-content {
  background-color: #1e1e1e;
  padding: 20px;
  border-radius: 8px;
  max-width: 400px;
  width: 100%;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
  color: #fff;
}

h3 {
  margin-top: 0;
  margin-bottom: 20px;
  font-size: 24px;
  text-align: center;
}

.confirm-button,
.cancel-button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 10px 20px;
  text-align: center;
  text-decoration: none;
  display: inline-block;
  font-size: 16px;
  margin-right: 10px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}

.cancel-button {
  background-color: #f44336;
}

.confirm-button:hover {
  background-color: #45a049;
}

.cancel-button:hover {
  background-color: #d32f2f;
}
</style>
