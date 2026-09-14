import { useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getProductById, imageUrl, type Product } from '../api/products'
import { routeForSlug, type ProductRoute } from '../api/products/registry'
import { schemaForProductType, type FieldSchema } from '../api/products/schema'
import type { ProductTypeValue } from '../api/types'
import { useAuth } from '../auth/useAuth'
import { useCart } from '../cart/useCart'
import { ApiError } from '../api/client'
import { Money } from '../components/Money'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import { NotFoundPage } from './NotFoundPage'
import './ProductDetailPage.css'

// Fields every card/detail view already surfaces elsewhere or that are plumbing, not
// user-facing attributes - skip these when rendering "everything else" generically below.
const HIDDEN_FIELDS = new Set(['id', 'subCategoryId', 'images', 'price', 'model', 'ownerId'])

function fallbackLabel(key: string): string {
  return key
    .replace(/([a-z0-9])([A-Z])/g, '$1 $2')
    .replace(/^./, (c) => c.toUpperCase())
}

// The Add/Update form (components/ProductForm.tsx) already carries a real Russian label - and,
// for enum fields, the actual option labels - per field per product type (api/products/schema.ts).
// The detail view used to auto-generate labels from the raw camelCase DTO key instead ("Core",
// "Rom" - untranslated, and enum fields showed a bare number like "0") - only visible once this
// page was actually opened in a browser (see speca.md's Фаза 1-3 note on why that took this long).
// Reusing the same schema here keeps the two in sync by construction, not by remembering to.
function describeField(schema: FieldSchema[], key: string, value: unknown): { label: string; display: string } | null {
  if (value === null || value === undefined || value === '') return null
  const field = schema.find((f) => f.name === key)
  const label = field?.label ?? fallbackLabel(key)

  if (field?.type === 'select' && field.options) {
    const option = field.options.find((o) => o.value === Number(value))
    return { label, display: option?.label ?? String(value) }
  }
  if (typeof value === 'boolean') return { label, display: value ? 'Да' : 'Нет' }
  return { label, display: String(value) }
}

function AddToCartControl({ product, route }: { product: Product; route: ProductRoute }) {
  const { isAuthenticated } = useAuth()
  const { addToCart } = useCart()
  const [quantity, setQuantity] = useState(1)
  const [status, setStatus] = useState<'idle' | 'adding' | 'added' | 'error'>('idle')
  const [error, setError] = useState<string | null>(null)

  if (!isAuthenticated) {
    return (
      <p className="product-detail__cart-hint">
        <Link to="/login" className="btn btn-secondary">
          Войдите
        </Link>{' '}
        чтобы добавить в корзину
      </p>
    )
  }

  async function handleAdd() {
    setStatus('adding')
    setError(null)
    try {
      await addToCart(
        {
          productType: route.productType,
          subCategoryId: product.subCategoryId,
          productId: product.id,
          price: product.price,
        },
        quantity,
      )
      setStatus('added')
    } catch (err) {
      setStatus('error')
      setError(err instanceof ApiError ? err.message : 'Не удалось добавить в корзину')
    }
  }

  return (
    <div className="product-detail__cart-control">
      {error && <div className="form-error">{error}</div>}
      <input
        type="number"
        min={1}
        value={quantity}
        onChange={(e) => setQuantity(Math.max(1, Number(e.target.value) || 1))}
        className="product-detail__quantity"
        aria-label="Количество"
      />
      <button type="button" className="btn btn-primary" onClick={handleAdd} disabled={status === 'adding'}>
        {status === 'added' ? 'В корзине ✓' : status === 'adding' ? 'Добавляем…' : 'В корзину'}
      </button>
    </div>
  )
}

export function ProductDetailPage() {
  const { productType: slug, id } = useParams<{ productType: string; id: string }>()
  const route = routeForSlug(slug ?? '')
  const productId = Number(id)
  const [activeImage, setActiveImage] = useState(0)

  const { data: product, error, isLoading, reload } = useAsync(
    () => (route ? getProductById(route.productType, productId).then((r) => r.data) : Promise.resolve(null)),
    [route?.productType, productId],
  )

  if (!route) return <NotFoundPage />
  if (isLoading) return <Spinner />
  if (error) return <ErrorState message={error} onRetry={reload} />
  if (!product) return <NotFoundPage />

  const images = product.images
  const cover = images[activeImage] ?? images[0]
  const title = typeof product.model === 'string' ? product.model : `Объявление №${product.id}`

  return (
    <div className="product-detail">
      <div className="product-detail__gallery">
        <div className="product-detail__main-image">
          {cover ? <img src={imageUrl(cover.imageName)} alt={title} /> : <div className="product-detail__placeholder" />}
        </div>
        {images.length > 1 && (
          <div className="product-detail__thumbs" role="group" aria-label="Другие фото">
            {images.map((img, i) => (
              <button
                key={img.id}
                type="button"
                className={i === activeImage ? 'product-detail__thumb product-detail__thumb--active' : 'product-detail__thumb'}
                onClick={() => setActiveImage(i)}
                aria-label={`Фото ${i + 1}`}
                aria-pressed={i === activeImage}
              >
                {/* Decorative duplicate of the main image above (which already carries the real alt text). */}
                <img src={imageUrl(img.imageName)} alt="" />
              </button>
            ))}
          </div>
        )}
      </div>

      <div className="product-detail__info">
        <p className="product-detail__breadcrumb">{route.label}</p>
        <h1>{title}</h1>
        <p className="product-detail__price">
          <Money amount={product.price} />
        </p>

        <AddToCartControl product={product} route={route} />

        <dl className="product-fields">
          {(() => {
            const schema = schemaForProductType(route.productType as ProductTypeValue)
            return Object.entries(product)
              .filter(([key]) => !HIDDEN_FIELDS.has(key))
              .map(([key, value]) => {
                const field = describeField(schema, key, value)
                if (!field) return null
                return (
                  <div key={key} className="product-fields__row">
                    <dt>{field.label}</dt>
                    <dd>{field.display}</dd>
                  </div>
                )
              })
          })()}
        </dl>
      </div>
    </div>
  )
}
