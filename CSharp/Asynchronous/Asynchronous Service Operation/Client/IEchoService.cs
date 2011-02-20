using System;
using System.ServiceModel;

namespace Asynchronous.Client
{
    [ServiceContract]
    internal interface IEchoService
    {
        [OperationContract]
        string Echo(string data);
    }
}