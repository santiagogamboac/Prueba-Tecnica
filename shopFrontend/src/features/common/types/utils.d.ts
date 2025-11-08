export type Nulleable<T> = {
  [key in keyof T]: T[key] | null
}

type Paths<T> = {
  [P in keyof T]: P extends string ? Paths<T[P]> : P
}[keyof T]

type PathsWithSlash<T> = T extends Record<string, unknown> ? `/${Paths<T>}` : never
