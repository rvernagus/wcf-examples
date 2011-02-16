using System.ServiceModel;

namespace Discovery.Client
{
    [ServiceContract]
    public interface IEchoService
    {
        [OperationContract]
        string Echo(string text);
    }
}