using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace shareshare
{

    public class DataMessage
    {
        public DataMessage(){}
        public string action;
        public string symbol;
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class DataFeed
    {
        public static Random _r = new Random();

        // "John Smith"
        [JsonProperty]
        public string symbol { get; set; }

        [JsonProperty]
        public double ask { get; set; }

        [JsonProperty]
        public double bid { get; set; }


        [JsonProperty]
        public int bidsize { get; set; }


        [JsonProperty]
        public int asksize { get; set; }


        public DataFeed(string sym)
        {
            symbol = sym;
            ask = _r.NextDouble() * 100;
            bid = _r.NextDouble() * 100;
            bidsize = _r.Next(300, 400);
            asksize = _r.Next(300, 400);
        }


    }

    public class messageParser
    {
        
        public static bool VerifyLogin(string msg)
        {
            dynamic stuff = JsonConvert.DeserializeObject(msg);
            string userName = stuff.username;
            string password = stuff.password;
            string action = stuff.action;
            if (action == "login" && userName == "xxhu" && password == "121212")
            {
                return true;
            }
            return false;

        }


        public static DataMessage GetMessage(string msg)
        {
            dynamic stuff = JsonConvert.DeserializeObject(msg);
            DataMessage dm = new DataMessage();
            dm.action = stuff.action;
            dm.symbol = stuff.symbol;
            return dm;
        }


        public static string GetJsonMessage(List<string> _list)
        {
            List<DataFeed> _feds = new List<DataFeed>();
            foreach (var v in _list)
            {
                _feds.Add(new DataFeed(v));
            }
            return JsonConvert.SerializeObject(_feds, Formatting.Indented) + "@@";
        }



    }
}
