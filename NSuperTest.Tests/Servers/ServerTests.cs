using FluentAssertions;
using Xunit;
using NSuperTest;
using Microsoft.AspNetCore.Hosting;

namespace NSuperTest.Tests.Servers
{
    public class ServerTests
    {
        [Fact]
        public void ShouldCreateWithUrl()
        {
            var server = new Server("http://www.google.com");

            server.Should().NotBeNull();

            var testBuilder = server.Get("/");

            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeAssignableTo<ITestBuilder>();
        }

        [Fact]
        public void ShouldCreateWithAStartupClass()
        {
            var server = new Server<Startup>();

            server.Should().NotBeNull();
            var testBuilder = server.Get("/");

            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeAssignableTo<ITestBuilder>();
        }

        [Fact]
        public void ShouldCreateFromConfig()
        {
            var server = new Server();

            server.Should().NotBeNull();
            var testBuilder = server.Get("/");

            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeAssignableTo<ITestBuilder>();
        }

        [Fact]
        public void ShouldAllowACustomWebhostBuilder()
        {
            var builder = new WebHostBuilder()
                                .UseKestrel()
                                .UseUrls(new string[] { "http://localhost:3099" })
                                .UseStartup<Startup>();

            var server = new Server<Startup>(builder);
            server.Should().NotBeNull();

            var testBuilder = server.Get("/");
            testBuilder.Should().NotBeNull();
            testBuilder.Should().BeAssignableTo<ITestBuilder>();
        }
    }
}
