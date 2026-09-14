import { createContext } from 'react'
import type * as authApi from '../api/auth'

export interface AuthUser {
  id: string
  userName: string
  roles: string[]
}

export interface AuthContextValue {
  user: AuthUser | null
  isAuthenticated: boolean
  hasRole: (...roles: string[]) => boolean
  login: (userName: string, password: string) => Promise<void>
  register: (payload: authApi.RegisterPayload) => Promise<void>
  logout: () => void
}

export const AuthContext = createContext<AuthContextValue | null>(null)
