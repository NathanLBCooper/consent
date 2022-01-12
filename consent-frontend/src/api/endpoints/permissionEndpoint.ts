import { Permission } from "../permission";
import { IPermissionEndpoint } from "./iPermissionEndpoint";

export class PermissionEndpoint implements IPermissionEndpoint {
    constructor(private baseUrl: string) {
    }

    public async getAll(): Promise<Permission[]> {
        const response: Response = await fetch(new URL(`permissions`, this.baseUrl).href);
        return await response.json() as Permission[];
    }

    public async Put(permission: Permission): Promise<void> {
        const response = await fetch(
            new URL(`permissions/${permission.id}`, this.baseUrl).href,
            {
                headers: { 'Content-Type': 'application/json' },
                method: 'PUT',
                body: JSON.stringify(permission)
            });

        // todo. or will the fetch throw? Why is isn't this needed in the get?
        if (response.ok) {
            return;
        } else {
            const result = await response.json();
            return Promise.reject(result.errors);
        }
    }
}
