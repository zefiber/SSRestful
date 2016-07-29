using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Database;
using shareshare.Message;

namespace shareshare.Business
{
    public class BusinessClient
    {

        [DataMember]
        public string tokenguid { get; set; }

        [DataMember]
        public List<Account> _accountList;


        public Customer _cusomter;

        public bool VerifyNewOrder(NewOrderReqMessage msg, out string error)
        {
            error = "wrong";
            return true;
        }


        public bool VerifyUpdateOrder(NewOrderReqMessage msg, out string error)
        {
            error = "wrong";
            return true;
        }

        public void UpdateOrderToDb(ClientOrder order)
        {


        }

    }

}
