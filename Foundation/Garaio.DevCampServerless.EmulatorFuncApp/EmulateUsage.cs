using System;
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
                       
            var minute = DateTimeOffset.UtcNow.Minute;

            var context = new EmulationContext();
            var steps = Faker.Value.Random.ListItems(EmulationSteps.Value);

            foreach (var step in steps)
            {
                try
                {
                    context = await step(context);
                }
                catch (Exception e)
                {
                    log.LogWarning(e, "Emulator function step failed");
                }

                await Task.Delay(Faker.Value.Random.Int(0, 1000) * minute);
            }
        }
    }
}
