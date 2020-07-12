using NSuperTest;
using TestApi.Tests.Fixtures;
using Xunit;

namespace TestApi.Tests
{
    public class PersonsTests //: IClassFixture<TestApiFixture>
    {
        Server<Startup> server;
        // public PersonsTests(TestApiFixture fixture)
        // {
        //     //server = fixture.Server;
        // }

        [Fact]
        public void ShouldGetBadRequest()
        {
            // server
            //     .Post("/persons")
            //     .Send(new {
            //         Age = 10,
            //         Name = "Test"
            //     })
            //     .Expect(400)
            //     .End();
        }

        [Fact]
        public void ShouldGiveModelValidationResults()
        {
            // server
            //     .Post("/persons")
            //     .Send(new {
            //         Age = 200,
            //         Name = "t1000"
            //     })
            //     .Expect(200)
            //     .End();
        }
        
    }
}