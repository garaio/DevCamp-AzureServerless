export interface SearchResult {
    resultKey?: string;
    resultText?: string;
    type?: SearchResultType;
}

export enum SearchResultType {
    Undefined = 0,
    Entity = 1,
    Intend = 2
}
