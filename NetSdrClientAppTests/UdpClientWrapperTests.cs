using NUnit.Framework;
using NetSdrClientApp.Networking;
using System.Threading.Tasks;

namespace NetSdrClientAppTests
{
    [TestFixture]
    public class UdpClientWrapperTests
    {
        [Test]
        public void StopListening_WhenNotStarted_ShouldNotThrow()
        {
            var wrapper = new UdpClientWrapper(0);
            Assert.DoesNotThrow(() => wrapper.StopListening());
        }

        [Test]
        public void Exit_WhenNotStarted_ShouldNotThrow()
        {
            var wrapper = new UdpClientWrapper(0);
            Assert.DoesNotThrow(() => wrapper.Exit());
        }

        [Test]
        public void GetHashCode_ShouldReturnConsistentValueForSameEndpoint()
        {
            // Arrange
            var wrapper1 = new UdpClientWrapper(12348);
            var wrapper2 = new UdpClientWrapper(12348);

            // Act & Assert
            Assert.That(wrapper1.GetHashCode(), Is.EqualTo(wrapper2.GetHashCode()));
        }

        [Test]
        public async Task StartAndStopListening_ShouldHandleCancellationGracefully()
        {
            // Arrange
            var wrapper = new UdpClientWrapper(0);

            // Act
            var listenTask = wrapper.StartListeningAsync();
            
            await Task.Delay(50);
            
            wrapper.StopListening();

            // Assert
            Assert.DoesNotThrowAsync(async () => await listenTask);
        }
    }
}
