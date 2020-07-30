using System;
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Registration
{
    public class ServerRegistry
    {
        public Dictionary<string, IServerBuilder> Servers { get; }

        public ServerRegistry()
        {
            Servers = new Dictionary<string, IServerBuilder>();
        }

        public void Register(string name, IServerBuilder builder)
        {
            if(Servers.ContainsKey(name))
            {
                throw new ServerRegistrationException($"The server '{name}' has already been registered with the server registry. Perhaps you are accidentally including it in more than one IRegisterServers implementation.");
            }
            Servers.Add(name, builder);
        }
    }
}
