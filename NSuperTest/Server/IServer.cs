namespace NSuperTest.Server
{
    public interface IServer
    {
        string Name { get; }
        IHttpRequestClient GetClient();
    }
}