import axios from 'axios'
import { API_BASE_URL } from '../consts/app'
import { useBoundStore } from '../../../store'
console.log('API_BASE_URL:', API_BASE_URL)

export const shopApi = axios.create({

    baseURL: API_BASE_URL
})

shopApi.interceptors.response.use(
    (res) => res.data,
    (error) => Promise.reject(error)
)

shopApi.interceptors.request.use((config) => {
    const { token } = useBoundStore.getState()
    if (token) {
        config.headers.Authorization = `Bearer ${token}`
    }

    return config
})