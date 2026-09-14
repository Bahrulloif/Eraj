import { useState } from 'react'
import type { FormEvent } from 'react'
import { useAsync } from '../hooks/useAsync'
import { getAllProfiles } from '../api/profile'
import { addRoleToUser, deleteRoleFromUser, getRoles } from '../api/role'
import { ApiError } from '../api/client'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'

export function AdminRolesPage() {
  const profiles = useAsync(() => getAllProfiles().then((r) => r.data), [])
  const roles = useAsync(() => getRoles().then((r) => r.data), [])

  const [userId, setUserId] = useState('')
  const [roleId, setRoleId] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const isLoading = profiles.isLoading || roles.isLoading
  const loadError = profiles.error ?? roles.error

  async function handleGrant(e: FormEvent) {
    e.preventDefault()
    if (!userId || !roleId) return
    setError(null)
    setIsSubmitting(true)
    try {
      await addRoleToUser(userId, roleId)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось назначить роль')
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleRevoke(targetUserId: string, targetRoleId: string) {
    setError(null)
    try {
      await deleteRoleFromUser(targetUserId, targetRoleId)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось снять роль')
    }
  }

  return (
    <div>
      <h1>Роли пользователей</h1>
      <p className="hint-text">
        Список ролей на пользователя бэкенд не отдаёт напрямую — ниже просто форма выдачи/снятия по id пользователя
        и роли (тот же вызов, что делал бы <code>SuperAdmin</code> вручную через Swagger).
      </p>

      {isLoading && <Spinner />}
      {loadError && <ErrorState message={loadError} onRetry={() => { profiles.reload(); roles.reload() }} />}

      {profiles.data && roles.data && (
        <>
          <form className="form inline-form" onSubmit={handleGrant}>
            {error && <div className="form-error">{error}</div>}
            <div className="form-field">
              <label htmlFor="userId">Пользователь</label>
              <select id="userId" value={userId} onChange={(e) => setUserId(e.target.value)} required>
                <option value="" disabled>
                  Выберите пользователя
                </option>
                {profiles.data.map((p) => (
                  <option key={p.id} value={p.id}>
                    {p.name} {p.surname} ({p.telephoneNumber})
                  </option>
                ))}
              </select>
            </div>
            <div className="form-field">
              <label htmlFor="roleId">Роль</label>
              <select id="roleId" value={roleId} onChange={(e) => setRoleId(e.target.value)} required>
                <option value="" disabled>
                  Выберите роль
                </option>
                {roles.data.map((r) => (
                  <option key={r.roleId} value={r.roleId}>
                    {r.roleName}
                  </option>
                ))}
              </select>
            </div>
            <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
              Выдать роль
            </button>
          </form>

          <h2>Снять роль</h2>
          <p className="hint-text">Тот же выбор пользователя/роли выше, действие — снять вместо выдать.</p>
          <button
            type="button"
            className="btn btn-danger"
            disabled={!userId || !roleId}
            onClick={() => handleRevoke(userId, roleId)}
          >
            Снять выбранную роль у выбранного пользователя
          </button>
        </>
      )}
    </div>
  )
}
