using FluentAssertions;
using NSuperTest.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace NSuperTest.Tests.Client
{
    public class RequestBuilderTests
    {
        private RequestBuilder builder;
        
        public RequestBuilderTests()
        {
            builder = new RequestBuilder();
        }

        [Fact]
        public void ShouldBuildBasicRequest()
        {
            var req = builder.Build("/test", HttpMethod.Get);
            req.Method.Should().Be(HttpMethod.Get);
            req.RequestUri.OriginalString.Should().Be("/test");
        }

        [Fact]
        public void ShouldSupport1ElementQuery()
        {
            var query = new Query() { { "age", "10" } };
            var req = builder.Build("/test", HttpMethod.Get, query: query);
            req.RequestUri.OriginalString.Should().Be("/test?age=10");
        }

        [Fact]
        public void ShouldSupportMultiElementQuery()
        {
            var query = new Query() { { "age", "10" }, { "name", "tom" } };
            var req = builder.Build("/test", HttpMethod.Get, query: query);
            req.RequestUri.OriginalString
                .Should().Be("/test?age=10&name=tom");
        }

        [Fact]
        public void ShouldSupportHeaders()
        {
            Assert.True(false, "Remember");
        }
    }
}
