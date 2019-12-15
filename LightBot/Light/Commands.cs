using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LightBot
{
    public interface Command<TResult>
    {
        Task<TResult> Run(LightClient client);
    }

    public class DebugCommand : Command<string>
    {
        private readonly string command;

        public DebugCommand(string command)
        {
            this.command = command;
        }

        public async Task<string> Run(LightClient client)
        {
            return await client.SendCommand(command);
        }
    }

    public class SetStateCommand : Command<Unit>
    {
        private readonly bool isOn;

        public SetStateCommand(bool isOn)
        {
            this.isOn = isOn;
        }

        public async Task<Unit> Run(LightClient client)
        {
            string command = JsonConvert.SerializeObject(new
            {
                system = new
                {
                    set_relay_state = new
                    {
                        state = isOn ? 1 : 0
                    }
                }
            });
            await client.SendCommand(command);
            return default;
        }
    }
}