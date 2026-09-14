import { api, buildQuery } from './client'
import type { ProductTypeValue } from './types'

export const OrderStatus = { Delivered: 0, Canceled: 1, NotPaid: 2 } as const
export type OrderStatusValue = (typeof OrderStatus)[keyof typeof OrderStatus]

export interface Order {
  id: number
  userId: string
  productType: ProductTypeValue
  productId: number
  subCategoryId: number
  model: string
  quantity: number
  price: number
  orderDate: string
  orderStatus: OrderStatusValue
  deliveryAddressId: number
}

export interface AddOrderPayload {
  productType: ProductTypeValue
  productId: number
  subCategoryId: number
  model: string
  quantity: number
  deliveryAddressId: number
}

// Price is deliberately not part of AddOrderPayload - the backend always re-derives it from the
// product's own table (GetRealPrice, see ../../Eraj/CLAUDE.md commit 06aee14) and ignores
// whatever the client sends, so there's no point pretending the form controls it. userId is
// similarly always overwritten server-side with the caller's own id.
export const addOrder = (payload: AddOrderPayload) =>
  api.post<null>('/api/Order/post/order', { userId: 'ignored', price: 0, orderDate: new Date().toISOString(), orderStatus: OrderStatus.NotPaid, ...payload })

export const getOrders = () => api.get<Order[]>('/api/Order/get/orders?pageSize=200')

export const getOrderById = (orderId: number) => api.get<Order>(`/api/Order/get/ordersById${buildQuery({ orderId })}`)

export const deleteOrder = (orderId: number) => api.del<null>(`/api/Order/delete/order${buildQuery({ orderId })}`)
