import { useNavigate, useParams } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getProductById } from '../api/products'
import { routeForSlug } from '../api/products/registry'
import { ProductForm } from '../components/ProductForm'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import { NotFoundPage } from './NotFoundPage'

export function EditListingPage() {
  const { productType: slug, id } = useParams<{ productType: string; id: string }>()
  const navigate = useNavigate()
  const route = routeForSlug(slug ?? '')
  const productId = Number(id)

  const { data: product, error, isLoading, reload } = useAsync(
    () => (route ? getProductById(route.productType, productId).then((r) => r.data) : Promise.resolve(null)),
    [route?.productType, productId],
  )

  if (!route) return <NotFoundPage />
  if (isLoading) return <Spinner />
  if (error) return <ErrorState message={error} onRetry={reload} />
  if (!product) return <NotFoundPage />

  return (
    <div>
      <h1>Изменить объявление: {route.label}</h1>
      <ProductForm productType={route.productType} product={product} onSaved={() => navigate('/my-listings')} />
    </div>
  )
}
