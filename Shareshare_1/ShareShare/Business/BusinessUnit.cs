using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.PriceServer;
using Database;
using shareshare.Message;
using Infrastructure;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace shareshare.Business
{
    public partial class BusinessUnit
    {


 
        private DbAccess _dataAccess;
        private IIncomeClient _client;
        private IExchangeServer _exchange;
        MessageConverter _messageConverter = null;


        bool _ifLogin = false;
        public bool IfLogin
        {
            get { return _ifLogin; }
            set { _ifLogin = value; }
        }


        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


        public BusinessUnit(IUnityContainer container, IIncomeClient client)
        {
            _dataAccess = container.Resolve<DbAccess>();
            _client = client;
            _messageConverter = container.Resolve<MessageConverter>();
            _ifLogin = false;
            _exchange = container.Resolve<IExchangeServer>();
        }

        public void StartLoginTimer()
        {
            System.Threading.Thread.Sleep(5000);
            if (!_ifLogin)
            {
                _client.Stop();
            }
        }

        public void SubscribePrice(List<int> list)
        {
            _client.Subscribe(list);
        }

        public void UnSubscribePrice(List<int> list)
        {
            _client.UnSubscribe(list);
        }


        public void HandleClientMessage(string msg)
        {
            RequestMessage rgm = JsonConvert.DeserializeObject<RequestMessage>(msg, _messageConverter);
            rgm.Bunit = this;
            if (rgm != null)
            {
                GeneralMessage gm = rgm.ValidRequest();
                if (gm != null)
                {
                    _client.SendMessage(gm.GetMessage(), true);
                }
                else
                {
                    string str = rgm.HandleMessage();
                    if (rgm.HasResponse())
                    {
                        _client.SendMessage(str, true);
                    }  
                }
            }
            else
            {
                this.LogError("Unrecognize message: {0}", msg);
                _client.Stop();
            }

        }



        public CashAccount CreateAccountFromDbAccount(Account ac)
        {
            if (ac.account_type == ConstantV.ACCOUNT_TYPE_CASH)
            {
                return new CashAccount(_dataAccess, ac);
            }
            else if (ac.account_type == ConstantV.ACCOUNT_TYPE_REGULAR)
            {
                return new RegularAccount(_dataAccess,_exchange, ac);
            }
            else if (ac.account_type == ConstantV.ACCOUNT_TYPE_SHORT)
            {
                return new ShortAccount(_dataAccess, _exchange, ac);
            }
            return null;
        }

        /*
        //check direction // check holding //check balance //check exchange
        public ClientOrder PlaceOrder(NewOrderReqMessage req, out string message)
        {
            ClientOrder ret = null;
            if (!req.ValidRequest(out message))
            {   
                return ret;
            }

            Account ac = _dataAccess.GetAccountById(req.account_id);
            //ac.orders = _dataAccess.GetOrderByServerAccount(ac.account_id);
            if (ac != null)
            {
                
            }
            else
            {
                message = "Server error, no account found";
                return ret;
            }
            ret = new ClientOrder();
            ret.price = req.price;
            //ret.account_id = req.account_id;
            ret.client_order_id = req.client_order_id;
            ret.open_shares = req.quantity;
            ret.fill_shares = 0;
            ret.is_buy = req.is_buy;
            ret.equity_id = req.equity_id;
            ret.status = ConstantV.ORDER_STATUS_CREATE;
            ret.server_order_id = int.MinValue;
            ret.order_type = req.order_type;
            //here we can save to database
          
            int id = _iExchange.GetNextSendOrderId();
            if (id < 0)
            {
                message = "Server error, fail to get order server id";
                return null;
            }
            ret.server_order_id = id;
            
            
            
            
            _dataAccess.SaveOrder(ret);

            if (!_iExchange.SendOrder(ret))//if failed we need to give server order id
            {
                message = "Server error, order server failed";
                return null;
            }
            
            message = "Successfully created order";
            return ret;
        }*/


        
       public ClientOrder ServerUpdateOrder(BusinessOrder or) //called by order server
       {
           
           ClientOrder order = _dataAccess.GetOrderByServerOrderId(or.server_order_id);
           /*
           if (order != null)
           {
               if (order.status == ConstantV.ORDER_STATUS_FILLED
                   ||order.status == ConstantV.ORDER_STATUS_CANCELLED)
               {
                   return null;
               }
               int totalshare_d = order.fill_shares + order.open_shares;
               int totalshare_s = or.filled + or.remaining;
               if (totalshare_d == totalshare_s)
               {
                   if (or.filled < order.fill_shares)
                   {
                       //error
                       return null;
                   }
                   order.status = or.status;
                   order.fill_shares = or.filled;
                   order.open_shares = or.remaining;
                   order.ave_fill_price = or.avgFillPrice;
                   if (order.status == ConstantV.ORDER_STATUS_CANCELLED || order.status == ConstantV.ORDER_STATUS_FILLED)// need to create transaction and position
                   {
                       Account ac = _dataAccess.GetAccountById(order.account_id);
                       Activity act = null;
                       ac.FinishTrade(order, out act);
                       _dataAccess.SaveActivity(act);
                       if (act != null)
                       {
                           _dataAccess.SaveActivity(act);
                       }
                       _dataAccess.SaveAccount(ac);
                       //we need to update account position and transaction.
                   }

                   _dataAccess.SaveOrder(order);
                   return order;
                   //}
               }
               else
               {
                   //server error
               }
           }
           else //log business error
           {
                
           }*/
           return order;
       }

       public void LogoutToServer()
       {
           _client.Stop();
       }


        public void LogoutToDatabase()
        {
            if (IfLogin)
            {
                _dataAccess.DeleteAccessToken(UserName);
                IfLogin = false;
            }
        }


    }
}
