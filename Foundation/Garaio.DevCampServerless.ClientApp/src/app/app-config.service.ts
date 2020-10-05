import { Injectable } from '@angular/core';

import { environment } from '../../src/environments/environment';

interface Config {
    api: {
        baseUrl: string;
        authCode: string;
    };
}

@Injectable()
export class AppConfigService {
    private appConfig: Config;

    load() {
        return fetch('assets/config.json')
        .then(res => res.json())
        .then((json) => {
            if (!environment.production) {
                console.log(json);
            }
            this.appConfig = json as Config;
        })
        .catch(err => console.error(err));
    }

    get() {
        return this.appConfig;
    }
}
