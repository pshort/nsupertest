using Moq;
using NSuperTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NSuperTestTests.Agent
{
    public class AgentTests
    {
        Mock<IHttpRequestClient> client;
        ITestBuilder builder;
        HttpRequestMessage message;

        public AgentTests()
        {
            client = new Mock<IHttpRequestClient>();
            client.Setup(c => c.MakeRequest(It.IsAny<HttpRequestMessage>()))
                .Callback<HttpRequestMessage>(r => message = r);

            builder = TestBuilderFactory.Create("/test", client.Object);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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


        [Fact]
        public void ShouldSetPostMultiPart()
        {
            builder.SetMethod(HttpMethod.Post);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("Test"), "test");

            builder
                .Send(content)
                .End();

            client.Verify(
                c => c.MakeRequest(It.Is<HttpRequestMessage>(
                    m => m.Content.Headers.ContentType.MediaType == "multipart/form-data" &&
                    m.Content == content
                )), Times.Once());
        }

        private bool VerifyMessage(HttpRequestMessage message)
        {
            return true;

        }
    }
}
