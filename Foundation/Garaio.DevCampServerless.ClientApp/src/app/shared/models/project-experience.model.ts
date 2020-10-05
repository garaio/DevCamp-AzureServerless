import { PublishState } from './publish-state.model';

export interface ProjectExperience {
    entityKey?: string;
    personKey?: string;
    projectKey?: string;
    roleInProject?: string;
    description?: string;
    status?: PublishState;
}
