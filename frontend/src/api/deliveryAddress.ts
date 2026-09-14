import { api, buildQuery } from './client'
import type { PagedApiResponse, PageParams } from './types'

export interface DeliveryAddress {
  id: number
  applicationUserId: string | null
  addressId: number
}

export const getDeliveryAddresses = (params: PageParams = {}) =>
  api.get<DeliveryAddress[]>(`/api/DeliveryAddress/get/deliveryAddress${buildQuery(params)}`) as Promise<
    PagedApiResponse<DeliveryAddress[]>
  >

export const addDeliveryAddress = (addressId: number) =>
  api.post<DeliveryAddress>('/api/DeliveryAddress/post/deliveryAddress', { addressId })

export const deleteDeliveryAddress = (deliveryAddressId: number) =>
  api.del<null>(`/api/DeliveryAddress/delete/deliveryAddress${buildQuery({ deliveryAddressId })}`)
