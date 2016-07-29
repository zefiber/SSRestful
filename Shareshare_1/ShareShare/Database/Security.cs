using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.DynamoDBv2.Model;
using System.Runtime.Serialization;

namespace Database
{
    [DataContract]
    [DynamoDBTable("security")]
    public class Security
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public int ss_id { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string symbol { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string name { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string type { get; set; }
        //[DynamoDBProperty("Authors")]    //String Set datatype

        [DataMember]
        [DynamoDBProperty]
        public string exchange { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string currency { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string category { get; set; }

        [DataMember]
        [DynamoDBProperty("descriptions")]
        public List<string> description { get; set; }

    }

    [CollectionDataContract(Name = "Securities", Namespace = "")]
    public class Securities : List<Security>
    {
    } 
}
