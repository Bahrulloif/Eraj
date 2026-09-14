import { useState } from 'react'
import type { FormEvent } from 'react'
import { useAsync } from '../hooks/useAsync'
import { addAddress, deleteAddress, getAddresses, updateAddress, type Address, type AddressPayload } from '../api/address'
import { ApiError } from '../api/client'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

const emptyForm: AddressPayload = { country: '', city: '', street: '' }

export function AddressesPage() {
  const { data: addresses, error, isLoading, reload } = useAsync(() => getAddresses({ pageSize: 100 }).then((r) => r.data), [])
  const [editing, setEditing] = useState<Address | null>(null)
  const [form, setForm] = useState<AddressPayload>(emptyForm)
  const [formError, setFormError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isFormOpen, setIsFormOpen] = useState(false)

  function startAdd() {
    setEditing(null)
    setForm(emptyForm)
    setFormError(null)
    setIsFormOpen(true)
  }

  function startEdit(address: Address) {
    setEditing(address)
    setForm({ country: address.country, city: address.city, street: address.street })
    setFormError(null)
    setIsFormOpen(true)
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setFormError(null)
    setIsSubmitting(true)
    try {
      if (editing) await updateAddress(editing.id, form)
      else await addAddress(form)
      setIsFormOpen(false)
      reload()
    } catch (err) {
      setFormError(err instanceof ApiError ? err.message : 'Не удалось сохранить адрес')
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete(address: Address) {
    if (!confirm(`Удалить адрес «${address.city}, ${address.street}»?`)) return
    try {
      await deleteAddress(address.id)
      reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить адрес')
    }
  }

  return (
    <div>
      <div className="page-header">
        <h1>Мои адреса</h1>
        <button type="button" className="btn btn-primary" onClick={startAdd}>
          Добавить адрес
        </button>
      </div>

      {isFormOpen && (
        <form className="form inline-form" onSubmit={handleSubmit}>
          {formError && <div className="form-error">{formError}</div>}
          <div className="form-field">
            <label htmlFor="country">Страна</label>
            <input id="country" value={form.country} onChange={(e) => setForm((f) => ({ ...f, country: e.target.value }))} required />
          </div>
          <div className="form-field">
            <label htmlFor="city">Город</label>
            <input id="city" value={form.city} onChange={(e) => setForm((f) => ({ ...f, city: e.target.value }))} required />
          </div>
          <div className="form-field">
            <label htmlFor="street">Улица, дом</label>
            <input id="street" value={form.street} onChange={(e) => setForm((f) => ({ ...f, street: e.target.value }))} required />
          </div>
          <div className="inline-form__actions">
            <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
              {editing ? 'Сохранить' : 'Добавить'}
            </button>
            <button type="button" className="btn btn-secondary" onClick={() => setIsFormOpen(false)}>
              Отмена
            </button>
          </div>
        </form>
      )}

      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reload} />}
      {addresses && addresses.length === 0 && <p>Адресов пока нет.</p>}
      {addresses && addresses.length > 0 && (
        <ul className="item-list">
          {addresses.map((address) => (
            <li key={address.id} className="item-list__item">
              <span>
                {address.country}, {address.city}, {address.street}
              </span>
              <span className="item-list__actions">
                <button type="button" className="btn btn-secondary" onClick={() => startEdit(address)}>
                  Изменить
                </button>
                <button type="button" className="btn btn-danger" onClick={() => handleDelete(address)}>
                  Удалить
                </button>
              </span>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}
