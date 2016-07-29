using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

namespace Database
{
    public class DbAccess
    {
        AmazonDynamoDBClient _client;
        public DynamoDBContext _context;
        public DbAccess()
        {
            _client = new AmazonDynamoDBClient();
            _client.Config.ServiceURL = "http://localhost:7777";
            _context = new DynamoDBContext(_client);
            
        }

        

        public List<Security> LoadSecurityTable()
        {
            return _context.Scan<Security>().ToList<Security>();
        }

        public Customer GetCustomerByUser(string username)
        {
            return _context.Load<Customer>(username);
        }

        public CustomerWatchList GetCustomerWatchLisstByUser(string username)
        {
            return _context.Load<CustomerWatchList>(username);
        }

        public void SaveCustomerWatchLisstByUser(CustomerWatchList username)
        {
             _context.Save<CustomerWatchList>(username);
        }

        public Account GetAccountById(UInt64 id)
        {
            return _context.Load<Account>(id);

        }

        public AccessToken GetAccessToken(string user, string token)
        {
            return _context.Load<AccessToken>(user, token);
        }

        public void SaveAccessToken(AccessToken tk)
        {
            _context.Save<AccessToken>(tk);
        }

        public void DeleteAccessToken(string username)
        {
            _context.Delete<AccessToken>(username);
        }


        public RegisterToken GetRegisterToken(string token)
        {
            return _context.Load<RegisterToken>(token);
        }

        public void SaveRegisterToken(RegisterToken tk)
        {
            _context.Save<RegisterToken>(tk);
        }

        public void DeleteRegisterToken(string token)
        {
            _context.Delete<RegisterToken>(token);
        }




        public void SaveOrder(ClientOrder order)
        {
            try
            {
                _context.Save<ClientOrder>(order);
            }
            catch (Exception e)
            {
                
            }
            
        }

        public Account GetNewAvailbleAccount(string username, decimal initialcash, string type)
        {
            Account ac = new Account();
            ac.currency = "usd"; //only trade us equity and suport usd this time
            ac.account_type = type;
            ac.cash_balance = initialcash;
            ac.owner_ids = new HashSet<string>();
            ac.owner_ids.Add(username);
            for (; ; )
            {
                ac.account_id = GetNextAvailableAccountNumber();
                if (SaveAccount(ac))
                {
                    return ac;
                }
            }
        }

        public List<Account> GetAccountsById(string username)
        {

            HashSet<UInt64> accountIds = GetCustomerByUser(username).accounts;
            List<Account> ret = new List<Account>();
            if (accountIds != null)
            {
                foreach (var v in accountIds)
                {

                    Account ac = GetAccountById(v);
                    if (ac != null)
                    {
                        ret.Add(ac);
                    }
                }
            }
            return ret;
        }

        public void SaveActivity(Activity act)
        {
            try
            {
                _context.Save<Activity>(act);
            }
            catch (Exception e)
            {
                
            }
        }

