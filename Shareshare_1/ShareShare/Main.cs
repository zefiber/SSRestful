using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;
using shareshare.Message;
using Newtonsoft.Json;
using shareshare.PriceServer;
using Newtonsoft.Json.Linq;
using Infrastructure;

namespace shareshare
{
    class Program
    {
        public static WebSocket websocket = new WebSocket("ws://localhost:8877/");
        // WebSocket websocket = new WebSocket("ws://192.168.1.2:8877/");
        // WebSocket websocket = new WebSocket("ws://107.22.132.180:8877/");

        public static void Login()
        {
            Console.WriteLine("username:");
            string us = Console.ReadLine();
            Console.WriteLine("password:");
            string pass = Console.ReadLine();
            LoginRequestMessage lrm = new LoginRequestMessage()
            {
                username = us,
                password = pass,
                kickothersession = true

            };
            websocket.Send(lrm.GetMessage());

        }

        public static void Subscribe()
        {
            Console.WriteLine("list:");
            string us = Console.ReadLine();

            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

            

            string[] words = us.Split(delimiterChars);

            List<int> list = new List<int>();

            foreach (var v in words)
            {
                int num = 0;
                if (int.TryParse(v, out num))
                {
                    list.Add(num);
                }
            }

            MarketRequestMessage mrm = new MarketRequestMessage()
            {
                equitylist = list
            };
            websocket.Send(mrm.GetMessage());

        }


        public static void UnSubscribe()
        {
            Console.WriteLine("list:");
            string us = Console.ReadLine();

            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };



            string[] words = us.Split(delimiterChars);

            List<int> list = new List<int>();

            foreach (var v in words)
            {
                int num = 0;
                if (int.TryParse(v, out num))
                {
                    list.Add(num);
                }
            }

            MarketCancelMessage mrm = new MarketCancelMessage()
            {
                equitylist = list
            };
            websocket.Send(mrm.GetMessage());

        }

        public static void CreateAccount()
        {
            Console.WriteLine("account type:");
            string us = Console.ReadLine();
            Console.WriteLine("deposit amount:");
            string deposit = Console.ReadLine();
            CreateAccountRequestMessage crm = new CreateAccountRequestMessage();
            crm.account_type = us;
            decimal am = 10000.00M;
            decimal.TryParse(deposit, out am);
            crm.original_deposit = am;
            websocket.Send(crm.GetMessage());
        }


        public static void TransferAccount()
        {
            Console.WriteLine("account from:");
            string from = Console.ReadLine();
            Console.WriteLine("account to:");
            string to = Console.ReadLine();
            Console.WriteLine("amount:");
            string amount = Console.ReadLine();

            AccountTransferMessage crm = new AccountTransferMessage();
            crm.from_account_id = Convert.ToUInt64(from);
            crm.to_account_id = Convert.ToUInt64(to);
            crm.amount = Convert.ToDecimal(amount);
            websocket.Send(crm.GetMessage());
        }


        public static void ResetAccount()
        {
            Console.WriteLine("account number:");
            string ac = Console.ReadLine();
            Console.WriteLine("reset amount:");
            string deposit = Console.ReadLine();
            ResetAccountMessage crm = new ResetAccountMessage();
            crm.account_id = Convert.ToUInt64(ac);
            crm.new_balance = Convert.ToDecimal(deposit);
            websocket.Send(crm.GetMessage());
        }



        public static void Register()
        {
            Console.WriteLine("username:");
            string us = Console.ReadLine();
            Console.WriteLine("password:");
            string pass = Console.ReadLine();
            Console.WriteLine("email:");
            string email = Console.ReadLine();
            RegisterRequestMessage re = new RegisterRequestMessage()
            {
                email = email,
                password = pass,
                username = us
            };
            websocket.Send(re.GetMessage());

        }

