namespace NSuperTest.Registration.ProxyServer
{
    public static class ProxyRegistryExtension
    {
        public static void RegisterProxyServer(this ServerRegistry reg, string name, string address)
        {
            reg.Register(name, new ProxyServerBuilder(address));
        }
    }
}