import type { ApiResponse } from './types'

const TOKEN_KEY = 'maxshop_token'

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string | null): void {
  if (token) localStorage.setItem(TOKEN_KEY, token)
  else localStorage.removeItem(TOKEN_KEY)
}

// Thrown for both network failures and non-2xx responses that did carry a parsed ApiResponse -
// callers that want the server's own error text can read `.errors`/`.statusCode`, everyone else
// can just show `.message`.
export class ApiError extends Error {
  statusCode: number
  errors: string[]

  constructor(message: string, statusCode: number, errors: string[] = []) {
    super(message)
    this.name = 'ApiError'
    this.statusCode = statusCode
    this.errors = errors
  }
}

// Turns { a: 1, b: undefined, c: 'x' } into "?a=1&c=x" - undefined/null entries are dropped so
// callers can pass a filter object as-is without stripping empty fields themselves.
export function buildQuery(params: object | undefined): string {
  if (!params) return ''
  const search = new URLSearchParams()
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue
    search.set(key, String(value))
  }
  const qs = search.toString()
  return qs ? `?${qs}` : ''
}

interface RequestOptions {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE'
  body?: unknown
  /** Send as multipart/form-data instead of JSON (product image uploads etc). */
  form?: FormData
  auth?: boolean
}

// One request/response shape for the whole app: JSON in, ApiResponse<T> out, Bearer token
// attached automatically, 401 broadcast so AuthContext can clear itself without this module
// importing it back (would be circular).
async function request<T>(path: string, options: RequestOptions = {}): Promise<ApiResponse<T>> {
  const { method = 'GET', body, form, auth = true } = options
  const headers: Record<string, string> = {}

  if (auth) {
    const token = getToken()
    if (token) headers.Authorization = `Bearer ${token}`
  }

  let requestBody: BodyInit | undefined
  if (form) {
    requestBody = form // browser sets multipart boundary itself - don't set Content-Type by hand
  } else if (body !== undefined) {
    headers['Content-Type'] = 'application/json'
    requestBody = JSON.stringify(body)
  }

  let response: Response
  try {
    response = await fetch(path, { method, headers, body: requestBody })
  } catch {
    throw new ApiError('Не удалось связаться с сервером. Проверьте соединение.', 0)
  }

  if (response.status === 401) {
    window.dispatchEvent(new Event('maxshop:unauthorized'))
  }

  // ASP.NET's own [ApiController] auto-validation (invalid ModelState) returns a plain
  // ProblemDetails object, not our ApiResponse<T> shape - normalize both into the same ApiError.
  let parsed: unknown
  try {
    parsed = response.status === 204 ? null : await response.json()
  } catch {
    if (!response.ok) {
      throw new ApiError(`Сервер ответил ${response.status}`, response.status)
    }
    parsed = null
  }

  if (!response.ok) {
    const body = parsed as { message?: string; errors?: string[] | Record<string, string[]>; title?: string }
    let errors: string[] = []
    if (Array.isArray(body?.errors)) {
      errors = body.errors
    } else if (body?.errors && typeof body.errors === 'object') {
      // ProblemDetails.errors is { fieldName: ["message", ...] }
      errors = Object.values(body.errors).flat()
    }
    const message = body?.message || errors[0] || body?.title || `Ошибка ${response.status}`
    throw new ApiError(message, response.status, errors)
  }

  return parsed as ApiResponse<T>
}

export const api = {
  get: <T>(path: string, opts?: Omit<RequestOptions, 'method' | 'body' | 'form'>) =>
    request<T>(path, { ...opts, method: 'GET' }),
  post: <T>(path: string, body?: unknown, opts?: Omit<RequestOptions, 'method' | 'body'>) =>
    request<T>(path, { ...opts, method: 'POST', body }),
  put: <T>(path: string, body?: unknown, opts?: Omit<RequestOptions, 'method' | 'body'>) =>
    request<T>(path, { ...opts, method: 'PUT', body }),
  // body is optional - only RoleController's DeleteRoleFromUser actually needs a DELETE with a
  // JSON body (unusual, but that's the real signature: DeleteRoleFromUser(AddRoleToUserDto)).
  del: <T>(path: string, body?: unknown, opts?: Omit<RequestOptions, 'method' | 'body' | 'form'>) =>
    request<T>(path, { ...opts, method: 'DELETE', body }),
  postForm: <T>(path: string, form: FormData, opts?: Omit<RequestOptions, 'method' | 'form'>) =>
    request<T>(path, { ...opts, method: 'POST', form }),
  putForm: <T>(path: string, form: FormData, opts?: Omit<RequestOptions, 'method' | 'form'>) =>
    request<T>(path, { ...opts, method: 'PUT', form }),
}
