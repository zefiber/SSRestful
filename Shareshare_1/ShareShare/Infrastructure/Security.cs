using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class Security
    {
        [JsonProperty]
        public int ss_id { get; set; }

        [JsonProperty]
        public string symbol { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public string type { get; set; }
        //[DynamoDBProperty("Authors")]    //String Set datatype

        [JsonProperty]
        public string exchange { get; set; }

        [JsonProperty]
        public string currency { get; set; }

        [JsonProperty]
        public string category { get; set; }

        [JsonProperty]
        public List<string> description { get; set; }

    }

    [CollectionDataContract(Name = "Securities", Namespace = "")]
    public class Securities : List<Security>
    {
    } 
}
