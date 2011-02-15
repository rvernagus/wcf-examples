using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Discovery.Client
{
    public class EchoServiceClient : ClientBase<IEchoService>, IEchoService
    {
        public EchoServiceClient(Binding binding, EndpointAddress remoteAddress)
                : base(binding, remoteAddress)
        {
        }

        public string Echo(string text)
        {
            return Channel.Echo(text);
        }
    }
}