export async function authFetch(url, options = {}) {
    const token = localStorage.getItem('accessToken')

    const headers = {
        'Content-Type': 'application/json',
        ...options.headers,
        ...(token ? { Authorization: `Bearer ${token}` } : {})
    }

    const res = await fetch(url, {
        ...options,
        headers,
        credentials: 'include'
    })

    if (res.status === 401) {
        try {
            await refreshToken()
            return authFetch(url, options) // повтор
        } catch (err) {
            localStorage.removeItem('accessToken')
            // Редирект только если мы не на странице логина
            if (!window.location.pathname.startsWith('/login')) {
                window.location.href = '/login'
            }
            throw new Error('Unauthorized')
        }
    }

    return res
}

export async function refreshToken() {
    const res = await fetch('https://task-master.cloudpub.ru/auth/refresh', {
        method: 'POST',
        credentials: 'include',
    })

    if (!res.ok) throw new Error('Refresh failed')

    const data = await res.json()
    localStorage.setItem('accessToken', data.token)
}
