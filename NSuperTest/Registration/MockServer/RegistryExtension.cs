using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration.MockServer
{
    public static class RegistryExtension
    {
        public static void RegisterMockServer(this ServerRegistry reg, string name, string address)
        {
            reg.Servers.Add(name, new MockServerBuilder(address));
        }
    }
}
