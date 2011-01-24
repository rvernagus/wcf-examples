using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Client
{
    public class Service1Client : ClientBase<IService1>, IService1
    {
        public Service1Client()
            : base("clientEndpoint")
        {
        }

        public Service1Client(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        public bool DoWork()
        {
            return Channel.DoWork();
        }
    }
}