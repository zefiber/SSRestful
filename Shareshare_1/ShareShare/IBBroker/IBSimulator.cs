using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.PriceServer;
using IBApi;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using shareshare.Restful;
using Infrastructure;
using Newtonsoft.Json;
using shareshare.Message;
using shareshare.Business;
using Database;
using System.Threading;
using System.Runtime.InteropServices;

namespace shareshare.IBBroker
{


    public class IBSimulator : IExchangeServer ,  EWrapper
    {
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }




        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetSystemTime(ref SYSTEMTIME lpSystemTime);

        ConcurrentDictionary<int, EquityMarket> _marketMap = new ConcurrentDictionary<int, EquityMarket>();
        ConcurrentQueue<Database.ClientOrder> _orderQueue = new ConcurrentQueue<Database.ClientOrder>();
       // private EClientSocket _clientSocket;
        private Action<int, string> _priceMessageTarget = null;
        private Action<Database.ClientOrder> _orderMessageTarget = null;
        ConcurrentHashSet<int> _secIdTable = new ConcurrentHashSet<int>();
        RestClient _ssClient;
        private BusinessUnit _bUnit = null;
        string _serviceUrl;
        private bool _serverStop = true;
        private ManualResetEvent _orderEvent = new ManualResetEvent(false);

        private bool _synWithServer = false;
        private DbAccess _dataAccess;
        private string _osName;
        public IBSimulator(RestClient restfulClient,DbAccess dba)
        {
            //_clientSocket = new EClientSocket(this);
            _ssClient = restfulClient;
            _dataAccess = dba;
        }

        public void SetPriceCallBack(Action<int, string> messageTarget)
        {
            _priceMessageTarget = messageTarget;
        }

        public void SetOrderCallBack(Action<Database.ClientOrder> messageTarget)
        {
            _orderMessageTarget = messageTarget;
        }

        public void SubscribePrice(List<int> equitylist)
        {
            foreach (var equityId in equitylist)
            {
                if (_secIdTable.Add(equityId))
                {
                    EquityMarket eq = new EquityMarket();
                    _marketMap.TryAdd(equityId , eq);
                }
                
                Security sec = _ssClient.GetSecurityById(equityId);
                if (sec != null)
                {
                    Contract contract = new Contract { Symbol = sec.symbol, Exchange = "SMART", Currency = sec.currency, SecType = sec.type };
                    //_clientSocket.reqMktData(equityId, contract, "", false);
                }
                else
                {
                    //report service error should we?
                }
            }
        }

        public void UnSubscribePrice(List<int> equitylist)
        {
            EquityMarket eq = null;
            foreach (var equityId in equitylist)
            {
                if (_secIdTable.Contains(equityId))
                {
                    //_clientSocket.cancelMktData(equityId);
                    _secIdTable.Remove(equityId);
                    _marketMap.TryRemove(equityId, out eq);
                }
            }
        }

        public EquityMarket GetEquityMarket(int equityid)
        {
            EquityMarket eq = null;
            _marketMap.TryGetValue(equityid, out eq);
            return eq;
        }

        public void StartServer(int clientId)
        {
            //_clientSocket.eConnect("", 7496, clientId);
           // _bUnit = bunit;
            _serverStop = false;
            _osName = "OS" + clientId;
            nextValidId(1);
            new Task(() =>
            {

                Random ran = new Random(System.DateTime.Now.Millisecond);
                while (!_serverStop)
                {
                    if (_secIdTable.Count > 0)
                    {
                        try
                        {
                            int sec = ran.Next(5) + 1;
                            Thread.Sleep(sec * 100);
                            int sec1 = ran.Next(_secIdTable.Count);
                            double price = Math.Round(ran.NextDouble(), 2);
                            if (sec % 4 == 0)
                            {
                                tickPrice(_secIdTable.ToList()[sec1], 1, price, 1);
                            }
                            else if (sec % 4 == 1)
                            {
                                tickPrice(_secIdTable.ToList()[sec1], 2, price, 1);
                            }
                            else if (sec % 4 == 2)
                            {
                                tickSize(_secIdTable.ToList()[sec1], 0, sec);

                            }
                            else if (sec % 4 == 3)
                            {
                                tickSize(_secIdTable.ToList()[sec1], 3, sec);

                            }
                        }

                        catch (Exception e)
                        {

                        }


                    }
                }

                //

            }
            ).Start();


        }

