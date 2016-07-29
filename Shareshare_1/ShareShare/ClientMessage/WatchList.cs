using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using shareshare.PriceServer;

namespace shareshare.Message
{
    /*watch list message*/
    [JsonObject(MemberSerialization.OptIn)]
    public class CreateWatchListRequest : GeneralMessage
    {
        public CreateWatchListRequest()
        {
            action = Constant.MSG_CREATE_WATCHLIST;
        }

        [JsonProperty]
        public string watch_list_name { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class DeleteWatchListRequest : GeneralMessage
    {
        public DeleteWatchListRequest()
        {
            action = Constant.MSG_DELETE_WATCHLIST;
        }

        [JsonProperty]
        public string watch_list_name { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }



    [JsonObject(MemberSerialization.OptIn)]
    public class CreateWatchListItemRequest : GeneralMessage
    {
        public CreateWatchListItemRequest()
        {
            action = Constant.MSG_CREATE_WATCHLIST_ITEM;
        }

        [JsonProperty]
        public string watch_list_name { get; set; }


        [JsonProperty]
        public int equity_id { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class DeleteWatchListItemRequest : GeneralMessage
    {
        public DeleteWatchListItemRequest()
        {
            action = Constant.MSG_DELETE_WATCHLIST_ITEM;
        }


        [JsonProperty]
        public string watch_list_name { get; set; }


        [JsonProperty]
        public int equity_id { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class WatchListResponseMessage : GeneralMessage
    {

        public WatchListResponseMessage()
        {
            action = Constant.MSG_WATCHLISTRESP;
        }

        [JsonProperty]
        public bool success { get; set; }

        [JsonProperty]
        public string fail_reason { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }
}
