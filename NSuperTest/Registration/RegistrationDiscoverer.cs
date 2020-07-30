using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSuperTest.Registration
{
    public class RegistrationDiscoverer
    {
        public IEnumerable<IRegisterServers> FindRegistrations()
        {
            var type = typeof(IRegisterServers);
            var registries = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => Activator.CreateInstance(t) as IRegisterServers);
            return registries;
        }
    }
}
