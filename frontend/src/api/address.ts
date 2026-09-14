import { api, buildQuery } from './client'
import type { PagedApiResponse, PageParams } from './types'

export interface Address {
  id: number
  country: string
  city: string
  street: string
}

export interface AddressPayload {
  country: string
  city: string
  street: string
}

export const getAddresses = (params: PageParams = {}) =>
  api.get<Address[]>(`/api/Address/get/address${buildQuery(params)}`) as Promise<PagedApiResponse<Address[]>>

export const getAddressById = (addressId: number) => api.get<Address>(`/api/Address/get/addressById${buildQuery({ addressId })}`)

export const addAddress = (payload: AddressPayload) => api.post<Address>('/api/Address/post/address', payload)

export const updateAddress = (id: number, payload: AddressPayload) => api.put<Address>('/api/Address/put/address', { id, ...payload })

export const deleteAddress = (addressId: number) => api.del<null>(`/api/Address/delete/address${buildQuery({ addressId })}`)
