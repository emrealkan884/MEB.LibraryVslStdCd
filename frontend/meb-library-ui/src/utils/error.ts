interface AxiosLikeError {
  response?: {
    data?: unknown
    status?: number
    statusText?: string
  }
  message?: string
}

export function getErrorMessage(error: unknown, fallback = 'Beklenmeyen bir hata oluştu'): string {
  if (typeof error === 'string' && error.trim().length > 0) {
    return error
  }

  if (error && typeof error === 'object') {
    const axiosError = error as AxiosLikeError

    if (axiosError.response) {
      const { data, status, statusText } = axiosError.response

      if (data && typeof data === 'object') {
        if ('title' in data && typeof (data as { title?: unknown }).title === 'string') {
          return (data as { title: string }).title
        }

        if ('message' in data && typeof (data as { message?: unknown }).message === 'string') {
          return (data as { message: string }).message
        }
      }

      const statusLabel = `${status ?? ''} ${statusText ?? ''}`.trim()
      return statusLabel.length > 0 ? `Sunucu hatası (${statusLabel})` : fallback
    }

    if (typeof axiosError.message === 'string' && axiosError.message.trim().length > 0) {
      return axiosError.message
    }
  }

  return fallback
}
