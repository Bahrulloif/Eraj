import { ProductType, type ProductTypeSlug, type ProductTypeValue, PRODUCT_TYPE_LABELS, PRODUCT_TYPE_SLUGS } from '../types'

// Route naming is NOT consistent across the 11 product controllers (grepped directly from
// WebApi/Controllers/{TransportController,KompTechController,RealEstateController}/*.cs, not
// guessed by analogy - see Фаза 2 note in ../../../speca.md). Car in particular updates via POST
// (post/updateCar), not PUT like every other type - that's a real backend quirk, not a typo here.
export interface ProductRoute {
  productType: ProductTypeValue
  slug: ProductTypeSlug
  label: string
  controller: string
  listPath: string
  byIdPath: string
  /** Query param name the *ById endpoint expects, e.g. GetCarById(int carId) -> "carId". */
  idParam: string
  addPath: string
  updatePath: string
  updateMethod: 'PUT' | 'POST'
  deletePath: string
}

// Each entry below is spelled out by hand instead of generated from a shared pattern - the whole
// point of this file is that there ISN'T one shared pattern (see header comment).
export const PRODUCT_ROUTES: Record<ProductTypeValue, ProductRoute> = {
  [ProductType.Car]: {
    productType: ProductType.Car,
    slug: 'car',
    label: PRODUCT_TYPE_LABELS[ProductType.Car],
    controller: 'Car',
    listPath: '/api/Car/get/car',
    byIdPath: '/api/Car/get/carById',
    idParam: 'carId',
    addPath: '/api/Car/post/addCar',
    updatePath: '/api/Car/post/updateCar',
    updateMethod: 'POST',
    deletePath: '/api/Car/delete/deleteCar',
  },
  [ProductType.Truck]: {
    productType: ProductType.Truck,
    slug: 'truck',
    label: PRODUCT_TYPE_LABELS[ProductType.Truck],
    controller: 'Truck',
    listPath: '/api/Truck/get/GetTruck',
    byIdPath: '/api/Truck/get/GetTruckById',
    idParam: 'truckId',
    addPath: '/api/Truck/post/AddTruck',
    updatePath: '/api/Truck/put/UpdateTruck',
    updateMethod: 'PUT',
    deletePath: '/api/Truck/delete/DeleteTruck',
  },
  [ProductType.Motorbike]: {
    productType: ProductType.Motorbike,
    slug: 'motorbike',
    label: PRODUCT_TYPE_LABELS[ProductType.Motorbike],
    controller: 'Motorbike',
    listPath: '/api/Motorbike/get/getMotorbike',
    byIdPath: '/api/Motorbike/get/getMotorbikeById',
    idParam: 'motorbikeId',
    addPath: '/api/Motorbike/post/addMotorbike',
    updatePath: '/api/Motorbike/put/updateMotorbike',
    updateMethod: 'PUT',
    deletePath: '/api/Motorbike/delete/deleteMotorbike',
  },
  [ProductType.SpareAccessorTransp]: {
    productType: ProductType.SpareAccessorTransp,
    slug: 'spare-transp',
    label: PRODUCT_TYPE_LABELS[ProductType.SpareAccessorTransp],
    controller: 'SpareAccessorTransp',
    listPath: '/api/SpareAccessorTransp/get/spareAccessorTransp',
    byIdPath: '/api/SpareAccessorTransp/get/spareAccessorTranspById',
    idParam: 'spareAccessorTranspId',
    addPath: '/api/SpareAccessorTransp/post/spareAccessorTransp',
    updatePath: '/api/SpareAccessorTransp/put/spareAccessorTransp',
    updateMethod: 'PUT',
    deletePath: '/api/SpareAccessorTransp/delete/spareAccessorTransp',
  },
  [ProductType.NoteBook]: {
    productType: ProductType.NoteBook,
    slug: 'notebook',
    label: PRODUCT_TYPE_LABELS[ProductType.NoteBook],
    controller: 'NoteBook',
    listPath: '/api/NoteBook/get/notebook',
    byIdPath: '/api/NoteBook/get/notebookById',
    idParam: 'notebookId',
    addPath: '/api/NoteBook/post/notebook',
    updatePath: '/api/NoteBook/put/notebook',
    updateMethod: 'PUT',
    deletePath: '/api/NoteBook/delete/notebook',
  },
  [ProductType.SmartPhone]: {
    productType: ProductType.SmartPhone,
    slug: 'smartphone',
    label: PRODUCT_TYPE_LABELS[ProductType.SmartPhone],
    controller: 'SmartPhone',
    listPath: '/api/SmartPhone/get/smartphone',
    byIdPath: '/api/SmartPhone/get/smartphoneById',
    idParam: 'smartPhoneId',
    addPath: '/api/SmartPhone/post/smartphone',
    updatePath: '/api/SmartPhone/put/smartphone',
    updateMethod: 'PUT',
    deletePath: '/api/SmartPhone/delete/smartphone',
  },
  [ProductType.Tablet]: {
    productType: ProductType.Tablet,
    slug: 'tablet',
    label: PRODUCT_TYPE_LABELS[ProductType.Tablet],
    controller: 'Tablet',
    listPath: '/api/Tablet/get/tablet',
    byIdPath: '/api/Tablet/get/tabletById',
    idParam: 'tabletId',
    addPath: '/api/Tablet/post/tablet',
    updatePath: '/api/Tablet/put/tablet',
    updateMethod: 'PUT',
    deletePath: '/api/Tablet/delete/tablet',
  },
  [ProductType.SpareAccessorKomp]: {
    productType: ProductType.SpareAccessorKomp,
    slug: 'spare-komp',
    label: PRODUCT_TYPE_LABELS[ProductType.SpareAccessorKomp],
    controller: 'SpareAccessorKomp',
    listPath: '/api/SpareAccessorKomp/get/spareAccessorKomp',
    byIdPath: '/api/SpareAccessorKomp/get/spareAccessorKompById',
    idParam: 'spareAccessorKompId',
    addPath: '/api/SpareAccessorKomp/post/spareAccessorKomp',
    updatePath: '/api/SpareAccessorKomp/put/spareAccessorKomp',
    updateMethod: 'PUT',
    deletePath: '/api/SpareAccessorKomp/delete/spareAccessorKomp',
  },
  [ProductType.Apartment]: {
    productType: ProductType.Apartment,
    slug: 'apartment',
    label: PRODUCT_TYPE_LABELS[ProductType.Apartment],
    controller: 'Apartment',
    listPath: '/api/Apartment/get/apartment',
    byIdPath: '/api/Apartment/get/apartmentById',
    idParam: 'apartmentId',
    addPath: '/api/Apartment/post/apartment',
    updatePath: '/api/Apartment/put/apartment',
    updateMethod: 'PUT',
    deletePath: '/api/Apartment/delete/apartment',
  },
  [ProductType.CommercialRealEstate]: {
    productType: ProductType.CommercialRealEstate,
    slug: 'commercial-real-estate',
    label: PRODUCT_TYPE_LABELS[ProductType.CommercialRealEstate],
    controller: 'CommercialRealEstate',
    listPath: '/api/CommercialRealEstate/get/commercialRealEstate',
    byIdPath: '/api/CommercialRealEstate/get/commercialRealEstateById',
    idParam: 'commercialRealEstateId',
    addPath: '/api/CommercialRealEstate/post/commercialRealEstate',
    updatePath: '/api/CommercialRealEstate/put/commercialRealEstate',
    updateMethod: 'PUT',
    deletePath: '/api/CommercialRealEstate/delete/commercialRealEstate',
  },
  [ProductType.Cottage]: {
    productType: ProductType.Cottage,
    slug: 'cottage',
    label: PRODUCT_TYPE_LABELS[ProductType.Cottage],
    controller: 'Cottage',
    listPath: '/api/Cottage/get/cottage',
    byIdPath: '/api/Cottage/get/cottageById',
    idParam: 'cottageId',
    addPath: '/api/Cottage/post/cottage',
    updatePath: '/api/Cottage/put/cottage',
    updateMethod: 'PUT',
    deletePath: '/api/Cottage/delete/cottage',
  },
}

export function routeForSlug(slug: string): ProductRoute | null {
  const productType = PRODUCT_TYPE_SLUGS[slug as ProductTypeSlug]
  return productType ? PRODUCT_ROUTES[productType] : null
}
