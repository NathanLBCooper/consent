export interface ContractSummary {
    id: number;
    name: string;
    description: string;
    numberOfDraftVersions: number,
    numberOfActiveVersions: number,
    numberOfDeprecatedVersions: number,
    numberOfDeactivatedVersions: number
}
