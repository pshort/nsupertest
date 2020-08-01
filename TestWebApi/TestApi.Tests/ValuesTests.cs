using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSuperTest.Client;
using NSuperTest.Assertions;
using Xunit;
using System.Net.Http;

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
        public async Task ShouldGiveValues()
        {
            var c2 = new HttpClient();
            c2.BaseAddress = new System.Uri("http://www.google.com");
            await c2.GetAsync("/").ExpectStatus(200);

            await client
                .GetAsync("/values")
                .ExpectStatus(200)
                .ExpectBody<IEnumerable<string>>(body =>
                {
                    body.Count().Should().Be(2);
                    body.ElementAt(0).Should().Be(value1);
                    body.ElementAt(1).Should().Be(value2);
                });
        }
    }
}
