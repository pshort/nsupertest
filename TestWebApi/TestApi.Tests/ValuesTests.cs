using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NSuperTest;
using TestApi.Tests.Fixtures;
using Xunit;

namespace TestApi.Tests
{
    public class ValuesTests : IClassFixture<TestApiFixture>
    {
        Server<Startup> server;
        const string value1 = "three";
        const string value2 = "four";

        public ValuesTests(TestApiFixture fixture)
        {
            server = fixture.Server;
        }

        [Fact]
        public void ShouldGiveValues()
        {
            server
                .Get("/values")
                .Expect(200)
                .End<IEnumerable<string>>((r, m) => {
                    Assert.Equal(2, m.Count());
                    Assert.Equal(value1, m.ElementAt(0));
                    Assert.Equal(value2, m.ElementAt(1));
                });
        }
    }
}
