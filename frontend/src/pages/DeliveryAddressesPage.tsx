import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getAddresses } from '../api/address'
import { addDeliveryAddress, deleteDeliveryAddress, getDeliveryAddresses } from '../api/deliveryAddress'
import { ApiError } from '../api/client'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

export function DeliveryAddressesPage() {
  const deliveryAddresses = useAsync(() => getDeliveryAddresses({ pageSize: 100 }).then((r) => r.data), [])
  const addresses = useAsync(() => getAddresses({ pageSize: 100 }).then((r) => r.data), [])

  const [selectedAddressId, setSelectedAddressId] = useState('')
  const [formError, setFormError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const isLoading = deliveryAddresses.isLoading || addresses.isLoading
  const error = deliveryAddresses.error ?? addresses.error
  const addressById = new Map((addresses.data ?? []).map((a) => [a.id, a]))
  // An address already used as a delivery address doesn't need to be offered again.
  const usedAddressIds = new Set((deliveryAddresses.data ?? []).map((d) => d.addressId))
  const availableAddresses = (addresses.data ?? []).filter((a) => !usedAddressIds.has(a.id))

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    if (!selectedAddressId) return
    setFormError(null)
    setIsSubmitting(true)
    try {
      await addDeliveryAddress(Number(selectedAddressId))
      setSelectedAddressId('')
      deliveryAddresses.reload()
    } catch (err) {
      setFormError(err instanceof ApiError ? err.message : 'Не удалось добавить адрес доставки')
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete(id: number) {
    if (!confirm('Удалить этот адрес доставки?')) return
    try {
      await deleteDeliveryAddress(id)
      deliveryAddresses.reload()
    } catch (err) {
      alert(err instanceof ApiError ? err.message : 'Не удалось удалить адрес доставки')
    }
  }

  return (
    <div>
      <div className="page-header">
        <h1>Адреса доставки</h1>
      </div>

      {addresses.data && addresses.data.length === 0 && (
        <p>
          Сначала добавьте адрес в <Link to="/addresses">разделе «Мои адреса»</Link>.
        </p>
      )}

      {availableAddresses.length > 0 && (
        <form className="form inline-form" onSubmit={handleSubmit}>
          {formError && <div className="form-error">{formError}</div>}
          <div className="form-field">
            <label htmlFor="addressId">Адрес</label>
            <select id="addressId" value={selectedAddressId} onChange={(e) => setSelectedAddressId(e.target.value)} required>
              <option value="" disabled>
                Выберите адрес
              </option>
              {availableAddresses.map((a) => (
                <option key={a.id} value={a.id}>
                  {a.country}, {a.city}, {a.street}
                </option>
              ))}
            </select>
          </div>
          <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
            Добавить как адрес доставки
          </button>
        </form>
      )}

      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={() => { deliveryAddresses.reload(); addresses.reload() }} />}
      {deliveryAddresses.data && deliveryAddresses.data.length === 0 && <p>Адресов доставки пока нет.</p>}
      {deliveryAddresses.data && deliveryAddresses.data.length > 0 && (
        <ul className="item-list">
          {deliveryAddresses.data.map((d) => {
            const address = addressById.get(d.addressId)
            return (
              <li key={d.id} className="item-list__item">
                <span>{address ? `${address.country}, ${address.city}, ${address.street}` : `Адрес №${d.addressId}`}</span>
                <span className="item-list__actions">
                  <button type="button" className="btn btn-danger" onClick={() => handleDelete(d.id)}>
                    Удалить
                  </button>
                </span>
              </li>
            )
          })}
        </ul>
      )}
    </div>
  )
}
