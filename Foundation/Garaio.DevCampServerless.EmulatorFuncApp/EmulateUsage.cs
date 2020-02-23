using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public static class EmulateUsage
    {
        public static readonly Lazy<EntityFaker> Faker = new Lazy<EntityFaker>(() => new EntityFaker());
        public static readonly Lazy<EmulationSteps> EmulationSteps = new Lazy<EmulationSteps>(() => new EmulationSteps(Faker));

        [FunctionName(nameof(EmulateUsage))]
        public static async Task Run([TimerTrigger("%" + Constants.Configurations.ScheduleExpression + "%", RunOnStartup = true)]TimerInfo timer, ILogger log)
        {
            log.LogInformation($"Emulator function triggered at: {DateTime.Now}");
            
            var context = new EmulationContext();

            // Generate random flow of steps, weighted and sorted by priority
            var max = EmulationSteps.Value.Max(s => s.Priority) + 1;
            var steps = Faker.Value.Random.ListItems(EmulationSteps.Value.SelectMany(s => Enumerable.Repeat(s, max - s.Priority)).ToList())
                .OrderBy(s => s.Priority)
                .GroupBy(s => s.Id)
                .Select(g => g.First());

            foreach (var step in steps)
            {
                try
                {
                    context = await step.Method(context);
                }
                catch (Exception e)
                {
                    log.LogWarning(e, "Emulator function step failed");
                }

                await Task.Delay(Faker.Value.Random.Int(0, 1000) * DateTimeOffset.UtcNow.Minute);
            }
        }
    }
}
