namespace Garaio.DevCampServerless.Common.Model
{
    public class Skill : EntityBase
    {
        public string PersonKey { get; set; }

        public string TechnologyKey { get; set; }

        [EntityJsonPropertyConverter]
        public SkillLevel Level { get; set; }
    }
}
