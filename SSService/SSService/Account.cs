using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using System.Runtime.Serialization;

namespace SSService
{
    /*
    [DataContract]
    [DynamoDBTable("holding")]
    public class Holding
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public uint ss_id { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public uint position { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public bool is_buy { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public decimal profit { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public decimal average_price { get; set; }

    }

    [DataContract]
    [DynamoDBTable("activity")]
    public class Activity
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public decimal amount { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public bool inflow { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string description { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public DateTime time { get; set; }
    }

    [DataContract]
    [DynamoDBTable("order")]
    public class Order
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public uint corder_id { get; set; }

        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public int sorder_id { get; set; }

        //open //cancel // filled // partial filled //expired
        [DataMember]
        [DynamoDBProperty]
        public string status { get; set; }

        //market //limit
        [DataMember]
        [DynamoDBProperty]
        public string order_type { get; set; }

        //day //gtc //gtd
        [DataMember]
        [DynamoDBProperty]
        public string order_expiry { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public int equity_id { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public bool is_buy { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public int open_shares { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public int fill_shares { get; set; }

    }

    [DataContract]
    [DynamoDBTable("account")]
    public class Account
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public UInt64 account_id { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string account_status { get; set; }

        //Cash account , Trading account
        [DataMember]
        [DynamoDBProperty]
        public string account_type { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public decimal account_balance { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public decimal cash_balance { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string currency { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public HashSet<string> owner_ids { get; set; }

        
        [DataMember]
        [DynamoDBProperty]
        public List<Holding> holdings { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public List<Activity> activities { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public List<Order> orders { get; set; }
    }*/
}
