using NSuperTest.Registration;
using NSuperTest.Registration.MockServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Tests.Servers
{
    public class ServersSetup : IRegisterServers
    {
        public void Register(ServerRegistry reg)
        {
            reg.RegisterMockServer("Test", "http://www.google.com");
        }
    }
}
