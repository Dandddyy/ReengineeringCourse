using NUnit.Framework;
using EchoTcpServer;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoTcpServerTests
{
    [TestFixture]
    public class EchoServerTests
    {
        [Test]
        public async Task EchoServer_ShouldEchoDataBack()
        {
            // Arrange
            int testPort = 5055;
            using var server = new EchoServer(IPAddress.Loopback, testPort);
            var serverTask = Task.Run(() => server.StartAsync());
            
            await Task.Delay(100);

            using var client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, testPort);
            using var stream = client.GetStream();

            byte[] dataToSend = Encoding.UTF8.GetBytes("Hello Lab 6");
            byte[] receiveBuffer = new byte[1024];

            // Act
            await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
            int bytesRead = await stream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length);
            string response = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

            // Assert
            Assert.That(response, Is.EqualTo("Hello Lab 6"));

            // Cleanup
            server.Stop();
            await serverTask; 
        }
    }
}