import { Controller } from "./controller";
import { hydrate } from "./hydrate";
import { Storage } from "./storage";

export function configureBackend() {
    const realFetch = window.fetch;
    const storage = new Storage("backend");

    window.fetch = function (input: RequestInfo, init?: RequestInit | undefined): Promise<Response> {
        const url = input.toString();
        const method: string | undefined = init?.method;
        const headers: HeadersInit | undefined = init?.headers;
        const body = init?.body;

        return new Promise((resolve: (value: any) => void, reject: (reason: any) => void) => {
            const latencyMs = 0;
            setTimeout(handleRoute, latencyMs);

            const controller = new Controller(storage, resolve, reject);

            function handleRoute(): void {
                console.log(`monkey patched fetch handling: ${url}`)
                switch (true) {
                    case /\/permissions[/]?$/.test(url):
                        controller.getPermissions();
                        return;
                    case /\/permissions\/(\d+)$/.test(url):
                        controller.putPermission(parseInt(url.match(/\/contracts\/(\d+)$/)![0]), body);
                        return;
                    case /\/contracts[/]?$/.test(url):
                        controller.getContracts();
                        return;
                    case /\/contracts\/(\d+)$/.test(url):
                        controller.getContract(parseInt(url.match(/\/contracts\/(\d+)$/)![0]));
                        return;
                    case /\/participants[/]?$/.test(url):
                        controller.getParticipants();
                        return;
                    default:
                        console.log("Passing through to real fetch");
                        realFetch(input, init)
                            .then(response => resolve(response))
                            .catch(error => reject(error));
                        return;
                }
            }
        });
    };

    hydrate(storage);
}
