using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Garaio.DevCampServerless.EmulatorFuncApp
{
    public static class EmulateUsage
    {
        [FunctionName(nameof(EmulateUsage))]
        public static async Task Run([TimerTrigger("%" + Constants.Configurations.ScheduleExpression + "%")]TimerInfo timer, ILogger log)
        {
            log.LogInformation($"Emulator function triggered at: {DateTime.Now}");
        }
    }
}
