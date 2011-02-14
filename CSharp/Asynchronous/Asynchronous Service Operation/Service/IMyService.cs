using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state);

        // Note: [OperationContract] not needed
        void EndMakeCall(IAsyncResult result);
    }
}