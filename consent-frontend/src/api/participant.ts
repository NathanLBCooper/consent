import { ParticipantGroup } from "./participantGroup";

export interface Participant {
    id: number,
    key: string,
    participantGroups: ParticipantGroup[],
    language: string
}
