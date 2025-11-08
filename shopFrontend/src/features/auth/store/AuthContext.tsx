import { createContext, useContext, useEffect, useRef, useState } from 'react'
import { useIdleTimer } from 'react-idle-timer'
import { useShallow } from 'zustand/shallow'

import { login as loginWithApi, refreshToken as refreshTokenWithAPi } from '../../../features/auth/services'
import { useBoundStore } from '../../../store'
import type { AuthValues } from '../../common/types'
import { session } from '../../common/consts'
import type { AuthSlice, AuthState } from './authStore'
import { PageLoader } from '../../common/components/ui/PageLoader'

type ActionCallback = () => void
interface AuthContextValue {
  login: (params: AuthValues, cb?: ActionCallback) => Promise<void>
  logout: (cb?: ActionCallback) => void
  user: AuthState
  isLoggingIn: boolean
  isCheckingSession: boolean
  error: string
}

const AuthContext = createContext({} as AuthContextValue)

interface Props {
  children: React.ReactNode
}
export const AuthProvider: React.FC<Props> = ({ children }) => {
  const contextValue = useAuthProvider()

  if (contextValue.isCheckingSession) return <PageLoader />

  return <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
}

const useAuthProvider = (): AuthContextValue => {
  const authStore: AuthSlice = useBoundStore(
    useShallow(({
      isAuthenticated,
      setLogin,
      setLogout,
      setRefresh,
      token,
      refreshToken,
      userId,
      loggedInDate,
      firstName,
      lastName
    }) => ({
      isAuthenticated,
      token,
      refreshToken,
      userId,
      loggedInDate,
      setLogin,
      setLogout,
      setRefresh,
      firstName,
      lastName
    }))
  )
  const [isCheckingSession, setIsCheckingSession] = useState(true)
  const [isLoggingIn, setIsLogginIn] = useState(false)
  const [error, setError] = useState('')
  const refreshTokenIntervalIdRef = useRef<ReturnType<typeof setInterval> | undefined>(undefined)
  const mounted = useRef(false)

  const login: AuthContextValue['login'] = async (values, callback) => {
    setIsLogginIn(true)
    setError('')

    try {
      const data = await loginWithApi(values)
      authStore.setLogin(data)
      subscribeToRefreshToken()
      callback?.()
    } catch (error) {
      console.error(error)
      setError('Usuario o contraseña incorrecta')
    } finally {
      setIsLogginIn(false)
    }
  }

  const logout: AuthContextValue['logout'] = (callback) => {
    authStore.setLogout()
    unsubscribeToRefreshToken()
    callback?.()
  }

  const refreshToken = async (): Promise<void> => {
    const { token, refreshToken } = useBoundStore.getState()
    if (!token || !refreshToken) {
      logout()
      return
    }

    try {
      const authUser = await refreshTokenWithAPi({
        token,
        refreshToken
      })

      authStore.setRefresh(authUser)
    } catch (error) {
      console.error(error)
      logout()
    }
  }

  const checkIfSessionIsAlive = async (): Promise<void> => {
    if (!authStore.token || !authStore.refreshToken) {
      setIsCheckingSession(false)
      logout()
      return
    }

    setIsCheckingSession(true)
    setIsLogginIn(true)
    try {
      const authUser = await refreshTokenWithAPi({
        token: authStore.token,
        refreshToken: authStore.refreshToken
      })

      authStore.setRefresh(authUser)
      subscribeToRefreshToken()
    } catch (error) {
      console.error(error)
      logout()
    } finally {
      setIsLogginIn(false)
      setIsCheckingSession(false)
    }
  }

  const unsubscribeToRefreshToken = (): void => {
    clearInterval(refreshTokenIntervalIdRef.current)
    refreshTokenIntervalIdRef.current = undefined
  }

  const subscribeToRefreshToken = (): void => {
    unsubscribeToRefreshToken()

    refreshTokenIntervalIdRef.current = setInterval(refreshToken, session.autoRefresh)
  }

  useIdleTimer({
    onActive: () => checkIfSessionIsAlive(),
    onIdle: () => unsubscribeToRefreshToken(),
    timeout: session.idleTime
  })

  useEffect(() => {
    if (mounted.current) return

    checkIfSessionIsAlive()
    mounted.current = true
    return () => unsubscribeToRefreshToken()
  }, [])

  return {
    user: {
      isAuthenticated: authStore.isAuthenticated,
      token: authStore.token,
      refreshToken: authStore.refreshToken,
      userId: authStore.userId,
      loggedInDate: authStore.loggedInDate,
      firstName: authStore.firstName,
      lastName: authStore.lastName
    },
    isCheckingSession,
    isLoggingIn,
    login,
    logout,
    error
  }
}

export const useAuth = (): AuthContextValue => useContext(AuthContext)





