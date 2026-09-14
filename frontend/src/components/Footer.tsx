import './Footer.css'

export function Footer() {
  return (
    <footer className="site-footer">
      <div className="container site-footer__row">
        <span>© {new Date().getFullYear()} MaxShop</span>
        <span className="site-footer__muted">Маркетплейс: техника, транспорт, недвижимость</span>
      </div>
    </footer>
  )
}
