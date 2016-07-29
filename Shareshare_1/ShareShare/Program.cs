using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


using System.Net.Sockets;
using System.Net;
using shareshare.PriceServer;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Infrastructure;
using shareshare.IBBroker;
using Microsoft.Practices.Unity;
using shareshare.Restful;
using Database;
using shareshare.Business;

// Add using statements to access AWS SDK for .NET services. 
// Both the Service and its Model namespace need to be added 
// in order to gain access to a service. For example, to access
// the EC2 service, add:
// using Amazon.EC2;
// using Amazon.EC2.Model;

namespace shareshare
{


    class Program
    {
        public static void Main(string[] args)
        {

            IUnityContainer myContainer = new UnityContainer();

            DbAccess dba = new DbAccess();         
            myContainer.RegisterInstance<DbAccess>(dba);

            EmailService es = new EmailService();
            myContainer.RegisterInstance<EmailService>(es);

            string restfulUrl = Infrastructure.Util.ReadSetting(Infrastructure.Util.KEY_RESTFULURL);
            RestClient ssClient = new RestClient(restfulUrl);
            myContainer.RegisterInstance<RestClient>(ssClient);

            int id = Convert.ToInt32(Util.ReadSetting(Util.KEY_CLIENTNUM));
            int port = Convert.ToInt32(Util.ReadSetting(Util.KEY_WEBSERVERPORT));
            //IBPriceServer ib = myContainer.Resolve<IBPriceServer>();
             IBSimulator ib = myContainer.Resolve<IBSimulator>();
            myContainer.RegisterInstance<IExchangeServer>(ib);
            

            //BusinessUnit bu = myContainer.Resolve<BusinessUnit>();
            //myContainer.RegisterInstance<BusinessUnit>(bu);

            IncomeWebSocketServer income = myContainer.Resolve<IncomeWebSocketServer>();


            ib.StartServer(id);
            income.Start(Util.ReadSetting(Util.KEY_WEBSERVERURL), port);
            
            System.Console.ReadLine();
        }
    }

}