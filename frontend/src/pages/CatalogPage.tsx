import { Link, useParams } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getCategories, getCatalogById } from '../api/catalog'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

export function CatalogPage() {
  const { catalogId } = useParams<{ catalogId: string }>()
  const id = Number(catalogId)

  const catalog = useAsync(() => getCatalogById(id).then((r) => r.data), [id])
  // Real server-side filter now (Eraj commit 75a0464) - was a client-side .filter() over every
  // category before that.
  const categories = useAsync(() => getCategories({ catalogId: id }).then((r) => r.data), [id])

  const isLoading = catalog.isLoading || categories.isLoading
  const error = catalog.error ?? categories.error

  return (
    <div>
      <h1>{catalog.data?.catalogName ?? '…'}</h1>
      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={() => { catalog.reload(); categories.reload() }} />}
      {categories.data && (
        <div className="catalog-grid">
          {categories.data.map((cat) => (
            <Link key={cat.categoryId} to={`/category/${cat.categoryId}`} className="catalog-card">
              {cat.categoryName}
            </Link>
          ))}
          {categories.data.length === 0 && <p>В этом каталоге пока нет категорий.</p>}
        </div>
      )}
    </div>
  )
}
