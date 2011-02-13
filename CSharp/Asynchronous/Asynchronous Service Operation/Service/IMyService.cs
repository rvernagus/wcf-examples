using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        void MakeCall(string data);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state);

        // Note: [OperationContract] not needed
        string EndMakeCall(IAsyncResult result);
    }
}