import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/useAuth'
import { useAsync } from '../hooks/useAsync'
import { deleteProduct, getProducts, type Product } from '../api/products'
import { PRODUCT_TYPE_LABELS, ProductType, slugForProductType, type ProductTypeValue } from '../api/types'
import { ApiError } from '../api/client'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import { Money } from '../components/Money'

interface Row {
  productType: ProductTypeValue
  product: Product
}

const ALL_TYPES = Object.values(ProductType) as ProductTypeValue[]

// OwnerId is a real, filterable field on every product list endpoint now (Eraj commit 58b302f) -
// this used to be a localStorage-tracked "listings I added from this browser" best-effort list
// (see git history if that's ever needed again), since there was no way to ask the backend "which
// of these are mine" at all. One request per product type (11 total, all in parallel) - no
// combined "my listings across every type" endpoint exists, so this is the closest available.
async function loadMyListings(ownerId: string): Promise<Row[]> {
  const pages = await Promise.all(ALL_TYPES.map((productType) => getProducts(productType, { ownerId, pageSize: 200 })))
  return pages.flatMap((page, i) => page.items.map((product) => ({ productType: ALL_TYPES[i], product })))
}

export function MyListingsPage() {
  const { user } = useAuth()
  const { data: rows, error, isLoading, reload } = useAsync(() => loadMyListings(user!.id), [user!.id])
  const [deletingKey, setDeletingKey] = useState<string | null>(null)

  async function handleDelete(row: Row) {
    if (!confirm('Удалить это объявление?')) return
    const key = `${row.productType}-${row.product.id}`
    setDeletingKey(key)
    try {
      await deleteProduct(row.productType, row.product.id)
      reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить объявление')
    } finally {
      setDeletingKey(null)
    }
  }

  return (
    <div>
      <div className="page-header">
        <h1>Мои объявления</h1>
        <Link to="/my-listings/new" className="btn btn-primary">
          Добавить объявление
        </Link>
      </div>

      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reload} />}
      {rows && rows.length === 0 && <p>Объявлений пока нет.</p>}
      {rows && rows.length > 0 && (
        <ul className="item-list">
          {rows.map(({ productType, product }) => (
            <li key={`${productType}-${product.id}`} className="item-list__item">
              <Link to={`/products/${slugForProductType(productType)}/${product.id}`}>
                {typeof product.model === 'string' ? product.model : `Объявление №${product.id}`}
              </Link>
              <span className="item-list__meta">
                {PRODUCT_TYPE_LABELS[productType]} · <Money amount={product.price} />
              </span>
              <span className="item-list__actions">
                <Link to={`/my-listings/${slugForProductType(productType)}/${product.id}/edit`} className="btn btn-secondary">
                  Изменить
                </Link>
                <button
                  type="button"
                  className="btn btn-danger"
                  disabled={deletingKey === `${productType}-${product.id}`}
                  onClick={() => handleDelete({ productType, product })}
                >
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
