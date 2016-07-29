using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.Message;
using Database;
using shareshare.PriceServer;
using Infrastructure;

namespace shareshare.Business
{
    public class CashAccount
    {
        protected Account _account;
        protected DbAccess _dataAccess;

        public CashAccount(DbAccess dba, Account ac)
        {
            _dataAccess = dba;
            _account = ac;
        }

        public virtual bool CanTrade()
        {
            return false;
        }

        public bool TransferMoneyTo(CashAccount to, decimal amount)
        {
            if (_account.currency != to._account.currency)
            {
                return false;
            }
            if (TransferOut(amount, _account.currency))
            {
                if (!to.TransferIn(amount, _account.currency))
                {
                    this.LogError("Business error when transfer money");
                    return false;
                }
                return true;
            }
            return false;
        }


        public bool TransferOut(decimal amount, string currency)
        {
            if (_account.currency != currency || amount < 0)
            {
                return false;
            }
            while (true)
            {
                if (_account.cash_balance >= amount)
                {
                    _account.cash_balance -= amount;
                }
                else
                {
                    return false;
                }
                if (!_dataAccess.SaveAccount(_account))
                {
                    _account = _dataAccess.GetAccountById(_account.account_id);
                }
                else
                {
                    return true;
                }
            }
        }

        public bool TransferIn(decimal amount, string currency)
        {
            if (_account.currency != currency || amount < 0)
            {
                return false;
            }
            while (true)
            {
                _account.cash_balance += amount;
                if (!_dataAccess.SaveAccount(_account))
                {
                    _account = _dataAccess.GetAccountById(_account.account_id);
                }
                else
                {
                    return true;
                }
            }
        }

    }


    public class RegularAccount:CashAccount
    {

        protected IExchangeServer _iExchange;
        public RegularAccount(DbAccess dba, IExchangeServer exchange, Account ac):base(dba,ac)
        {
            _iExchange = exchange;
        }

        public override bool CanTrade()
        {
            return true;
        }

        public bool CreateOrder(ClientOrder corder)
        {
            
            while (true)
            {
                if (_account.orders == null)
                {
                    _account.orders = new Dictionary<int,ClientOrder>();
                }
                corder.server_order_id  = _iExchange.GetNextSendOrderId();
                if (_account.orders.ContainsKey(corder.server_order_id))
                {
                    return false;
                }
                _account.orders[corder.server_order_id] = corder;
                if (!_dataAccess.SaveAccount(_account))
                {
                    _account = _dataAccess.GetAccountById(_account.account_id);
                }
                else
                {
                    break;
                }
            }
            _iExchange.SendOrder(corder);
            return true;
        }


