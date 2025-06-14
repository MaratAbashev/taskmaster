import { createRouter, createWebHistory } from 'vue-router'
import Login from './views/Login.vue'
import Dashboard from './views/Dashboard.vue'
import TaskBoard from './views/TaskBoard.vue'
import { authFetch } from './utils/authFetch'

const routes = [
    { 
        path: '/login', 
        component: Login,
        meta: { requiresAuth: false }
    },
    { 
        path: '/dashboards', 
        component: Dashboard,
        meta: { requiresAuth: true }
    },
    { 
        path: '/board/:id', 
        component: TaskBoard,
        meta: { requiresAuth: true },
        beforeEnter: async (to, from, next) => {
            try {
                const res = await authFetch(`/taskboards/${to.params.id}`)
                if (!res.ok) {
                    next('/dashboards')
                    return
                }
                next()
            } catch (error) {
                console.error('Error checking board access:', error)
                next('/dashboards')
            }
        }
    },
    { 
        path: '/', 
        redirect: '/dashboards',
        meta: { requiresAuth: true }
    },
    { 
        path: '/:pathMatch(.*)*', 
        redirect: '/login',
        meta: { requiresAuth: false }
    }
]

const router = createRouter({
    history: createWebHistory('/'),
    routes
})

router.beforeEach(async (to, from, next) => {
    const requiresAuth = to.matched.some(record => record.meta.requiresAuth)
    const token = localStorage.getItem('accessToken')

    if (requiresAuth && !token) {
        next('/login')
        return
    }

    if (to.path === '/login' && token) {
        next('/dashboards')
        return
    }

    next()
})

export default router