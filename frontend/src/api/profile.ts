import { api } from './client'

export const Gender = { Male: 0, Female: 1 } as const
export type GenderValue = (typeof Gender)[keyof typeof Gender]

export interface Profile {
  id: string
  name: string
  surname: string
  email: string | null
  telephoneNumber: string
  dob: string
  image: string | null
  gender: GenderValue
  addressId: number | null
  cardNumber: string | null
}

// No "get my own profile" endpoint - GetProfile with no name filter already scopes to the caller
// for non-privileged users and returns them as a one-item list (see Eraj's ProfileService).
export const getMyProfile = () => api.get<Profile[]>('/api/Profile/get/profile')

// Same endpoint, but a privileged caller (Admin/SuperAdmin) gets every profile back unfiltered -
// used by the role-assignment picker (/admin/roles) to find a user by name/phone and their id.
export const getAllProfiles = () => api.get<Profile[]>('/api/Profile/get/profile')

export interface ProfilePayload {
  name: string
  surname: string
  email?: string | null
  telephoneNumber: string
  dob: string
  gender: GenderValue
  addressId?: number | null
  cardNumber?: string | null
}

// UpdateProfileDTO requires the id explicitly (Add doesn't - registration already created the row,
// see ../../Eraj/CLAUDE.md's "POST /api/Profile/post/profile" note: it 409s if one already exists,
// so in practice every logged-in user only ever needs Update here).
export const updateProfile = (id: string, payload: ProfilePayload) =>
  api.put<Profile>('/api/Profile/put/profile', { id, ...payload })
