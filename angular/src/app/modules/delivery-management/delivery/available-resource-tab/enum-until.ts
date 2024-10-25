export function getValueByEnum(enumValue: number | string, enumObject: any): string | null {
    for (const key in enumObject) {
        if (enumObject[key] === enumValue) {
            return `[${key}]`;
        }
    }
    return null;
}
