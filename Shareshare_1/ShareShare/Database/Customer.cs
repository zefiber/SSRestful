using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Amazon.DynamoDBv2.DataModel;

namespace Database
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

        
        [DataMember]
        [DynamoDBProperty]
        public bool connect_status { get; set; }


        //open //cancel // filled // partial filled //expired
        [DataMember]
        [DynamoDBProperty]
        public string last_device_id { get; set; }

        //market //limit
        [DataMember]
        [DynamoDBProperty]
        public string email { get; set; }


        [DataMember]
        [DynamoDBProperty]
        public string token { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public bool is_activated { get; set; }


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


        

        [DataMember]
        [DynamoDBProperty]
        public Dictionary<string, HashSet<int>> watch_list { get; set; }

        [DynamoDBVersion]
        public int? VersionNumber { get; set; }
    }





    
    [DataContract]
    [DynamoDBTable("customer")]
    public class CustomerWatchList
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public string username { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public Dictionary<string, HashSet<int>> watch_list { get; set; }
    }
    


    [DataContract]
    [DynamoDBTable("watchlist")]
    public class WatchList
    {
        [DataMember]
        [DynamoDBHashKey]
        public string name { get; set; }


        [DataMember]
        [DynamoDBProperty]
        public HashSet<int> equitylist { get; set; }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WatchList);
        }

        public bool Equals(WatchList wl)
        {
            return wl != null && wl.name.Equals(name);
        }

    }


    [DataContract]
    [DynamoDBTable("registertoken")]
    public class RegisterToken
    {
        [DataMember]
        [DynamoDBHashKey]
        public string token_id { get; set; }


        [DataMember]
        [DynamoDBProperty]
        public string username { get; set; }

    }


    [DataContract]
    [DynamoDBTable("accesstoken")]
    public class AccessToken
    {
        [DataMember]
        [DynamoDBHashKey]
        public string username { get; set; }
        

        [DataMember]
        [DynamoDBProperty]
        public string token { get; set; }
        
    }
}
