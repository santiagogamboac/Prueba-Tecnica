import { type StateCreator } from 'zustand'
import type { AuthUser, Nulleable } from '../../common/types'

export interface AuthState extends Nulleable<AuthUser> {
  isAuthenticated: boolean
  loggedInDate: Date | null
}

export interface AuthActions {
  setLogin: (params: AuthUser) => void
  setRefresh: (params: AuthUser) => void
  setLogout: () => void
}

export type AuthSlice = AuthState & AuthActions

const initialState: AuthState = {
  isAuthenticated: false,
  loggedInDate: null,
  firstName: null,
  lastName: null,
  token: null,
  refreshToken: null,
  userId: null
}
export const createAuthSlice: StateCreator<AuthSlice> = (set) => ({
  ...initialState,
  setLogin(newState) {
    set({
      isAuthenticated: true,
      loggedInDate: new Date(),
      ...newState
    })
  },
  setRefresh(newState) {
    set((prevState) => ({
      ...newState,
      isAuthenticated: true,
      loggedInDate: prevState.loggedInDate ? new Date(prevState.loggedInDate) : new Date()
    }))
  },
  setLogout() {
    set(initialState)
  }
})
