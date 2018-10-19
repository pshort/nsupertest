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

        const string value1 = "value1";
        const string value2 = "value2";

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
                    Assert.Equal(2, m.Count());
                    Assert.Equal(value1, m.ElementAt(0));
                    Assert.Equal(value2, m.ElementAt(1));
                });
        }
    }
}
