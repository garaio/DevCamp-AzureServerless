using Garaio.DevCampServerless.Common.Model;
using System.Collections.Generic;

namespace Garaio.DevCampServerless.EmulatorFuncApp.Emulation
{
    public class EmulationContext
    {
        public List<EntityBase> Entities { get; } = new List<EntityBase>();
    }
}
