import { useState } from 'react'
import type { FormEvent } from 'react'
import { useAsync } from '../hooks/useAsync'
import {
  addCatalog,
  addCategory,
  addSubCategory,
  deleteCatalog,
  deleteCategory,
  deleteSubCategory,
  getCategories,
  getSubCategories,
  getCatalogs,
  updateCatalog,
  updateCategory,
  updateSubCategory,
  type Catalog,
  type Category,
  type SubCategory,
} from '../api/catalog'
import { ApiError } from '../api/client'
import { ProductType, PRODUCT_TYPE_LABELS, type ProductTypeValue } from '../api/types'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

function CatalogsSection({ catalogs, reload }: { catalogs: Catalog[]; reload: () => void }) {
  const [name, setName] = useState('')
  const [editingId, setEditingId] = useState<number | null>(null)
  const [error, setError] = useState<string | null>(null)

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    try {
      if (editingId !== null) await updateCatalog(editingId, name)
      else await addCatalog(name)
      setName('')
      setEditingId(null)
      reload()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось сохранить')
    }
  }

  async function handleDelete(c: Catalog) {
    if (!confirm(`Удалить каталог «${c.catalogName}»? Это может затронуть вложенные категории.`)) return
    try {
      await deleteCatalog(c.catalogId)
      reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить')
    }
  }

  return (
    <section>
      <h2>Каталоги</h2>
      <form className="form inline-form" onSubmit={handleSubmit}>
        {error && <div className="form-error">{error}</div>}
        <div className="form-field">
          <label htmlFor="catalogName">Название</label>
          <input id="catalogName" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>
        <div className="inline-form__actions">
          <button type="submit" className="btn btn-primary">
            {editingId !== null ? 'Сохранить' : 'Добавить'}
          </button>
          {editingId !== null && (
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => {
                setEditingId(null)
                setName('')
              }}
            >
              Отмена
            </button>
          )}
        </div>
      </form>
      <ul className="item-list">
        {catalogs.map((c) => (
          <li key={c.catalogId} className="item-list__item">
            <span>{c.catalogName}</span>
            <span className="item-list__actions">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={() => {
                  setEditingId(c.catalogId)
                  setName(c.catalogName)
                }}
              >
                Изменить
              </button>
              <button type="button" className="btn btn-danger" onClick={() => handleDelete(c)}>
                Удалить
              </button>
            </span>
          </li>
        ))}
      </ul>
    </section>
  )
}

function CategoriesSection({ catalogs, categories, reload }: { catalogs: Catalog[]; categories: Category[]; reload: () => void }) {
  const [name, setName] = useState('')
  const [catalogId, setCatalogId] = useState(String(catalogs[0]?.catalogId ?? ''))
  const [editingId, setEditingId] = useState<number | null>(null)
  const [error, setError] = useState<string | null>(null)
  const catalogById = new Map(catalogs.map((c) => [c.catalogId, c]))

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    try {
      if (editingId !== null) await updateCategory(editingId, name, Number(catalogId))
      else await addCategory(name, Number(catalogId))
      setName('')
      setEditingId(null)
      reload()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось сохранить')
    }
  }

  async function handleDelete(c: Category) {
    if (!confirm(`Удалить категорию «${c.categoryName}»?`)) return
    try {
      await deleteCategory(c.categoryId)
      reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить')
    }
  }

  return (
    <section>
      <h2>Категории</h2>
      <form className="form inline-form" onSubmit={handleSubmit}>
        {error && <div className="form-error">{error}</div>}
        <div className="form-field">
          <label htmlFor="categoryName">Название</label>
          <input id="categoryName" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>
        <div className="form-field">
          <label htmlFor="categoryCatalogId">Каталог</label>
          <select id="categoryCatalogId" value={catalogId} onChange={(e) => setCatalogId(e.target.value)} required>
            {catalogs.map((c) => (
              <option key={c.catalogId} value={c.catalogId}>
                {c.catalogName}
              </option>
            ))}
          </select>
        </div>
        <div className="inline-form__actions">
          <button type="submit" className="btn btn-primary">
            {editingId !== null ? 'Сохранить' : 'Добавить'}
          </button>
          {editingId !== null && (
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => {
                setEditingId(null)
                setName('')
              }}
            >
              Отмена
            </button>
          )}
        </div>
      </form>
      <ul className="item-list">
        {categories.map((c) => (
          <li key={c.categoryId} className="item-list__item">
            <span>{c.categoryName}</span>
            <span className="item-list__meta">{catalogById.get(c.catalogId)?.catalogName ?? `Каталог №${c.catalogId}`}</span>
            <span className="item-list__actions">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={() => {
                  setEditingId(c.categoryId)
                  setName(c.categoryName)
                  setCatalogId(String(c.catalogId))
                }}
              >
                Изменить
              </button>
              <button type="button" className="btn btn-danger" onClick={() => handleDelete(c)}>
                Удалить
              </button>
            </span>
          </li>
        ))}
      </ul>
    </section>
  )
}

