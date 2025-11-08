import { type RouteObject as ReactObjectReactRouter } from 'react-router-dom'

import { type RoleValues } from '@/features/common/types/user'

export type RouteObject = ReactObjectReactRouter & {
  roles?: RoleValues[] | '*'
}
