import { Context, createContext } from 'react';
import { IContractEndpoint } from './api/endpoints/iContractEndpoint';
import { ContractEndpoint } from './api/endpoints/contractEndpoint';
import { IPermissionEndpoint } from './api/endpoints/iPermissionEndpoint';
import { PermissionEndpoint } from './api/endpoints/permissionEndpoint';
import { AppSettings } from './appSettings';
import { IParticipantEndpoint } from './api/endpoints/iParticipantEndpoint';
import { ParticipantEndpoint } from './api/endpoints/participantEndpoint';

export const DIContext = createContext<Container | undefined>(undefined) as Context<Container>;

export interface Container {
    permissionEndpoint: IPermissionEndpoint;
    contractEndpoint: IContractEndpoint;
    participantEndpoint: IParticipantEndpoint;
}

export function ConfigureDependencies(appSettings: AppSettings): Container {
    const container: Container = {
        permissionEndpoint: new PermissionEndpoint(appSettings.baseUrl),
        contractEndpoint: new ContractEndpoint(appSettings.baseUrl),
        participantEndpoint: new ParticipantEndpoint(appSettings.baseUrl)
    };

    return container;
}
