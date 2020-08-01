using FluentAssertions;
using NSuperTest.Client;
using NSuperTest.Assertions;
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
                .PostAsync("/persons", new {
                    Age = 10,
                    Name = "Test"
                })
                .ExpectStatus(200)
                .ExpectBody<CreatePersonResponse>(model =>
                {
                    model.Age.Should().Be(10);
                    model.Name.Should().Be("Test");
                    model.Id.Should().NotBe(0);
                });
        }

        [Fact]
        public async Task ShouldGiveModelValidationResults()
        {
            await client
                .PostAsync("/persons", new {
                    Age = 200,
                    Name = "t1000"
                })
                .ExpectStatus(400);
        }
        
    }
}