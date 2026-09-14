import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/useAuth'
import { useAsync } from '../hooks/useAsync'
import { getMyProfile, updateProfile, Gender, type Profile, type ProfilePayload } from '../api/profile'
import { ApiError } from '../api/client'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

// dob comes back as a full ISO timestamp ("0001-01-01T00:00:00" if never set) - <input type=date>
// only accepts the date part.
function toDateInputValue(iso: string): string {
  const date = iso.slice(0, 10)
  return date === '0001-01-01' ? '' : date
}

function formFromProfile(profile: Profile | null): ProfilePayload {
  if (!profile) return { name: '', surname: '', email: '', telephoneNumber: '', dob: '', gender: Gender.Male, cardNumber: '' }
  return {
    name: profile.name,
    surname: profile.surname,
    email: profile.email ?? '',
    telephoneNumber: profile.telephoneNumber,
    dob: toDateInputValue(profile.dob),
    gender: profile.gender,
    cardNumber: profile.cardNumber ?? '',
    addressId: profile.addressId,
  }
}

// A separate component (rather than syncing `form` from `profile` via an effect in the parent) so
// its local edit state simply starts from `initial` on mount - no effect, no risk of clobbering
// in-progress edits if the parent ever refetches.
function ProfileForm({ initial, userId, reload }: { initial: Profile | null; userId: string; reload: () => void }) {
  const { user } = useAuth()
  const [form, setForm] = useState<ProfilePayload>(() => formFromProfile(initial))
  const [formError, setFormError] = useState<string | null>(null)
  const [successMessage, setSuccessMessage] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setFormError(null)
    setSuccessMessage(null)
    setIsSubmitting(true)
    try {
      await updateProfile(userId, {
        ...form,
        email: form.email || null,
        cardNumber: form.cardNumber || null,
        dob: form.dob ? new Date(form.dob).toISOString() : new Date(0).toISOString(),
      })
      setSuccessMessage('Профиль сохранён')
      reload()
    } catch (err) {
      setFormError(err instanceof ApiError ? err.message : 'Не удалось сохранить профиль')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <form className="form" onSubmit={handleSubmit}>
      {formError && <div className="form-error">{formError}</div>}
      {successMessage && <div className="form-success">{successMessage}</div>}

      <div className="form-field">
        <label htmlFor="userName">Логин</label>
        <input id="userName" value={user?.userName ?? ''} disabled />
      </div>
      <div className="form-field">
        <label htmlFor="roles">Роли</label>
        <input id="roles" value={user?.roles.join(', ') || '—'} disabled />
      </div>
      <div className="form-field">
        <label htmlFor="name">Имя</label>
        <input id="name" value={form.name} onChange={(e) => setForm((f) => ({ ...f, name: e.target.value }))} required />
      </div>
      <div className="form-field">
        <label htmlFor="surname">Фамилия</label>
        <input id="surname" value={form.surname} onChange={(e) => setForm((f) => ({ ...f, surname: e.target.value }))} required />
      </div>
      <div className="form-field">
        <label htmlFor="telephoneNumber">Телефон</label>
        <input
          id="telephoneNumber"
          value={form.telephoneNumber}
          onChange={(e) => setForm((f) => ({ ...f, telephoneNumber: e.target.value }))}
          required
        />
      </div>
      <div className="form-field">
        <label htmlFor="email">Email</label>
        <input id="email" type="email" value={form.email ?? ''} onChange={(e) => setForm((f) => ({ ...f, email: e.target.value }))} />
      </div>
      <div className="form-field">
        <label htmlFor="dob">Дата рождения</label>
        <input id="dob" type="date" value={form.dob} onChange={(e) => setForm((f) => ({ ...f, dob: e.target.value }))} />
      </div>
      <div className="form-field">
        <label htmlFor="gender">Пол</label>
        <select
          id="gender"
          value={form.gender}
          onChange={(e) => setForm((f) => ({ ...f, gender: Number(e.target.value) as ProfilePayload['gender'] }))}
        >
          <option value={Gender.Male}>Мужской</option>
          <option value={Gender.Female}>Женский</option>
        </select>
      </div>
      <div className="form-field">
        <label htmlFor="cardNumber">Номер карты</label>
        <input id="cardNumber" value={form.cardNumber ?? ''} onChange={(e) => setForm((f) => ({ ...f, cardNumber: e.target.value }))} />
      </div>

      <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
        {isSubmitting ? 'Сохранение…' : 'Сохранить'}
      </button>
    </form>
  )
}

export function ProfilePage() {
  const { user } = useAuth()
  const { data: profile, error, isLoading, reload } = useAsync(() => getMyProfile().then((r) => r.data[0] ?? null), [])

  return (
    <div>
      <div className="page-header">
        <h1>Профиль</h1>
        <nav className="item-list__actions">
          <Link to="/addresses" className="btn btn-secondary">
            Мои адреса
          </Link>
          <Link to="/delivery-addresses" className="btn btn-secondary">
            Адреса доставки
          </Link>
        </nav>
      </div>

      {isLoading && <Spinner />}
      {error && <ErrorState message={error} onRetry={reload} />}
      {!isLoading && !error && user && (
        // key=profile?.id remounts the form (fresh local state) if the loaded profile identity
        // ever changes - not expected in practice (one profile per user), but keeps the "no sync
        // effect" invariant honest if it ever does.
        <ProfileForm key={profile?.id ?? 'new'} initial={profile} userId={user.id} reload={reload} />
      )}
    </div>
  )
}
