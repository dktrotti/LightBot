using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LightBot
{
    public class LightService
    {
        private readonly IConfiguration configuration;

        public LightService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


    }
}
