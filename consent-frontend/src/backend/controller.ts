import { Storage } from "./storage";

export class Controller {
    constructor(
        private storage: Storage,
        private resolve: (value: any) => void,
        private reject: (reason: any) => void
    ) { }

    private okResult(obj: any): void {
        this.resolve({
            json: () => Promise.resolve(obj),
        });
    }

    private ok(): void {
        this.resolve({
        });
    }

    private error() {
        this.reject({
        });
    }

    public getPermissions(): void {
        this.okResult(this.storage.get("permissions"));
    }

    public putPermission(id: number, permission: any): void {
        const permissions = this.storage.get("permissions") as any[];
        const index = permissions.findIndex(p => p.id === id);
        permission.id = id;
        permissions[index] = permission;
        this.ok();
    }

    public getContracts(): any {
        this.okResult(this.storage.get("contract-summaries"));
    }

    public getContract(id: number): any {
        this.okResult(undefined);
    }

    public getParticipants(): any {
        this.okResult(this.storage.get("participants"));
    }
}