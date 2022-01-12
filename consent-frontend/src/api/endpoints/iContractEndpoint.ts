import { Contract } from "../contract";
import { ContractSummary } from "../contractSummary";

export interface IContractEndpoint {
    getSummaries(): Promise<ContractSummary[]>;
    get(id: number): Promise<Contract>;
}
