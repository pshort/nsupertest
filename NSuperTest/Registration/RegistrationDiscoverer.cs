using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NSuperTest.Registration
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
    public class RegistrationDiscoverer
    {
        public IEnumerable<IRegisterServers> FindRegistrations()
        {
            var type = typeof(IRegisterServers);
            var registries = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetLoadableTypes())
                .Where(t => type.IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => Activator.CreateInstance(t) as IRegisterServers);
            return registries;
        }
    }
}
