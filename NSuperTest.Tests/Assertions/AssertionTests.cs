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
using NSuperTest.Assertions;
using NSuperTest.Models;

namespace NSuperTest.Tests.Assertions
{
    public class AssertionTests
    {
        HttpResponseMessage message;
        HttpRequestMessage request;
        //ITestBuilder builder;
        Mock<IHttpRequestClient> clientMock;
        IHttpRequestClient client;

        User user;

        public AssertionTests()
        {
            request = new HttpRequestMessage();
            message = new HttpResponseMessage();
            message.StatusCode = HttpStatusCode.OK;
            message.Content = new StringContent("Hello World");
            message.Headers.Add("TestHeader", "Test");

            user = new User { Name = "Peter", Age = 32, Id = 1 };

            clientMock = new Mock<IHttpRequestClient>();
            clientMock.Setup(c => c.AsyncMakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(() => message);

            client = clientMock.Object;
        }

        [Fact]
        public async Task ShouldAssertStatusCodes()
        {
            var req = new HttpRequestMessage();
            await client.AsyncMakeRequest(req).ExpectStatus(200);

            message.AssertStatusCode(200);
        }

        [Fact]
        public async Task ShouldAssertEnumStatusCode()
        {
            message.StatusCode = HttpStatusCode.PartialContent;
            await client.AsyncMakeRequest(request).ExpectStatus(HttpStatusCode.PartialContent);

            message.AssertStatusCode((int)HttpStatusCode.PartialContent);
        }

        // TODO : Do we want these back...

        //[Fact]
        //public void ShouldAssertOk()
        //{
        //    builder
        //        .ExpectOk()
        //        .End();
        //}

        //[Fact]
        //public void ShouldAssertCreated()
        //{
        //    message.StatusCode = HttpStatusCode.Created;

        //    builder
        //        .ExpectCreated()
        //        .End();
        //}

        //[Fact]
        //public void ShouldAssertNotFound()
        //{
        //    message.StatusCode = HttpStatusCode.NotFound;

        //    builder
        //        .ExpectNotFound()
        //        .End();
        //}

        //[Fact]
        //public void ShouldAssertBadRequest()
        //{
        //    message.StatusCode = HttpStatusCode.BadRequest;

        //    builder
        //        .ExpectBadRequest()
        //        .End();
        //}

        //[Fact]
        //public void ShouldAssertUnauthorized()
        //{
        //    message.StatusCode = HttpStatusCode.Unauthorized;

        //    builder
        //        .ExpectUnauthorized()
        //        .End();
        //}

        //[Fact]
        //public void ShouldAssertForbidden()
        //{
        //    message.StatusCode = HttpStatusCode.Forbidden;

        //    builder
        //        .ExpectForbidden()
        //        .End();
        //}

        //[Fact]
        //public void ShouldAssertRedirect()
        //{
        //    message.StatusCode = HttpStatusCode.Redirect;

        //    builder
        //        .ExpectRedirect()
        //        .End();
        //}

        [Fact]
        public void ShouldThrowAssertStatusCodesUnauthorized()
        {
            Func<Task> action = async () => await client.AsyncMakeRequest(request).ExpectStatus(401);
            action.Should().Throw<Exception>()
                .WithMessage("Expected status code Unauthorized (401) but got Ok (200)");
        }

        [Fact]
        public void ShouldThrowAssertStatusCodesForbidden()
        {
            Func<Task> action = async () => await client.AsyncMakeRequest(request).ExpectStatus(403);
            action.Should().Throw<Exception>()
                .WithMessage("Expected status code Forbidden (403) but got Ok (200)");
        }

        [Fact]
        public async Task ShouldAssertStatusAndCallback()
        {
            await client.AsyncMakeRequest(request)
                .ExpectStatus(200)
                .ExpectResponse(resp =>
                {
                    resp.StatusCode.Should().BeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

        [Fact]
        public async Task ShouldAssertBody()
        {
            await client.AsyncMakeRequest(request)
                .ExpectStatus(200)
                .ExpectBody("Hello World");
        }

        [Fact]
        public void ShouldThrowBadBody()
        {
            Func<Task> a = async () => await client.AsyncMakeRequest(request)
                .ExpectBody("Goodbye World");

            a.Should()
                .Throw<Exception>()
                .WithMessage("Expected body 'Goodbye World' but got 'Hello World'");
        }

        [Fact]
        public async Task ShouldAssertBodyAndCallback()
        {
            await client.AsyncMakeRequest(request)
                .ExpectBody("Hello World")
                .ExpectResponse(resp =>
                {
                    resp.Content.ReadAsStringAsync().Result.Should().StartWith("H");
                });
        }

        [Fact]
        public async Task ShouldAssertHeaders()
        {
            await client.AsyncMakeRequest(request)
                .ExpectHeader("TestHeader", "Test");
        }

        [Fact]
        public void ShouldThrowBadHeaderName()
        {
            Func<Task> a = async () => await client.AsyncMakeRequest(request).ExpectHeader("Content", "100");
            a.Should()
                .Throw<Exception>().WithMessage("Header 'Content' not found on response message");
        }

        [Fact]
        public void ShouldThrowBadHeaderValue()
        {
            Func<Task> a = async () => await client.AsyncMakeRequest(request).ExpectHeader("TestHeader", "100");
            a.Should()
                .Throw<Exception>().WithMessage("Header 'TestHeader' not found with value '100' on response message");
        }

        [Fact]
        public async Task ShouldAssertHeaderWithCallback()
        {
            await client.AsyncMakeRequest(request)
                .ExpectHeader("TestHeader", "Test")
                .ExpectResponse(resp =>
                {
                    resp.Headers.GetValues("TestHeader").First().Should().BeSameAs("Test");
                    resp.Headers.GetValues("TestHeader").Should().HaveCount(1);
                });
        }

        [Fact]
        public async Task ShouldAssertCallback()
        {
            await client.AsyncMakeRequest(request)
                .ExpectResponse(resp =>
                {
                    resp.Content.ReadAsStringAsync().Result.Should().Contain("Hello World");
                    resp.StatusCode.Should()
                        .BeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

        [Fact]
        public async Task ShouldAssertAnObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.AsyncMakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            await client.AsyncMakeRequest(request)
                .ExpectBody(user, false);
        }

        [Fact]
        public async Task ShouldAssertBadRequest()
        {
            var messageBody = new BadRequestResponse 
            {
                Errors = new ErrorList
                {
                    { "Age", new List<string> { "Test" } }
                }
            };

            message.StatusCode = HttpStatusCode.BadRequest;
            message.Content = new StringContent(JsonConvert.SerializeObject(messageBody));
            clientMock.Setup(c => c.AsyncMakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            await client.AsyncMakeRequest(request)
                .ExpectBadRequest(err => {
                    err.Count().Should().Be(1);
                    err["Age"].Should().BeEquivalentTo(new string[] { "Test" });
                });
        }

        [Fact]
        public void ShouldThrowBadObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.AsyncMakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            Func<Task> a = async () => await client.AsyncMakeRequest(request)
                                .ExpectBody(new User { Name = "Tom", Age = 11, Id = 1 });

            a.Should()
                .Throw<Exception>();
        }

        [Fact]
        public async Task ShouldAssertAnObjectBodyAndCallback()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.AsyncMakeRequest(It.IsAny<HttpRequestMessage>())).ReturnsAsync(message);

            await client.AsyncMakeRequest(request)
                .ExpectBody(user, false)
                .ExpectResponse(resp =>
                {
                    resp.StatusCode.Should()
                        .BeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

    }
}
