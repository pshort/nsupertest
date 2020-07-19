using System;
using FluentAssertions;
using NSuperTest.Server;
using Xunit;

namespace NSuperTest.Tests.Servers
{
    public class ServerFactoryTests
    {
       [Fact]
       public void ShouldNotCreateAnythingWithNoRegistration()
       {
            IServerFactory factory = new ServerFactory();
            Action act = () => factory.Build("test");
            act.Should()
                .Throw<ServerCreationException>()
                .WithMessage("There is no registered server with name 'test'");
       } 
    }
}