using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace NSupetTestTests
{
    [TestClass]
    public class AssertionTests
    {
        HttpResponseMessage message;
        TestBuilder builder;
        Mock<IHttpRequestClient> clientMock;

        User user;

        [TestInitialize]
        public void Init()
        {
            message = new HttpResponseMessage();
            message.StatusCode = HttpStatusCode.OK;
            message.Content = new StringContent("Hello World");
            message.Headers.Add("TestHeader", "Test");

            user = new User { Name = "Peter", Age = 32, Id = 1 };

            clientMock = new Mock<IHttpRequestClient>();
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(message);

            builder = new TestBuilder("/test", clientMock.Object);
            builder.SetMethod(HttpMethod.Get);
        }

        [TestMethod]
        public void ShouldAssertStatusCodes()
        {
            builder
                .Expect(200)
                .End();
        }

        [TestMethod]
        public void ShouldThrowAssertStatusCodes()
        {
            Action action = () => builder.Expect(401).End();
            action.ShouldThrow<Exception>()
                .WithMessage("Expected status code 401 but got 200");
        }

        [TestMethod]
        public void ShouldAssertStatusAndCallback()
        {
            builder
                .Expect(200, m =>
                {
                    m.StatusCode.ShouldBeEquivalentTo<HttpStatusCode>(HttpStatusCode.OK);
                });
        }

        [TestMethod]
        public void ShouldAssertBody()
        {
            builder
                .Expect("Hello World")
                .End();
        }

        [TestMethod]
        public void ShouldThrowBadBody()
        {
            Action a = () => builder.Expect("Goodbye World").End();

            a.ShouldThrow<Exception>()
                .WithMessage("Expected body 'Goodbye World' but got 'Hello World'");
        }

        [TestMethod]
        public void ShouldAssertBodyAndCallback()
        {
            builder
                .Expect("Hello World", m =>
                {
                    m.Content.ReadAsStringAsync().Result.Should().StartWith("H");
                });
        }

        [TestMethod]
        public void ShouldAssertHeaders()
        {
            builder
                .Expect("TestHeader", "Test")
                .End();
        }

        [TestMethod]
        public void ShouldThrowBadHeaderName()
        {
            Action a = () => builder.Expect("Content", "100").End();
            a.ShouldThrow<Exception>().WithMessage("Header 'Content' not found on response message");
        }

        [TestMethod]
        public void ShouldThrowBadHeaderValue()
        {
            Action a = () => builder.Expect("TestHeader", "100").End();
            a.ShouldThrow<Exception>().WithMessage("Header 'TestHeader' not found with value '100' on response message");
        }

        [TestMethod]
        public void ShouldAssertHeaderWithCallback()
        {
            builder
                .Expect("TestHeader", "Test", m =>
                {
                    m.Headers.GetValues("TestHeader").First().Should().BeSameAs("Test");
                    m.Headers.GetValues("TestHeader").Should().HaveCount(1);
                });
        }

        [TestMethod]
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

        [TestMethod]
        public void ShouldAssertAnObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(message);

            builder
                .Expect(user)
                .End();
        }

        [TestMethod]
        public void ShouldThrowBadObjectBody()
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(user));
            clientMock.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>())).Returns(message);

            Action a = () => builder
                                .Expect(new User { Name = "Tom", Age = 11, Id = 1 })
                                .End();

            a.ShouldThrow<Exception>();
        }

        [TestMethod]
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
