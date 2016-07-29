using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shareshare.PriceServer
{
    public class Constant
    {

        public const string MSG_REGISTERREQ = "registerrequest";
        public const string MSG_REGISTERRESP = "registerresponse";

        public const string MSG_LOGINREQ = "loginrequest";
        public const string MSG_LOGINRESP = "loginresponse";
        public const string MSG_LOGOUTREQ = "logoutrequest";

        public const string MSG_CREATE_ACCOUNT = "createaccountrequest";
        public const string MSG_ACCOUNT_RESP = "accountresponse";
        public const string MSG_RESET_ACCOUNT = "resetaccount";
        public const string MSG_ACCOUNT_TRANSFER_CASH = "transferaccountcash";

        public const string MSG_CREATE_WATCHLIST = "createwlrequest";
        public const string MSG_DELETE_WATCHLIST = "deletwlresponse";
        public const string MSG_CREATE_WATCHLIST_ITEM = "createwlitemrequest";
        public const string MSG_DELETE_WATCHLIST_ITEM = "deletwlitemrequest";
        public const string MSG_WATCHLISTRESP = "wlresponse";

        public const string MSG_SUBSCRIBE = "subscribe";
        public const string MSG_UNSUBSCRIBE = "unsubscribe";
        public const string MSG_STOPSERVICE = "stopservice";

        public const string MSG_ASK = "ask";
        public const string MSG_BID = "bid";
        public const string MSG_ASKSIZE = "asksize";
        public const string MSG_BIDSIZE = "bidsize";


        public const string MSG_NEW_ORDER_REQ = "neworder";
        public const string MSG_UPDATE_ORDER_REQ = "updateorder";
        public const string MSG_CANCEL_ORDER_REQ = "cancelorder";
        public const string MSG_ORDER_RES = "resporder";
        public const string MSG_SERVER_ORDER_MSG = "serverorderupdate";

        public const string MSG_DELIMETER = "";
        //public const string MSG_DELIMETER = "@@";
        public const int BUFFER_SIZE = 256;
    }
}
