import { useSearchParams, useParams } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getProducts } from '../api/products'
import { routeForSlug } from '../api/products/registry'
import type { ProductTypeSlug } from '../api/types'
import { ProductCard } from '../components/ProductCard'
import { Pagination } from '../components/Pagination'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import { NotFoundPage } from './NotFoundPage'
import './ProductListPage.css'

export function ProductListPage() {
  const { productType: slug } = useParams<{ productType: string }>()
  const [searchParams, setSearchParams] = useSearchParams()
  const route = routeForSlug(slug ?? '')

  const pageNumber = Number(searchParams.get('page') ?? '1')
  const subCategoryId = searchParams.get('subCategoryId')

  const { data, error, isLoading, reload } = useAsync(
    () =>
      route
        ? getProducts(route.productType, {
            pageNumber,
            pageSize: 20,
            subCategoryId: subCategoryId ? Number(subCategoryId) : undefined,
          })
        : Promise.resolve(null),
    [route?.productType, pageNumber, subCategoryId],
  )

  if (!route) return <NotFoundPage />

  return (
    <div>
      <h1>{route.label}</h1>
      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reload} />}
      {data && data.items.length === 0 && <p>Ничего не найдено.</p>}
      {data && data.items.length > 0 && (
        <>
          <div className="product-grid">
            {data.items.map((product) => (
              <ProductCard key={product.id} product={product} slug={slug as ProductTypeSlug} />
            ))}
          </div>
          <Pagination
            pageNumber={data.pageNumber}
            totalPage={data.totalPage}
            onChange={(page) => setSearchParams((p) => ({ ...Object.fromEntries(p), page: String(page) }))}
          />
        </>
      )}
    </div>
  )
}
