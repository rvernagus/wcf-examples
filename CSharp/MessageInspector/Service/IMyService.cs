using System.ServiceModel;

namespace MessageInspector
{
    [ServiceContract]
    internal interface IMyService
    {
        [OperationContract]
        string GetData(string data);
    }
}