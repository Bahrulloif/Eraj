import { useEffect, useMemo } from 'react'
import './ImageUploader.css'

interface ImageUploaderProps {
  files: File[]
  onChange: (files: File[]) => void
  /** Shown alongside new picks when editing - existing pictures already on the listing. */
  existingImageUrls?: string[]
}

// Mirrors the backend's own allow-list (FileService.IsAllowedImage) purely so a bad pick gets
// caught before an upload round-trip - the server re-checks by magic bytes regardless (see
// ../../Eraj/CLAUDE.md, commit c2cb888), this is not the security boundary.
const ACCEPTED = 'image/png,image/jpeg,image/gif,image/webp'

export function ImageUploader({ files, onChange, existingImageUrls }: ImageUploaderProps) {
  // Derived during render, not effect+setState - createObjectURL is synchronous and pure enough
  // for useMemo. The effect below only handles the one genuinely external-system concern: telling
  // the browser to release a blob URL once it's no longer the current one (or on unmount).
  const previews = useMemo(() => files.map((f) => URL.createObjectURL(f)), [files])

  useEffect(() => {
    return () => previews.forEach((u) => URL.revokeObjectURL(u))
  }, [previews])

  function handlePick(e: React.ChangeEvent<HTMLInputElement>) {
    const picked = Array.from(e.target.files ?? [])
    onChange([...files, ...picked])
    e.target.value = ''
  }

  function removeAt(index: number) {
    onChange(files.filter((_, i) => i !== index))
  }

  return (
    <div className="image-uploader">
      {existingImageUrls && existingImageUrls.length > 0 && (
        <div className="image-uploader__row">
          {existingImageUrls.map((url) => (
            <div key={url} className="image-uploader__thumb image-uploader__thumb--existing">
              <img src={url} alt="" />
            </div>
          ))}
        </div>
      )}
      {existingImageUrls && existingImageUrls.length > 0 && files.length === 0 && (
        <p className="image-uploader__hint">Текущие фото. Выберите новые, чтобы заменить их — иначе останутся эти.</p>
      )}

      {previews.length > 0 && (
        <div className="image-uploader__row">
          {previews.map((url, i) => (
            <div key={url} className="image-uploader__thumb">
              <img src={url} alt="" />
              <button type="button" onClick={() => removeAt(i)} aria-label="Убрать">
                ×
              </button>
            </div>
          ))}
        </div>
      )}

      <label className="btn btn-secondary image-uploader__pick">
        Добавить фото
        <input type="file" accept={ACCEPTED} multiple onChange={handlePick} className="visually-hidden" />
      </label>
    </div>
  )
}
