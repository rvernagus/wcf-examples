using System.ServiceModel;

namespace Discovery.Service
{
    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string text);
    }
}