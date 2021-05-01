using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Payments.WebApi.Controllers.Mock
{
    [ApiController]
    [Route("[controller]")]
    public class TestTraceController : ControllerBase
    {
        private static ActivitySource ActSource = new ActivitySource("TestSource2");
        

        [HttpGet("[action]")]
        public async Task<IActionResult> FireTrace(string msg)
        {
            using (var activity = ActSource.StartActivity("RootSpan"))
            {
                activity.AddTag("test", true);
                activity.AddEvent(new ActivityEvent("test_event"));
                activity.AddBaggage("msg", msg);

                await Task.Delay(TimeSpan.FromMilliseconds(100));
                activity.AddEvent(new ActivityEvent("test_event_end"));
            }
            return Ok();
        }
    }
}
