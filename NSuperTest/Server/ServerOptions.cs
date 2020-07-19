namespace NSuperTest.Server
{
    public class ServerOptions
    {
        public string Name { get; set; }
        public ServerType ServerType { get; set; }
        public InstantiationModel Instantiation { get; set; }
    }

    public enum ServerType
    {
        WebHostServer,
        ClientProxyServer,
    }

    public enum InstantiationModel
    {
        Transitive,
        Singleton
    }
}