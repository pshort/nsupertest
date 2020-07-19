namespace NSuperTest.Server
{
    public class ServerFactory : IServerFactory
    {
        public IServer Build(string name)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterStrategy(ServerOptions options)
        {
            throw new System.NotImplementedException();
        }
    }
}