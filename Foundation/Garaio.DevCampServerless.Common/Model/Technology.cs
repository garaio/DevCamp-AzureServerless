using System.Collections.Generic;

namespace Garaio.DevCampServerless.Common.Model
{
    public class Technology : EntityBase
    {
        [EntityJsonPropertyConverter]
        public TechnologyType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductUrl { get; set; }

        public string IconUrl { get; set; }

        /// <summary>
        /// Note: Manually mapped (not persisted on this entity)
        /// </summary>
        public IList<TechnologyLink> LinkedTechnologies { get; set; } = new List<TechnologyLink>();        
    }
}
