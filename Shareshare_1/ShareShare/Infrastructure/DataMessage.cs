using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using shareshare.PriceServer;
namespace shareshare.Message
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class GeneralMessage
    {

        public GeneralMessage() { }
        [JsonProperty]
        protected string action { get; set; }

        public string GetAction()
        {
            return action;
        }

        /*
        public virtual bool ValidRequest(out string msg);

        
        public virtual string HandleMessage()
        {
            return string.Empty;
        }*/


        public abstract string GetMessage();
        
    }

   

}
