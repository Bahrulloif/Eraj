// Decodes the JWT payload client-side purely for UI gating (show/hide, which nav links, redirect
// guests) - never a security boundary by itself. The backend re-checks role/ownership on every
// request regardless (see ../../Eraj/CLAUDE.md, BaseController.CurrentUserId/IsPrivilegedUser),
// so a tampered/expired token here only ever produces a wrong *display*, the API still rejects it.
export interface DecodedToken {
  userId: string
  userName: string
  roles: string[]
  expiresAt: number | null
}

const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'

export function decodeToken(token: string): DecodedToken | null {
  const parts = token.split('.')
  if (parts.length !== 3) return null

  try {
    const payload = JSON.parse(base64UrlDecode(parts[1])) as Record<string, unknown>
    const roleClaim = payload[ROLE_CLAIM]
    const roles = Array.isArray(roleClaim) ? roleClaim.map(String) : roleClaim ? [String(roleClaim)] : []

    return {
      userId: String(payload.sid ?? ''),
      userName: String(payload.name ?? ''),
      roles,
      expiresAt: typeof payload.exp === 'number' ? payload.exp * 1000 : null,
    }
  } catch {
    return null
  }
}

export function isExpired(decoded: DecodedToken): boolean {
  return decoded.expiresAt !== null && decoded.expiresAt <= Date.now()
}

function base64UrlDecode(segment: string): string {
  const base64 = segment.replace(/-/g, '+').replace(/_/g, '/').padEnd(segment.length + ((4 - (segment.length % 4)) % 4), '=')
  return decodeURIComponent(
    atob(base64)
      .split('')
      .map((c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
      .join(''),
  )
}
