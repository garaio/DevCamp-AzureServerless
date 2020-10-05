import { ProjectTechnology } from './project-technology.model';
import { PublishState } from './publish-state.model';

export interface Project {
    entityKey?: string;
    customerName?: string;
    projectName?: string;
    description?: string;
    projectUrl?: string;
    iconUrl?: string;
    status?: PublishState;
    usedTechnologies?: ProjectTechnology[];
}
