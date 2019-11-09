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
}