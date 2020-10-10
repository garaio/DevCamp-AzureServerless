using System;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class Configurations
    {
        public static string StorageConnectionString => Environment.GetEnvironmentVariable(Constants.Configurations.StorageConnectionString);
        public static string EventGridTopicEndpoint => Environment.GetEnvironmentVariable(Constants.Configurations.EventGridTopicEndpoint);
        public static string EventGridTopicKey => Environment.GetEnvironmentVariable(Constants.Configurations.EventGridTopicKey);
    }
}
