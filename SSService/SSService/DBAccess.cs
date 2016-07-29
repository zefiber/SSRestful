using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.DynamoDBv2.Model;

namespace SSService
{
    /*
    public class DbAccess
    {
        AmazonDynamoDBClient _client;
        public DynamoDBContext _context;
        public DbAccess(string url)
        {
            //AmazonDynamoDBConfig cf = new AmazonDynamoDBConfig();
            //cf.ServiceURL = "http://localhost:8000";
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
            
        }

        public bool AddRecord()
        {
            try
            {
                DynamoDBContext context = new DynamoDBContext(_client);
                int secId = 1001; // Some unique value.
                Security sec = new Security
                {
                    ss_id = secId,
                    symbol = "dwti",
                    currency = "usd",
                    exchange = "smart",
                    type = "stk",
                    category = "etf",
                    description = new List<string> { "eric like", "trade on file", "中文描述" }
                };
                // Save the book.
                context.Save(sec);
            }
            catch (AmazonServiceException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public List<Security> LoadSecurityTable()
        {
            return _context.Scan<Security>().ToList<Security>();
        }

        public Customer GetCustomerByUser(string username)
        {
            return _context.Load<Customer>(username);
        }

        public Account GetAccountById(UInt64 id)
        {
            return _context.Load<Account>(id);
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


    }*/
}
