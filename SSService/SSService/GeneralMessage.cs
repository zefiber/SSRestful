using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SSService
{
    [DataContract]
    public class GeneralMessage
    {
        [DataMember]
       public bool success{ get; set;}

        [DataMember]
       public string error_message{ get; set;}
    }


    [DataContract]
    public class WatchListItemMessage
    {
        [DataMember]
       public string name{ get; set;}

        [DataMember]
       public string item_name{ get; set;}
    }

}
