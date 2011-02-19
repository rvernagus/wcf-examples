using System.ServiceModel;

namespace Routing
{
    [ServiceContract]
    internal interface IEchoService
    {
        [OperationContract]
        string Echo(string text);
    }
}