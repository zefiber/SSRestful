using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using System.Runtime.Serialization;
//using shareshare.Business;

namespace Database
{
    [DataContract]
    [DynamoDBTable("holding")]
    public class Holding
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public int ss_id { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public int position { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public bool is_buy { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public decimal profit { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public decimal average_price { get; set; }


        [DataMember]
        [DynamoDBProperty]
        public decimal total_cost { get; set; }

    }

    [DataContract]
    [DynamoDBTable("activity")]
    public class Activity
    {
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public UInt64 account_id { get; set; }

        [DataMember]
        [DynamoDBRangeKey]
        public DateTime time { get; set; }

        [DataMember]
        [DynamoDBProperty]   //Partition key
        public decimal amount { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public bool inflow { get; set; }

        [DataMember]
        [DynamoDBProperty]
        public string description { get; set; }

        
    }

    

    [DataContract]
    [DynamoDBTable("account")]
    public class Account
    {
        
        [DataMember]
        [DynamoDBHashKey]   //Partition key
        public UInt64 account_id { get; set; }

        /*
        [DataMember]
        [DynamoDBProperty]
        public string account_status { get; set; }
        */
          
        //regular account , short account , cash
        [DataMember]
        [DynamoDBProperty]
        public string account_type { get; set; }

        /*
        [DataMember]
        [DynamoDBProperty]
        public decimal account_balance { get; set; }
        */
         
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
        public Dictionary<int,Holding> holdings { get; set; }
        

        /*
        [DataMember]
        [DynamoDBProperty]
        public List<Activity> activities { get; set; }
        */

        
        [DataMember]
        [DynamoDBProperty]
        public Dictionary<int, ClientOrder> orders { get; set; }


        [DynamoDBVersion]
        public int? VersionNumber { get; set; }

        


        /*
        public void FinishTrade(Order or , out Activity act)
        {
            act = null;
            if (or.fill_shares > 0)
            {
                //try three times if failed maked order status as DEAL
                decimal cost = or.fill_shares * or.ave_fill_price;
                if (or.is_buy)
                {
                    
                    cash_balance -= cost;
                    Holding b = holdings.FirstOrDefault(e => e.ss_id == or.equity_id);
                    if (b != null)
                    {
                        b.total_cost += or.ave_fill_price * or.fill_shares;
                        b.position += (uint)or.fill_shares;
                        b.average_price = Math.Round(b.total_cost / b.position, 3);
                    }
                    else
                    {
                        b = new Holding();
                        b.ss_id = or.equity_id;
                        b.position = (uint)or.fill_shares;
                        b.average_price = or.ave_fill_price;
                        b.is_buy = true;
                        b.total_cost = cost;
                        if (holdings == null)
                        {
                            holdings = new List<Holding>();
                        }
                    }
                    act = new Activity();
                    act.account_id = account_id;
                    act.amount = cost;
                    act.inflow = !or.is_buy;
                }
                else
                {
                    Holding b = holdings.FirstOrDefault(e => e.ss_id == or.equity_id);
                    if (b != null && b.position >= or.fill_shares)
                    {
                        cash_balance += cost;
                        b.position -= (uint)or.fill_shares;
                        if (b.position == 0)
                        {
                            holdings.Remove(b);
                        }
                        act = new Activity();
                        act.account_id = account_id;
                        act.amount = cost;
                        act.inflow = !or.is_buy;
                    }
                    else
                    {
                        //server error
                    }
                    
                }
            }
            else
            {
                //server error
            }
        }

        public bool CanBuyShare(int equityid, decimal cash)
        {
            
            msg = "";
            if (account_type == ConstantV.ACCOUNT_TYPE_SHORT)
            {
                if (holdings == null)
                {
                    msg = "You can only close short position in short account";
                    return false;
                }

            }

            decimal totalcost = cash;
            foreach (var v in orders)
            {
                if (v.is_buy == true && v.status != ConstantV.ORDER_STATUS_FILLED
                    && v.status != ConstantV.ORDER_STATUS_CANCELLED)
                {
                    totalcost += (v.open_shares + v.fill_shares);
                     if (totalcost > cash_balance)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public bool CanSellShare(int equityid, int size , out string msg)
        {
            if (holdings != null)
            {
                Holding b = holdings.FirstOrDefault(e => e.ss_id == equityid);
                if (b != null)
                {
                    int totalneed = size;
                    foreach (var v in orders)
                    {
                        if (v.equity_id == equityid && v.is_buy == false
                            && v.status != ConstantV.ORDER_STATUS_FILLED && v.status != ConstantV.ORDER_STATUS_CANCELLED)
                        {
                            totalneed += (v.open_shares + v.fill_shares);
                            if (totalneed <= b.position)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

*/

    }
}
