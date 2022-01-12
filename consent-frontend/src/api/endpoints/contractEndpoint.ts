import { Contract } from "../contract";
import { ContractSummary } from "../contractSummary";
import { IContractEndpoint } from "./iContractEndpoint";

export class ContractEndpoint implements IContractEndpoint {
    constructor(private baseUrl: string) {
    }

    public async getSummaries(): Promise<ContractSummary[]> {
        const response: Response = await fetch(new URL(`contracts`, this.baseUrl).href);
        return await response.json() as ContractSummary[];
    }

    public async get(id: number): Promise<Contract> {
        const response: Response = await fetch(new URL(`contracts/${id}`, this.baseUrl).href);
        return await response.json() as Contract;
    }
}
