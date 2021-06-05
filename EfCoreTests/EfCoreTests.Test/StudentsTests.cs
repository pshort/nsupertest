using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using NSuperTest;
using NSuperTest.Data;
using NSuperTest.Server;
using SampleEfCoreApp.Data;
using SampleEfCoreApp.Data.Models;
using Xunit;

namespace EfCoreTests.Test
{
    public class StudentsTests
    {
        private readonly IServer _server;
        private readonly TestClient _client;
        
        public StudentsTests()
        {
            _server = ServerFactory.GetServer(ServerRegistry.TestServer);
            _client = new TestClient(_server);
            _server.SetupTestData<SchoolContext>("./testdata/data.json");
        }
        
        [Fact]
        public async Task ShouldGetStudents()
        {
            await _client.GetAsync("/students")
                .ExpectStatus(200)
                .ExpectBody<IEnumerable<Student>>(b =>
                {
                    b.Count().Should().Be(2);
                    b.ElementAt(0).FirstMidName.Should().Be("Carson");
                });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ShouldGetStudentsById(int id)
        {
            await _client.GetAsync($"/students/{id}")
                .ExpectStatus(200)
                .ExpectBody<Student>(b =>
                {
                    b.Should().NotBeNull();
                    b.Id.Should().Be(id);
                });
        }

        [Fact]
        public async Task Should404OnBadId()
        {
            await _client.GetAsync($"/students/3")
                .ExpectStatus(404);
        }
    }
}