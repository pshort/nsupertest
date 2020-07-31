using NSuperTest;
using NSuperTest.Client;
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
        public void ShouldGetBadRequest()
        {
            client
                .Post("/persons")
                .Send(new
                {
                    Age = 10,
                    Name = "Test"
                })
                .Expect(400)
                .End();
        }

        [Fact]
        public void ShouldGiveModelValidationResults()
        {
            client
                .Post("/persons")
                .Send(new
                {
                    Age = 200,
                    Name = "t1000"
                })
                .Expect(200)
                .End();
        }
        
    }
}