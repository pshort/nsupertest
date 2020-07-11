using NSuperTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NSuperTest.Tests.Models;
using Xunit;

namespace NSuperTest.Tests.Assertions
{
    public class AssertionTests
    {
        HttpResponseMessage message;
        ITestBuilder builder;
        Mock<IHttpRequestClient> clientMock;

        User user;

        public AssertionTests()
        {
            message = new HttpResponseMessage();
            message.StatusCode = HttpStatusCode.OK;
            message.Content = new StringContent("Hello World");
            message.Headers.Add("TestHeader", "Test");

            user = new User { Name = "Peter", Age = 32, Id = 1 };

            clientMock = new Mock<IHttpRequestClient>();
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            builder = TestBuilderFactory.Create("/test", clientMock.Object);
            builder.SetMethod(HttpMethod.Get);
        }

        [Fact]
        public void ShouldAssertStatusCodes()
        {
            builder
                .Expect(200)
                .End();
        }

        [Fact]
        public void ShouldAssertEnumStatusCode()
        {
            message.StatusCode = HttpStatusCode.PartialContent;

            builder
                .Expect(HttpStatusCode.PartialContent)
                .End();
        }

        [Fact]
        public void ShouldAssertOk()
        {
            builder
                .ExpectOk()
                .End();
        }

        [Fact]
        public void ShouldAssertCreated()
        {
            message.StatusCode = HttpStatusCode.Created;

            builder
                .ExpectCreated()
                .End();
        }

        [Fact]
        public void ShouldAssertNotFound()
        {
            message.StatusCode = HttpStatusCode.NotFound;

            builder
                .ExpectNotFound()
                .End();
        }

        [Fact]
        public void ShouldAssertBadRequest()
        {
            message.StatusCode = HttpStatusCode.BadRequest;

            builder
                .ExpectBadRequest()
                .End();
        }

        [Fact]
        public void ShouldAssertUnauthorized()
        {
            message.StatusCode = HttpStatusCode.Unauthorized;

            builder
                .ExpectUnauthorized()
                .End();
        }

        [Fact]
        public void ShouldAssertForbidden()
        {
            message.StatusCode = HttpStatusCode.Forbidden;

            builder
                .ExpectForbidden()
                .End();
        }

        [Fact]
        public void ShouldAssertRedirect()
        {
            message.StatusCode = HttpStatusCode.Redirect;

            builder
                .ExpectRedirect()
                .End();
        }

        [Fact]
        public async Task ShouldThrowAssertStatusCodesUnauthorized()
        {
            Func<Task> action = async () => await builder.Expect(401).End();
            action.Should().Throw<Exception>()
                .WithMessage("Expected status code Unauthorized (401) but got Ok (200)");
        }

        [Fact]
        public async Task ShouldThrowAssertStatusCodesForbidden()
        {
            Func<Task> action = async () => await builder.Expect(403).End();
            action.Should().Throw<Exception>()
                .WithMessage("Expected status code Forbidden (403) but got Ok (200)");
        }

        [Fact]
        public void ShouldAssertStatusAndCallback()
        {
            builder
                .Expect(200, m =>
                {
                    m.StatusCode.Should()
                        .BeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

        [Fact]
        public void ShouldAssertBody()
        {
            builder
                .Expect("Hello World")
                .End();
        }

        [Fact]
        public async Task ShouldThrowBadBody()
        {
            Func<Task> a = async () => await builder.Expect("Goodbye World").End();

            a.Should()
                .Throw<Exception>()
                .WithMessage("Expected body 'Goodbye World' but got 'Hello World'");
        }

        [Fact]
        public void ShouldAssertBodyAndCallback()
        {
            builder
                .Expect("Hello World", m =>
                {
                    m.Content.ReadAsStringAsync().Result.Should().StartWith("H");
                });
        }

        [Fact]
        public void ShouldAssertHeaders()
        {
            builder
                .Expect("TestHeader", "Test")
                .End();
        }

        [Fact]
        public async Task ShouldThrowBadHeaderName()
        {
            Func<Task> a = async () => await builder.Expect("Content", "100").End();
            a.Should()
                .Throw<Exception>().WithMessage("Header 'Content' not found on response message");
        }

        [Fact]
        public async Task ShouldThrowBadHeaderValue()
        {
            Func<Task> a = async () => await builder.Expect("TestHeader", "100").End();
            a.Should()
                .Throw<Exception>().WithMessage("Header 'TestHeader' not found with value '100' on response message");
        }

        [Fact]
        public void ShouldAssertHeaderWithCallback()
        {
            builder
                .Expect("TestHeader", "Test", m =>
                {
                    m.Headers.GetValues("TestHeader").First().Should().BeSameAs("Test");
                    m.Headers.GetValues("TestHeader").Should().HaveCount(1);
                });
        }

        [Fact]
        public void ShouldAssertCallback()
        {
            builder
                .Expect(m =>
                {
                    m.Content.ReadAsStringAsync().Result.Should().Contain("Hello World");
                    m.StatusCode.Should()
                        .BeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                })
                .End();
        }

        [Fact]
        public void ShouldAssertAnObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            builder
                .Expect(user)
                .End();
        }

        [Fact]
        public async Task ShouldThrowBadObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            Func<Task> a = async () => await builder
                                .Expect(new User { Name = "Tom", Age = 11, Id = 1 })
                                .End();

            a.Should()
                .Throw<Exception>();
        }

        [Fact]
        public void ShouldAssertAnObjectBodyAndCallback()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            builder
                .Expect(user, m =>
                {
                    m.StatusCode.Should()
                        .BeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

    }
}
