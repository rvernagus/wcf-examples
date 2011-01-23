using System.ServiceModel;

namespace Client
{
    public class Service1Client : ClientBase<IService1>, IService1
    {
        public Service1Client()
            : base("clientEndpoint")
        {
        }
            
        public bool DoWork()
        {
            return Channel.DoWork();
        }
    }
}