        public virtual bool CheckRisk(NewOrderReqMessage req, out string msg)
        {
            msg = "";
            if (req.is_buy == false) //sell
            {
                if (_account.holdings != null)
                {
                    Holding b = null;
                    if (_account.holdings.TryGetValue(req.equity_id, out b))
                    {
                        int totalneed = req.quantity;
                        foreach (var v in _account.orders)
                        {
                            if (v.Key == req.equity_id && v.Value.is_buy == false
                                && v.Value.status != ConstantV.ORDER_STATUS_FILLED && v.Value.status != ConstantV.ORDER_STATUS_CANCELLED)
                            {
                                totalneed += (v.Value.open_shares + v.Value.fill_shares);
                                if (totalneed <= b.position)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                         
                msg = "You do not have enough shares to sell, check if you have live orders";       
                return false;
            }
            else
            {
                decimal cost = 0;
                if (req.order_type == ConstantV.ORDER_TYPE_LIMIT)
                {
                    cost = req.quantity * req.price;
                }
                else if (req.order_type == ConstantV.ORDER_TYPE_MARKET)
                {
                    //here we need to check if market close
                    if (Util.IfMarketOpenNow())
                    {
                        EquityMarket em = _iExchange.GetEquityMarket(req.equity_id);
                        if (em == null || em.ask <= 0 || em.bid <= 0 || em.asksize <= 0 || em.bidsize <= 0)
                        {
                            msg = "Server error, it is market hour, but not price feed";
                            return false;
                        }
                        cost = (decimal)Math.Round(req.quantity * em.ask, 2);
                        req.price = cost;
                    }
                    else
                    {
                        cost = req.quantity * Util.GetLastTradePrice(req.equity_id);
                        req.price = cost;
                    }
                }
                foreach (var v in _account.orders)
                {
                    if (v.Value.is_buy == true && v.Value.status != ConstantV.ORDER_STATUS_FILLED
                        && v.Value.status != ConstantV.ORDER_STATUS_CANCELLED)
                    {
                        cost += (v.Value.open_shares * v.Value.price + v.Value.fill_shares * v.Value.ave_fill_price);
                        if (cost > _account.cash_balance)
                        {
                            msg = "You do not have enough cash to buy in this account";
                            return false;
                        }
                    }
                }
                return true;

            }
        }

        /*here we need to think about it*/
        public virtual void FinishTrade(ClientOrder or, out Activity act)
        {
            while (true)
            {

                act = null;
                if (or.fill_shares > 0)
                {
                    decimal cost = or.fill_shares * or.ave_fill_price;
                    if (or.is_buy)
                    {

                        _account.cash_balance -= cost;
                        Holding b = null;
                        if (_account.holdings.TryGetValue(or.equity_id,out b))
                        {
                            b.total_cost += or.ave_fill_price * or.fill_shares;
                            b.position += or.fill_shares;
                            b.average_price = Math.Round(b.total_cost / b.position, 3);
                        }
                        else
                        {
                            b = new Holding();
                            b.ss_id = or.equity_id;
                            b.position = or.fill_shares;
                            b.average_price = or.ave_fill_price;
                            b.is_buy = true;
                            b.total_cost = cost;
                            if (_account.holdings == null)
                            {
                                _account.holdings = new Dictionary<int, Holding>();
                            }
                        }
                        act = new Activity();
                        act.account_id = _account.account_id;
                        act.amount = cost;
                        act.inflow = !or.is_buy;
                    }
                    else
                    {
                        Holding b = null; 
                        if (_account.holdings.TryGetValue(or.equity_id, out b) && b.position >= or.fill_shares)
                        {
                            _account.cash_balance += cost;
                            b.position -= or.fill_shares;
                            if (b.position == 0)
                            {
                                _account.holdings.Remove(or.equity_id);
                            }
                            act = new Activity();
                            act.account_id = _account.account_id;
                            act.amount = cost;
                            act.inflow = !or.is_buy;
                        }
                        else
                        {
                            //server error
                        }

                    }
                }
                else
                {
                    //server error
                }


                if (!_dataAccess.SaveAccount(_account))
                {
                    _account = _dataAccess.GetAccountById(_account.account_id);
                }
                else
                {
                    break;
                }

            }

            
        }
    }


    public class ShortAccount : RegularAccount
    {
        public ShortAccount(DbAccess dba, IExchangeServer exchange, Account ac) : base(dba, exchange,ac) { }


        public override bool CanTrade()
        {
            return true;
        }

        public override bool CheckRisk(NewOrderReqMessage req, out string msg)
        {
            msg = "";
            if (req.is_buy == true) //buy
            {
                if (_account.holdings != null)
                {
                    Holding b = null;// _account.holdings.FirstOrDefault(e => e.ss_id == req.equity_id);
                    if (_account.holdings.TryGetValue(req.equity_id, out b) && b != null)
                    {
                        int totalneed = req.quantity;
                        foreach (var v in _account.orders)
                        {
                            if (v.Value.equity_id == req.equity_id && v.Value.is_buy == false
                                && v.Value.status != ConstantV.ORDER_STATUS_FILLED && v.Value.status != ConstantV.ORDER_STATUS_CANCELLED)
                            {
                                totalneed += (v.Value.open_shares + v.Value.fill_shares); //this is positive value
                                if (totalneed + b.position <= 0)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                msg = "You do not have enough short position to close, check if you have live orders";
                return false;
            }
            else //short sell
            {
                int totalshort = req.quantity;
                if ((_account.holdings != null) &&
                   (_account.holdings.Count > 0 && _account.holdings[0].ss_id == req.equity_id))
                {
                    totalshort += -1 * _account.holdings[0].position;
                }
                foreach (var v in _account.orders)
                {
                    if (v.Value.is_buy == false && v.Value.status != ConstantV.ORDER_STATUS_FILLED
                        && v.Value.status != ConstantV.ORDER_STATUS_CANCELLED)
                    {
                        totalshort += (v.Value.open_shares + v.Value.fill_shares);
                    }
                }

                decimal coverMoney = 0.0M;
                if (Util.IfMarketOpenNow())
                {
                    EquityMarket em = _iExchange.GetEquityMarket(req.equity_id);
                    if (em == null || em.ask <= 0 || em.bid <= 0 || em.asksize <= 0 || em.bidsize <= 0)
                    {
                        msg = "Server error, it is market hour, but not price feed";
                        return false;
                    }
                    coverMoney = (decimal)Math.Round(req.quantity * em.ask, 2);
                }
                else
                {
                    coverMoney = req.quantity * Util.GetLastTradePrice(req.equity_id);
                }

                decimal historyJumpPerDay = 0.50M;
                if (coverMoney * (1 + historyJumpPerDay) > _account.cash_balance)
                {
                    return true;
                }
                else
                {
                    msg = string.Format("You need total {0} cash to finsih this trade", coverMoney);
                    return false;
                }

            }

        }
        



        public override void FinishTrade(ClientOrder or, out Activity act)
        {
            act = null;

        }

    }

}
