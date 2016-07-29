using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using shareshare.PriceServer;

namespace shareshare.Message
{
    /*market data message*/
    [JsonObject(MemberSerialization.OptIn)]
    public class MarketRequestMessage : RequestMessage
    {
        public MarketRequestMessage() 
        {
            action = Constant.MSG_SUBSCRIBE;
        }

        public override string HandleMessage()
        {
            if (equitylist != null && equitylist.Count > 0)
            {
                Bunit.SubscribePrice(equitylist);
            }
            return string.Empty;
        }

        [JsonProperty]
        public List<int> equitylist { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override bool HasResponse()
        {
            return false;
        }

    }


    [JsonObject(MemberSerialization.OptIn)]
    public class MarketCancelMessage : RequestMessage
    {
        public MarketCancelMessage()
        {
            action = Constant.MSG_UNSUBSCRIBE;
        }

        [JsonProperty]
        public List<int> equitylist { get; set; }

        public override string HandleMessage()
        {
            if (equitylist != null && equitylist.Count > 0)
            {
                Bunit.UnSubscribePrice(equitylist);
            }
            return string.Empty;
        }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override bool HasResponse()
        {
            return false;
        }
    }


    


    [JsonObject(MemberSerialization.OptIn)]
    public class MarketBidResponseMessage : GeneralMessage
    {
        public MarketBidResponseMessage(int id, double p)
        {
            action = Constant.MSG_BID;
            equityid = id;
            bid = p;
        }

        [JsonProperty]
        public int equityid { get; set; }


        [JsonProperty]
        public double bid { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class MarketAskResponseMessage : GeneralMessage
    {

        public MarketAskResponseMessage(int id, double p)
        {
            action = Constant.MSG_ASK;
            equityid = id;
            ask = p;
        }

        [JsonProperty]
        public int equityid { get; set; }


        [JsonProperty]
        public double ask { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MarketAskSizeResponseMessage : GeneralMessage
    {

        public MarketAskSizeResponseMessage(int id, int s)
        {
            action = Constant.MSG_ASKSIZE;
            equityid = id;
            asksize = s;
        }

        [JsonProperty]
        public int equityid { get; set; }


        [JsonProperty]
        public int asksize { get; set; }
        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MarketBidSizeResponseMessage : GeneralMessage
    {

        public MarketBidSizeResponseMessage(int id, int s)
        {
            action = Constant.MSG_BIDSIZE;
            equityid = id;
            bidsize = s;
        }

        [JsonProperty]
        public int equityid { get; set; }

        [JsonProperty]
        public int bidsize { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class MarketDataResponseMessage : GeneralMessage
    {

        [JsonProperty]
        public int equityid { get; set; }

        [JsonProperty]
        public double ask { get; set; }

        [JsonProperty]
        public double bid { get; set; }


        [JsonProperty]
        public int bidsize { get; set; }


        [JsonProperty]
        public int asksize { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

    }



    public class MessageDecoder
    {

        public static string GetMessageAction(string msg)
        {
            string ret = string.Empty;
            try
            {
                dynamic stuff = JsonConvert.DeserializeObject(msg);
                ret = stuff.action;
            }
            catch (Exception e)
            {

            }
            return ret;
        }

        public static int GetEquityId(string msg)
        {
            int ret = -1;
            try
            {
                dynamic stuff = JsonConvert.DeserializeObject(msg);
                ret = stuff.equityid;
            }
            catch (Exception e)
            {

            }
            return ret;
        }

    }
}
