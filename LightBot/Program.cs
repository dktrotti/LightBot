﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LightBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var json = @"{""smartlife.iot.dimmer"":{""get_default_behavior"":{}}}";

            //var service = new LightService(null);
            //var result = service.GetDefaultBehaviour("10.0.0.90");
            //result.Wait();
            //Console.WriteLine($"Got result: {result.Result}");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
