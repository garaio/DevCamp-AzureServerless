using System;

namespace Garaio.DevCampServerless.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EntityJsonPropertyConverterAttribute : Attribute
    {
        public EntityJsonPropertyConverterAttribute()
        {
        }
    }
}
