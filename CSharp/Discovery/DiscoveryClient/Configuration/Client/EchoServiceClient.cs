using System.ServiceModel;

namespace Discovery.Client
{
    public class EchoServiceClient : ClientBase<IEchoService>, IEchoService
    {
        public EchoServiceClient(string endpointConfigurationName)
                : base(endpointConfigurationName)
        {
        }

        public string Echo(string text)
        {
            return Channel.Echo(text);
        }
    }
}