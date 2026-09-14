import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../auth/useAuth'
import { ApiError } from '../api/client'

const initialForm = { name: '', surname: '', userName: '', password: '', telephoneNumber: '' }

export function RegisterPage() {
  const { register, login } = useAuth()
  const navigate = useNavigate()
  const [form, setForm] = useState(initialForm)
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  function update<K extends keyof typeof form>(key: K, value: string) {
    setForm((f) => ({ ...f, [key]: value }))
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault()
    setError(null)
    setIsSubmitting(true)
    try {
      await register(form)
      // Registration doesn't return a token - log in right after so the user isn't asked to
      // type their password twice in a row.
      await login(form.userName, form.password)
      navigate('/', { replace: true })
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'Не удалось зарегистрироваться')
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div>
      <h1>Регистрация</h1>
      <form className="form" onSubmit={handleSubmit}>
        {error && <div className="form-error">{error}</div>}
        <div className="form-field">
          <label htmlFor="name">Имя</label>
          <input id="name" value={form.name} onChange={(e) => update('name', e.target.value)} required />
        </div>
        <div className="form-field">
          <label htmlFor="surname">Фамилия</label>
          <input id="surname" value={form.surname} onChange={(e) => update('surname', e.target.value)} required />
        </div>
        <div className="form-field">
          <label htmlFor="userName">Логин</label>
          <input id="userName" value={form.userName} onChange={(e) => update('userName', e.target.value)} required autoComplete="username" />
        </div>
        <div className="form-field">
          <label htmlFor="telephoneNumber">Телефон</label>
          <input
            id="telephoneNumber"
            value={form.telephoneNumber}
            onChange={(e) => update('telephoneNumber', e.target.value)}
            required
            placeholder="+992900000000"
          />
        </div>
        <div className="form-field">
          <label htmlFor="password">Пароль</label>
          <input
            id="password"
            type="password"
            value={form.password}
            onChange={(e) => update('password', e.target.value)}
            required
            minLength={4}
            autoComplete="new-password"
          />
        </div>
        <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
          {isSubmitting ? 'Регистрация…' : 'Зарегистрироваться'}
        </button>
      </form>
      <p>
        Уже есть аккаунт? <Link to="/login">Войти</Link>
      </p>
    </div>
  )
}
