export interface TechnologyLink {
    entityKey?: string;
    fromTechnologyKey?: string;
    toTechnologyKey?: string;
    type?: LinkType;
}

export enum LinkType {
    Undefined = 0,
    IsSimilar = 1,
    IsPartOf = 2,
    BasesOn = 3,
    Uses = 4,
    IsSuccessorOf = 5
}
