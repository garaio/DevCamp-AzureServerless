export interface Skill {
    entityKey?: string;
    personKey?: string;
    technologyKey?: string;
    level?: SkillLevel;
}

export enum SkillLevel {
    None = 0,
    Basic = 1,
    Elementary = 2,
    Intermediate = 3,
    Professional = 4,
    Expert = 5,
}
