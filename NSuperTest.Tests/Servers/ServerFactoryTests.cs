using System;
using System.Collections.Generic;
using FluentAssertions;
using NSuperTest.Registration;
using NSuperTest.Server;
using Xunit;

namespace NSuperTest.Tests.Servers
{
    public class ServerFactoryTests
    {
        [Fact]
        public void ShouldFailIfNoServersRegistered()
        {
            var factory = new ServerFactory(new List<IRegisterServers> { });
            Action a = () => factory.Build("Test");
            a.Should().Throw<ServerNotRegisteredException>()
                .WithMessage("There is no registered server with the name 'Test'. Please make sure you are registering a server with that name in an IRegisterServers implementaition.");
        }
    }
}