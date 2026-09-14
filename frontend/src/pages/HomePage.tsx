import { Link } from 'react-router-dom'
import { useAsync } from '../hooks/useAsync'
import { getCatalogs } from '../api/catalog'
import {
  getHitOfTheDay,
  getHitOfTheMonth,
  getHitOfTheYear,
  getHotDiscount,
  getPopularCategory,
  getPopularProduct,
  getRecommendedProduct,
} from '../api/ratingAndTop'
import { useAuth } from '../auth/useAuth'
import { RatingAndTopRow } from '../components/RatingAndTopRow'
import { Spinner } from '../components/Spinner'
import { ErrorState } from '../components/ErrorState'
import './HomePage.css'

export function HomePage() {
  const { isAuthenticated } = useAuth()
  const { data: catalogs, error, isLoading, reload } = useAsync(() => getCatalogs().then((r) => r.data), [])

  return (
    <div>
      <section className="home-hero">
        <h1>MaxShop</h1>
        <p>Техника, транспорт и недвижимость — в одном маркетплейсе.</p>
      </section>

      <section>
        <h2>Каталог</h2>
        {isLoading && <Spinner label="Загружаем каталоги…" />}
        {error && <ErrorState message={error} onRetry={reload} />}
        {catalogs && (
          <div className="catalog-grid">
            {catalogs.map((c) => (
              <Link key={c.catalogId} to={`/catalog/${c.catalogId}`} className="catalog-card">
                {c.catalogName}
              </Link>
            ))}
          </div>
        )}
      </section>

      {isAuthenticated && <RatingAndTopRow title="Рекомендуем вам" load={getRecommendedProduct} />}
      <RatingAndTopRow title="Горячие скидки" load={getHotDiscount} />
      <RatingAndTopRow title="Популярные категории" load={getPopularCategory} />
      <RatingAndTopRow title="Популярные товары" load={getPopularProduct} />
      <RatingAndTopRow title="Хит дня" load={getHitOfTheDay} />
      <RatingAndTopRow title="Хит месяца" load={getHitOfTheMonth} />
      <RatingAndTopRow title="Хит года" load={getHitOfTheYear} />
    </div>
  )
}
