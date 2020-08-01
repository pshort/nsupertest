using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSuperTest.Client;
using NSuperTest.Assertions;
using Xunit;

namespace TestApi.Tests
{
    public class ValuesTests
    {
        TestClient client;
        const string value1 = "three";
        const string value2 = "four";

        public ValuesTests()
        {
            client = new TestClient("TestServer");
        }

        [Fact]
        public async Task ShouldBeAsync()
        {
            await client
                .GetAsync("/values")
                .ExpectStatus(200)
                .ExpectBody<IEnumerable<string>>(model =>
                {
                    model.Count().Should().Be(2);
                    model.ElementAt(0).Should().Be(value1);
                    model.ElementAt(1).Should().Be(value2);
                });
        }

        [Fact]
        public async Task ShouldGiveValues()
        {
            await client
                .Get("/values")
                .Expect(200)
                .End<IEnumerable<string>>((m, r) =>
                {
                    r.Count().Should().Be(2);
                    r.ElementAt(0).Should().Be(value1);
                    r.ElementAt(1).Should().Be(value2);
                });
        }
    }
}
