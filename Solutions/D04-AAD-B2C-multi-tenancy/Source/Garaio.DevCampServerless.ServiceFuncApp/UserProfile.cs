using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            var claims = claimsPrincipal?.Claims.Select(c => $"{c.Type} = {c.Value}") ?? Enumerable.Empty<string>(); ;
            log.LogInformation(string.Join(", ", claims));

            /* Looks like that:
               iss = https://gadcsb2c.b2clogin.com/2f38e366-3cac-425f-a1c2-0ac4843fe371/v2.0/
               exp = 1585219968
               nbf = 1585216368
               aud = 99a3a4cf-7e11-474e-bc13-26d86a6ea21b
               http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier = 88b85ab0-7d59-434b-96b4-98b438d74a5d
               name = Jonas Schmied
               jobTitle = Consultant
               tfp = B2C_1_signupsignin
               nonce = 6222920c-f76b-4dc1-bc12-f4f63ab63a07
               http://schemas.microsoft.com/identity/claims/scope = access
               azp = fab71150-01f7-4e06-a951-456e520e8833
               ver = 1.0
               iat = 1585216368
               http://schemas.microsoft.com/2017/07/functions/claims/authlevel = Function
               http://schemas.microsoft.com/2017/07/functions/claims/keyid = client
            */

            if (claimsPrincipal?.Identity?.IsAuthenticated != true)
            {
                return new UnauthorizedResult();
            }

            // Read / transform claims
            // https://docs.microsoft.com/en-us/azure/architecture/multitenant-identity/claims

            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Id of user in AAD B2C (Guid)
            var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value ?? claimsPrincipal.FindFirst("name")?.Value; // Display name (AAD-linked users may use a different claim)
            var clientId = claimsPrincipal.FindFirst("azp")?.Value; // App Id which authenticated user an has right to access this API granted by the scope
            var jobTitle = claimsPrincipal.FindFirst("jobTitle")?.Value;

            var aadTenantId = claimsPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value ?? claimsPrincipal.FindFirst("tid")?.Value; // Only for AAD-linked users: Tenant / Directory (Guid)
            var aadUpn = claimsPrincipal.FindFirst(ClaimTypes.Upn)?.Value ?? claimsPrincipal.FindFirst("upn")?.Value; // Only for AAD-linked users: Tenant / Directory (Guid)
            var aadGroupIds = claimsPrincipal.FindAll("groups")?.SelectMany(c => string.IsNullOrEmpty(c.Value) ? Enumerable.Empty<string>() : JsonConvert.DeserializeObject<ICollection<string>>(c.Value)); // Only for AAD-linked users: Ids of AD groups where the user is a member
                        
            return string.IsNullOrEmpty(jobTitle) ? new OkObjectResult(userName) : new OkObjectResult($"{userName}, {jobTitle}"); ;
        }
    }
}
