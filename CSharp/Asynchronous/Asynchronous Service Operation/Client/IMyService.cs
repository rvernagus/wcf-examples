﻿using System;
using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        void MakeCall(string data);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginMakeCall(string data, AsyncCallback callback, object state);

        // Note: [OperationContract] not needed
        void EndMakeCall(IAsyncResult result);
    }
}