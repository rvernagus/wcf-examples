using System.ServiceModel;

namespace Asynchronous.Service
{
    interface IEchoCallback
    {
        [OperationContract(IsOneWay = true)]
        void EchoComplete(string data);
    }
}