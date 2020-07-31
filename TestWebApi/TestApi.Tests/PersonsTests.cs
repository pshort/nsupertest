using FluentAssertions;
using NSuperTest.Client;
using System.Threading.Tasks;
using TestApi.Models;
using Xunit;

namespace TestApi.Tests
{
    public class PersonsTests
    {
        TestClient client;
        public PersonsTests()
        {
            client = new TestClient("TestServer");
        }

        [Fact]
        public async Task ShouldGetBadRequest()
        {
            await client
                .Post("/persons")
                .Send(new
                {
                    Age = 10,
                    Name = "Test"
                })
                .Expect(200)
                .End<CreatePersonResponse>((m, r) =>
                {
                    r.Age.Should().Be(10);
                    r.Name.Should().Be("Test");
                    r.Id.Should().NotBe(0);
                });
        }

        [Fact]
        public async Task ShouldGiveModelValidationResults()
        {
            await client
                .Post("/persons")
                .Send(new
                {
                    Age = 200,
                    Name = "t1000"
                })
                .Expect(400)
                .End();
        }
        
    }
}