function SubCategoriesSection({ categories, subCategories, reload }: { categories: Category[]; subCategories: SubCategory[]; reload: () => void }) {
  const [name, setName] = useState('')
  const [categoryId, setCategoryId] = useState(String(categories[0]?.categoryId ?? ''))
  const [productTypeValue, setProductTypeValue] = useState('')
  const [editingId, setEditingId] = useState<number | null>(null)
  const [error, setError] = useState<string | null>(null)
  const categoryById = new Map(categories.map((c) => [c.categoryId, c]))

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    const productType = productTypeValue ? (Number(productTypeValue) as ProductTypeValue) : null
    try {
      if (editingId !== null) await updateSubCategory(editingId, name, Number(categoryId), productType)
      else await addSubCategory(name, Number(categoryId), productType)
      setName('')
      setProductTypeValue('')
      setEditingId(null)
      reload()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось сохранить')
    }
  }

  async function handleDelete(s: SubCategory) {
    if (!confirm(`Удалить подкатегорию «${s.subCategoryName}»? Проверьте, не завязаны ли на неё товары.`)) return
    try {
      await deleteSubCategory(s.subCategoryId)
      reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить')
    }
  }

  return (
    <section>
      <h2>Подкатегории</h2>
      <form className="form inline-form" onSubmit={handleSubmit}>
        {error && <div className="form-error">{error}</div>}
        <div className="form-field">
          <label htmlFor="subCategoryName">Название</label>
          <input id="subCategoryName" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>
        <div className="form-field">
          <label htmlFor="subCategoryCategoryId">Категория</label>
          <select id="subCategoryCategoryId" value={categoryId} onChange={(e) => setCategoryId(e.target.value)} required>
            {categories.map((c) => (
              <option key={c.categoryId} value={c.categoryId}>
                {c.categoryName}
              </option>
            ))}
          </select>
        </div>
        <div className="form-field">
          <label htmlFor="subCategoryProductType">Товарный тип</label>
          <select id="subCategoryProductType" value={productTypeValue} onChange={(e) => setProductTypeValue(e.target.value)}>
            <option value="">— не задан —</option>
            {(Object.values(ProductType) as ProductTypeValue[]).map((pt) => (
              <option key={pt} value={pt}>
                {PRODUCT_TYPE_LABELS[pt]}
              </option>
            ))}
          </select>
        </div>
        <div className="inline-form__actions">
          <button type="submit" className="btn btn-primary">
            {editingId !== null ? 'Сохранить' : 'Добавить'}
          </button>
          {editingId !== null && (
            <button
              type="button"
              className="btn btn-secondary"
              onClick={() => {
                setEditingId(null)
                setName('')
                setProductTypeValue('')
              }}
            >
              Отмена
            </button>
          )}
        </div>
      </form>
      <ul className="item-list">
        {subCategories.map((s) => (
          <li key={s.subCategoryId} className="item-list__item">
            <span>{s.subCategoryName}</span>
            <span className="item-list__meta">
              {categoryById.get(s.categoryId)?.categoryName ?? `Категория №${s.categoryId}`}
              {s.productType != null ? ` · ${PRODUCT_TYPE_LABELS[s.productType]}` : ' · тип не задан'}
            </span>
            <span className="item-list__actions">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={() => {
                  setEditingId(s.subCategoryId)
                  setName(s.subCategoryName)
                  setCategoryId(String(s.categoryId))
                  setProductTypeValue(s.productType != null ? String(s.productType) : '')
                }}
              >
                Изменить
              </button>
              <button type="button" className="btn btn-danger" onClick={() => handleDelete(s)}>
                Удалить
              </button>
            </span>
          </li>
        ))}
      </ul>
    </section>
  )
}

export function AdminCatalogPage() {
  const catalogs = useAsync(() => getCatalogs().then((r) => r.data), [])
  const categories = useAsync(() => getCategories().then((r) => r.data), [])
  const subCategories = useAsync(() => getSubCategories().then((r) => r.data), [])

  const isLoading = catalogs.isLoading || categories.isLoading || subCategories.isLoading
  const error = catalogs.error ?? categories.error ?? subCategories.error
  const reloadAll = () => {
    catalogs.reload()
    categories.reload()
    subCategories.reload()
  }

  return (
    <div>
      <h1>Управление каталогом</h1>
      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reloadAll} />}
      {catalogs.data && categories.data && subCategories.data && (
        <>
          <CatalogsSection catalogs={catalogs.data} reload={reloadAll} />
          <CategoriesSection catalogs={catalogs.data} categories={categories.data} reload={reloadAll} />
          <SubCategoriesSection categories={categories.data} subCategories={subCategories.data} reload={reloadAll} />
        </>
      )}
    </div>
  )
}
