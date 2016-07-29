using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using shareshare.Message;
namespace shareshare.PriceServer
{

    /*
    public class IncomeTCPClient : IIncomeClient
    {
        TcpClient _clientSocket;
        int  _clientNoo;

        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();

        IncomeTCPServer _iServer;

        public IncomeTCPClient(TcpClient inClientSocket)
        {
            _clientSocket = inClientSocket;
        }

        public void StartClient(IncomeTCPServer iServer,  int clineNo)
        {
            _iServer = iServer;
            _clientNoo = clineNo;
            Thread ctThread = new Thread(startCommu);
            ctThread.Start();

        }

        private HashSet<int> _equityList = new HashSet<int>();
        public HashSet<int> EquityList  // read-only instance property
        {
            get
            {
                return _equityList;
            }
        }


        private string ReceiveOneMessage()
        {
            string content = sb.ToString();
            int index = content.IndexOf("@@");
            if (index > -1)
            {
                string ret = sb.ToString(0, index);
                sb.Remove(0, index + 2);
                return ret;
            }
            else
            {
                NetworkStream networkStream = _clientSocket.GetStream();
                int size = networkStream.Read(buffer, 0, BufferSize);
                sb.Append(Encoding.ASCII.GetString(buffer, 0, size));
                return ReceiveOneMessage();
            }
        }

        public void SendMessage(string msg)
        {
            try
            {
                NetworkStream networkStream = _clientSocket.GetStream();
                byte[] sendBytes = Encoding.ASCII.GetBytes(msg);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch (Exception e) //problem
            {
                Stop();
            }
        }

        public void Stop()
        {
            try
            {
                _clientSocket.Close();
            }
            catch (Exception e)
            {
            }
            _iServer.RemoveClient(_clientNoo);

        }

        private void startCommu()
        {
            
            try
            {
                for (;;)
                {
                    string msg = ReceiveOneMessage();

                    GeneralMessage gm = JsonConvert.DeserializeObject<GeneralMessage>(msg, new MessageConverter());
                    if (gm is MarketRequestMessage)
                    {
                        MarketRequestMessage mrm = (MarketRequestMessage)gm;
                        foreach (var v in mrm.equitylist)
                        {
                            _equityList.Add(v);
                        }
                        _iServer.Subscribe(_clientNoo, mrm.equitylist);
                    }
                    else if (gm is MarketCancelMessage)
                    {
                        MarketCancelMessage mcm = (MarketCancelMessage)gm;
                        foreach (var v in mcm.equitylist)
                        {
                            _equityList.Remove(v);
                        }
                        _iServer.UnSubscribe(_clientNoo, mcm.equitylist);
                    }
                    else
                    {
                        break;
                    }

                }

            }

            catch (Exception e) //log error
            {
                
            }
            Stop();

        }

    } 
    */
    public class IncomeTCPServer
    {

        private ConcurrentDictionary<int, ConcurrentHashSet<int>> _equityClientMap = new ConcurrentDictionary<int, ConcurrentHashSet<int>>();
        private ConcurrentDictionary<int, IIncomeClient> _idClientMap = new ConcurrentDictionary<int, IIncomeClient>();

        private ConcurrentDictionary<int, int> _orderClientMap = new ConcurrentDictionary<int, int>();

        private IExchangeServer _outGoServer;
        public IExchangeServer OutGoServer
        {
            get { return _outGoServer; }
            set { _outGoServer = value; }
        }
        protected int _counter = 0;

        public IncomeTCPServer(IExchangeServer outgoserver)
        {
            _outGoServer = outgoserver;
            _outGoServer.SetPriceCallBack(UpdatePrice);
            _outGoServer.SetOrderCallBack(UpdateOrder);
        }


        public void Subscribe(int clientid, List<int> equitylist)
        {
            List<int> outlist = new List<int>();
            foreach (var equityid in equitylist)
            {
                if (_equityClientMap.ContainsKey(equityid))
                {
                    _equityClientMap[equityid].Add(clientid);
                }
                else
                {
                    _equityClientMap[equityid] = new ConcurrentHashSet<int>();
                    _equityClientMap[equityid].Add(clientid);
                    outlist.Add(equityid);
                }
            }
            if (outlist.Count > 0)
            {
                OutGoServer.SubscribePrice(outlist);
            }
        }


        public void UnSubscribe(int clientid, List<int> equitylist)
        {
            List<int> outlist = new List<int>();
            foreach (var equityid in equitylist)
            {
                if (_equityClientMap.ContainsKey(equityid))
                {
                    _equityClientMap[equityid].Remove(clientid);

                    if (_equityClientMap[equityid].Count == 0)
                    {
                        outlist.Add(equityid);
                        ConcurrentHashSet<int> clients;
                        _equityClientMap.TryRemove(equityid, out clients);
                    }
                }
            }
            if (outlist.Count > 0)
            {
                OutGoServer.UnSubscribePrice(outlist);
            }
        }

        private void UpdatePrice(int equityid, string msg)
        {
            ConcurrentHashSet<int> set = null;
            if (_equityClientMap.TryGetValue(equityid, out set))
            {
                foreach (var v in set)
                {
                    IIncomeClient ic;
                    if (_idClientMap.TryGetValue(v, out ic))
                    {
                        ic.SendMessage(msg,false);
                    }

                }
            }
        }

        public void AddOrderToMap(int serverOrderId, int clientNum)
        {
            if (!_orderClientMap.TryAdd(serverOrderId, clientNum))
            {
                //log error
            }
        }

        private void UpdateOrder(Database.ClientOrder order)
        {
            int clienNum;
            if (_orderClientMap.TryGetValue(order.server_order_id, out clienNum))
            {
                IIncomeClient ic;
                if (_idClientMap.TryGetValue(clienNum, out ic))
                {
                    ic.HandleOrder(order);
                }
            }
        }


        public void AddClient(int clientid, IIncomeClient client)
        {
            _idClientMap.TryAdd(clientid, client);
        }

        public void RemoveClient(int clientid)
        {
            IIncomeClient oc = null;
            if (_idClientMap.TryRemove(clientid, out oc))
            {
                UnSubscribe(clientid, new List<int>(oc.EquityList));
            }
        }

        public virtual void Start(string url, int port)
        {
            _counter = 0;
            IPAddress address = IPAddress.Parse(url);
            TcpListener serverSocket = new TcpListener(address, port);
            TcpClient clientSocket = default(TcpClient);
            

            serverSocket.Start();
            Console.WriteLine(" >> " + "Server Started");

            /*
            while (true)
            {
                _counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(_counter) + " started!");
                IIncomeClient client = new IncomeTCPClient(clientSocket);
                AddClient(_counter, client);
                client.StartClient(this,_counter);
            }*/

        }



    }
}
