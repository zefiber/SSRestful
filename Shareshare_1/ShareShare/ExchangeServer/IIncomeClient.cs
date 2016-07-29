using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;

namespace shareshare.PriceServer
{
    public interface IIncomeClient
    {
        HashSet<int> EquityList
        {
            get;
        }
        void StartClient(IncomeTCPServer iServer, int clineNo);
        void SendMessage(string msg, bool iflog);
        void Stop();
        void HandleOrder(ClientOrder order);

        void Subscribe(List<int> list);
        void UnSubscribe(List<int> list);
    }
}
