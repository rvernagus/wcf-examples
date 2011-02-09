using System.ServiceModel;

namespace Client
{
    [ServiceContract]
    public interface IServiceContract
    {
        [OperationContract]
        string GetEnvironment();
    }
}