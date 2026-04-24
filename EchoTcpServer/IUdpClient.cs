using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics.CodeAnalysis;

namespace EchoTcpServer
{
    public interface IUdpClient : IDisposable
    {
        void Send(byte[] dgram, int bytes, IPEndPoint endPoint);
    }
    
    [ExcludeFromCodeCoverage]
    public class StandardUdpClient : IUdpClient
    {
        private readonly UdpClient _client = new UdpClient();
        public void Send(byte[] dgram, int bytes, IPEndPoint endPoint) => _client.Send(dgram, bytes, endPoint);
        public void Dispose() => _client.Dispose();
    }
}