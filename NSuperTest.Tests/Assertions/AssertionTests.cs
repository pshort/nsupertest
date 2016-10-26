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
using NUnit.Framework;
using NSuperTestTests.Models;

namespace NSuperTestTests.Assertions
{
    [TestFixture]
    public class AssertionTests
    {
        HttpResponseMessage message;
        ITestBuilder builder;
        Mock<IHttpRequestClient> clientMock;

        User user;

        [SetUp]
        public void Init()
        {
            message = new HttpResponseMessage();
            message.StatusCode = HttpStatusCode.OK;
            message.Content = new StringContent("Hello World");
            message.Headers.Add("TestHeader", "Test");

            user = new User { Name = "Peter", Age = 32, Id = 1 };

            clientMock = new Mock<IHttpRequestClient>();
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(() => message);

            builder = TestBuilderFactory.Create("/test", clientMock.Object);
            builder.SetMethod(HttpMethod.Get);
        }

        [Test]
        public void ShouldAssertStatusCodes()
        {
            builder
                .Expect(200)
                .End();
        }

        [Test]
        public void ShouldAssertEnumStatusCode()
        {
            message.StatusCode = HttpStatusCode.PartialContent;

            builder
                .Expect(HttpStatusCode.PartialContent)
                .End();
        }

        [Test]
        public void ShouldAssertOk()
        {
            builder
                .ExpectOk()
                .End();
        }

        [Test]
        public void ShouldAssertCreated()
        {
            message.StatusCode = HttpStatusCode.Created;

            builder
                .ExpectCreated()
                .End();
        }

        [Test]
        public void ShouldAssertNotFound()
        {
            message.StatusCode = HttpStatusCode.NotFound;

            builder
                .ExpectNotFound()
                .End();
        }

        [Test]
        public void ShouldAssertBadRequest()
        {
            message.StatusCode = HttpStatusCode.BadRequest;

            builder
                .ExpectBadRequest()
                .End();
        }

        [Test]
        public void ShouldAssertUnauthorized()
        {
            message.StatusCode = HttpStatusCode.Unauthorized;

            builder
                .ExpectUnauthorized()
                .End();
        }

        [Test]
        public void ShouldAssertRedirect()
        {
            message.StatusCode = HttpStatusCode.Redirect;

            builder
                .ExpectRedirect()
                .End();
        }

        [Test]
        public void ShouldThrowAssertStatusCodes()
        {
            Action action = () => builder.Expect(401).End();
            action.ShouldThrow<Exception>()
                .WithMessage("Expected status code Unauthorized (401) but got Ok (200)");
        }

        [Test]
        public void ShouldAssertStatusAndCallback()
        {
            builder
                .Expect(200, m =>
                {
                    m.StatusCode.ShouldBeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

        [Test]
        public void ShouldAssertBody()
        {
            builder
                .Expect("Hello World")
                .End();
        }

        [Test]
        public void ShouldThrowBadBody()
        {
            Action a = () => builder.Expect("Goodbye World").End();

            a.ShouldThrow<Exception>()
                .WithMessage("Expected body 'Goodbye World' but got 'Hello World'");
        }

        [Test]
        public void ShouldAssertBodyAndCallback()
        {
            builder
                .Expect("Hello World", m =>
                {
                    m.Content.ReadAsStringAsync().Result.Should().StartWith("H");
                });
        }

        [Test]
        public void ShouldAssertHeaders()
        {
            builder
                .Expect("TestHeader", "Test")
                .End();
        }

        [Test]
        public void ShouldThrowBadHeaderName()
        {
            Action a = () => builder.Expect("Content", "100").End();
            a.ShouldThrow<Exception>().WithMessage("Header 'Content' not found on response message");
        }

        [Test]
        public void ShouldThrowBadHeaderValue()
        {
            Action a = () => builder.Expect("TestHeader", "100").End();
            a.ShouldThrow<Exception>().WithMessage("Header 'TestHeader' not found with value '100' on response message");
        }

        [Test]
        public void ShouldAssertHeaderWithCallback()
        {
            builder
                .Expect("TestHeader", "Test", m =>
                {
                    m.Headers.GetValues("TestHeader").First().Should().BeSameAs("Test");
                    m.Headers.GetValues("TestHeader").Should().HaveCount(1);
                });
        }

        [Test]
        public void ShouldAssertCallback()
        {
            builder
                .Expect(m =>
                {
                    m.Content.ReadAsStringAsync().Result.Should().Contain("Hello World");
                    m.StatusCode.ShouldBeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                })
                .End();
        }

        [Test]
        public void ShouldAssertAnObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(message);

            builder
                .Expect(user)
                .End();
        }

        [Test]
        public void ShouldThrowBadObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(message);

            Action a = () => builder
                                .Expect(new User { Name = "Tom", Age = 11, Id = 1 })
                                .End();

            a.ShouldThrow<Exception>();
        }

        [Test]
        public void ShouldAssertAnObjectBodyAndCallback()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(message);

            builder
                .Expect(user, m =>
                {
                    m.StatusCode.ShouldBeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

    }
}
