import { api, buildQuery } from './client'
import type { ProductTypeValue } from './types'

export interface Catalog {
  catalogId: number
  catalogName: string
}

export interface Category {
  categoryId: number
  categoryName: string
  catalogId: number
}

export interface SubCategory {
  subCategoryId: number
  subCategoryName: string
  categoryId: number
  // Which of the 11 product tables this subcategory's listings live in - added on the backend
  // (Eraj commit 75a0464) specifically to replace the hardcoded productCatalogMap.ts this
  // frontend used to carry. Null for a subcategory nobody has wired up yet (Admin's choice).
  productType: ProductTypeValue | null
}

export const getCatalogs = () => api.get<Catalog[]>('/api/Catalog/get/catalogs', { auth: false })

export const getCatalogById = (catalogId: number) =>
  api.get<Catalog>(`/api/Catalog/get/catalogById?catalogId=${catalogId}`, { auth: false })

// Real server-side filters now (Eraj commit 75a0464) - catalogId/categoryId narrow the result
// instead of the caller fetching everything and filtering client-side.
export const getCategories = (params: { catalogId?: number } = {}) =>
  api.get<Category[]>(`/api/Category/get/category${buildQuery(params)}`, { auth: false })

export const getSubCategories = (params: { categoryId?: number; productType?: ProductTypeValue } = {}) =>
  api.get<SubCategory[]>(`/api/SubCategory/get/subcategory${buildQuery(params)}`, { auth: false })

export const getCategoryById = (categoryId: number) =>
  api.get<Category>(`/api/Category/get/categoryById?categoryId=${categoryId}`, { auth: false })

export const getSubCategoryById = (subCategoryId: number) =>
  api.get<SubCategory>(`/api/SubCategory/get/subcategoryById?subCategoryId=${subCategoryId}`, { auth: false })

// --- Admin/SuperAdmin mutations (Authorize(Roles = "SuperAdmin, Admin") on the backend) ---

export const addCatalog = (catalogName: string) => api.post<null>('/api/Catalog/post/catalog', { catalogName })

export const updateCatalog = (catalogId: number, catalogName: string) =>
  api.put<null>('/api/Catalog/put/catalog', { catalogId, catalogName })

// Route really is "deleted/catalog", not "delete/catalog" - the only inconsistent one of the
// three (Category/SubCategory both use "delete/...") - copied verbatim from
// WebApi/Controllers/CatalogController.cs, not a typo introduced here.
export const deleteCatalog = (catalogId: number) => api.del<null>(`/api/Catalog/deleted/catalog?catalogId=${catalogId}`)

export const addCategory = (categoryName: string, catalogId: number) =>
  api.post<null>('/api/Category/post/category', { categoryName, catalogId })

export const updateCategory = (categoryId: number, categoryName: string, catalogId: number) =>
  api.put<null>('/api/Category/put/category', { categoryId, categoryName, catalogId })

export const deleteCategory = (categoryId: number) => api.del<null>(`/api/Category/delete/category?categoryId=${categoryId}`)

export const addSubCategory = (subCategoryName: string, categoryId: number, productType?: ProductTypeValue | null) =>
  api.post<null>('/api/SubCategory/post/subcategory', { subCategoryName, categoryId, productType })

export const updateSubCategory = (
  subCategoryId: number,
  subCategoryName: string,
  categoryId: number,
  productType?: ProductTypeValue | null,
) => api.put<null>('/api/SubCategory/put/subcategory', { subCategoryId, subCategoryName, categoryId, productType })

export const deleteSubCategory = (subCategoryId: number) =>
  api.del<null>(`/api/SubCategory/delete/subcategory?subCategoryId=${subCategoryId}`)
