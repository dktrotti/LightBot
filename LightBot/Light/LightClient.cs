using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace LightBot
{
    public class LightClient
    {
        private const int MAX_RESPONSE_LENGTH = 2048;

        private readonly IPAddress address;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public LightClient(IPAddress address)
        {
            this.address = address;
        }

        public async Task<string> SendCommand(string json)
        {
            await semaphore.WaitAsync();
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endpoint = new IPEndPoint(address, 9999);

            await Task.Factory.FromAsync(
                (callback, stateObject) => socket.BeginConnect(endpoint, callback, stateObject),
                socket.EndConnect,
                null);

            var bytesSent = await SendMessage(socket, json);
            if (bytesSent != json.Length)
            {
                throw new SocketWriteException($"Tried to write {json.Length} bytes, only wrote {bytesSent} (msg={json})");
            }

            var response = await ReceiveMessage(socket);
            semaphore.Release();
            return response;
        }

        /// <summary>
        /// Sends a message using the provided socket. Returns the number of bytes sent.
        /// </summary>
        private async Task<int> SendMessage(Socket socket, string message)
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
                null);
        }

        private async Task<string> ReceiveMessage(Socket socket)
        {
            var bytes = new byte[MAX_RESPONSE_LENGTH];
            var bytesReceived = await Task.Factory.FromAsync(
                (callback, stateObject) => socket.BeginReceive(
                    bytes,
                    0,
                    MAX_RESPONSE_LENGTH,
                    SocketFlags.None,
                    callback,
                    stateObject),
                socket.EndReceive,
                null);
            return TpLinkUtils.Decrypt(bytes);
        }
    }

    public class SocketWriteException : Exception
    {
        public SocketWriteException() : base() { }
        public SocketWriteException(string message) : base(message) { }
        public SocketWriteException(string message, Exception inner) : base(message, inner) { }
    }
}
