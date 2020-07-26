using System.Threading.Tasks;

namespace NSuperTest.Messaging
{
    public interface IRequestClient
    {
         Task<IClientResponse> AsyncMakeRequest();
    }
}