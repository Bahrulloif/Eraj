// Формы ответов бэкенда (Domain.Responses.Response<T> / PagedResponse<T>) - см. ../../Eraj/CLAUDE.md.
// Успех кладёт текст в `message`, ошибка - в `errors` (оба поля есть всегда, второе может быть []).
export interface ApiResponse<T> {
  data: T
  message: string | null
  errors: string[]
  statusCode: number
}

export interface PagedApiResponse<T> extends ApiResponse<T> {
  pageNumber: number
  pageSize: number
  totalPage: number
  totalRecord: number
}

export interface PageParams {
  pageNumber?: number
  pageSize?: number
}

// Domain.Enum.ProductType - один на каждую из 11 товарных сущностей. Значения зафиксированы на
// бэке (см. Domain/Enum/ProductType.cs) - не переставлять местами.
export const ProductType = {
  Car: 1,
  Motorbike: 2,
  Truck: 3,
  SpareAccessorTransp: 4,
  NoteBook: 5,
  SmartPhone: 6,
  Tablet: 7,
  SpareAccessorKomp: 8,
  Apartment: 9,
  CommercialRealEstate: 10,
  Cottage: 11,
} as const

export type ProductTypeValue = (typeof ProductType)[keyof typeof ProductType]

// Ключи-слаги для маршрутов/URL (/products/:productType) - человекочитаемые, стабильные.
export const PRODUCT_TYPE_SLUGS = {
  car: ProductType.Car,
  motorbike: ProductType.Motorbike,
  truck: ProductType.Truck,
  'spare-transp': ProductType.SpareAccessorTransp,
  notebook: ProductType.NoteBook,
  smartphone: ProductType.SmartPhone,
  tablet: ProductType.Tablet,
  'spare-komp': ProductType.SpareAccessorKomp,
  apartment: ProductType.Apartment,
  'commercial-real-estate': ProductType.CommercialRealEstate,
  cottage: ProductType.Cottage,
} as const

export type ProductTypeSlug = keyof typeof PRODUCT_TYPE_SLUGS

const SLUG_BY_PRODUCT_TYPE = Object.fromEntries(
  Object.entries(PRODUCT_TYPE_SLUGS).map(([slug, value]) => [value, slug]),
) as Record<ProductTypeValue, ProductTypeSlug>

export function slugForProductType(productType: ProductTypeValue): ProductTypeSlug {
  return SLUG_BY_PRODUCT_TYPE[productType]
}

export const PRODUCT_TYPE_LABELS: Record<ProductTypeValue, string> = {
  [ProductType.Car]: 'Автомобили',
  [ProductType.Motorbike]: 'Мотоциклы',
  [ProductType.Truck]: 'Грузовики',
  [ProductType.SpareAccessorTransp]: 'Запчасти для транспорта',
  [ProductType.NoteBook]: 'Ноутбуки',
  [ProductType.SmartPhone]: 'Смартфоны',
  [ProductType.Tablet]: 'Планшеты',
  [ProductType.SpareAccessorKomp]: 'Запчасти для компьютеров',
  [ProductType.Apartment]: 'Квартиры',
  [ProductType.CommercialRealEstate]: 'Коммерческая недвижимость',
  [ProductType.Cottage]: 'Коттеджи',
}

export const OrderStatus = {
  Delivered: 0,
  Canceled: 1,
  NotPaid: 2,
} as const

export interface Picture {
  id: number
  imageName: string
}

export interface Role {
  id: string
  name: string
}
