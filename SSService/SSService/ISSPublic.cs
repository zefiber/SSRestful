using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.ServiceModel;
using Database;

namespace SSService
{
    [ServiceContract]
    public interface ISSPublic
    {
        //customer api
        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/activation/{token}")]
        [OperationContract]
        string ActivateAccount(string token);



        //security api
        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/securitis/?sec_id={sec_id}")]
        [return: MessageParameter(Name = "Security")]
        [OperationContract]
        Security GetSecurityById(int sec_id);



        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Bare,
           UriTemplate = "/securitis/{sec_sym}")]
        [OperationContract]
        List<Security> GetSimilarSymbol(string sec_sym);







        /************************************************/
        [WebInvoke(Method = "POST",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "customer/register")]
        [OperationContract]
        GeneralMessage RegisterUser(Customer cus);




        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/account/get/{username}")]
        [OperationContract]
        List<Account> GetAccounts(string username);


        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/watchlist/get/{username}")]
        [OperationContract]
        Dictionary<string, HashSet<int>> GetWatchList(string username);



        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/add/{username}/{name}", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage CreateWatchList(string username, string name);



        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/delete/{username}/{name}", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage DeleteWatchList(string username, string name);


        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/additem/{username}/{lname}/?sid={sid}", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage CreateWatchListItem(string username, string lname, int sid);


        //GET operation
        [WebInvoke(UriTemplate = "/watchlist/deleteitem/{username}/{lname}/?sid={sid}", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        GeneralMessage DeleteWatchListItem(string username, string lname, int sid);




    }
}
