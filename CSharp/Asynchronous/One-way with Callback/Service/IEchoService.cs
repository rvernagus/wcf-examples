using System;
using System.ServiceModel;

namespace Asynchronous.Service
{
    [ServiceContract(CallbackContract = typeof(IEchoCallback), SessionMode = SessionMode.Required)]
    internal interface IEchoService
    {
        [OperationContract(IsOneWay = true)]
        void Echo(string data);
    }
}