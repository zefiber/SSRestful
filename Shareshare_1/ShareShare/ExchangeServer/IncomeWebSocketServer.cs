using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using Newtonsoft.Json;
using Microsoft.Practices.Unity;

namespace shareshare.PriceServer
{
   
    
    public class IncomeWebSocketServer : IncomeTCPServer
    {
        
        private WebSocketServer _server;
        IUnityContainer _container;
        public IncomeWebSocketServer(IUnityContainer container, IExchangeServer outgoserver)
            : base(outgoserver)
        {
            _container = container;
           // MessageConverter mc = _container.Resolve<MessageConverter>(); //only one messageconverter is good
           // _container.RegisterInstance<MessageConverter>(mc);
        }

        public override void Start(string url, int port)
        {
            string add = string.Format("ws://{0}:{1}",url,port);
            _server = new WebSocketServer(add);
            
            _server.Start(socket =>
            {
                IncomeWebSocketClient iwc = new IncomeWebSocketClient(_container,socket);
                AddClient(_counter, iwc);
                iwc.StartClient(this, _counter);
                _counter += 1;
                

            });
        }
    }


}
