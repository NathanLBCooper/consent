export class Storage {
    constructor(private prefix: string) { }

    public get(key: string): any {
        const value = localStorage.getItem(`${this.prefix}-${key}`);
        return value != null ? JSON.parse(value) : undefined;
    }

    public set(key: string, values: any) {
        localStorage.setItem(`${this.prefix}-${key}`, JSON.stringify(values));
    }
}
