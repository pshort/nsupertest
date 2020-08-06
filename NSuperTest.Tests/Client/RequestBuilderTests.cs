using FluentAssertions;
using NSuperTest.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

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
            var headers = new Headers { { "H1", "header" } };
            var req = builder.Build("/test", HttpMethod.Get, headers: headers);
            req.Headers.Contains("H1").Should().Be(true);
            var header = req.Headers.GetValues("H1");
            header.Count().Should().Be(1);
            header.First().Should().Be("header");
        }

        [Fact]
        public void ShouldSupportMultipleDifferentHeaders()
        {
            var headers = new Headers { { "H1", "header" }, { "h2", "value2" } };
            var req = builder.Build("/test", HttpMethod.Get, headers: headers);
            req.Headers.Contains("H1").Should().Be(true);
            var h1 = req.Headers.GetValues("H1");
            h1.Count().Should().Be(1);
            h1.First().Should().Be("header");
            var h2 = req.Headers.GetValues("h2");
            h2.Count().Should().Be(1);
            h2.First().Should().Be("value2");
        }

        [Fact]
        public void ShouldSupportBodies()
        {
            var body = new { Name = "Tom", Age = 11 };
            var req = builder.Build("/test", HttpMethod.Post, body);
            var content = req.Content.ReadAsStringAsync().Result;
            content.Should().Be("{\"Name\":\"Tom\",\"Age\":11}");
        }
    }
}
