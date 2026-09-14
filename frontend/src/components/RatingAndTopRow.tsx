import { Link } from 'react-router-dom'
import type { RatingAndTopItem } from '../api/ratingAndTop'
import { ensureSubCategoryMapLoaded, productTypeForSubCategory } from '../api/productCatalogMap'
import { slugForProductType } from '../api/types'
import { imageUrl } from '../api/products'
import { useAsync } from '../hooks/useAsync'
import { Money } from './Money'
import { Spinner } from './Spinner'
import './RatingAndTopRow.css'

interface RatingAndTopRowProps {
  title: string
  load: () => Promise<{ data: RatingAndTopItem[] }>
}

export function RatingAndTopRow({ title, load }: RatingAndTopRowProps) {
  // Wait for the subcategory->ProductType map alongside this row's own data, so the very first
  // render already has real links instead of racing the fetch (see productCatalogMap.ts).
  const { data, isLoading } = useAsync(
    () => Promise.all([load(), ensureSubCategoryMapLoaded()]).then(([res]) => res.data),
    [load],
  )

  // Silent on error/empty (a marketing row, not core functionality) - HomePage stays useful even
  // if e.g. RecommendedProduct 401s for a guest browsing without a token yet, or a fresh dev DB
  // has no order history to rank anything by.
  if (!isLoading && (!data || data.length === 0)) return null

  return (
    <section className="rating-row">
      <h2>{title}</h2>
      {isLoading && <Spinner />}
      {data && (
        <div className="rating-row__scroller">
          {data.map((item) => {
            const productType = productTypeForSubCategory(item.subCategoryId)
            // item.productId alone isn't a unique React key here - unlike a single product-type
            // list, this row mixes multiple types together (see RatingAndTopDTO - no ProductType
            // field, same gap productCatalogMap.ts works around), so e.g. Car #1 and NoteBook #1
            // collide on a bare `1`. Caught live in a real browser render (console warning
            // "two children with the same key") - curl-only testing couldn't have caught this.
            const key = `${productType ?? 'unmapped'}-${item.productId}`
            const cover = item.images[0]
            const card = (
              <div className="rating-row__card">
                <div className="rating-row__image">
                  {cover ? <img src={imageUrl(cover.imageName)} alt={item.model} loading="lazy" /> : <div className="rating-row__placeholder" />}
                </div>
                <p className="rating-row__title">{item.model}</p>
                <p className="rating-row__price">
                  <Money amount={item.discountPrice || item.price} />
                  {item.discountPrice > 0 && item.discountPrice < item.price && (
                    <span className="rating-row__old-price">
                      <Money amount={item.price} />
                    </span>
                  )}
                </p>
              </div>
            )
            // Same map as elsewhere (see productCatalogMap.ts) - a subCategoryId this frontend
            // doesn't recognize can't be linked anywhere real, so it's shown but not clickable.
            return productType ? (
              <Link key={key} to={`/products/${slugForProductType(productType)}/${item.productId}`} className="rating-row__link">
                {card}
              </Link>
            ) : (
              <div key={key}>{card}</div>
            )
          })}
        </div>
      )}
    </section>
  )
}
