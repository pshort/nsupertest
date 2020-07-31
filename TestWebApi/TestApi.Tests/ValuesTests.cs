using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSuperTest.Client;
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
        public async Task ShouldGiveValues()
        {
            await client
                .Get("/values")
                .Expect(200)
                .End();
        }
    }
}
