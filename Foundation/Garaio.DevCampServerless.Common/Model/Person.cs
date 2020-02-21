using System;
using System.Collections.Generic;

namespace Garaio.DevCampServerless.Common.Model
{
    public class Person : EntityBase
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string JobTitle { get; set; }

        public string Slogan { get; set; }

        public DateTimeOffset EmployedSince { get; set; }

        [EntityJsonPropertyConverter]
        public PublishState Status { get; set; }

        /// <summary>
        /// Note: Manually mapped (not persisted on this entity)
        /// </summary>
        public IList<ProjectExperience> Projects { get; set; } = new List<ProjectExperience>();

        /// <summary>
        /// Note: Manually mapped (not persisted on this entity)
        /// </summary>
        public IList<Skill> Skills { get; set; } = new List<Skill>();
    }
}
