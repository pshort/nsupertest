using NSuperTest.Data;
using NSuperTest.Registration;
using NSuperTest.Registration.NetCoreServer;
using SampleEfCoreApp;
using SampleEfCoreApp.Data;

namespace EfCoreTests.Test
{
    public class ServerRegistry : IRegisterServers
    {
        public const string TestServer = "TestServer";
        
        public void Register(NSuperTest.Registration.ServerRegistry reg)
        {
            reg.RegisterNetCoreServer<Startup>(TestServer)
                .WithInMemoryContext<SchoolContext, Startup>();
        }
    }
}


/* build a test server

    var server = new Server<Startup>()
        .WithInMemoryContext<SchoolContext>("testData.json") // setting this is the default data. Calling set on a test clears it.
        .WithHttpService<Client>("fake.json")                // simple format to define some canned service
        .WithHttpService<Client, FakeService>()
        .WithMassTransit("TestQueue");
        
        */
        