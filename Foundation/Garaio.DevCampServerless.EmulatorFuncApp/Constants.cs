namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public static class Constants
    {
        public static class Configurations
        {
            public const string StorageConnectionString = nameof(StorageConnectionString);
            public const string ScheduleExpression = nameof(ScheduleExpression);
            public const string ServiceFuncUrl = nameof(ServiceFuncUrl);
            public const string ServiceFuncKey = nameof(ServiceFuncKey);
        }

        public static class Metrics
        {
        }

        public static class Data
        {
            public const string Directory = "data";
            public const string TechnologiesFile = "Technologies.json";
            public const string TechnologyLinksFile = "TechnologyLinks.json";
        }
    }
}