        public void StopServer()
        {
            //_clientSocket.Close();
        }

        public virtual void tickPrice(int tickerId, int field, double price, int canAutoExecute)
        {
            if (_priceMessageTarget != null)
            {
                if (field == 1)//bid
                {
                    EquityMarket eq = null;
                    if (_marketMap.TryGetValue(tickerId, out eq))
                    {
                        eq.bid = price;
                    }
                    MarketBidResponseMessage gm = new MarketBidResponseMessage(tickerId,price);
                    new Task(() => { _priceMessageTarget(tickerId, gm.GetMessage()); }).Start();
                }

                else if (field == 2)//ask
                {
                    EquityMarket eq = null;
                    if (_marketMap.TryGetValue(tickerId, out eq))
                    {
                        eq.ask = price;
                    }
                    MarketAskResponseMessage gm = new MarketAskResponseMessage(tickerId, price);
                    new Task(() => { _priceMessageTarget(tickerId, gm.GetMessage()); }).Start();
                }
                
            }
        }

        public virtual void tickSize(int tickerId, int field, int size)
        {
            if (_priceMessageTarget != null)
            {
                if (field == 0)//bid
                {
                    EquityMarket eq = null;
                    if (_marketMap.TryGetValue(tickerId, out eq))
                    {
                        eq.bidsize = size;
                    }
                    MarketBidSizeResponseMessage gm = new MarketBidSizeResponseMessage(tickerId, size);
                    new Task(() => { _priceMessageTarget(tickerId, gm.GetMessage()); }).Start();
                }

                else if (field == 3)//ask
                {
                    EquityMarket eq = null;
                    if (_marketMap.TryGetValue(tickerId, out eq))
                    {
                        eq.asksize = size;
                    }
                    MarketAskSizeResponseMessage gm = new MarketAskSizeResponseMessage(tickerId, size);
                    new Task(() => { _priceMessageTarget(tickerId, gm.GetMessage()); }).Start();
                }
            }
        }




        public int GetNextSendOrderId()
        {
            return _dataAccess.GetNextAvailableSeedNumber(_osName);
        }

        //simulate sending order
        public bool SendOrder(Database.ClientOrder order)
        {

            ///call order now, simulation occur here
            _orderQueue.Enqueue(order);
            _orderEvent.Set();
            return true;
        }

        public void CancelOrder(int serverOrderNumber)
        {


        }


        public virtual void error(Exception e)
        {

        }

        public virtual void error(string str)
        {

        }

        public virtual void error(int id, int errorCode, string errorMsg)
        {

        }

        public virtual void connectionClosed()
        {
            
        }

        public virtual void currentTime(long time)
        {
            var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
            var date = posixTime.AddSeconds(time); //syn time
            SYSTEMTIME systime = new SYSTEMTIME();
            systime.wYear = (ushort)date.Year;
            systime.wMonth = (ushort)date.Month;
            systime.wDay = (ushort)date.Day;
            systime.wHour = (ushort)date.Hour;
            systime.wMinute = (ushort)(date.Minute);
            systime.wSecond = (ushort)(date.Second);
            systime.wMilliseconds = (ushort)(date.Millisecond);
            try
            {
                SetSystemTime(ref systime);
            }catch(Exception e)
            {

            }
            _synWithServer = true;
        }

        

        public virtual void tickString(int tickerId, int tickType, string value)
        {

        }

        public virtual void tickGeneric(int tickerId, int field, double value)
        {

        }

