using System;
using FluentAssertions;
using NSuperTest.Server;
using NSuperTest.Server.Mock;
using Xunit;

namespace NSuperTest.Tests.Servers
{
    public class ServerFactoryTests
    {
        IServerFactory factory;

        public ServerFactoryTests()
        {
            factory = new ServerFactory();
        }

        [Fact]
        public void ShouldNotCreateAnythingWithNoRegistration()
        {
            Action act = () => factory.Build("test");
            act.Should()
                .Throw<ServerCreationException>()
                .WithMessage("There is no registered server with name 'test'");
        }

        [Fact]
        public void ShouldCreateServerWithRegisteredName()
        {
            var opts = new ServerOptions
            {
                Name = "TestServer",
                Instantiation = InstantiationModel.Transitive,
                ServerType = ServerType.ClientProxyServer
            };

            factory.RegisterStrategy(opts);
            IServer server = factory.Build("TestServer");
            server.Name.Should().Be(opts.Name);
        }
    }
}