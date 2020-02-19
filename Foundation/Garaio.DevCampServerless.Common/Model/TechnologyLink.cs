namespace Garaio.DevCampServerless.Common.Model
{
    public class TechnologyLink : EntityBase
    {
        public string FromTechnologyKey { get; set; }

        public string ToTechnologyKey { get; set; }

        [EntityJsonPropertyConverter]
        public LinkType Type { get; set; }
    }
}
