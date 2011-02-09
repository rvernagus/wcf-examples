using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Client
{
    public class ServiceClient : ClientBase<IServiceContract>, IServiceContract
    {
        public ServiceClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public string GetEnvironment()
        {
            return Channel.GetEnvironment();
        }
    }
}