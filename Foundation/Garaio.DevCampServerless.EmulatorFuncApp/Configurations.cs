﻿using System;

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public static class Configurations
    {
        public static string StorageConnectionString => Environment.GetEnvironmentVariable(Constants.Configurations.StorageConnectionString);
        public static string ServiceFuncUrl => Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncUrl);
        public static string ServiceFuncKey => Environment.GetEnvironmentVariable(Constants.Configurations.ServiceFuncKey);
    }
}
