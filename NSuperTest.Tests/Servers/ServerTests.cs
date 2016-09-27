using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSuperTest.Tests.Servers
{
    [TestFixture]
    public class ServerTests
    {
        [Test]
        public void ShouldCreateWithUrl()
        {
            var server = new Server("http://www.google.com");

            server.Should().NotBeNull();

            var testBuilder = server.Get("/");

            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeOfType<TestBuilder>();
        }

        [Test]
        public void ShouldCreateWithAStartupClass()
        {
            var server = new Server<Startup>();

            server.Should().NotBeNull();
            var testBuilder = server.Get("/");

            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeOfType<TestBuilder>();
        }

        [Test]
        public void ShouldCreateFromConfig()
        {
            var server = new Server();

            server.Should().NotBeNull();
            var testBuilder = server.Get("/");

            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeOfType<TestBuilder>();
        }
    }
}
