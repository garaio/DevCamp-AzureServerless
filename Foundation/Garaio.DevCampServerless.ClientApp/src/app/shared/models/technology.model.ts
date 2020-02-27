import { TechnologyLink } from './technology-link.model';

export interface Technology {
    rowKey?: string;
    type?: TechnologyType;
    name?: string;
    description?: string;
    productUrl?: string;
    iconUrl?: string;
    linkedTechnologies?: TechnologyLink[];
}

export enum TechnologyType {
    Undefined = 0,
    Language = 1,
    Framework = 2,
    Service = 3,
    Platform = 4
}
