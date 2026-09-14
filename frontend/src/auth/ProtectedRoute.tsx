import type { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuth } from './useAuth'

interface ProtectedRouteProps {
  children: ReactNode
  /** If given, at least one of these roles is required (in addition to just being logged in). */
  roles?: string[]
}

export function ProtectedRoute({ children, roles }: ProtectedRouteProps) {
  const { isAuthenticated, hasRole } = useAuth()
  const location = useLocation()

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />
  }

  if (roles && !hasRole(...roles)) {
    return <Navigate to="/" replace />
  }

  return <>{children}</>
}
