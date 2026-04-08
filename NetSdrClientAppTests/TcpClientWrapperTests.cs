using NUnit.Framework;
using NetSdrClientApp.Networking;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetSdrClientAppTests
{
    [TestFixture]
    public class TcpClientWrapperTests
    {
        [Test]
        public void Connected_InitialState_ShouldBeFalse()
        {
            var wrapper = new TcpClientWrapper("127.0.0.1", 12345);
            Assert.That(wrapper.Connected, Is.False);
        }

        [Test]
        public void Disconnect_WhenNotConnected_ShouldNotThrow()
        {
            var wrapper = new TcpClientWrapper("127.0.0.1", 12345);
            Assert.DoesNotThrow(() => wrapper.Disconnect());
        }

        [Test]
        public void SendMessageAsync_Bytes_WhenNotConnected_ShouldThrowInvalidOperationException()
        {
            var wrapper = new TcpClientWrapper("127.0.0.1", 12345);
            Assert.ThrowsAsync<InvalidOperationException>(async () => await wrapper.SendMessageAsync(new byte[] { 1, 2, 3 }));
        }

        [Test]
        public void SendMessageAsync_String_WhenNotConnected_ShouldThrowInvalidOperationException()
        {
            var wrapper = new TcpClientWrapper("127.0.0.1", 12345);
            Assert.ThrowsAsync<InvalidOperationException>(async () => await wrapper.SendMessageAsync("Hello"));
        }

        [Test]
        public void Connect_WhenServerIsDown_ShouldCatchExceptionAndRemainDisconnected()
        {
            var wrapper = new TcpClientWrapper("127.0.0.1", 55555);
            wrapper.Connect();
            
            Assert.That(wrapper.Connected, Is.False);
        }

        [Test]
        public async Task ConnectAndDisconnect_WithRealLocalServer_ShouldChangeState()
        {
            // Arrange
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;

            var wrapper = new TcpClientWrapper("127.0.0.1", port);

            // Act
            wrapper.Connect();
            
            using var client = await listener.AcceptTcpClientAsync();

            // Assert
            Assert.That(wrapper.Connected, Is.True);

            // Act
            wrapper.Disconnect();

            // Assert
            Assert.That(wrapper.Connected, Is.False);

            listener.Stop();
        }
    }
}
