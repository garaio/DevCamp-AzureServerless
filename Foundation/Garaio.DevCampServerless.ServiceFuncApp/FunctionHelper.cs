using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class FunctionHelper
    {
        private static JsonSerializerSettings serializerSettings;
        public static JsonSerializerSettings SerializerSettings => serializerSettings ?? (serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
    }
}
