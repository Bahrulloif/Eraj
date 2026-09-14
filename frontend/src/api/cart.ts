import { api } from './client'
import type { ProductTypeValue } from './types'

export interface CartItem {
  id: number
  applicationUserId: string
  productId: number
  subCategoryId: number
  // Nullable: rows added before Eraj commit 75a0464 (Cart.ProductType didn't exist) stay NULL
  // forever - the migration backfilling it couldn't recover data that was never stored.
  productType: ProductTypeValue | null
  dateOfPurchase: string
  amount: number | null
  quantity: number
}

export interface AddCartPayload {
  productId: number
  subCategoryId: number
  productType: ProductTypeValue
  dateOfPurchase: string
  amount: number
  quantity: number
}

// AddCartDTO/CartDTO carry an ApplicationUserId field, but the backend always overwrites it with
// the caller's own id (see CartService.AddCart) - not worth asking the form to fill in.
export const addCart = (payload: AddCartPayload) => api.post<null>('/api/Cart/post/cart', { applicationUserId: 'ignored', ...payload })

export const getCart = () => api.get<CartItem[]>('/api/Cart/get/cart?pageSize=200')

export const deleteCart = (cartId: number) => api.del<null>(`/api/Cart/delete/cart?cartId=${cartId}`)
