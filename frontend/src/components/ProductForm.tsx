import { useState } from 'react'
import type { FormEvent } from 'react'
import type { Product } from '../api/products'
import { addProduct, imageUrl, updateProduct } from '../api/products'
import { schemaForProductType } from '../api/products/schema'
import { subCategoriesForProductType } from '../api/productCatalogMap'
import type { ProductTypeValue } from '../api/types'
import { ApiError } from '../api/client'
import { ImageUploader } from './ImageUploader'

type FieldValue = string | boolean

function initialValues(productType: ProductTypeValue, product?: Product): Record<string, FieldValue> {
  const schema = schemaForProductType(productType)
  const values: Record<string, FieldValue> = {}
  for (const field of schema) {
    const raw = product?.[field.name]
    if (field.type === 'checkbox') values[field.name] = raw === true
    else if (field.type === 'date' && typeof raw === 'string') values[field.name] = raw.slice(0, 10)
    else values[field.name] = raw === undefined || raw === null ? '' : String(raw)
  }
  return values
}

interface ProductFormProps {
  productType: ProductTypeValue
  /** Present when editing an existing listing. */
  product?: Product
  onSaved: () => void
}

export function ProductForm({ productType, product, onSaved }: ProductFormProps) {
  const schema = schemaForProductType(productType)
  const subCategoryOptions = subCategoriesForProductType(productType)
  const isEditing = !!product

  const [values, setValues] = useState<Record<string, FieldValue>>(() => initialValues(productType, product))
  const [subCategoryId, setSubCategoryId] = useState(() => String(product?.subCategoryId ?? subCategoryOptions[0] ?? ''))
  const [images, setImages] = useState<File[]>([])
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  function setField(name: string, value: FieldValue) {
    setValues((v) => ({ ...v, [name]: value }))
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    setIsSubmitting(true)
    try {
      if (isEditing) {
        await updateProduct(productType, product.id, values, Number(subCategoryId), images)
      } else {
        await addProduct(productType, values, Number(subCategoryId), images)
      }
      onSaved()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось сохранить объявление')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="form product-form" onSubmit={handleSubmit}>
      {error && <div className="form-error">{error}</div>}

      {subCategoryOptions.length > 1 && (
        <div className="form-field">
          <label htmlFor="subCategoryId">Подкатегория</label>
          <select id="subCategoryId" value={subCategoryId} onChange={(e) => setSubCategoryId(e.target.value)} required>
            {subCategoryOptions.map((id) => (
              <option key={id} value={id}>
                №{id}
              </option>
            ))}
          </select>
        </div>
      )}

      {schema.map((field) => (
        <div className="form-field" key={field.name}>
          <label htmlFor={field.name}>{field.label}</label>
          {field.type === 'select' && (
            <select
              id={field.name}
              value={String(values[field.name])}
              required={field.required}
              onChange={(e) => setField(field.name, e.target.value)}
            >
              {!field.required && <option value="">—</option>}
              {field.options?.map((opt) => (
                <option key={opt.value} value={opt.value}>
                  {opt.label}
                </option>
              ))}
            </select>
          )}
          {field.type === 'checkbox' && (
            <input
              id={field.name}
              type="checkbox"
              checked={values[field.name] === true}
              onChange={(e) => setField(field.name, e.target.checked)}
            />
          )}
          {field.type === 'textarea' && (
            <textarea
              id={field.name}
              value={String(values[field.name])}
              required={field.required}
              onChange={(e) => setField(field.name, e.target.value)}
              rows={4}
            />
          )}
          {(field.type === 'text' || field.type === 'number' || field.type === 'decimal' || field.type === 'date') && (
            <input
              id={field.name}
              type={field.type === 'number' ? 'number' : field.type === 'decimal' ? 'number' : field.type === 'date' ? 'date' : 'text'}
              step={field.type === 'decimal' ? 'any' : undefined}
              value={String(values[field.name])}
              required={field.required}
              onChange={(e) => setField(field.name, e.target.value)}
            />
          )}
        </div>
      ))}

      <div className="form-field">
        <label>Фото</label>
        <ImageUploader
          files={images}
          onChange={setImages}
          existingImageUrls={product?.images.map((img) => imageUrl(img.imageName))}
        />
      </div>

      <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
        {isSubmitting ? 'Сохранение…' : isEditing ? 'Сохранить изменения' : 'Опубликовать'}
      </button>
    </form>
  )
}
