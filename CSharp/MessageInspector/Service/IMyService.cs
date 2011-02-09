using System.Net.Security;
using System.ServiceModel;

namespace MessageInspector
{
    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
    internal interface IMyService
    {
        [OperationContract]
        string GetData(string data);
    }
}