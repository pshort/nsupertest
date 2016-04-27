using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSuperTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NSupetTestTests
{
    [TestClass]
    public class AgentTests
    {
        Mock<IHttpRequestClient> client;
        ITestBuilder builder;
        HttpRequestMessage message;

        [TestInitialize]
        public void Init()
        {
            client = new Mock<IHttpRequestClient>();
            client.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>()))
                .Callback<HttpRequestMessage>(r => message = r);

            builder = new TestBuilder("/test", client.Object);
        }

        [TestMethod]
        public void ShouldPerformGet()
        {
            builder.SetMethod(HttpMethod.Get);

            builder
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m => 
                    m.Method == HttpMethod.Get && m.RequestUri.OriginalString == "/test")), 
                Times.Once());
        }

        [TestMethod]
        public void ShouldPerformPost()
        {
            builder.SetMethod(HttpMethod.Post);

            builder
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                    m.Method == HttpMethod.Post && m.RequestUri.OriginalString == "/test")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldPerformPut()
        {
            builder.SetMethod(HttpMethod.Put);

            builder
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                    m.Method == HttpMethod.Put && m.RequestUri.OriginalString == "/test")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldPerformDelete()
        {
            builder.SetMethod(HttpMethod.Delete);

            builder
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                    m.Method == HttpMethod.Delete && m.RequestUri.OriginalString == "/test")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldSetHeader()
        {
            builder.SetMethod(HttpMethod.Post);

            builder
                .Set("TestHeader", "TestValue")
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                    m.Headers.GetValues("TestHeader").Single() == "TestValue")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldSetBearerToken()
        {
            builder.SetMethod(HttpMethod.Get);

            builder
                .SetBearerToken("test")
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                    m.Headers.GetValues("Authorization").Single() == "Bearer test")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldSetBearerByFuncWithString()
        {
            builder.SetMethod(HttpMethod.Get);

            builder
                .SetBearerToken(() => "hello")
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                m.Headers.GetValues("Authorization").Single() == "Bearer hello")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldSetBearerByFuncWithTaskOfString()
        {
            builder.SetMethod(HttpMethod.Get);

            builder
                .SetBearerToken(async () =>
                {
                    return await Task.FromResult("hello");
                })
                .End();


            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                m.Headers.GetValues("Authorization").Single() == "Bearer hello")),
                Times.Once());
        }

        [TestMethod]
        public void ShouldSetPostBody()
        {
            builder.SetMethod(HttpMethod.Post);

            builder
                .Send(new { Name = "Peter", Age = 32 })
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(m =>
                        m.Content.ReadAsStringAsync().Result == "{\"Name\":\"Peter\",\"Age\":32}"
                        && m.Content.Headers.ContentType.MediaType == "application/json"
                    )), Times.Once());
        }
    }
}
