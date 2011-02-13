using System.ServiceModel;

namespace Asynchronous.ClientSide.Service

{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        void MakeCall();
    }
}