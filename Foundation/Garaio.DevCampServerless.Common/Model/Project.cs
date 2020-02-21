using System.Collections.Generic;

namespace Garaio.DevCampServerless.Common.Model
{
    public class Project : EntityBase
    {
        public string CustomerName { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string ProjectUrl { get; set; }

        public string IconUrl { get; set; }

        [EntityJsonPropertyConverter]
        public PublishState Status { get; set; }
        
        /// <summary>
        /// Note: Manually mapped (not persisted on this entity)
        /// </summary>
        public IList<ProjectTechnology> UsedTechnologies { get; set; } = new List<ProjectTechnology>();
    }
}
