using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.Business;


namespace shareshare.PriceServer
{
    public interface IExchangeServer
    {
        

        void SetPriceCallBack(Action<int ,string> messageTarget);

        void SetOrderCallBack(Action<Database.ClientOrder> messageTarget);

        void SubscribePrice(List<int> equitylist);

        void UnSubscribePrice(List<int> equitylist);

        int  GetNextSendOrderId();

        bool SendOrder(Database.ClientOrder order);

        void CancelOrder(int serverOrderNumber);

        void StartServer(int clientnum);

        void StopServer();

        void SyncServerTime();

        EquityMarket GetEquityMarket(int equityid);
    }
}
