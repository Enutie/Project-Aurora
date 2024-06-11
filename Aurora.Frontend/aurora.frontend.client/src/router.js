import { createRouter, createWebHistory } from 'vue-router'
import GameView from './views/GameView.vue'
import GameOverView from '@/views/GameOverView.vue'
import  CreateGame from './components/CreateGame.vue'

const routes = [
    {
      path: '/',
      name: 'CreateGame',
      component: CreateGame,
    },
    {
      path: '/game/:id',
      name: 'Game',
      component: GameView,
      props: true,
    },
    {
      path: '/game-over/:winner',
    name: 'GameOver',
    component: GameOverView,
    props: true,
    }
  ]
  
  const router = createRouter({
    history: createWebHistory(),
    routes,
  })
  
  export default router;