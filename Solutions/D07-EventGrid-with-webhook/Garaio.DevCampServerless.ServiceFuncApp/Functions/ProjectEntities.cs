using Garaio.DevCampServerless.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Garaio.DevCampServerless.ServiceFuncApp.Functions
{
    public static class ProjectEntities
    {
        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(GetAll) + nameof(ProjectEntities))]
        public static async Task<IActionResult> GetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.Projects)] HttpRequest req,
            ILogger log)
        {
            var results = await EntityManager.Get<Project>(log).GetAllAsync();

            return new OkObjectResult(FunctionHelper.ToJson(results));
        }

        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(Get) + nameof(ProjectEntities))]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.Projects + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            var result = await EntityManager.Get<Project>(log).GetAsync(key);

            if (result == null)
            {
                return new NotFoundResult();
            }

            // Map child entities
            result.UsedTechnologies = await EntityManager.Get<ProjectTechnology>(log).GetWhereAsync(p => p.ProjectKey == key);

            return new OkObjectResult(FunctionHelper.ToJson(result));
        }

        [FunctionName(nameof(Create) + nameof(ProjectEntities))]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Routes.Projects)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<Project>(requestBody);
            if (entity == null)
            {
                return new BadRequestResult();
            }

            entity.RowKey = !string.IsNullOrWhiteSpace(entity.RowKey) ? entity.RowKey : EntityBase.NewRowKey;

            var result = await EntityManager.Get<Project>(log).CreateOrUpdate(entity);

            if (result == null)
            {
                return new UnprocessableEntityResult();
            }

            var key = result.RowKey;

            // Update child entities
            await EntityManager.Get<ProjectTechnology>(log).Synchronize(p => p.ProjectKey == key, entity.UsedTechnologies.Select(x => { x.ProjectKey = key; return x; }).ToArray());

            return new CreatedResult(key, FunctionHelper.ToJson(result));
        }

        [FunctionName(nameof(Update) + nameof(ProjectEntities))]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Routes.Projects + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<Project>(requestBody);
            if (entity == null)
            {
                return new BadRequestResult();
            }

            entity.RowKey = key;

            var result = await EntityManager.Get<Project>(log).CreateOrUpdate(entity);

            if (result == null)
            {
                return new UnprocessableEntityResult();
            }

            // Update child entities
            await EntityManager.Get<ProjectTechnology>(log).Synchronize(p => p.ProjectKey == key, entity.UsedTechnologies.Select(x => { x.ProjectKey = key; return x; }).ToArray());

            return new CreatedResult(key, FunctionHelper.ToJson(result));
        }

        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(Delete) + nameof(ProjectEntities))]
        public static async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.Routes.Projects + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            var result = await EntityManager.Get<Project>(log).Delete(key);

            // Delete child entities
            if (result)
            {
                await EntityManager.Get<ProjectTechnology>(log).DeleteWhere(p => p.ProjectKey == key);
            }

            return result ? (StatusCodeResult)new OkResult() : new NotFoundResult();
        }
    }
}
