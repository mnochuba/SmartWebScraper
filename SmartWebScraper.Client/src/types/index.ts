export type SearchResult = {
  [key: string]: string
}

export type History = {
  id: string
  searchPhrase: string
  targetUrl: string
  rankings: SearchResult
  searchDate: string
}

export type Result<T> = {
  code: string
  data: T
  errors: string[]
  isSuccessful: boolean
}