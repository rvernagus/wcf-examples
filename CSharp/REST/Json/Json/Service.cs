using System;

namespace REST.Json
{

    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("Called GetData with {0}", value);
        }

        public CompositeType GetDataUsingDataContract(bool boolValue)
        {
            var composite = new CompositeType
                                {
                                    BoolValue = boolValue,
                                    StringValue = boolValue.ToString()
                                };
            return composite;
        }
    }
}
