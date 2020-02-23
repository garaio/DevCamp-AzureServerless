using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Garaio.DevCampServerless.Common.Model;

namespace Garaio.DevCampServerless.ServiceFuncApp
{
    public static class PersonEntities
    {
        [FunctionName(nameof(GetAll) + nameof(PersonEntities))]
        public static async Task<IActionResult> GetAll(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.Persons)] HttpRequest req,
            ILogger log)
        {
            var results = await EntityManager.Get<Person>(log).GetAllAsync();

            return new OkObjectResult(JsonConvert.SerializeObject(results, FunctionHelper.SerializerSettings));
        }

        [FunctionName(nameof(Get) + nameof(PersonEntities))]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Constants.Routes.Persons + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            var result = await EntityManager.Get<Person>(log).GetAsync(key);

            if (result == null)
            {
                return new NotFoundResult();
            }

            // Map child entities
            result.Skills = await EntityManager.Get<Skill>(log).GetWhereAsync(s => s.PersonKey == key);
            result.Projects = await EntityManager.Get<ProjectExperience>(log).GetWhereAsync(p => p.PersonKey == key);

            return new OkObjectResult(JsonConvert.SerializeObject(result, FunctionHelper.SerializerSettings));
        }

        [FunctionName(nameof(Create) + nameof(PersonEntities))]
        public static async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Routes.Persons)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<Person>(requestBody);
            if (entity == null)
            {
                return new BadRequestResult();
            }

            entity.RowKey = !string.IsNullOrWhiteSpace(entity.RowKey) ? entity.RowKey : EntityBase.NewRowKey;

            var result = await EntityManager.Get<Person>(log).CreateOrUpdate(entity);

            if (result == null)
            {
                return new UnprocessableEntityResult();
            }

            var key = result.RowKey;

            // Update child entities
            await EntityManager.Get<Skill>(log).Synchronize(s => s.PersonKey == key, entity.Skills.Select(x => { x.PersonKey = key; return x; }).ToArray());
            await EntityManager.Get<ProjectExperience>(log).Synchronize(p => p.PersonKey == key, entity.Projects.Select(x => { x.PersonKey = key; return x; }).ToArray());            

            return new CreatedResult(key, JsonConvert.SerializeObject(result, FunctionHelper.SerializerSettings));
        }

        [FunctionName(nameof(Update) + nameof(PersonEntities))]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = Constants.Routes.Persons + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<Person>(requestBody);
            if (entity == null)
            {
                return new BadRequestResult();
            }

            entity.RowKey = key;

            var result = await EntityManager.Get<Person>(log).CreateOrUpdate(entity);

            if (result == null)
            {
                return new UnprocessableEntityResult();
            }

            // Update child entities
            await EntityManager.Get<Skill>(log).Synchronize(s => s.PersonKey == key, entity.Skills.Select(x => { x.PersonKey = key; return x; }).ToArray());
            await EntityManager.Get<ProjectExperience>(log).Synchronize(p => p.PersonKey == key, entity.Projects.Select(x => { x.PersonKey = key; return x; }).ToArray());

            return new CreatedResult(key, JsonConvert.SerializeObject(result, FunctionHelper.SerializerSettings));
        }

        [FunctionName(nameof(Delete) + nameof(PersonEntities))]
        public static async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = Constants.Routes.Persons + "/{key}")] HttpRequest req,
            string key,
            ILogger log)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return new BadRequestResult();
            }

            var result = await EntityManager.Get<Person>(log).Delete(key);

            // Delete child entities
            if (result)
            {
                await EntityManager.Get<Skill>(log).DeleteWhere(s => s.PersonKey == key);
                await EntityManager.Get<ProjectExperience>(log).DeleteWhere(p => p.PersonKey == key);
            }

            return result ? (StatusCodeResult)new OkResult() : new NotFoundResult();
        }
    }
}
