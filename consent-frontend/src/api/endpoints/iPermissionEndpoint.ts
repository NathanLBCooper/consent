import { Permission } from "../permission";

export interface IPermissionEndpoint {
    getAll(): Promise<Permission[]>;
}
