export type Page<T> = {
    items: T[],
    page: number,
    pageSize: number,
    totalCount: number,
    totalPages: number,
    hasPrevious: boolean,
    haNext: boolean
}

export type PageRequest = {
    Page: number,
    PageSize: number
}