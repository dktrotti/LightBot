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
        public LightService(IConfiguration configuration)
        {

        }

        public async Task<string> GetDefaultBehaviour(string ipAddress)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var address = new IPEndPoint(IPAddress.Parse(ipAddress), 9999);

            await Task.Factory.FromAsync(
                (callback, stateObject) => socket.BeginConnect(address, callback, stateObject),
                socket.EndConnect,
                TaskCreationOptions.None);

            var json = @"{""smartlife.iot.dimmer"":{""get_default_behavior"":{}}}";
            var bytesSent = await sendMessage(socket, json);

            return await receiveMessage(socket);
        }

        /// <summary>
        /// Sends a message using the provided socket. Returns the number of bytes sent.
        /// </summary>
        private async Task<int> sendMessage(Socket socket, string message)
        {
            var bytes = TpLinkUtils.Encrypt(message);
            return await Task.Factory.FromAsync(
                (callback, stateObject) => socket.BeginSend(
                    bytes,
                    0,
                    bytes.Length,
                    SocketFlags.None,
                    callback,
                    stateObject),
                socket.EndSend,
                TaskCreationOptions.None);
        }

        private async Task<string> receiveMessage(Socket socket) {
            var bytes = new byte[2048];
            var bytesReceived = await Task.Factory.FromAsync(
                (callback, stateObject) => socket.BeginReceive(
                    bytes,
                    0,
                    2048,
                    SocketFlags.None,
                    callback,
                    stateObject),
                socket.EndReceive,
                TaskCreationOptions.None);
            return TpLinkUtils.Decrypt(bytes);
        }

    }
}
