using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace shareshare
{
    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;

        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();

        private List<string> _list = new List<string>();

        public void startClient(TcpClient inClientSocket, string clineNo)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            _list.Add("aapl");
            Thread ctThread = new Thread(startCommu);
            ctThread.Start();
            
        }


        private string ReceiveOneMessage()
        {
            
            string content = sb.ToString();
            
            int index = content.IndexOf("@@");
            if (index > -1)
            {
                string ret = sb.ToString(0, index );
                sb.Remove(0, index + 2);
                return ret;

            }
            else
            {
                NetworkStream networkStream = clientSocket.GetStream();
                int size = networkStream.Read(buffer, 0, BufferSize);
                sb.Append(Encoding.ASCII.GetString(buffer, 0, size));
                return ReceiveOneMessage();
            }
        }

        private void SendMessage(IAsyncResult result)
        {
            try
            {
                Thread.Sleep(1000);
                NetworkStream networkStream = clientSocket.GetStream();
                string msg = messageParser.GetJsonMessage(_list);

                byte[] sendBytes = Encoding.ASCII.GetBytes(msg);

                networkStream.BeginWrite(sendBytes, 0, sendBytes.Length, new AsyncCallback(SendMessage), null);
            }
            catch (Exception e)
            {

            }
        }

        private void startCommu()
        {
            
            try
            {
                string msg = ReceiveOneMessage();
                if (messageParser.VerifyLogin(msg))
                {
                    SendMessage(null);
                    for (; ; )
                    {
                        msg = ReceiveOneMessage();
                        DataMessage db = messageParser.GetMessage(msg);
                        if (db.action == "subscribe")
                        {
                            _list.Add(db.symbol);
                        }
                        else if (db.action == "unsubscribe")
                        {
                            _list.Remove(db.symbol);
                        }
                        else
                        {
                            break;
                        }

                    }

                }
            }
            catch (Exception e) //log error
            {
                clientSocket.Close();
                
            }
            //here we exit 
          
        }




    } 
}
