import { api, buildQuery } from '../client'
import type { PagedApiResponse, PageParams, Picture } from '../types'
import type { ProductTypeValue } from '../types'
import { PRODUCT_ROUTES } from './registry'

// The 11 product DTOs share these fields (see every GetXDTO in Eraj/Domain/DTOs/.../*.cs) plus a
// long tail of type-specific ones (Car alone has ~35). Rather than 11 near-duplicate interfaces,
// list/detail views work off this common shape and reach into the rest dynamically - see
// ProductListPage/ProductDetailPage's generic field rendering.
export interface Product {
  id: number
  subCategoryId: number
  price: number
  model?: string
  images: Picture[]
  [field: string]: unknown
}

export interface ProductListParams extends PageParams {
  /** Free-text filter - each type's own field (Model/Name/...), see registry per-type notes if any differ. */
  search?: string
  subCategoryId?: number
  /** Real server-side filter now (Eraj commit 58b302f) - used by MyListingsPage to ask for "mine". */
  ownerId?: string
}

export interface ProductPage {
  items: Product[]
  pageNumber: number
  pageSize: number
  totalPage: number
  totalRecord: number
}

// subCategoryId/ownerId are real server-side filters now (Eraj commit 58b302f) - this used to
// fetch the backend's max page size (200) and filter+paginate client-side instead.
export async function getProducts(
  productType: ProductTypeValue,
  { subCategoryId, ownerId, pageNumber = 1, pageSize = 20, search }: ProductListParams = {},
): Promise<ProductPage> {
  const { listPath } = PRODUCT_ROUTES[productType]
  const res = (await api.get<Product[]>(
    `${listPath}${buildQuery({ pageNumber, pageSize, subCategoryId, ownerId, model: search, name: search })}`,
    // ownerId-filtered calls (MyListingsPage) still hit an AllowAnonymous endpoint - no token
    // needed to filter public listings by an id the caller already knows, same as the rest of
    // this file's calls.
    { auth: false },
  )) as PagedApiResponse<Product[]>
  return { items: res.data, pageNumber: res.pageNumber, pageSize: res.pageSize, totalPage: res.totalPage, totalRecord: res.totalRecord }
}

export function getProductById(productType: ProductTypeValue, id: number) {
  const { byIdPath, idParam } = PRODUCT_ROUTES[productType]
  return api.get<Product>(`${byIdPath}${buildQuery({ [idParam]: id })}`, { auth: false })
}

export function deleteProduct(productType: ProductTypeValue, id: number) {
  const { deletePath, idParam } = PRODUCT_ROUTES[productType]
  return api.del<null>(`${deletePath}${buildQuery({ [idParam]: id })}`)
}

export function imageUrl(imageName: string): string {
  return `/Images/${imageName}`
}

// Every AddXDTO/UpdateXDTO is bound with [FromForm] (multipart, for the optional Images list) -
// plain field values still go in the same form body as regular fields, not JSON. Booleans must be
// the literal strings "true"/"false" for ASP.NET's form-value model binder to parse a `bool`/`bool?`.
function buildProductFormData(fields: Record<string, unknown>, subCategoryId: number, images: File[], id?: number): FormData {
  const form = new FormData()
  if (id !== undefined) form.append('Id', String(id))
  form.append('SubCategoryId', String(subCategoryId))
  for (const [key, value] of Object.entries(fields)) {
    if (value === undefined || value === null || value === '') continue
    const pascalKey = key.charAt(0).toUpperCase() + key.slice(1)
    form.append(pascalKey, typeof value === 'boolean' ? String(value) : String(value))
  }
  for (const file of images) form.append('Images', file)
  return form
}

export function addProduct(productType: ProductTypeValue, fields: Record<string, unknown>, subCategoryId: number, images: File[]) {
  const { addPath } = PRODUCT_ROUTES[productType]
  return api.postForm<null>(addPath, buildProductFormData(fields, subCategoryId, images))
}

// Car is the one type that updates via POST rather than PUT (see registry.ts's header comment) -
// PRODUCT_ROUTES.updateMethod is what actually decides the verb here, not an assumption.
export function updateProduct(
  productType: ProductTypeValue,
  id: number,
  fields: Record<string, unknown>,
  subCategoryId: number,
  images: File[],
) {
  const { updatePath, updateMethod } = PRODUCT_ROUTES[productType]
  const form = buildProductFormData(fields, subCategoryId, images, id)
  return updateMethod === 'PUT' ? api.putForm<null>(updatePath, form) : api.postForm<null>(updatePath, form)
}