        public bool SaveAccount(Account account)
        {
            try
            {
                _context.Save<Account>(account);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool SaveCustomer(Customer cust)
        {
            try
            {
                _context.Save<Customer>(cust);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public UInt64 GetNextAvailableAccountNumber()
        {
            string tableName = "seed";

            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>() { { "name", new AttributeValue { S = "account" } } },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#Q", "seed_number"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":incr",new AttributeValue {N = "1"}}
                },
                UpdateExpression = "SET #Q = #Q + :incr",
                TableName = tableName,
                ReturnValues = "UPDATED_NEW"
            };

            var response = _client.UpdateItem(request);
            UInt64 ret = 0;
            UInt64.TryParse(response.Attributes["seed_number"].N, out ret);
            return ret;
        }


        public void ResetSeedNumber(string name, int value)
        {
            Table productCatalog = Table.LoadTable(_client, "seed");
            var seed = new Document();
            seed["name"] = name;
            seed["seed_number"] = value;
            productCatalog.PutItem(seed);
        }


        public int GetNextAvailableSeedNumber(string name)
        {
            string tableName = "seed";
            int ret = int.MinValue;
            var request = new UpdateItemRequest
            {
                Key = new Dictionary<string, AttributeValue>() { { "name", new AttributeValue { S = name } } },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#Q", "seed_number"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":incr",new AttributeValue {N = "1"}}
                },
                UpdateExpression = "SET #Q = #Q + :incr",
                TableName = tableName,
                ReturnValues = "UPDATED_NEW"
            };
            try
            {
                var response = _client.UpdateItem(request);
                int.TryParse(response.Attributes["seed_number"].N, out ret);
            }
            catch 
            {
                ret = int.MinValue;
            }
            if (ret == int.MinValue)
            {
                ret = 1;
                ResetSeedNumber(name, ret); 
            }
            return ret;
        }


        /*
        public IEnumerable<ClientOrder> GetOrderByServerAccount(UInt64 accountid)
        {
            return _context.Query<ClientOrder>(accountid);
        }

        public void DeleteAccountOrders(UInt64 account_id)
        {

            foreach(var or in GetOrderByServerAccount(account_id))
            {
                _context.Delete<ClientOrder>(or);
            }

        }*/


        public ClientOrder GetOrderByServerOrderId(int id)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "order",
                IndexName = "server_order_id",
                KeyConditionExpression = "#dt = :v_id",
                ExpressionAttributeNames = new Dictionary<String, String> {
                    {"#dt", "server_order_id"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                     {":v_id", new AttributeValue { N =  id.ToString() }}
                },
                ScanIndexForward = true
            };

            var result = _client.Query(queryRequest);
            var items = result.Items;
            if (items.Count == 1)
            {
                ClientOrder o = new ClientOrder();
                //Int32.TryParse(items[0]["server_order_id"].N, out o.server_order_id);
                o.server_order_id = Convert.ToInt32(items[0]["server_order_id"].N);
                //o.account_id = Convert.ToUInt64(items[0]["account_id"].N);
                o.client_order_id = items[0]["client_order_id"].S;
                o.open_shares = Convert.ToInt32(items[0]["open_shares"].N);
                o.fill_shares = Convert.ToInt32(items[0]["fill_shares"].N);
                o.status = items[0]["status"].S;
                return o;
            }
            else
            {
                //log server error
            }
            return null;
        }
       
        public TableDescription GetTableInformation(string tableName)
        {
            Console.WriteLine("\n*** Retrieving table information ***");
            var request = new DescribeTableRequest
            {
                TableName = tableName
            };
            var response = _client.DescribeTable(request);
            
            TableDescription description = response.Table;
            
            Console.WriteLine("Name: {0}", description.TableName);
            Console.WriteLine("# of items: {0}", description.ItemCount);
            Console.WriteLine("Provision Throughput (reads/sec): {0}",
                             description.ProvisionedThroughput.ReadCapacityUnits);
            Console.WriteLine("Provision Throughput (writes/sec): {0}",
                             description.ProvisionedThroughput.WriteCapacityUnits);
            return description;
        }



        public long GetTableCount(string tableName)
        {
            Console.WriteLine("\n*** Retrieving table information ***");
            var request = new DescribeTableRequest
            {
                TableName = tableName
            };
            var response = _client.DescribeTable(request);
            TableDescription description = response.Table;
            Console.WriteLine("Name: {0}", description.TableName);
            Console.WriteLine("# of items: {0}", description.ItemCount);
            Console.WriteLine("Provision Throughput (reads/sec): {0}",
                             description.ProvisionedThroughput.ReadCapacityUnits);
            Console.WriteLine("Provision Throughput (writes/sec): {0}",
                             description.ProvisionedThroughput.WriteCapacityUnits);
            return description.ItemCount;

        }


    }
}
