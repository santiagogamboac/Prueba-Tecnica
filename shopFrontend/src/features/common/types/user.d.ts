import type { z } from 'zod'

import type { AuthUserSchema, UserCreateSchema } from '../shemas/userSchema'



export type AuthUser = z.infer<typeof AuthUserSchema>
export type UserCreate = z.infer<typeof UserCreateSchema>

export interface AuthValues {
  userName: string
  password: string
}
