import { createContext } from 'react'
import type { CartItem } from '../api/cart'
import type { Product } from '../api/products'
import type { ProductTypeValue } from '../api/types'

export interface CartRow {
  cart: CartItem
  // Fetched live via getProductById(cart.productType, cart.productId) once the row's productType
  // is known - null while loading, or permanently for a pre-existing row with no productType (see
  // api/cart.ts's CartItem.productType comment).
  product: Product | null
}

export interface AddToCartInput {
  productType: ProductTypeValue
  subCategoryId: number
  productId: number
  price: number
}

export interface CartContextValue {
  items: CartRow[]
  count: number
  isLoading: boolean
  error: string | null
  addToCart: (input: AddToCartInput, quantity: number) => Promise<void>
  removeFromCart: (cartId: number) => Promise<void>
  reload: () => void
}

export const CartContext = createContext<CartContextValue | null>(null)
