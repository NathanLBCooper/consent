import { Storage } from "./storage";

export function hydrate(storage: Storage) {
    storage.set("permissions", [
        {
            id: 1,
            key: "collect_data",
            description: "Collect data on search terms, watched videos, add interactions",
            active: true
        },
        {
            id: 2,
            key: "collect_location_data",
            description: "Collect location data based on IP address and GPS and other device sensor data",
            active: true
        },
        {
            id: 3,
            key: "share_data",
            description: "Share data with our affiliates",
            active: false
        },
        {
            id: 4,
            key: "share_data_initrode",
            description: "Share data with with initrode",
            active: true
        },
        {
            id: 5,
            key: "share_data_initech",
            description: "Share data with with initrode",
            active: true
        }
    ]);
    storage.set("contract-summaries", [
        {
            id: 1,
            name: "All-in-one Contract",
            userGroups: ["all-users"],
            stateCounts: {
                draft: 0,
                active: 0,
                legacy: 0,
                deprecated: 0,
                removed: 1
            }
        },
        {
            id: 2,
            name: "Data protection contract",
            userGroups: ["all-users"],
            stateCounts: {
                draft: 0,
                active: 1,
                legacy: 1,
                deprecated: 2,
                removed: 0
            }
        },
        {
            id: 3,
            name: "Data sharing contract",
            userGroups: ["all-users"],
            stateCounts: {
                draft: 1,
                active: 2,
                legacy: 1,
                deprecated: 0,
                removed: 0
            }
        }
    ]);
    storage.set("contracts", []);
    storage.set("participants", [
        { id: 1, key: "one@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 3, key: "odd" }], language: "en" },
        { id: 2, key: "two@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 2, key: "even" }], language: "en" },
        { id: 3, key: "three@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 3, key: "odd" }], language: "en" },
        { id: 4, key: "four@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 2, key: "even" }], language: "en" },
        { id: 5, key: "five@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 3, key: "odd" }], language: "en" },
        { id: 6, key: "six@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 2, key: "even" }], language: "en" },
        { id: 7, key: "seven@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 3, key: "odd" }], language: "en" },
        { id: 8, key: "eight@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 2, key: "even" }], language: "en" },
        { id: 9, key: "nine@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 3, key: "odd" }], language: "en" },
        { id: 10, key: "ten@example.com", participantGroups: [{ id: 1, key: "all_users" }, { id: 2, key: "even" }], language: "en" }
    ])
}
