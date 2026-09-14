import './Spinner.css'

export function Spinner({ label = 'Загрузка…' }: { label?: string }) {
  return (
    <div className="spinner" role="status">
      <span className="spinner__circle" aria-hidden="true" />
      <span>{label}</span>
    </div>
  )
}
