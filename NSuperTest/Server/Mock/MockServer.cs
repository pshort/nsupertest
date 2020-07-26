namespace NSuperTest.Server.Mock
{
    public class MockServer : IServer
    {
        public MockServer(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }

        public IHttpRequestClient GetClient()
        {
            throw new System.NotImplementedException();
        }
    }
}