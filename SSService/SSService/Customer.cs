using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Amazon.DynamoDBv2.DataModel;

namespace SSService
{
    [DataContract]
    [DynamoDBTable("customer")]
    public class Customer
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public string username { get; set; }

        [DataMember]
        [DynamoDBProperty]   
        public string password { get; set; }

        //open //cancel // filled // partial filled //expired
        [DataMember]
        [DynamoDBProperty]
        public uint connect_status { get; set; }

        //market //limit
        [DataMember]
        [DynamoDBProperty]
        public string email { get; set; }

        //day //gtc //gtd
        [DataMember]
        [DynamoDBProperty]
        public string first_name { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string last_name { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string address_city { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string address_province { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string address_country { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public HashSet<UInt64> accounts { get; set; }
    }
}
