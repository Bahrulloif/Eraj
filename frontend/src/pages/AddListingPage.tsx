import { Link, useNavigate, useParams } from 'react-router-dom'
import { ProductForm } from '../components/ProductForm'
import { ensureSubCategoryMapLoaded } from '../api/productCatalogMap'
import { PRODUCT_TYPE_LABELS, ProductType, slugForProductType, type ProductTypeValue } from '../api/types'
import { routeForSlug } from '../api/products/registry'
import { useAsync } from '../hooks/useAsync'
import { Spinner } from '../components/Spinner'

const ALL_TYPES = Object.values(ProductType) as ProductTypeValue[]

function TypePicker() {
  return (
    <div>
      <h1>Новое объявление</h1>
      <p className="hint-text">Выберите категорию товара.</p>
      <div className="catalog-grid">
        {ALL_TYPES.map((pt) => (
          <Link key={pt} to={`/my-listings/new/${slugForProductType(pt)}`} className="catalog-card">
            {PRODUCT_TYPE_LABELS[pt]}
          </Link>
        ))}
      </div>
    </div>
  )
}

export function AddListingPage() {
  const { productType: slug } = useParams<{ productType?: string }>()
  const navigate = useNavigate()

  // ProductForm's subcategory picker (subCategoriesForProductType) reads a synchronous cache that
  // needs to be warm before the form renders - see productCatalogMap.ts.
  const subCategoryMap = useAsync(() => ensureSubCategoryMapLoaded(), [])

  if (!slug) return <TypePicker />

  const route = routeForSlug(slug)
  if (!route) return <TypePicker />
  if (subCategoryMap.isLoading) return <Spinner />

  return (
    <div>
      <h1>Новое объявление: {route.label}</h1>
      {/* MyListingsPage now finds this by querying ownerId=me directly (Eraj commit 58b302f) -
          no need to resolve/remember the new listing's id client-side anymore. */}
      <ProductForm productType={route.productType} onSaved={() => navigate('/my-listings')} />
    </div>
  )
}
