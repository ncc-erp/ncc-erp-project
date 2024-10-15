export function getValueByEnum<T>(enumValue: number, enumObject: T): string | null {
    for (const key in enumObject) {
        if (enumObject[key] === enumValue) {
            return `[${key}]`;
        }
    }
    return null;
}
