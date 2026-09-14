import { Outlet } from 'react-router-dom'
import { Header } from './Header'
import { Footer } from './Footer'

export function Layout() {
  return (
    <div className="page-shell">
      <Header />
      <main className="page-shell__content container">
        <Outlet />
      </main>
      <Footer />
    </div>
  )
}
