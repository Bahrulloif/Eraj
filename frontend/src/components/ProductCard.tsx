import { Link } from 'react-router-dom'
import type { Product } from '../api/products'
import { imageUrl } from '../api/products'
import type { ProductTypeSlug } from '../api/types'
import { Money } from './Money'
import './ProductCard.css'

function productTitle(product: Product): string {
  // No single "title" field is common to all 11 DTOs - Model covers most, Brand/TypeOfRealEstate
  // fill in the rest reasonably (see individual GetXDTOs in Eraj/Domain/DTOs).
  const candidate = product.model ?? product.brand ?? product.description
  return typeof candidate === 'string' && candidate ? candidate : `Объявление №${product.id}`
}

export function ProductCard({ product, slug }: { product: Product; slug: ProductTypeSlug }) {
  const cover = product.images[0]

  return (
    <Link to={`/products/${slug}/${product.id}`} className="product-card">
      <div className="product-card__image">
        {cover ? <img src={imageUrl(cover.imageName)} alt={productTitle(product)} loading="lazy" /> : <div className="product-card__placeholder" />}
      </div>
      <div className="product-card__body">
        <p className="product-card__title">{productTitle(product)}</p>
        <p className="product-card__price">
          <Money amount={product.price} />
        </p>
      </div>
    </Link>
  )
}
