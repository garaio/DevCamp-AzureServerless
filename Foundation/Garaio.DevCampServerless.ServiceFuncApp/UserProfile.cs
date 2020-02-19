using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class UserProfile
    {
        [FunctionName(nameof(Get) + nameof(UserProfile))]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.UserProfile)] HttpRequest req,
            ClaimsPrincipal claimsPrincipal,
            ILogger log)
        {
            var userName = claimsPrincipal?.Identity?.Name ?? "Not Authenticated";

            return new OkObjectResult(userName);
        }
    }
}
