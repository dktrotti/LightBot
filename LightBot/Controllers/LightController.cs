using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LightBot
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class LightController : ControllerBase
    {
        private readonly LightService lightService;

        public LightController(LightService lightService)
        {
            this.lightService = lightService;
        }

        [HttpPost("{location}/debug")]
        public async Task<ActionResult<string>> RunDebugCommand(string location, [FromBody] string command)
        {
#if DEBUG
            return await lightService.RunDebugCommand(location, UnescapeCommand(command));
#else
            return "Not in debug mode!";
#endif
        }

        private static string UnescapeCommand(string command)
        {
            return command.Replace(@"\""", @"""");
        }

        [HttpPost("{location}/set_state")]
        public async Task<ActionResult<Unit>> SetLightState(string location, [FromBody] bool isOn)
        {
            return await lightService.SetLightState(location, isOn);
        }
    }
}
