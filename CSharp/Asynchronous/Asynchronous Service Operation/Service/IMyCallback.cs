using System.ServiceModel;

namespace Asynchronous.Service
{
    //[ServiceContract] attribute not needed on callback
    interface IMyCallback
    {
        [OperationContract(IsOneWay = true)]
        void MakeCallComplete();
    }
}