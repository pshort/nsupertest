namespace NSuperTest.Server
{
    public interface IServer
    {
         IHttpRequestClient GetClient();

         ITestBuilder Get(string url);
         ITestBuilder Put(string url);
         ITestBuilder Post(string url);
         ITestBuilder Patch(string url);
         ITestBuilder Delete(string url);
    }
}