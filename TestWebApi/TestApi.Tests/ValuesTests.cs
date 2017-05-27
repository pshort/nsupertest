using System;
using System.Collections.Generic;
using System.Linq;
using NSuperTest;
using Xunit;

namespace TestApi.Tests
{
    public class ValuesTests
    {
        Server server;

        public ValuesTests()
        {
            server = new Server<Startup>();
        }

        [Fact]
        public void ShouldGiveValues()
        {
            server
                .Get("/api/values")
                .Expect(200)
                .End<IEnumerable<string>>((r, m) => {
                    Assert.Equal(m.Count(), 2);
                    Assert.Equal(m.ElementAt(0), "value1");
                    Assert.Equal(m.ElementAt(1), "value2");
                });
        }
    }
}
