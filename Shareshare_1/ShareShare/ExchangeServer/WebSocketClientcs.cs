using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using Newtonsoft.Json;
using shareshare.Business;
using Database;
using shareshare.Message;
using Microsoft.Practices.Unity;
using Infrastructure;
using System.Threading.Tasks;

namespace shareshare.PriceServer
{
    public class IncomeWebSocketClient : IIncomeClient
    {

        private IWebSocketConnection _icon;
        IncomeTCPServer _iServer;
        int _clientNoo;
        
        bool _ifConnected = false;
        private string _clientToken;
        IUnityContainer _container;
        BusinessUnit _bUnit;
        private string _username;

        
        public IncomeWebSocketClient(IUnityContainer container, IWebSocketConnection icon)
        {
            _container = container;
            _icon = icon;
            _bUnit = new BusinessUnit(container,this);
            
        }

        private HashSet<int> _equityList = new HashSet<int>();
        public HashSet<int> EquityList  // read-only instance property
        {
            get
            {
                return _equityList;
            }
        }

        public void StartClient(IncomeTCPServer iServer, int clineNo)
        {
            _iServer = iServer;
            _clientNoo = clineNo;
            _icon.OnOpen = OpenEvent;
            _icon.OnClose = OnStop;
            _icon.OnMessage = MessageEvent;
            _icon.OnError = this.OnException;
        }

        private void OpenEvent()
        {
            _ifConnected = true;
            Console.WriteLine("NEW SOCKET CONNECTING IN: {0}", _clientNoo);
            this.LogInfo("Open websocket client!");
            _bUnit.StartLoginTimer();
           // Console.WriteLine("Open websocket client!");
        }


        public void HandleOrder(ClientOrder order)
        {
            ServerOrderResponseMessage sor = new ServerOrderResponseMessage();
            sor.order = order;
            SendMessage(sor.GetMessage(),true);
        }

        public void Subscribe(List<int> list)
        {
            foreach (var equityid in list)
            {
                _equityList.Add(equityid);
            }
            _iServer.Subscribe(_clientNoo, list);
        }

        public void UnSubscribe(List<int> list)
        {
            foreach (var equityid in list)
            {
                _equityList.Remove(equityid);
            }
            _iServer.UnSubscribe(_clientNoo, list);

        }

        private void MessageEvent(string msg)
        {
            try
            {
                this.LogInfo("<-----  {0}" ,msg);
                _bUnit.HandleClientMessage(msg);
            }
            catch (Exception e)
            {
                this.LogException(e,"business logic exception,close socket");
                Stop();
            }
        }

        public void SendMessage(string msg, bool iflog)
        {
            if (_ifConnected)
            {
                if (iflog)
                {
                    this.LogInfo("-----> {0}", msg);
                }
                _icon.Send(msg);
            }
        }


        public void SendSuncMessage(string msg, bool iflog)
        {
            if (_ifConnected)
            {
                if (iflog)
                {
                    this.LogInfo("-----> {0}", msg);
                }
                try
                {
                    if (_icon.SendSync(msg))
                    {
                        Console.WriteLine("successfuly sent");
                    }
                    else
                    {
                        Console.WriteLine("fail to sent");
                    }

                }
                catch (Exception e)
                {
                    this.LogException(e, "failed to send to client");
                    Stop();
                }
            }
        }

        public void OnException(Exception e)
        {

            this.LogException(e, "web socket got exception, stop the socket");
            Stop();
        }

        public void OnStop()
        {
            
            this.LogInfo("client stop the web socket");
            Stop();
        }

        public void Stop()
        {

            if (_ifConnected)
            {
                Console.WriteLine(string.Format("disconnect client {0}", _clientNoo));
                this.LogInfo("call stop function in web socket");
                _iServer.RemoveClient(_clientNoo);
                _bUnit.LogoutToDatabase();

                try
                {
                    _icon.Close();
                }
                catch (Exception e)
                {

                }
                _ifConnected = false;
            }
        }
    }
}
