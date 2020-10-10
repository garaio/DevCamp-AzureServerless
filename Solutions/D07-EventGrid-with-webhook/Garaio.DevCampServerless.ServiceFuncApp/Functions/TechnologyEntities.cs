using Garaio.DevCampServerless.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Garaio.DevCampServerless.ServiceFuncApp.Functions
{
    public static class TechnologyEntities
    {
        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(GetAll) + nameof(TechnologyEntities))]
        public static async Task<IActionResult> GetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.Technologies)] HttpRequest req,
            ILogger log)
        {
            var results = await EntityManager.Get<Technology>(log).GetAllAsync();

            return new OkObjectResult(FunctionHelper.ToJson(results));
        }

        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(Get) + nameof(TechnologyEntities))]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.Technologies + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            var result = await EntityManager.Get<Technology>(log).GetAsync(key);

            if (result == null)
            {
                return new NotFoundResult();
            }

            // Map child entities
            result.LinkedTechnologies = await EntityManager.Get<TechnologyLink>(log).GetWhereAsync(t => t.FromTechnologyKey == key || t.ToTechnologyKey == key);

            return new OkObjectResult(FunctionHelper.ToJson(result));
        }

        [FunctionName(nameof(Create) + nameof(TechnologyEntities))]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Routes.Technologies)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<Technology>(requestBody);
            if (entity == null)
            {
                return new BadRequestResult();
            }

            entity.RowKey = !string.IsNullOrWhiteSpace(entity.RowKey) ? entity.RowKey : EntityBase.NewRowKey;

            var result = await EntityManager.Get<Technology>(log).CreateOrUpdate(entity);

            if (result == null)
            {
                return new UnprocessableEntityResult();
            }

            var key = result.RowKey;

            // Update child entities
            // -> Not supported as key reference is not unique

            return new CreatedResult(key, FunctionHelper.ToJson(result));
        }

        [FunctionName(nameof(Update) + nameof(TechnologyEntities))]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Routes.Technologies + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<Technology>(requestBody);
            if (entity == null)
            {
                return new BadRequestResult();
            }

            entity.RowKey = key;

            var result = await EntityManager.Get<Technology>(log).CreateOrUpdate(entity);

            if (result == null)
            {
                return new UnprocessableEntityResult();
            }

            // Update child entities
            await EntityManager.Get<TechnologyLink>(log).Synchronize(t => t.FromTechnologyKey == key || t.ToTechnologyKey == key, entity.LinkedTechnologies);

            return new CreatedResult(key, FunctionHelper.ToJson(result));
        }

        [SuppressMessage("Microsoft.Performance", "IDE0060:ReviewUnusedParameters")]
        [FunctionName(nameof(Delete) + nameof(TechnologyEntities))]
        public static async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.Routes.Technologies + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            var result = await EntityManager.Get<Technology>(log).Delete(key);

            // Delete child entities
            if (result)
            {
                await EntityManager.Get<TechnologyLink>(log).DeleteWhere(t => t.FromTechnologyKey == key || t.ToTechnologyKey == key);
            }

            return result ? (StatusCodeResult)new OkResult() : new NotFoundResult();
        }
    }
}
