import { getSubCategories } from './catalog'
import type { ProductTypeValue } from './types'

// Used to be a hardcoded subCategoryId -> ProductType table here, worked around a backend gap
// (SubCategory had no field saying which of the 11 product tables it pointed at). Fixed on the
// backend (Eraj commit 75a0464): SubCategory now has a real ProductType column. This module keeps
// the same small synchronous API the rest of the frontend already used, just backed by a cached
// fetch of the real data instead of a table that would silently drift out of sync.
let cache: Map<number, ProductTypeValue> | null = null
let inFlight: Promise<void> | null = null

async function load(): Promise<void> {
  const res = await getSubCategories()
  const next = new Map<number, ProductTypeValue>()
  for (const s of res.data) {
    if (s.productType != null) next.set(s.subCategoryId, s.productType)
  }
  cache = next
}

// Callers rendering a list of items that link out by subcategory (RatingAndTopRow, ProductForm's
// subcategory picker) should await this before reading productTypeForSubCategory, so the very
// first render already has real data instead of racing the fetch and showing everything as
// unmapped. Fetches once; later calls reuse the same in-flight/resolved promise.
export function ensureSubCategoryMapLoaded(): Promise<void> {
  if (!inFlight) inFlight = load()
  return inFlight
}

// Synchronous, best-effort read of the cache - null if not loaded yet OR the subcategory genuinely
// has no ProductType set (an Admin hasn't wired it up - see AdminCatalogPage). Fine for badges/
// optional links; for a real list of navigational links, await ensureSubCategoryMapLoaded() first.
export function productTypeForSubCategory(subCategoryId: number): ProductTypeValue | null {
  return cache?.get(subCategoryId) ?? null
}

/** Reverse lookup, for the "add a listing" form - which subcategory id(s) to offer/default to. */
export function subCategoriesForProductType(productType: ProductTypeValue): number[] {
  if (!cache) return []
  return [...cache.entries()].filter(([, pt]) => pt === productType).map(([id]) => id)
}
