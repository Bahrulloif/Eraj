import { use } from 'react'
import { CartContext, type CartContextValue } from './context'

export function useCart(): CartContextValue {
  const ctx = use(CartContext)
  if (!ctx) throw new Error('useCart must be used within CartProvider')
  return ctx
}
