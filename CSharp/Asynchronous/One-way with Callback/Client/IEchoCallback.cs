using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract]
    interface IEchoCallback
    {
        [OperationContract(IsOneWay = true)]
        void EchoComplete(string data);
    }
}