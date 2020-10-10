using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Garaio.DevCampServerless.ServiceFuncApp.Functions
{
    public static class UserProfile
    {
        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(Get) + nameof(UserProfile))]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.UserProfile)] HttpRequest req,
            ClaimsPrincipal claimsPrincipal,
            ILogger log)
        {
            await Task.Yield();

            var userName = claimsPrincipal?.Identity?.Name ?? "Not Authenticated";

            return new OkObjectResult(userName);
        }
    }
}
