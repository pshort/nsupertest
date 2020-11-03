using System.Threading.Tasks;
using NSuperTest;
using Xunit;

namespace TestApi.Tests
{
    public class ProxyTests
    {
        [Fact]
        public async Task ShouldCallGoogle()
        {
            var client = new TestClient("Test2");

            var response = await client.GetAsync("/")
                .ExpectStatus(200);
        }
    }
}