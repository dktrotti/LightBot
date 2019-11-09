using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LightBot
{
    [ApiController]
    [Route("[controller]")]
    public class LightController : ControllerBase
    {
        private readonly LightService lightService;

        public LightController(LightService lightService)
        {
            this.lightService = lightService;
        }

        [HttpPost]
        public ActionResult<string> RunDebugCommand([FromQuery] string command)
        {
            // TODO: Run command
            return "TODO";
        }
    }
}
