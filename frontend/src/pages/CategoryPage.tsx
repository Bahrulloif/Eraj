import { Link, useParams } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getSubCategories, getCategoryById } from '../api/catalog'
import { slugForProductType } from '../api/types'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

export function CategoryPage() {
  const { categoryId } = useParams<{ categoryId: string }>()
  const id = Number(categoryId)

  const category = useAsync(() => getCategoryById(id).then((r) => r.data), [id])
  // Real server-side filter now (Eraj commit 75a0464) - was a client-side .filter() before that.
  const subCategories = useAsync(() => getSubCategories({ categoryId: id }).then((r) => r.data), [id])

  const isLoading = category.isLoading || subCategories.isLoading
  const error = category.error ?? subCategories.error

  return (
    <div>
      <h1>{category.data?.categoryName ?? '…'}</h1>
      {isLoading && <Spinner />}
      {error && (
        <ErrorState message={error} onRetry={() => { category.reload(); subCategories.reload() }} />
      )}
      {subCategories.data && (
        <div className="catalog-grid">
          {subCategories.data.map((sub) => {
            // SubCategory.productType comes straight from the backend now (Eraj commit 75a0464) -
            // no more guessing it from a hardcoded id table. Still nullable (an Admin may not have
            // wired this subcategory to a product type yet) - shown as plain text, not a dead link.
            if (sub.productType == null) {
              return (
                <span key={sub.subCategoryId} className="catalog-card catalog-card--disabled">
                  {sub.subCategoryName}
                </span>
              )
            }
            return (
              <Link
                key={sub.subCategoryId}
                to={`/products/${slugForProductType(sub.productType)}?subCategoryId=${sub.subCategoryId}`}
                className="catalog-card"
              >
                {sub.subCategoryName}
              </Link>
            )
          })}
          {subCategories.data.length === 0 && <p>В этой категории пока нет подкатегорий.</p>}
        </div>
      )}
    </div>
  )
}
