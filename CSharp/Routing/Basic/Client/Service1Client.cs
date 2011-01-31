using System.ServiceModel;

namespace Client
{
    class Service1Client : ClientBase<IService1Contract>, IService1Contract
    {
        public Service1Client()
            : base("service1")
        {
            
        }

        public string GetData(int value)
        {
            return Channel.GetData(value);
        }
    }
}