export interface ContractSummary {
    id: number;
    name: string;
    userGroups: string[];
    stateCounts: {
        draft: number,
        active: number,
        legacy: number,
        deprecated: number,
        removed: number
    }
}
