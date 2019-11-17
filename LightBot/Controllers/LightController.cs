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

        [HttpPost]
        //public ActionResult<string> RunDebugCommand()
        public async Task<ActionResult<string>> RunDebugCommand([FromBody] string command)
        {
            return await lightService.RunDebugCommand("dne", UnescapeCommand(command));
        }

        private static string UnescapeCommand(string command)
        {
            return command.Replace(@"\""", @"""");
        }
    }
}
