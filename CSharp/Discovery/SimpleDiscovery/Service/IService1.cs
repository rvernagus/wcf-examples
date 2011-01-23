using System.ServiceModel;

namespace Service
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool DoWork();
    }
}