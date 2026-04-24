using Moq;
using NUnit.Framework;
using EchoTcpServer;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EchoTcpServerTests
{
    [TestFixture]
    public class UdpTimedSenderTests
    {
        [Test]
        public void StartSending_WhenAlreadyRunning_ShouldThrowException()
        {
            var mockUdp = new Mock<IUdpClient>();
            using var sender = new UdpTimedSender("127.0.0.1", 60000, mockUdp.Object);

            sender.StartSending(1000);

            Assert.Throws<InvalidOperationException>(() => sender.StartSending(1000));
        }

        [Test]
        public async Task StartSending_ShouldSendMessagesPeriodically()
        {
            // Arrange
            var mockUdp = new Mock<IUdpClient>();
            int callCount = 0;
            mockUdp.Setup(u => u.Send(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<IPEndPoint>()))
                .Callback(() => callCount++);

            using var sender = new UdpTimedSender("127.0.0.1", 60000, mockUdp.Object);

            // Act
            sender.StartSending(50);
            await Task.Delay(150);
            sender.StopSending();

            // Assert
            Assert.That(callCount, Is.GreaterThanOrEqualTo(2), "The timer should have triggered the sending at least 2 times.");
        }
    }
}