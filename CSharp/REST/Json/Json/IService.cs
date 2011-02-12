using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace REST.Json
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebGet]
        string GetData(int value);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }


    [DataContract]
    public class CompositeType
    {
        private bool _boolValue = true;
        private string _stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return _boolValue; }
            set { _boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }
    }
}