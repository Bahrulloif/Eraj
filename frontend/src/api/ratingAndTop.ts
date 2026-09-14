import { api, buildQuery } from './client'
import type { PagedApiResponse, PageParams, Picture } from './types'

export interface RatingAndTopItem {
  productId: number
  subCategoryId: number
  model: string
  price: number
  discountPrice: number
  images: Picture[]
}

const list = (path: string, params: PageParams, auth: boolean) =>
  api.get<RatingAndTopItem[]>(`${path}${buildQuery(params)}`, { auth }) as Promise<PagedApiResponse<RatingAndTopItem[]>>

export const getPopularCategory = (params: PageParams = {}) => list('/api/RatingAndTop/get/popularCategory', params, false)
// Route really is "hotdicount", not "hotdiscount" - copied verbatim from
// WebApi/Controllers/RatingAndTopController.cs, not a typo introduced here.
export const getHotDiscount = (params: PageParams = {}) => list('/api/RatingAndTop/get/hotdicount', params, false)
export const getPopularProduct = (params: PageParams = {}) => list('/api/RatingAndTop/get/popularProduct', params, false)
export const getHitOfTheDay = (params: PageParams = {}) => list('/api/RatingAndTop/get/hitOfTheDay', params, false)
export const getHitOfTheMonth = (params: PageParams = {}) => list('/api/RatingAndTop/get/hitOfTheMonth', params, false)
export const getHitOfTheYear = (params: PageParams = {}) => list('/api/RatingAndTop/get/hitOfTheYear', params, false)
// The only personalized one - requires auth, reads the caller's own order history server-side.
export const getRecommendedProduct = (params: PageParams = {}) => list('/api/RatingAndTop/get/recommendedProduct', params, true)
