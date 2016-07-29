using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using shareshare.PriceServer;
using shareshare.Business;
using System.Globalization;
using Infrastructure;
using Database;

namespace shareshare.Message
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NewOrderReqMessage : RequestMessage
    {
        private DbAccess _dataAccess;
        public NewOrderReqMessage(DbAccess dba)
        {
            _dataAccess = dba;
            action = Constant.MSG_NEW_ORDER_REQ;
        }

        public NewOrderReqMessage()
        {
            action = Constant.MSG_NEW_ORDER_REQ;
        }

        [JsonProperty]
        public UInt64 account_id { get; set; }

        [JsonProperty]
        public int equity_id { get; set; }

        [JsonProperty]
        public bool is_buy { get; set; }

        [JsonProperty]
        public int quantity { get; set; }

        [JsonProperty]
        public decimal price { get; set; }

        //market //limit
        [JsonProperty]
        public string order_type { get; set; }

        //day //gtc //gtd
        [JsonProperty]
        public string order_date_type { get; set; }

        [JsonProperty]
        public string order_expiry { get; set; }

        [JsonProperty]
        public string client_order_id { get; set; }


        public override GeneralMessage ValidRequest()
        {
            ClientOrderResponseMessage ret = null;
            if (quantity <= 0)
            {
                ret = new ClientOrderResponseMessage();
                ret.success = false;
                ret.message = "Quantiy must be greater than 0";
                return ret;
            }
            if (order_type != ConstantV.ORDER_TYPE_LIMIT || order_type != ConstantV.ORDER_TYPE_MARKET)
            {
                ret = new ClientOrderResponseMessage();
                ret.success = false;
                ret.message = string.Format("System does not support Order Type : {0}", order_type);
                return ret;
            }
            if (order_type == ConstantV.ORDER_TYPE_LIMIT)
            {
                if (price <= 0)
                {
                    ret = new ClientOrderResponseMessage();
                    ret.success = false;
                    ret.message = "Price can not be negative";
                    return ret;
                }

                if (order_date_type == ConstantV.ORDER_EXPIRY_GTD)
                {
                    try
                    {
                        DateTime dt = DateTime.ParseExact(order_expiry, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime av = Util.GetNextAvailableTradeLimitDate();
                        if (dt.Date < av.Date)
                        {
                            ret = new ClientOrderResponseMessage();
                            ret.success = false;
                            ret.message = string.Format("The earliest order date is {0}", av.ToString("MM/dd/yyyy"));
                            return ret;
                        }
                    }
                    catch
                    {
                        ret = new ClientOrderResponseMessage();
                        ret.success = false;
                        ret.message = "order_expiry musy be in the format MM/dd/yyyy";
                        return ret;
                    }

                }
            }

            return ret;
        }

        public override string HandleMessage()
        {
            LoginResponseMessage ret = new LoginResponseMessage();
            Account ac = _dataAccess.GetAccountById(account_id);
            string msg = "";
            if (ac != null)
            {
                CashAccount ca = Bunit.CreateAccountFromDbAccount(ac);
                if (ca != null && ca.CanTrade())
                {
                    RegularAccount ra = (RegularAccount)ca;
                    if (ra.CheckRisk(this, out msg))
                    {
                        ClientOrder order = new ClientOrder();
                        order.price = price;
                        order.client_order_id = client_order_id;
                        order.open_shares = quantity;
                        order.fill_shares = 0;
                        order.is_buy = is_buy;
                        order.equity_id = equity_id;
                        order.status = ConstantV.ORDER_STATUS_CREATE;
                        order.order_type = order_type;
                        order.order_date_type = order_date_type;
                        order.order_expiry = order_expiry;
                        if (!ra.CreateOrder(order))
                        {
                            ret.success = false;
                            ret.failreason = string.Format("Failed to create client order");
                        }
                        //here we save to database
                        //_orderserver.SendOrder(order);
                    }
                    else
                    {
                        ret.success = false;
                        ret.failreason = msg;
                    }

                }
                else
                {
                    ret.success = false;
                    ret.failreason = string.Format("business error, account {0} is not tradable", account_id);
                }
            }
            else
            {
                ret.success = false;
                ret.failreason = string.Format("database error, can't find account id {0}",account_id);
            }


            return ret.ToString();
        }


        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override bool HasResponse()
        {
            return true;
        }
    }


    


    [JsonObject(MemberSerialization.OptIn)]
    public class UpdateOrderReqMessage : RequestMessage
    {
        public UpdateOrderReqMessage()
        {
            action = Constant.MSG_UPDATE_ORDER_REQ;
        }

        [JsonProperty]
        public int server_order_id { get; set; }

        [JsonProperty]
        public uint quantity { get; set; }

        [JsonProperty]
        public decimal price { get; set; }

        //market //limit
        [JsonProperty]
        public string order_type { get; set; }

        //day //gtc //gtd
        [JsonProperty]
        public string order_expiry { get; set; }

        
        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override GeneralMessage ValidRequest()
        {
            
            return null;
        }

        public override bool HasResponse()
        {
            return true;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class CancelOrderReqMessage : RequestMessage
    {
        public CancelOrderReqMessage()
        {
            action = Constant.MSG_CANCEL_ORDER_REQ;
        }

        [JsonProperty]
        public int server_order_id { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override bool HasResponse()
        {
            return true;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ClientOrderResponseMessage : GeneralMessage
    {
        public ClientOrderResponseMessage()
        {
            action = Constant.MSG_ORDER_RES;
        }
        [JsonProperty]
        public UInt64 account_id { get; set; }

        [JsonProperty]
        public string client_order_id { get; set; }

        [JsonProperty]
        public bool success { get; set; }

        [JsonProperty]
        public string status { get; set; }

        [JsonProperty]
        public string message { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ServerOrderResponseMessage : GeneralMessage
    {
        public ServerOrderResponseMessage()
        {
            action = Constant.MSG_SERVER_ORDER_MSG;
        }
        [JsonProperty]
        public Database.ClientOrder order { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

    }



}
