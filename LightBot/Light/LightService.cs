using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LightBot
{
    public class LightServiceConfig {
        public Dictionary<string, string>? addresses { get; set; }
    }

    public class LightService
    {
        private readonly IReadOnlyDictionary<string, LightClient> clients;
        private readonly ILogger logger;

        public LightService(IOptions<LightServiceConfig> config, ILogger<LightService> logger)
        {
            this.logger = logger;
            this.clients = buildClients(config.Value);
        }

        private IReadOnlyDictionary<string, LightClient> buildClients(LightServiceConfig config)
        {
            return config.addresses
                .Select(kp => new KeyValuePair<string, LightClient?>(kp.Key, buildClient(kp.Value)))
                .Where(kp => kp.Value != null)
                .ToDictionary(kp => kp.Key, kp => kp.Value!);
        }

        private LightClient? buildClient(string address)
        {
            try
            {
                return new LightClient(IPAddress.Parse(address));
            }
            catch (FormatException e)
            {
                logger.LogWarning(e, "Failed to create LightClient for {Address}", address);
            };
            return null;
        }

        public async Task<string> RunDebugCommand(string location, string command)
        {
            return await new DebugCommand(command).Run(clients[location]);
        }
    }
}