        public virtual void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureExpiry, double dividendImpact, double dividendsToExpiry)
        {
        }

        public virtual void tickSnapshotEnd(int tickerId)
        {
        }

        public void SyncServerTime()
        {
            _synWithServer = false;
            //_clientSocket.reqCurrentTime();
        }

        public virtual void nextValidId(int orderId)
        {
            _serverStop = false;
            System.Console.WriteLine("Successfully Connect to exchange");
            SyncServerTime();
            new Task(() =>
            {
                while (!_serverStop)
                {
                    _orderEvent.WaitOne();
                    Random ran = new Random(System.DateTime.Now.Millisecond);
                    int sec = ran.Next(10);
                    Thread.Sleep(sec * 100);
                    Database.ClientOrder or = null;
                    while (_orderQueue.TryDequeue(out or))
                    {
                        if (or.order_type == ConstantV.ORDER_TYPE_MARKET)
                        {
                            EquityMarket em = GetEquityMarket(or.equity_id);
                            double fill = 0;
                            if (or.is_buy)
                            {
                                fill = em.ask;
                            }
                            else
                            {
                                fill = em.bid;
                            }
                            orderStatus(or.server_order_id, ConstantV.ORDER_STATUS_FILLED, or.open_shares, 0, fill, 0, 0, fill, 0, "");
                        }
                        else
                        {
                            orderStatus(or.server_order_id, ConstantV.ORDER_STATUS_SUBMITTED, or.open_shares, 0, 0, 0, 0, 0, 0, "");
                        }
                    }
                }
            }
            ).Start();
        }

        public virtual void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
        }

        public virtual void managedAccounts(string accountsList)
        {
        }

        public virtual void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
        }

        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            
        }

        public virtual void accountSummaryEnd(int reqId)
        {
            
        }

        public virtual void updateAccountValue(string key, string value, string currency, string accountName)
        {
        }

        public virtual void updatePortfolio(Contract contract, int position, double marketPrice, double marketValue, double averageCost, double unrealisedPNL, double realisedPNL, string accountName)
        {

        }

        public virtual void updateAccountTime(string timestamp)
        {
        }

        public virtual void accountDownloadEnd(string account)
        {
        }

        public virtual void orderStatus(int orderId, string status, int filled, int remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
        {
            //do it here then invovle callback
            BusinessOrder bo = new BusinessOrder();
            bo.status = status;
            bo.avgFillPrice = (decimal)Math.Round(avgFillPrice,2);
            bo.filled = filled;
            bo.remaining = remaining;
            bo.server_order_id = orderId;
            Database.ClientOrder ord = _bUnit.ServerUpdateOrder(bo);
            if (ord != null)
            {
                _orderMessageTarget(ord);
            }

        }

        public virtual void openOrder(int orderId, Contract contract, IBApi.Order order, OrderState orderState)
        {
            
        }

        public virtual void openOrderEnd()
        {
            
        }

        public virtual void contractDetails(int reqId, ContractDetails contractDetails)
        {

        }

        public virtual void contractDetailsEnd(int reqId)
        {
        }

        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
        }

        public virtual void execDetailsEnd(int reqId)
        {
        }

        public virtual void commissionReport(CommissionReport commissionReport)
        {
        }

        public virtual void fundamentalData(int reqId, string data)
        {
        }

        public virtual void historicalData(int reqId, string date, double open, double high, double low, double close, int volume, int count, double WAP, bool hasGaps)
        {
            
        }

        public virtual void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            

        }

        public virtual void marketDataType(int reqId, int marketDataType)
        {
        }

        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {


        }

        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
        {
        }

        public virtual void updateNewsBulletin(int msgId, int msgType, String message, String origExchange)
        {
        }

        public virtual void position(string account, Contract contract, int pos, double avgCost)
        {
            
        }

        public virtual void positionEnd()
        {
        }

        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
        }

        public virtual void scannerParameters(string xml)
        {
        }

        public virtual void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
        }

        public virtual void scannerDataEnd(int reqId)
        {
        }

        public virtual void receiveFA(int faDataType, string faXmlData)
        {
        }

        public virtual void bondContractDetails(int requestId, ContractDetails contractDetails)
        {
        }








    }
}
