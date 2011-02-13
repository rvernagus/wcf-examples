using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        void MakeCall(string data);
    }
}