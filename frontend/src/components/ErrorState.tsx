import './ErrorState.css'

export function ErrorState({ message, onRetry }: { message: string; onRetry?: () => void }) {
  return (
    <div className="error-state" role="alert">
      <p>{message}</p>
      {onRetry && (
        <button type="button" onClick={onRetry} className="error-state__retry">
          Повторить
        </button>
      )}
    </div>
  )
}