        public static void TestRegister()
        {
           
            websocket.Open();
            websocket.Opened += delegate(object sender, EventArgs e)
            {
                
               RegisterRequestMessage re = new RegisterRequestMessage()
               {
                 email= "xxhu1@983222",
                 password = "okoko22k",
                 username = "ab2cd"
               };
               websocket.Send(re.GetMessage());


               re = new RegisterRequestMessage()
               {
                   email = "xxhuaa",
                   password = "ac2d122",
                   username = "abc22dd"
               };
               websocket.Send(re.GetMessage());

               re = new RegisterRequestMessage()
               {
                   email = "xxhu83@hotmail.com",
                   password = "121212",
                   username = "xxhu1983"
               };
               websocket.Send(re.GetMessage());
                

                
               re = new RegisterRequestMessage()
               {
                   email = "xxhu83@gmail.com",
                   password = "121212",
                   username = "xxhu1984"
               };
               websocket.Send(re.GetMessage());
                
                LoginRequestMessage lrm = new LoginRequestMessage()
                {
                    username = "xxhu1984",
                    password = "121212",
                    kickothersession = true
                    
                };
                websocket.Send(lrm.GetMessage());
                
                /*
                CreateWatchListRequest cwr = new CreateWatchListRequest()
                {
                      watch_list_name = "watchlist2"
                };
                websocket.Send(cwr.GetMessage());


                cwr.watch_list_name = "watchlist3";
                websocket.Send(cwr.GetMessage());
                
                CreateWatchListItemRequest cwlir= new CreateWatchListItemRequest()
                {
                    watch_list_name = "watchlist3",
                     equity_id = 2
                };
                websocket.Send(cwlir.GetMessage());

                cwlir.equity_id = 10;
                websocket.Send(cwlir.GetMessage());

                cwlir.equity_id = 11;
                websocket.Send(cwlir.GetMessage());
                */

                MarketRequestMessage mrm = new MarketRequestMessage()
                {
                    equitylist = new List<int> { 7887, 895, 1158, 380 }
                };
                websocket.Send(mrm.GetMessage());


                System.Threading.Thread.Sleep(5000);


                MarketCancelMessage mcm = new MarketCancelMessage()
                {
                    equitylist = new List<int> { 895, 380 }
                };
                websocket.Send(mcm.GetMessage());

                /*
                DeleteWatchListItemRequest dwli = new DeleteWatchListItemRequest()
                {
                    watch_list_name = "watchlist3",
                    equity_id = 3
                };
                websocket.Send(dwli.GetMessage());

                dwli.equity_id = 10;
                websocket.Send(dwli.GetMessage());


                DeleteWatchListRequest dwl = new DeleteWatchListRequest()
                {
                    watch_list_name = "watchlist2"
                };
                websocket.Send(dwl.GetMessage());

                dwl.watch_list_name = "watchlist2";
                websocket.Send(dwl.GetMessage());
                */
                /*
                CreateAccountRequestMessage ca = new CreateAccountRequestMessage()
                {
                     username = "xxhuzl123",
                     original_deposit = 555555

                };
                websocket.Send(ca.GetMessage());
                websocket.Send(ca.GetMessage());
                websocket.Send(ca.GetMessage());

                ResetAccountMessage reset = new ResetAccountMessage()
                {
                    account_id = 100024,
                    new_balance = 999999.00M
                };
                websocket.Send(reset.GetMessage());
                */

                


            };
            websocket.MessageReceived += delegate(object sender, MessageReceivedEventArgs e)
            {
                Console.WriteLine(e.Message);
            };

        }



