using System;
using System.ServiceModel;

namespace Client
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool DoWork();
    }
}