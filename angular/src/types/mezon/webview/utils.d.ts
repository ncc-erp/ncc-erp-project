export declare function urlSafeDecode(urlencoded: string): string;
export declare function urlParseQueryString(queryString: string): Record<string, string | null>;
export declare function urlParseHashParams(locationHash: string): Record<string, string | null>;
export declare function urlAppendHashParams(url: string, addHash: string): string;
export declare function sessionStorageSet<T>(key: string, value: T): boolean;
export declare function sessionStorageGet(key: string): any;
export declare function setCssProperty(name: string, value: string): void;