        public static void TestOrder(WebSocket websocket, Dictionary<int, int> dict)
        {
            //WebSocket websocket = new WebSocket("ws://192.168.1.2:8877/");
            // WebSocket websocket = new WebSocket("ws://107.22.132.180:8877/");

            websocket.Open();
            websocket.Opened += delegate(object sender, EventArgs e)
            {

                LoginRequestMessage lrm = new LoginRequestMessage()
                {
                    username = "ze",
                    password = "123456",
                    kickothersession = true

                };
                websocket.Send(lrm.GetMessage());

                /*
                ResetAccountMessage ram = new ResetAccountMessage()
                {
                    account_id = 100000,
                    new_balance = 100000

                };
                websocket.Send(ram.GetMessage());
                */
                
                

                /*
                System.Threading.Thread.Sleep(5000);


                //short sell test
                NewOrderReqMessage norm = new NewOrderReqMessage()
                {

                    account_id = 100000,
                    client_order_id = Guid.NewGuid().ToString(),
                    equity_id = 7887,
                    is_buy = false,
                    order_type = "market",
                    quantity = 100
                };
                websocket.Send(norm.GetMessage());


                //buy market order test
                norm = new NewOrderReqMessage()
                {

                    account_id = 100000,
                    client_order_id = Guid.NewGuid().ToString(),
                    equity_id = 7887,
                    is_buy = true,
                    order_type = "market",
                    quantity = 100
                };
                websocket.Send(norm.GetMessage());
                */
                /*
                MarketRequestMessage mrm = new MarketRequestMessage()
                {
                    equitylist = new List<int> { 7887,895,1158,380 }
                };
                websocket.Send(mrm.GetMessage());

                System.Threading.Thread.Sleep(5000);

                MarketCancelMessage mcm = new MarketCancelMessage()
                {
                    equitylist = new List<int> {  895, 380 }
                };
                websocket.Send(mcm.GetMessage());*/
               // System.Threading.Thread.Sleep(2000);



                /*
                NewOrderReqMessage norm = new NewOrderReqMessage()
                {

                    account_id = 888888,
                    client_order_id = Guid.NewGuid().ToString(),
                    equity_id = 7887,
                    is_buy = false,
                    order_type = "market",
                    quantity = 100
                };
                websocket.Send(norm.GetMessage());
                */

                /*
                NewOrderReqMessage norm = new NewOrderReqMessage()
                {
                    account_id = 888888,
                    client_order_id = Guid.NewGuid().ToString(),
                    equity_id = 7887,
                    is_buy = true,
                    order_type = "market",
                    quantity = 100
                };
                websocket.Send(norm.GetMessage());
                */
                int total = 100;
                for (int i = 0; i < total; i++)
                {

                    MarketRequestMessage mrm = new MarketRequestMessage()
                    {
                        equitylist = new List<int> { i }
                    };
                    websocket.Send(mrm.GetMessage());
                }


            };
            websocket.MessageReceived += delegate(object sender, MessageReceivedEventArgs e)
            {
                
                var o = JObject.Parse(e.Message);
                var msgProperty = o.Property("equityid");
                //This will be "Apple"     
                if (msgProperty != null)
                {
                    int id = (int)o["equityid"];
                    Console.WriteLine(id);
                    if (dict.ContainsKey(id))
                    {
                        dict[id] += 1;
                    }
                    else
                    {
                        dict[id] = 1;
                    }
                }
                /*
                GeneralMessage gm = JsonConvert.DeserializeObject<GeneralMessage>(e.Message, new MessageConverter());
                if (gm is MarketAskResponseMessage || gm is MarketAskSizeResponseMessage
                    || gm is MarketBidResponseMessage || gm is MarketBidSizeResponseMessage)
                {
                    Console.WriteLine(e.Message);
                }*/

                
            };

        }



        public static void Main(string[] args)
        {
           // WebSocket websocket = new WebSocket("ws://localhost:8877/");
           


            while (true)
            {
                string str = Console.ReadLine();
                if (str == "logout")
                {
                    break;
                }
                else if (str == "register")
                {
                    Register();
                }
                else if (str == "login")
                {
                    Login();
                }
                else if (str == "connect")
                {
                    websocket = new WebSocket("ws://localhost:8877/");
                    websocket.Open();
                    websocket.MessageReceived += delegate(object sender, MessageReceivedEventArgs e)
                    {
                        Console.WriteLine(e.Message);
                    };
                    websocket.Closed += delegate(object sender, EventArgs e)
                    {
                        Console.WriteLine("client socket disconnected");
                    };
                }
                else if (str == "sub")
                {
                    Subscribe();
                }
                else if (str == "usub")
                {
                    UnSubscribe();
                }
                else if (str == "ca")
                {
                    CreateAccount();
                }
                else if (str == "ra")
                {
                    ResetAccount();
                }
                else if(str == "ta")
                {
                    TransferAccount();
                }

            }

        
            LogoutRequestMessage lout = new LogoutRequestMessage();
            websocket.Send(lout.GetMessage());

        }



       
    }
}
