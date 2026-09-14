import { Link, NavLink } from 'react-router-dom'
import { useAuth } from '../auth/useAuth'
import { useCart } from '../cart/useCart'
import { useAsync } from '../hooks/useAsync'
import { getCatalogs } from '../api/catalog'
import './Header.css'

export function Header() {
  const { user, isAuthenticated, logout } = useAuth()
  const { data: catalogs } = useAsync(() => getCatalogs().then((r) => r.data), [])
  const { count } = useCart()

  // Clearing auth while sitting on a protected page (e.g. /cart) makes ProtectedRoute redirect to
  // /login with state.from pointing back at that page - correct for "your session just expired",
  // wrong for a deliberate logout click. Tried an SPA `navigate('/', {replace:true})` first, but
  // it races the data router's async transition against the synchronous auth-state clear:
  // ProtectedRoute can still fire its own redirect-with-state first, and that stale `from` then
  // leaks into a *later* login (even a different person signing in on the same tab), bouncing
  // them to a page they never visited - reproduced live, confirmed the SPA-navigate fix alone
  // didn't close it (see frontend/speca.md). A hard reload sidesteps the race entirely - no SPA
  // transition to interleave with, and it's a rare, deliberate action where the extra reload is a
  // non-issue (also clears any other in-memory state - CartContext etc. - for a clean slate).
  function handleLogout() {
    logout()
    window.location.href = '/'
  }

  return (
    <header className="site-header">
      <div className="container site-header__row">
        <Link to="/" className="site-header__logo">
          MaxShop
        </Link>

        <nav className="site-header__nav" aria-label="Каталоги">
          {catalogs?.map((c) => (
            <NavLink key={c.catalogId} to={`/catalog/${c.catalogId}`} className="site-header__nav-link">
              {c.catalogName}
            </NavLink>
          ))}
        </nav>

        <div className="site-header__auth">
          {isAuthenticated ? (
            <>
              {user!.roles.some((r) => ['Businessman', 'Admin', 'SuperAdmin'].includes(r)) && (
                <NavLink to="/my-listings" className="site-header__nav-link">
                  Мои объявления
                </NavLink>
              )}
              {user!.roles.some((r) => ['Admin', 'SuperAdmin'].includes(r)) && (
                <NavLink to="/admin/catalog" className="site-header__nav-link">
                  Каталог (админ)
                </NavLink>
              )}
              {user!.roles.includes('SuperAdmin') && (
                <NavLink to="/admin/roles" className="site-header__nav-link">
                  Роли
                </NavLink>
              )}
              <NavLink to="/cart" className="site-header__nav-link">
                Корзина{count > 0 && <span className="site-header__badge">{count}</span>}
              </NavLink>
              <NavLink to="/orders" className="site-header__nav-link">
                Заказы
              </NavLink>
              <NavLink to="/profile" className="site-header__nav-link">
                {user!.userName}
              </NavLink>
              <button type="button" className="site-header__logout" onClick={handleLogout}>
                Выйти
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="site-header__nav-link">
                Войти
              </Link>
              <Link to="/register" className="site-header__cta">
                Регистрация
              </Link>
            </>
          )}
        </div>
      </div>
    </header>
  )
}
