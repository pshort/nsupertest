namespace NSuperTest.Server
{
    public interface IServerFactory
    {
         IServer Build(string name);
         void RegisterStrategy(ServerOptions options);
    }
}