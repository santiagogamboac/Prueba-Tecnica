export const session = {
    idleTime: 20 * 60 * 1000,
    autoRefresh: 20 * 60 * 1000
} as const

export const SERVER_BASE_URL = import.meta.env.VITE_SERVER_BASE_URL as string
export const API_BASE_URL = `${SERVER_BASE_URL}/api`
export const PUBLIC_URL = import.meta.env.VITE_PUBLIC_URL as string
