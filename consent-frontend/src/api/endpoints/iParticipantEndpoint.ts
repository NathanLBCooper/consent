import { Participant } from "../participant";

export interface IParticipantEndpoint {
    get(): Promise<Participant[]>;
}
