import { ProductType, type ProductTypeValue } from '../types'

export type FieldType = 'text' | 'textarea' | 'number' | 'decimal' | 'select' | 'checkbox' | 'date'

export interface SelectOption {
  value: number
  label: string
}

export interface FieldSchema {
  /** Matches the DTO's JSON property name exactly (camelCase). */
  name: string
  label: string
  type: FieldType
  required: boolean
  options?: SelectOption[]
  /** Multi-line free text (Car's ~15 optional equipment strings) gets a textarea, not a cramped input. */
}

// Every field below was copied from the actual C# DTO (Domain/DTOs/.../*DTO.cs) + its enum
// definitions, not reconstructed from memory - see speca.md's Фаза 6 note for the full list of
// files read. `id`/`subCategoryId`/`images` are handled outside this schema (see ProductForm) -
// id and subCategoryId aren't user-editable free text, and images need a dedicated uploader.

const gearboxOptions: SelectOption[] = [
  { value: 0, label: 'Механика' },
  { value: 1, label: 'Автомат' },
  { value: 2, label: 'Вариатор (CVT)' },
  { value: 3, label: 'Робот' },
]
const conditionOptions: SelectOption[] = [
  { value: 0, label: 'Новый' },
  { value: 1, label: 'С пробегом' },
]
const steeringWheelOptions: SelectOption[] = [
  { value: 0, label: 'Левый' },
  { value: 1, label: 'Правый' },
]
const truckBodyTypeOptions: SelectOption[] = [
  { value: 0, label: 'Бортовой' },
  { value: 1, label: 'Фургон' },
  { value: 2, label: 'Цистерна' },
  { value: 3, label: 'Рефрижератор' },
  { value: 4, label: 'Самосвал' },
  { value: 5, label: 'Другое' },
]
const truckEngineTypeOptions: SelectOption[] = [
  { value: 0, label: 'Дизель' },
  { value: 1, label: 'Бензин' },
  { value: 2, label: 'Электро' },
  { value: 3, label: 'Гибрид' },
  { value: 4, label: 'Газ' },
]
const truckTransmissionOptions: SelectOption[] = [
  { value: 0, label: 'Механика' },
  { value: 1, label: 'Автомат' },
  { value: 2, label: 'Робот' },
]
const cpuBrandOptions: SelectOption[] = [
  { value: 0, label: 'Intel' },
  { value: 1, label: 'AMD' },
  { value: 2, label: 'Apple' },
  { value: 3, label: 'ARM' },
  { value: 4, label: 'Другой' },
]
const renovationOptions: SelectOption[] = [
  { value: 0, label: 'Без ремонта' },
  { value: 1, label: 'Косметический' },
  { value: 2, label: 'Современный' },
  { value: 3, label: 'Дизайнерский' },
]
const buildingTypeOptions: SelectOption[] = [
  { value: 0, label: 'Офис' },
  { value: 1, label: 'Торговое помещение' },
  { value: 2, label: 'Промышленное' },
  { value: 3, label: 'Склад' },
  { value: 4, label: 'Смешанное' },
  { value: 5, label: 'Другое' },
]
const typeOfEstateOptions: SelectOption[] = [
  { value: 0, label: 'Коттедж' },
  { value: 1, label: 'Таунхаус' },
  { value: 2, label: 'Дом фермера' },
  { value: 3, label: 'Вилла' },
  { value: 4, label: 'Другое' },
]
const wallMaterialOptions: SelectOption[] = [
  { value: 0, label: 'Кирпич' },
  { value: 1, label: 'Бетон' },
  { value: 2, label: 'Дерево' },
  { value: 3, label: 'Камень' },
  { value: 4, label: 'Другое' },
]

