import { useCallback, useEffect, useMemo, useState } from 'react'
import type { ReactNode } from 'react'
import * as authApi from '../api/auth'
import { ApiError } from '../api/client'
import { getToken, setToken as persistToken } from '../api/client'
import { decodeToken, isExpired, type DecodedToken } from './jwt'
import { AuthContext, type AuthContextValue, type AuthUser } from './context'

function toUser(decoded: DecodedToken): AuthUser {
  return { id: decoded.userId, userName: decoded.userName, roles: decoded.roles }
}

function readStoredUser(): AuthUser | null {
  const token = getToken()
  if (!token) return null
  const decoded = decodeToken(token)
  if (!decoded || isExpired(decoded)) {
    persistToken(null)
    return null
  }
  return toUser(decoded)
}

export function AuthProvider({ children }: { children: ReactNode }) {
  // Read synchronously on first render (localStorage, not a network call) - no loading gap for
  // ProtectedRoute to worry about, unlike an effect-based read that would run after paint.
  const [user, setUser] = useState<AuthUser | null>(() => readStoredUser())

  useEffect(() => {
    // Fired by api/client.ts on any 401 response - the token is stale/invalid from the server's
    // point of view even if it still looks unexpired locally, so clear it the same way.
    const onUnauthorized = () => {
      persistToken(null)
      setUser(null)
    }
    window.addEventListener('maxshop:unauthorized', onUnauthorized)
    return () => window.removeEventListener('maxshop:unauthorized', onUnauthorized)
  }, [])

  const login = useCallback(async (userName: string, password: string) => {
    const res = await authApi.login({ userName, password })
    const decoded = decodeToken(res.data)
    if (!decoded) throw new ApiError('Сервер вернул некорректный токен', 500)
    persistToken(res.data)
    setUser(toUser(decoded))
  }, [])

  const register = useCallback(async (payload: authApi.RegisterPayload) => {
    await authApi.register(payload)
  }, [])

  const logout = useCallback(() => {
    persistToken(null)
    setUser(null)
  }, [])

  const hasRole = useCallback((...roles: string[]) => !!user && roles.some((r) => user.roles.includes(r)), [user])

  const value = useMemo<AuthContextValue>(
    () => ({ user, isAuthenticated: !!user, hasRole, login, register, logout }),
    [user, hasRole, login, register, logout],
  )

  return <AuthContext value={value}>{children}</AuthContext>
}
