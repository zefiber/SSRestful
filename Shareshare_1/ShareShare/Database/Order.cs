using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using System.Runtime.Serialization;

namespace Database
{
    [DynamoDBTable("serverorder")]
    public class ServerOrder
    {

        [DynamoDBHashKey] 
        public int server_order_id { get; set; }

        [DynamoDBProperty]
        public UInt64 account_id { get; set; }

        [DynamoDBProperty]
        public string client_order_id { get; set; }

    }

    [DataContract]
    [DynamoDBTable("clientorder")]
    public class ClientOrder
    {

        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public string client_order_id { get; set; }

        [DataMember]
        [DynamoDBProperty]   //Partition key
        public int server_order_id { get; set; }

        [DataMember]
        [DynamoDBProperty]   //Partition key
        public decimal price { get; set; }

        //open //cancel // filled // partial filled //expired
        [DataMember]
        [DynamoDBProperty]
        public string status { get; set; }

        //market //limit
        [DataMember]
        [DynamoDBProperty]
        public string order_type { get; set; }

        //expre date, of gtc and day order
        [DataMember]
        [DynamoDBProperty]
        public string order_expiry { get; set; }

        
        //day //gtc //gtd
        [DataMember]
        [DynamoDBProperty]
        public string order_date_type { get; set; }

        

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



        [DataMember]
        [DynamoDBProperty]
        public decimal ave_fill_price { get; set; }



     

    }




}
