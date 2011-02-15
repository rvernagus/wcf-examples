using System.ServiceModel;
using System.ServiceModel.Description;

namespace Discovery.Client
{
    public class EchoServiceClient : ClientBase<IEchoService>, IEchoService
    {
        public EchoServiceClient(ServiceEndpoint endpoint)
                : base(endpoint)
        {
        }

        public string Echo(string text)
        {
            return Channel.Echo(text);
        }
    }
}