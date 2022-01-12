import { Participant } from "../participant";
import { IParticipantEndpoint } from "./iParticipantEndpoint";

export class ParticipantEndpoint implements IParticipantEndpoint {
    constructor(private baseUrl: string) {
    }

    public async get(): Promise<Participant[]> {
        const response: Response = await fetch(new URL(`participants`, this.baseUrl).href);
        return await response.json() as Participant[];
    }
}
