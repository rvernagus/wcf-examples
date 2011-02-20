using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract]
    internal interface IEchoService
    {
        [OperationContract]
        string Echo(string data);
    }
}