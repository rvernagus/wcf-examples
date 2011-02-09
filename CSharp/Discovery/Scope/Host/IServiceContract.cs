using System.ServiceModel;

namespace Service
{
    [ServiceContract]
    public interface IServiceContract
    {
        [OperationContract]
        string GetEnvironment();
    }
}