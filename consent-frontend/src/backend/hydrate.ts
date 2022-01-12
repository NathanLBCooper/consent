import { Storage } from "./storage";

export function hydrate(storage: Storage) {
    storage.set("permissions", [
        {
            id: 1,
            key: "terms_and_conditions",
            description: "The basic permission required to use the platform. Note: Legacy. Only sufficient to use the core products."
        },
        {
            id: 2,
            key: "share_data",
            description: "Can share the participant's data. Note: Okay to use until 2022, and we must have moved to partner specific permissions."
        },
        {
            id: 3,
            key: "share_data_initech",
            description: "Can share the participant's data with initech"
        },
        {
            id: 4,
            key: "share_data_initrode",
            description: "Can share the participant's data with initrode"
        },
        {
            id: 5,
            key: "terms_and_conditions_v2",
            description: "The basic permission required to use the platform."

        },
        {
            id: 6,
            key: "record_meeting",
            description: "Can record a remote meeting"
        }
    ]);
    storage.set("contract-summaries", [
        {
            id: 1,
            name: "User contract",
            description: "General user agreement",
            numberOfDraftVersions: 1,
            numberOfActiveVersions: 2,
            numberOfDeprecatedVersions: 1,
            numberOfDeactivatedVersions: 0
        },
        {
            id: 2,
            name: "Share Data Contract",
            description: "Agreement to sharing data",
            numberOfDraftVersions: 1,
            numberOfActiveVersions: 1,
            numberOfDeprecatedVersions: 0,
            numberOfDeactivatedVersions: 0
        },
        {
            id: 3,
            name: "Record Remote Meetings",
            description: "Agreement to record Zoom/team/etc calls",
            numberOfDraftVersions: 1,
            numberOfActiveVersions: 1,
            numberOfDeprecatedVersions: 0,
            numberOfDeactivatedVersions: 0
        }
    ]);
    storage.set("contracts", []);
    storage.set("participants", [
        { id: 1, key: "one@example.com", participantGroups: [{ id: 1, key: "in_person" }], language: "en" },
        { id: 2, key: "two@example.com", participantGroups: [{ id: 1, key: "in_person" }], language: "en" },
        { id: 3, key: "three@example.com", participantGroups: [{ id: 1, key: "in_person" }], language: "en" },
        { id: 4, key: "four@example.com", participantGroups: [{ id: 1, key: "in_person" }, {id: 3, key: "AB_test_group_B"}], language: "en" },
        { id: 5, key: "five@example.com", participantGroups: [{ id: 1, key: "in_person" }, {id: 3, key: "AB_test_group_B"}], language: "en" },
        { id: 6, key: "six@example.com", participantGroups: [{ id: 2, key: "remote" }], language: "en" },
        { id: 7, key: "seven@example.com", participantGroups: [{ id: 2, key: "remote" }], language: "en" },
        { id: 8, key: "eight@example.com", participantGroups: [{ id: 2, key: "remote" }], language: "en" },
        { id: 9, key: "nine@example.com", participantGroups: [{ id: 2, key: "remote" }, {id: 3, key: "AB_test_group_B"}], language: "en" },
        { id: 10, key: "ten@example.com", participantGroups: [{ id: 2, key: "remote" }, {id: 3, key: "AB_test_group_B"}], language: "en" }
    ])
}
