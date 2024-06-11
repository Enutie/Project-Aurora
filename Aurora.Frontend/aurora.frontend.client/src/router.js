import { createRouter, createWebHistory } from 'vue-router'
const GameView = () => import('./views/GameView.vue');
const GameOverView = () => import('@/views/GameOverView.vue');
const CreateGame = () => import('./components/CreateGame.vue');
const NotFoundView = () => import('./views/NotFoundView.vue')

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
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'NotFound',
      component: NotFoundView, // Create this component
    }
    
  ]
  
  const router = createRouter({
    history: createWebHistory(),
    routes,
  })
  
  export default router;