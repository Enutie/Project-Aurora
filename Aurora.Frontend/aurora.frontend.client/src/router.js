import { createRouter, createWebHistory } from 'vue-router'
import GameView from './views/GameView.vue'
import CreateGame from './backup/CreateGame.vue'
import Example from './components/Example.vue'

const routes = [
    // {
    //   path: '/',
    //   name: 'CreateGame',
    //   component: CreateGame,
    // },
    {
      path: '/game/:id',
      name: 'Game',
      component: GameView,
      props: true,
    },
    {
      path: '/example',
      name: 'Example',
      component: Example,
    }
  ]
  
  const router = createRouter({
    history: createWebHistory(),
    routes,
  })
  
  export default router;