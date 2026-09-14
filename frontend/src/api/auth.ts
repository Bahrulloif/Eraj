import { api } from './client'

export interface LoginPayload {
  userName: string
  password: string
}

export interface RegisterPayload {
  name: string
  surname: string
  userName: string
  password: string
  telephoneNumber: string
}

// Both return the raw JWT string as `data` on success.
export const login = (payload: LoginPayload) => api.post<string>('/api/AccountService/post/login', payload, { auth: false })

export const register = (payload: RegisterPayload) =>
  api.post<null>('/api/AccountService/post/register', payload, { auth: false })
