import './Pagination.css'

interface PaginationProps {
  pageNumber: number
  totalPage: number
  onChange: (page: number) => void
}

export function Pagination({ pageNumber, totalPage, onChange }: PaginationProps) {
  if (totalPage <= 1) return null

  return (
    <nav className="pagination" aria-label="Страницы">
      <button type="button" disabled={pageNumber <= 1} onClick={() => onChange(pageNumber - 1)}>
        Назад
      </button>
      <span className="pagination__status">
        {pageNumber} / {totalPage}
      </span>
      <button type="button" disabled={pageNumber >= totalPage} onClick={() => onChange(pageNumber + 1)}>
        Вперёд
      </button>
    </nav>
  )
}
