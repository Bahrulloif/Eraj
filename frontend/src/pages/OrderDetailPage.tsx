import { Link, useParams } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getOrderById, OrderStatus } from '../api/order'
import { slugForProductType } from '../api/types'
import { Money } from '../components/Money'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import { NotFoundPage } from './NotFoundPage'

const STATUS_LABELS = {
  [OrderStatus.Delivered]: 'Доставлен',
  [OrderStatus.Canceled]: 'Отменён',
  [OrderStatus.NotPaid]: 'Не оплачен',
}

export function OrderDetailPage() {
  const { id } = useParams<{ id: string }>()
  const orderId = Number(id)
  const { data: order, error, isLoading, reload } = useAsync(() => getOrderById(orderId).then((r) => r.data), [orderId])

  if (isLoading) return <Spinner />
  if (error) return <ErrorState message={error} onRetry={reload} />
  if (!order) return <NotFoundPage />

  return (
    <div>
      <h1>Заказ №{order.id}</h1>
      <dl className="profile-fields">
        <dt>Товар</dt>
        <dd>
          <Link to={`/products/${slugForProductType(order.productType)}/${order.productId}`}>{order.model}</Link>
        </dd>
        <dt>Количество</dt>
        <dd>{order.quantity}</dd>
        <dt>Цена за штуку</dt>
        <dd>
          <Money amount={order.price} />
        </dd>
        <dt>Итого</dt>
        <dd>
          <Money amount={order.price * order.quantity} />
        </dd>
        <dt>Статус</dt>
        <dd>{STATUS_LABELS[order.orderStatus]}</dd>
        <dt>Дата заказа</dt>
        <dd>{new Date(order.orderDate).toLocaleString('ru-RU')}</dd>
      </dl>
    </div>
  )
}
