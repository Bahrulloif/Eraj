import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useCart } from '../cart/useCart'
import { useAsync } from '../hooks/useAsync'
import { getDeliveryAddresses } from '../api/deliveryAddress'
import { getAddresses } from '../api/address'
import { addOrder } from '../api/order'
import { imageUrl } from '../api/products'
import { slugForProductType } from '../api/types'
import { ApiError } from '../api/client'
import { Money } from '../components/Money'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import './CartPage.css'

export function CartPage() {
  const { items, isLoading, error, removeFromCart, reload } = useCart()
  const navigate = useNavigate()

  const deliveryAddresses = useAsync(() => getDeliveryAddresses({ pageSize: 100 }).then((r) => r.data), [])
  const addresses = useAsync(() => getAddresses({ pageSize: 100 }).then((r) => r.data), [])
  const addressById = new Map((addresses.data ?? []).map((a) => [a.id, a]))

  const [deliveryAddressId, setDeliveryAddressId] = useState('')
  const [checkoutId, setCheckoutId] = useState<number | null>(null)
  const [checkoutError, setCheckoutError] = useState<string | null>(null)

  async function handleCheckout(cartId: number) {
    const row = items.find((r) => r.cart.id === cartId)
    if (!row?.product || row.cart.productType == null || !deliveryAddressId) return
    setCheckoutId(cartId)
    setCheckoutError(null)
    try {
      await addOrder({
        productType: row.cart.productType,
        productId: row.cart.productId,
        subCategoryId: row.cart.subCategoryId,
        model: typeof row.product.model === 'string' ? row.product.model : `Объявление №${row.product.id}`,
        quantity: row.cart.quantity,
        deliveryAddressId: Number(deliveryAddressId),
      })
      await removeFromCart(cartId)
      navigate('/orders')
    } catch (err) {
      setCheckoutError(err instanceof ApiError ? err.message : 'Не удалось оформить заказ')
    } finally {
      setCheckoutId(null)
    }
  }

  return (
    <div>
      <h1>Корзина</h1>

      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reload} />}
      {!isLoading && items.length === 0 && (
        <p>
          Корзина пуста. <Link to="/">Перейти к каталогу</Link>.
        </p>
      )}

      {items.length > 0 && (
        <>
          <div className="inline-form">
            <div className="form-field">
              <label htmlFor="deliveryAddressId">Адрес доставки для оформления</label>
              <select id="deliveryAddressId" value={deliveryAddressId} onChange={(e) => setDeliveryAddressId(e.target.value)}>
                <option value="">Выберите адрес…</option>
                {(deliveryAddresses.data ?? []).map((d) => {
                  const address = addressById.get(d.addressId)
                  return (
                    <option key={d.id} value={d.id}>
                      {address ? `${address.country}, ${address.city}, ${address.street}` : `Адрес №${d.addressId}`}
                    </option>
                  )
                })}
              </select>
            </div>
            {deliveryAddresses.data && deliveryAddresses.data.length === 0 && (
              <p>
                Нет адресов доставки. <Link to="/delivery-addresses">Добавить</Link>.
              </p>
            )}
          </div>

          {checkoutError && <div className="form-error">{checkoutError}</div>}

          <ul className="cart-list">
            {items.map(({ cart, product }) => {
              const cover = product?.images[0]
              const title = product ? (typeof product.model === 'string' ? product.model : `Объявление №${product.id}`) : null
              return (
                <li key={cart.id} className="cart-item">
                  <div className="cart-item__image">
                    {cover ? <img src={imageUrl(cover.imageName)} alt="" /> : <div className="cart-item__placeholder" />}
                  </div>
                  <div className="cart-item__info">
                    {product && cart.productType != null ? (
                      <Link to={`/products/${slugForProductType(cart.productType)}/${product.id}`}>{title}</Link>
                    ) : (
                      <span className="cart-item__unknown">
                        Товар №{cart.productId} (данные недоступны — товар удалён или строка добавлена до фикса типов)
                      </span>
                    )}
                    <span className="cart-item__meta">
                      {cart.quantity} шт. {cart.amount !== null && <Money amount={cart.amount} />}
                    </span>
                  </div>
                  <div className="cart-item__actions">
                    <button
                      type="button"
                      className="btn btn-primary"
                      disabled={!product || !deliveryAddressId || checkoutId === cart.id}
                      onClick={() => handleCheckout(cart.id)}
                      title={!product ? 'Нет данных о товаре' : !deliveryAddressId ? 'Выберите адрес доставки' : ''}
                    >
                      {checkoutId === cart.id ? 'Оформляем…' : 'Оформить'}
                    </button>
                    <button type="button" className="btn btn-danger" onClick={() => removeFromCart(cart.id)}>
                      Удалить
                    </button>
                  </div>
                </li>
              )
            })}
          </ul>
        </>
      )}
    </div>
  )
}
