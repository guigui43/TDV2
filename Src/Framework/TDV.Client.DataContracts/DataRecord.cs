using System;
using System.Runtime.Serialization;

namespace TDV.Client.Data
{
    [Serializable]
    [DataContract(Name = "DataRecord", Namespace = "net.pipe://TDV")]
    public class DataRecord
    {
        [DataMember(Name = "DataRecordKey")]
        public string DataRecordKey { get; private set; }
         [DataMember(Name = "PropertyName")]
        public string PropertyName { get; private set; }
         [DataMember(Name = "PropertyValue")]
        public object PropertyValue { get; private set; }

        public DataRecord(string key, string name, object value)
        {
            DataRecordKey = key;
            PropertyName = name;
            PropertyValue = value;
        }
    }

    [Serializable]
    [DataContract(Name = "RequestRecord", Namespace = "net.pipe://TDV")]
    public class RequestRecord
    {
        [DataMember(Name = "AssetType")]
        public AssetType AssetType { get; set; }
    }
}
