using System.Collections.Generic;
using NSuperTest.Server.Mock;

namespace NSuperTest.Server
{
    public class ServerFactory : IServerFactory
    {
        private Dictionary<string, ServerOptions> _servers;

        public ServerFactory()
        {
            _servers = new Dictionary<string, ServerOptions>();
        }
        
        public IServer Build(string name)
        {
            if(!_servers.ContainsKey(name))
            {
                throw new ServerCreationException($"There is no registered server with name '{name}'");
            }
            
            return new MockServer(name);
        }

        public void RegisterStrategy(ServerOptions options)
        {
            _servers.Add(options.Name, options);
        }
    }
}