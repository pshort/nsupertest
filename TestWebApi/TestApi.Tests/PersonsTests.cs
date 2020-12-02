using FluentAssertions;
using NSuperTest;
using System.Threading.Tasks;
using TestApi.Models;
using Xunit;
using System.Net.Http;
using Newtonsoft.Json.Schema;
using System.IO;

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
        public async Task ShouldUseRequest()
        {
            var request = new PostRequest("/persons", new { Age = 10, Name = "Test" });

            await client
                .MakeRequestAsync(request)
                .ExpectStatus(200)
                .ExpectHeader("Test", "test")
                .ExpectBody<CreatePersonResponse>(model =>
                {
                    model.Age.Should().Be(10);
                    model.Name.Should().Be("Test");
                    model.Id.Should().NotBe(0);
                    model.Id.Should().Be(10);
                });
        }

        [Fact]
        public async Task ShouldGetBadRequest()
        {
            await client
                .PostAsync("/persons", 
                    body: new {
                        Age = 10,
                        Name = "Test"
                    },
                    headers: new Headers { { "nameOverride", "peter" } },
                    query: new Query { { "setId", "9" } }
                )
                .ExpectStatus(200)
                .ExpectBody<CreatePersonResponse>(model =>
                {
                    model.Age.Should().Be(10);
                    model.Name.Should().Be("peter");
                    model.Id.Should().NotBe(0);
                    model.Id.Should().Be(9);
                });
        }

        [Fact]
        public async Task ShouldValidateResponseSchema()
        {
            await client
                .PostAsync("/persons", 
                    body: new {
                        Age = 10,
                        Name = "Test"
                    },
                    headers: new Headers { { "nameOverride", "peter" } },
                    query: new Query { { "setId", "9" } }
                )
                .ExpectStatus(200)
                .ExpectSchema<CreatePersonResponse>();
        }

        [Fact]
        public async Task ShouldValidateResponseSchemaString()
        {
            var schema = await File.ReadAllTextAsync("./responseSchema.json");
            
            await client
                .PostAsync("/persons", 
                    body: new {
                        Age = 10,
                        Name = "Test"
                    },
                    headers: new Headers { { "nameOverride", "peter" } },
                    query: new Query { { "setId", "9" } }
                )
                .ExpectStatus(200)
                .ExpectSchema(schema);
        }


        [Fact]
        public async Task ShouldGiveModelValidationResults()
        {
            await client
                .PostAsync("/persons", new {
                    Age = 200,
                    Name = "t1000"
                })
                .ExpectBadRequest(errors => {
                    errors.Count.Should().Be(1);
                    errors["Age"].Should().BeEquivalentTo(new [] { "The field Age must be between 0 and 100." });
                });
        }
    }
}
