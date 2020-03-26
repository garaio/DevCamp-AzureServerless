namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class Constants
    {
        public static class Configurations
        {
            public const string StorageConnectionString = nameof(StorageConnectionString);
        }

        public static class Routes
        {
            public const string Search = "search";
            public const string UserProfile = "user";
            public const string Persons = "persons";
            public const string Projects = "projects";
            public const string Technologies = "technologies";
        }

        public static class QueryParams
        {
            public const string SearchQuery = "query";
        }

        public static class Metrics
        {
            public const string EntityDeletedPattern = "{0}Deleted";
            public const string EntityUpdatedPattern = "{0}Updated";
            public const string EntityCreatedPattern = "{0}Created";
        }
    }
}
