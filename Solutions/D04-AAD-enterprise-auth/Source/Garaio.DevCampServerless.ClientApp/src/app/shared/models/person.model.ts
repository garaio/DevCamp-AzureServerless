import { ProjectExperience } from './project-experience.model';
import { Skill } from './skill.model';
import { PublishState } from './publish-state.model';

export interface Person {
    rowKey?: string;
    firstname?: string;
    lastname?: string;
    jobTitle?: string;
    slogan?: string;
    employedSince?: Date;
    status?: PublishState;
    projects?: ProjectExperience[];
    skills?: Skill[];
}
