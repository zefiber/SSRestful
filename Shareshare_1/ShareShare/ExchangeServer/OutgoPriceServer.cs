using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace shareshare.PriceServer
{
    /*
    public class OutgoPriceServer //: IOutgoPriceServer
    {

        private ConcurrentQueue<GeneralMessage> _queue = new ConcurrentQueue<GeneralMessage>();
        private Action<int, string> _messageTarget = null;
        ConcurrentDictionary<int, bool> _secIdTable = new ConcurrentDictionary<int, bool>();
        private ManualResetEvent _hasReq = new ManualResetEvent(false);

        Socket _client = null;
        IPEndPoint _remoteEP;
        bool _stop = false;


        

        public OutgoPriceServer(string outip, int outport)
        {
            IPAddress address = IPAddress.Parse(outip);
            _remoteEP = new IPEndPoint(address, outport);
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }


        private bool Send(String data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                int left = byteData.Length;
                int index = 0;
                for (; ; )
                {
                    int bytes = _client.Send(byteData, index, left, SocketFlags.None);
                    if (bytes == left)
                    {
                        return true;
                    }
                    else if (bytes > 0)
                    {
                        left = left - bytes;
                        index += bytes;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public void SetCallBack(Action<int, string> messageTarget)
        {
            _messageTarget = messageTarget;
        }

        public void SubscribePrice(int equityId)
        {

            if (!_secIdTable.ContainsKey(equityId))
            {
                _secIdTable.TryAdd(equityId, false);
            }
            if (!_secIdTable[equityId])
            {
                MarketRequestMessage mrm = new MarketRequestMessage(equityId);
                _queue.Enqueue(mrm);
                _hasReq.Set();
            }
        }

        public void UnSubscribePrice(int equityId)
        {
            bool remove = false;

            if (_secIdTable.ContainsKey(equityId))
            {
                remove = _secIdTable[equityId];
                bool temp;
                _secIdTable.TryRemove(equityId, out temp);
            }

            if (remove)
            {
                MarketCancelMessage mcm = new MarketCancelMessage(equityId);
                _queue.Enqueue(mcm);
                _hasReq.Set();
            }
        }


        public void StartServer()
        {
            _client.BeginConnect(_remoteEP, new AsyncCallback(ConnectCallback), _client);
            Task task = new Task(new Action(HandleMessage));
            task.Start();
        }



        private void HandleMessage()
        {

            byte[] buffer = new byte[Constant.BUFFER_SIZE];
            StringBuilder sb = new StringBuilder();
            while (!_stop)
            {
                try
                {

                    int size = _client.Receive(buffer);
                    sb.Append(Encoding.ASCII.GetString(buffer, 0, size));
                    for (; ; )
                    {
                        string content = sb.ToString();
                        int index = content.IndexOf(Constant.MSG_DELIMETER);
                        if (index > -1)
                        {
                            string message = sb.ToString(0, index);
                            int id = MessageDecoder.GetEquityId(message);
                            if (id != -1)
                            {
                                bool remove = false;

                                if (_secIdTable.ContainsKey(id))
                                {
                                    _secIdTable[id] = true;
                                    if (_messageTarget != null)
                                    {
                                        new Task(() => { _messageTarget(id, message); }).Start();
                                    }
                                }
                                else // we receive the message has been unsubscribe
                                {
                                    remove = true;
                                }

                                if (remove)
                                {
                                    MarketCancelMessage mcm = new MarketCancelMessage(id);
                                    _queue.Enqueue(mcm);
                                    _hasReq.Set();
                                }
                            }
                            //handle this msg
                            //get id here and forward message
                            sb.Remove(0, index + 2);//delemeter length
                        }
                        else
                        {
                            break;
                        }

                    }

                }
                catch (Exception e)
                {
                    _stop = true;
                }

            }
            _hasReq.Set();

        }


        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;
                // Complete the connection.
                client.EndConnect(ar);
                while (!_stop)
                {
                    _hasReq.WaitOne();
                    if (_queue.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        GeneralMessage gm;
                        while (_queue.TryDequeue(out gm))
                        {
                            sb.Append(gm.GetMessage());
                        }
                        Send(sb.ToString()); //send to upper price server

                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                _stop = true;
                Console.WriteLine(e.ToString());
            }
        }

        public void StopServer()
        {
            if (!_stop)
            {
                try
                {
                    _stop = true;
                    _hasReq.Set();
                    _client.Shutdown(SocketShutdown.Both);
                    _client.Close();

                }
                catch (Exception e)
                {

                }
            }
            //here we need to notify our server
        }
    }
     */
}
