import { Link } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { deleteOrder, getOrders, OrderStatus, type Order } from '../api/order'
import { slugForProductType } from '../api/types'
import { ApiError } from '../api/client'
import { Money } from '../components/Money'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

const STATUS_LABELS: Record<Order['orderStatus'], string> = {
  [OrderStatus.Delivered]: 'Доставлен',
  [OrderStatus.Canceled]: 'Отменён',
  [OrderStatus.NotPaid]: 'Не оплачен',
}

export function OrdersPage() {
  const { data: orders, error, isLoading, reload } = useAsync(() => getOrders().then((r) => r.data), [])

  async function handleDelete(order: Order) {
    if (!confirm(`Удалить заказ «${order.model}»?`)) return
    try {
      await deleteOrder(order.id)
      reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить заказ')
    }
  }

  return (
    <div>
      <h1>Мои заказы</h1>
      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reload} />}
      {orders && orders.length === 0 && (
        <p>
          Заказов пока нет. <Link to="/">Перейти к каталогу</Link>.
        </p>
      )}
      {orders && orders.length > 0 && (
        <ul className="item-list">
          {orders.map((order) => (
            <li key={order.id} className="item-list__item">
              <Link to={`/products/${slugForProductType(order.productType)}/${order.productId}`}>{order.model}</Link>
              <span className="item-list__meta">
                {order.quantity} шт. × <Money amount={order.price} /> · {STATUS_LABELS[order.orderStatus]} ·{' '}
                {new Date(order.orderDate).toLocaleDateString('ru-RU')}
              </span>
              <span className="item-list__actions">
                <Link to={`/orders/${order.id}`} className="btn btn-secondary">
                  Подробнее
                </Link>
                <button type="button" className="btn btn-danger" onClick={() => handleDelete(order)}>
                  Удалить
                </button>
              </span>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
