using NSuperTest.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSuperTest.Server
{
    public class ServerFactory
    {
        private static Lazy<ServerFactory> _instance = new Lazy<ServerFactory>(() => new ServerFactory());
        public static ServerFactory Instance { get {  return _instance.Value; } }

        private ServerRegistry registry;

        public ServerFactory()
        {
            var registrations = new RegistrationDiscoverer().FindRegistrations();
            LoadRegistrations(registrations);
        }

        public ServerFactory(IEnumerable<IRegisterServers> registrations)
        {
            LoadRegistrations(registrations);
        }

        private void LoadRegistrations(IEnumerable<IRegisterServers> registrations)
        {
            registry = new ServerRegistry();
            foreach(var r in registrations)
            {
                r.Register(registry);
            }
        }

        public static IServer GetServer(string name)
        {
            return Instance.Build(name);
        }

        public IServer Build(string name)
        {
            if(!registry.Servers.ContainsKey(name))
            {
                throw new ServerNotRegisteredException($"There is no registered server with the name '{name}'. Please make sure you are registering a server with that name in an IRegisterServers implementaition.");
            }

            return registry.Servers[name].Build();
        }
    }
}
