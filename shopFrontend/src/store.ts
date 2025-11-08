import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import { createAuthSlice, type AuthSlice } from './features/auth/store/authStore';



type BoundStore = AuthSlice

type StorageState = { token: string; refreshToken: string; loggedInDate: Date } | null

export const useBoundStore = create(
  persist<BoundStore, [], [], StorageState>(
    (...a) => ({
      ...createAuthSlice(...a)
    }),
    {
      name: 'auth-store',
      version: 1,
      partialize: ({ token, refreshToken, loggedInDate }) => {
        const hasValues = token && refreshToken && loggedInDate

        if (hasValues) return { token, refreshToken, loggedInDate }
        return null
      }
    }
  )
)
