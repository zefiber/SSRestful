using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using Database;

namespace SSService
{

    

    [ServiceContract]
    public interface ISSService
    {
        /*
        [WebInvoke(Method = "OPTIONS",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/account/get")]
        [OperationContract]
        List<Account> GetAccounts();


        [WebInvoke(Method = "OPTIONS",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/watchlist/get")]
        [OperationContract]
        List<WatchList> GetWatchList();



        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/add/{name}", Method = "OPTIONS",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage CreateWatchList(string name);



        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/delete/{name}", Method = "OPTIONS",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage DeleteWatchList(string name);


        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/additem/{lname}/?sid={sid}", Method = "OPTIONS",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage CreateWatchListItem(string lname, int sid);


        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/deleteitem/{lname}/?sid={sid}", Method = "OPTIONS",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage DeleteWatchListItem(string lname, int sid);*/

    }

}
