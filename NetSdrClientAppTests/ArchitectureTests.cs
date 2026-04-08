using NetArchTest.Rules;
using NetSdrClientApp;
using NUnit.Framework;

namespace NetSdrClientAppTests
{
    [TestFixture]
    public class ArchitectureTests
    {
        [Test]
        public void Messages_ShouldNotDependOn_Networking()
        {
            var result = Types.InAssembly(typeof(NetSdrClient).Assembly)
                .That().ResideInNamespace("NetSdrClientApp.Messages")
                .ShouldNot().HaveDependencyOn("NetSdrClientApp.Networking")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True, "Architectural error: The Messages module should not know about the Networking module!");
        }

        [Test]
        public void Interfaces_ShouldStartWith_I()
        {
            var result = Types.InAssembly(typeof(NetSdrClient).Assembly)
                .That().AreInterfaces()
                .Should().HaveNameStartingWith("I")
                .GetResult();

            Assert.That(result.IsSuccessful, Is.True, "All interfaces must begin with the letter 'I'.");
        }
    }
}
