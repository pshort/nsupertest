using FluentAssertions;
using NSuperTest.Registration;
using NSuperTest.Registration.MockServer;
using NSuperTest.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NSuperTest.Tests.Servers
{
    public class ServerRegistrationTests
    {
        [Fact]
        public void ShouldFindRegistry()
        {
            // test the default instance
            var server = ServerFactory.Instance.Build("Test");
            server.Should().NotBeNull().And.BeOfType<MockServer>();
            server.Address.Should().Be("http://www.google.com");
        }

        [Fact]
        public void ShouldDiscoverServices()
        {
            var disco = new RegistrationDiscoverer();
            var registers = disco.FindRegistrations();
            registers.Should().NotBeEmpty().And.HaveCount(1);
            registers.First().Should().BeOfType<ServersSetup>();
        }

        [Fact]
        public void ShouldPreventDoubleRegistration()
        {
            var reg = new ServerRegistry();
            reg.Register("Test", new MockServerBuilder("test"));
            Action act = () => reg.Register("Test", new MockServerBuilder("test2"));
            act.Should().Throw<ServerRegistrationException>()
                .WithMessage("The server 'Test' has already been registered with the server registry. Perhaps you are accidentally including it in more than one IRegisterServers implementation.");
        }
    }
}
