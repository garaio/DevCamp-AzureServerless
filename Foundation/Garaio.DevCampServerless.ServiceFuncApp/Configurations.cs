using System;
using System.Collections.Generic;
using System.Text;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class Configurations
    {
        public static string StorageConnectionString => Environment.GetEnvironmentVariable(Constants.Configurations.StorageConnectionString);
    }
}
