import { Navigate } from 'react-router-dom'
import { lazyImport } from '../../features/common/utils/lazyImport'
import type { RouteObject } from '../common/types'

const { Login } = lazyImport(() => import('../../features/auth/pages/Login'), 'Login')

export const authRoutes: RouteObject[] = [
  {
    path: '/',
    element: <Login />
  },

  {
    path: '*',
    element: <Navigate replace to='/' />
  }
]

export default authRoutes
