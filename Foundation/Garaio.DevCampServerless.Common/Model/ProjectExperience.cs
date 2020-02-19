namespace Garaio.DevCampServerless.Common.Model
{
    public class ProjectExperience : EntityBase
    {
        public string PersonKey { get; set; }

        public string ProjectKey { get; set; }

        public string RoleInProject { get; set; }

        public string Description { get; set; }

        [EntityJsonPropertyConverter]
        public PublishState Status { get; set; }
    }
}
