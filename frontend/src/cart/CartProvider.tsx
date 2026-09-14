import { useCallback, useMemo, useState } from 'react'
import type { ReactNode } from 'react'
import { useAuth } from '../auth/useAuth'
import { useAsync } from '../hooks/useAsync'
import { addCart, deleteCart, getCart, type CartItem } from '../api/cart'
import { getProductById } from '../api/products'
import { CartContext, type AddToCartInput, type CartRow } from './context'

const EMPTY_ITEMS: CartRow[] = []

// Attaches each cart row's real product (title/image/current price) by fetching it live via
// (productType, productId) - both are genuine fields on Cart now (Eraj commit 75a0464), so this
// no longer needs the localStorage sidecar the pre-fix version relied on (see git history if
// that's ever needed for reference). A row with no productType (pre-existing, from before that
// column existed) just gets product: null - same "unknown item" display as before, but now
// because the backend genuinely doesn't know, not because this browser forgot.
async function attachProducts(cartItems: CartItem[]): Promise<CartRow[]> {
  return Promise.all(
    cartItems.map(async (cart): Promise<CartRow> => {
      if (cart.productType == null) return { cart, product: null }
      try {
        const res = await getProductById(cart.productType, cart.productId)
        return { cart, product: res.data }
      } catch {
        return { cart, product: null }
      }
    }),
  )
}

export function CartProvider({ children }: { children: ReactNode }) {
  const { isAuthenticated } = useAuth()

  const {
    data: items,
    isLoading,
    error,
    reload,
  } = useAsync(() => (isAuthenticated ? getCart().then((res) => attachProducts(res.data)) : Promise.resolve(EMPTY_ITEMS)), [isAuthenticated])

  const [isMutating, setIsMutating] = useState(false)

  const addToCart = useCallback(
    async (input: AddToCartInput, quantity: number) => {
      setIsMutating(true)
      try {
        await addCart({
          productId: input.productId,
          subCategoryId: input.subCategoryId,
          productType: input.productType,
          dateOfPurchase: new Date().toISOString(),
          amount: input.price,
          quantity,
        })
        reload()
      } finally {
        setIsMutating(false)
      }
    },
    [reload],
  )

  const removeFromCart = useCallback(
    async (cartId: number) => {
      setIsMutating(true)
      try {
        await deleteCart(cartId)
        reload()
      } finally {
        setIsMutating(false)
      }
    },
    [reload],
  )

  const value = useMemo(
    () => ({
      items: items ?? EMPTY_ITEMS,
      count: (items ?? EMPTY_ITEMS).length,
      isLoading: isLoading || isMutating,
      error,
      addToCart,
      removeFromCart,
      reload,
    }),
    [items, isLoading, isMutating, error, addToCart, removeFromCart, reload],
  )

  return <CartContext value={value}>{children}</CartContext>
}
