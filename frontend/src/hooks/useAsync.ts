import { useEffect, useRef, useState } from 'react'
import { ApiError } from '../api/client'

interface AsyncState<T> {
  data: T | null
  error: string | null
  isLoading: boolean
}

/**
 * Minimal query-style hook: runs `fn` whenever `deps` change, tracks loading/error/data, and
 * ignores results from a request that's since been superseded by a newer one (stale-response
 * guard) - the small footprint this project needs instead of pulling in react-query (see
 * speca.md's open questions - revisit if this starts hurting).
 */
export function useAsync<T>(fn: () => Promise<T>, deps: unknown[]): AsyncState<T> & { reload: () => void } {
  const [state, setState] = useState<AsyncState<T>>({ data: null, error: null, isLoading: true })
  const requestId = useRef(0)
  const [reloadToken, setReloadToken] = useState(0)

  useEffect(() => {
    const id = ++requestId.current
    setState((s) => ({ ...s, isLoading: true, error: null }))

    fn()
      .then((data) => {
        if (requestId.current === id) setState({ data, error: null, isLoading: false })
      })
      .catch((err: unknown) => {
        if (requestId.current !== id) return
        const message = err instanceof ApiError ? err.message : 'Не удалось загрузить данные'
        setState({ data: null, error: message, isLoading: false })
      })
    // oxlint-disable-next-line react/exhaustive-deps -- deps is an intentionally caller-supplied array
  }, [...deps, reloadToken])

  return { ...state, reload: () => setReloadToken((t) => t + 1) }
}