const PRODUCT_SCHEMAS: Record<ProductTypeValue, FieldSchema[]> = {
  [ProductType.Car]: [
    { name: 'model', label: 'Модель', type: 'text', required: true },
    { name: 'brand', label: 'Марка', type: 'text', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
    { name: 'yearOfIssue', label: 'Год выпуска', type: 'number', required: true },
    { name: 'engine', label: 'Двигатель', type: 'text', required: true },
    { name: 'body', label: 'Кузов', type: 'text', required: true },
    { name: 'gearbox', label: 'Коробка передач', type: 'select', required: true, options: gearboxOptions },
    { name: 'driverUnit', label: 'Привод', type: 'text', required: true },
    { name: 'engineCapacity', label: 'Объём двигателя, л', type: 'decimal', required: true },
    { name: 'mileage', label: 'Пробег, км', type: 'number', required: true },
    { name: 'manufacturerState', label: 'Страна производства', type: 'text', required: true },
    { name: 'fuelPer100km', label: 'Расход, л/100км', type: 'decimal', required: true },
    { name: 'numberOfSeats', label: 'Число мест', type: 'number', required: true },
    { name: 'condition', label: 'Состояние', type: 'select', required: true, options: conditionOptions },
    { name: 'accelerTo100km', label: 'Разгон до 100 км/ч, с', type: 'decimal', required: false },
    { name: 'trunkVolume', label: 'Объём багажника, л', type: 'number', required: false },
    { name: 'clearance', label: 'Клиренс, мм', type: 'number', required: false },
    { name: 'steeringWheel', label: 'Руль', type: 'select', required: false, options: steeringWheelOptions },
    { name: 'color', label: 'Цвет', type: 'text', required: false },
    { name: 'powerSteering', label: 'Усилитель руля', type: 'checkbox', required: false },
    { name: 'interiorColor', label: 'Цвет салона', type: 'text', required: false },
    { name: 'settingsMemory', label: 'Память настроек', type: 'checkbox', required: false },
    { name: 'multimediaAndNavigation', label: 'Мультимедиа и навигация', type: 'text', required: false },
    { name: 'climateControl', label: 'Климат-контроль', type: 'text', required: false },
    { name: 'drivingAssistance', label: 'Помощь при вождении', type: 'text', required: false },
    { name: 'antiTheftSystem', label: 'Противоугонная система', type: 'text', required: false },
    { name: 'airbags', label: 'Подушки безопасности', type: 'text', required: false },
    { name: 'heating', label: 'Обогрев', type: 'text', required: false },
    { name: 'tiresAndWheels', label: 'Шины и диски', type: 'text', required: false },
    { name: 'headlights', label: 'Фары', type: 'text', required: false },
    { name: 'audioSystems', label: 'Аудиосистема', type: 'text', required: false },
    { name: 'electricLifts', label: 'Электростеклоподъёмники', type: 'text', required: false },
    { name: 'electricDrive', label: 'Электропривод', type: 'text', required: false },
    { name: 'activeSafety', label: 'Активная безопасность', type: 'text', required: false },
  ],
  [ProductType.Truck]: [
    { name: 'model', label: 'Модель', type: 'text', required: true },
    { name: 'brand', label: 'Марка', type: 'text', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'priceDiscount', label: 'Цена со скидкой', type: 'decimal', required: true },
    { name: 'yearOfIssue', label: 'Год выпуска', type: 'number', required: true },
    { name: 'bodyType', label: 'Тип кузова', type: 'select', required: false, options: truckBodyTypeOptions },
    { name: 'power', label: 'Мощность, л.с.', type: 'number', required: true },
    { name: 'engineType', label: 'Тип двигателя', type: 'select', required: true, options: truckEngineTypeOptions },
    { name: 'engineCapacity', label: 'Объём двигателя, л', type: 'decimal', required: true },
    { name: 'environmentalClass', label: 'Экологический класс', type: 'text', required: false },
    { name: 'transmission', label: 'Коробка передач', type: 'select', required: true, options: truckTransmissionOptions },
    { name: 'wheelFormula', label: 'Колёсная формула', type: 'text', required: false },
    { name: 'loadCapacity', label: 'Грузоподъёмность, кг', type: 'decimal', required: true },
    { name: 'permittedMaximumWeight', label: 'Полная масса, кг', type: 'decimal', required: true },
    { name: 'mileage', label: 'Пробег, км', type: 'number', required: true },
  ],
  [ProductType.Motorbike]: [
    { name: 'model', label: 'Модель', type: 'text', required: true },
    { name: 'brand', label: 'Марка', type: 'text', required: true },
    { name: 'region', label: 'Регион', type: 'text', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
    { name: 'yearOfIssue', label: 'Год выпуска', type: 'date', required: true },
    { name: 'manufacturerCountry', label: 'Страна производства', type: 'text', required: false },
    { name: 'engineType', label: 'Тип двигателя', type: 'text', required: false },
    { name: 'engineCapacity', label: 'Объём двигателя', type: 'text', required: false },
    { name: 'power', label: 'Мощность', type: 'text', required: false },
    { name: 'fuelSupply', label: 'Подача топлива', type: 'text', required: false },
    { name: 'numberOfCycles', label: 'Число тактов', type: 'text', required: false },
    { name: 'gearBox', label: 'Коробка передач', type: 'text', required: false },
    { name: 'mileage', label: 'Пробег', type: 'text', required: false },
    { name: 'passengers', label: 'Мест для пассажиров', type: 'text', required: false },
  ],
  [ProductType.SpareAccessorTransp]: [
    { name: 'model', label: 'Название', type: 'text', required: true },
    { name: 'description', label: 'Описание', type: 'textarea', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
  ],
  [ProductType.NoteBook]: [
    { name: 'model', label: 'Модель', type: 'text', required: true },
    { name: 'core', label: 'Процессор', type: 'select', required: true, options: cpuBrandOptions },
    { name: 'ram', label: 'ОЗУ, ГБ', type: 'number', required: true },
    { name: 'diagonal', label: 'Диагональ экрана, дюймы', type: 'decimal', required: true },
    { name: 'rom', label: 'Накопитель, ГБ', type: 'number', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
    { name: 'color', label: 'Цвет', type: 'text', required: true },
  ],
  [ProductType.SmartPhone]: [
    { name: 'model', label: 'Модель', type: 'text', required: true },
    { name: 'core', label: 'Процессор', type: 'text', required: true },
    { name: 'ram', label: 'ОЗУ, ГБ', type: 'number', required: true },
    { name: 'diagonal', label: 'Диагональ экрана, дюймы', type: 'decimal', required: true },
    { name: 'rom', label: 'Память, ГБ', type: 'number', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
    { name: 'color', label: 'Цвет', type: 'text', required: true },
  ],
  [ProductType.Tablet]: [
    { name: 'model', label: 'Модель', type: 'text', required: true },
    { name: 'core', label: 'Процессор', type: 'text', required: true },
    { name: 'ram', label: 'ОЗУ, ГБ', type: 'number', required: true },
    { name: 'diagonal', label: 'Диагональ экрана, дюймы', type: 'decimal', required: true },
    { name: 'rom', label: 'Память, ГБ', type: 'number', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
    { name: 'color', label: 'Цвет', type: 'text', required: true },
  ],
  [ProductType.SpareAccessorKomp]: [
    { name: 'model', label: 'Название', type: 'text', required: true },
    { name: 'description', label: 'Описание', type: 'textarea', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'discountPrice', label: 'Цена со скидкой', type: 'decimal', required: true },
  ],
  [ProductType.Apartment]: [
    { name: 'numberOfRooms', label: 'Число комнат', type: 'number', required: true },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'pricePerM2', label: 'Цена за м²', type: 'decimal', required: true },
    { name: 'totalArea', label: 'Общая площадь, м²', type: 'decimal', required: true },
    { name: 'floor', label: 'Этаж', type: 'number', required: true },
    { name: 'renovation', label: 'Ремонт', type: 'select', required: true, options: renovationOptions },
    { name: 'ceilingHeight', label: 'Высота потолков, м', type: 'decimal', required: true },
    { name: 'yearOfHouseBuild', label: 'Год постройки дома', type: 'number', required: true },
    { name: 'floorsInTheHouse', label: 'Этажей в доме', type: 'number', required: true },
    { name: 'parking', label: 'Парковка', type: 'checkbox', required: false },
    { name: 'kitchenArea', label: 'Площадь кухни, м²', type: 'decimal', required: true },
    { name: 'isNewBuilding', label: 'Новостройка', type: 'checkbox', required: false },
  ],
  [ProductType.CommercialRealEstate]: [
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'area', label: 'Площадь, м²', type: 'decimal', required: true },
    { name: 'buildingType', label: 'Тип помещения', type: 'select', required: true, options: buildingTypeOptions },
    { name: 'floor', label: 'Этаж', type: 'number', required: true },
  ],
  [ProductType.Cottage]: [
    { name: 'typeOfRealEstate', label: 'Тип объекта', type: 'select', required: true, options: typeOfEstateOptions },
    { name: 'price', label: 'Цена', type: 'decimal', required: true },
    { name: 'pricePerM2', label: 'Цена за м²', type: 'decimal', required: true },
    { name: 'houseArea', label: 'Площадь дома, м²', type: 'decimal', required: true },
    { name: 'plotArea', label: 'Площадь участка, сот.', type: 'decimal', required: true },
    { name: 'renovation', label: 'Ремонт', type: 'select', required: true, options: renovationOptions },
    { name: 'numberOfRooms', label: 'Число комнат', type: 'number', required: true },
    { name: 'wallMaterial', label: 'Материал стен', type: 'select', required: true, options: wallMaterialOptions },
    { name: 'parking', label: 'Парковка', type: 'checkbox', required: false },
  ],
}

export function schemaForProductType(productType: ProductTypeValue): FieldSchema[] {
  return PRODUCT_SCHEMAS[productType]
}
