using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract]
    interface IMyCallback
    {
        [OperationContract(IsOneWay = true)]
        void MakeCallComplete();
    }
}