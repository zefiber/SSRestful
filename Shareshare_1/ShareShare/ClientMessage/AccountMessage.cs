using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.PriceServer;
using Newtonsoft.Json;
using shareshare.Business;
using Database;

namespace shareshare.Message
{
    /*account message*/
    [JsonObject(MemberSerialization.OptIn)]
    public class CreateAccountRequestMessage : RequestMessage
    {
        const int MAX_ACCOUNT_NUMBER = 5;
        const decimal UNLIMIT_ACCOUNT_FEE = 0.99M;
        private DbAccess _dataAccess;
        
        public CreateAccountRequestMessage(DbAccess db)
        {
            action = Constant.MSG_CREATE_ACCOUNT;
            _dataAccess = db;
        }

        public CreateAccountRequestMessage()
        {
            action = Constant.MSG_CREATE_ACCOUNT;
        }

        [JsonProperty]
        public decimal original_deposit { get; set; }


        [JsonProperty]
        public string account_type { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override GeneralMessage ValidRequest()
        {
            AccountResponseMessage ret = null;
            if (!Bunit.IfLogin)
            {
                ret = new AccountResponseMessage();
                ret.fail_reason = "You need to login first";
                ret.success = false;
                return ret;

            }
            if (original_deposit < 0)
            {
                ret = new AccountResponseMessage(); 
                ret.fail_reason = "Negative deposit amount";
                ret.success = false;
                return ret;
            }
            if (account_type != ConstantV.ACCOUNT_TYPE_REGULAR
                && account_type != ConstantV.ACCOUNT_TYPE_SHORT
                && account_type != ConstantV.ACCOUNT_TYPE_CASH)
            {
                ret = new AccountResponseMessage(); 
                ret.fail_reason = "Invalid account type";
                ret.success = false;
                return ret;
            }
            return ret;
        }

        public override bool HasResponse()
        {
            return true;
        }

        public override string HandleMessage()
        {
            AccountResponseMessage resp = new AccountResponseMessage();
            Customer cs = _dataAccess.GetCustomerByUser(Bunit.UserName);
            if (cs.accounts.Count == MAX_ACCOUNT_NUMBER)
            {
                resp.success = false;
                resp.fail_reason = string.Format("Failed to create account, reach maximun {0}. pay {1} dollar to create unlimit number of accounts", MAX_ACCOUNT_NUMBER, UNLIMIT_ACCOUNT_FEE);
            }
            else
            {
                Account acc = _dataAccess.GetNewAvailbleAccount(Bunit.UserName,original_deposit, account_type);
                cs.accounts.Add(acc.account_id);
                resp.account_id = acc.account_id;
                resp.success = true;
                _dataAccess.SaveCustomer(cs);
            }
            return resp.GetMessage(); ;
        }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class AccountTransferMessage : RequestMessage
    {
        private DbAccess _dataAccess;
        public AccountTransferMessage(DbAccess db)
        {
            _dataAccess = db;
            action = Constant.MSG_ACCOUNT_TRANSFER_CASH;
        }

        public AccountTransferMessage()
        {
            action = Constant.MSG_ACCOUNT_TRANSFER_CASH;
        }

        [JsonProperty]
        public UInt64 from_account_id { get; set; }


        [JsonProperty]
        public UInt64 to_account_id { get; set; }

        [JsonProperty]
        public decimal amount { get; set; }


        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override GeneralMessage ValidRequest()
        {
            AccountResponseMessage ret = null;
            if (!Bunit.IfLogin)
            {
                ret = new AccountResponseMessage();
                ret.fail_reason = "You need to login first";
                ret.success = false;
                return ret;

            }
            if (amount < 0)
            {
                ret = new AccountResponseMessage();
                ret.fail_reason = "Negative transfer amount";
                ret.success = false;
                return ret;
            }
            return ret;
        }

        public override string HandleMessage()
        {
            AccountResponseMessage resp = new AccountResponseMessage();
            Account from = _dataAccess.GetAccountById(from_account_id);
            if (from != null && from.owner_ids.Contains(Bunit.UserName))
            {
                Account to = _dataAccess.GetAccountById(to_account_id);
                if (to != null && to.owner_ids.Contains(Bunit.UserName))
                {
                    CashAccount fcash = new CashAccount(_dataAccess, from);
                    CashAccount tcash = new CashAccount(_dataAccess, to);
                    if (fcash.TransferMoneyTo(tcash, amount))
                    {
                        resp.account_id = from_account_id;
                        resp.success = true;
                    }
                    else
                    {
                        resp.fail_reason = string.Format("Failed to transfer, no enough money");
                        resp.success = false;
                    }
                }
                else
                {
                    resp.fail_reason = string.Format("Invalid account or ownership {0} in database", to_account_id);
                    resp.success = false;
                }
            }
            else
            {
                resp.fail_reason = string.Format("Invalid account or ownership {0} in database", from_account_id);
                resp.success = false;
            }

            return resp.GetMessage(); ;
        }

        public override bool HasResponse()
        {
            return true;
        }
    }


    
    [JsonObject(MemberSerialization.OptIn)]
    public class ResetAccountMessage : RequestMessage
    {
        private DbAccess _dataAccess;
        public ResetAccountMessage(DbAccess db)
        {
            action = Constant.MSG_RESET_ACCOUNT;
            _dataAccess = db;
        }

        public ResetAccountMessage()
        {
            action = Constant.MSG_RESET_ACCOUNT;
        }

        [JsonProperty]
        public UInt64 account_id { get; set; }

        [JsonProperty]
        public decimal new_balance { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        

        public override GeneralMessage ValidRequest()
        {
            AccountResponseMessage ret = null;
            if (!Bunit.IfLogin)
            {
                ret = new AccountResponseMessage();
                ret.fail_reason = "You need to login first";
                ret.success = false;
                return ret;

            }
            if (new_balance < 0)
            {
                ret = new AccountResponseMessage();
                ret.fail_reason = "Negative new balance amount";
                ret.success = false;
                return ret;
            }
            return ret;
        }

        public override bool HasResponse()
        {
            return true;
        }


        public override string HandleMessage()
        {
            AccountResponseMessage resp = new AccountResponseMessage();
            Account ac = _dataAccess.GetAccountById(account_id);
            if (ac != null && ac.owner_ids.Contains(Bunit.UserName))
            {
                ac.holdings = null;
                ac.cash_balance = new_balance;
                ac.orders = null;
                for (; ; )
                {
                    if (_dataAccess.SaveAccount(ac))
                    {
                        break;
                    }
                    ac = _dataAccess.GetAccountById(account_id);
                }
                resp.success = true;
            }
            else
            {
                resp.success = false;
                resp.fail_reason = string.Format("Invalid account or account ownership {0}",account_id);
            }

            return resp.GetMessage();
        }
    }




    [JsonObject(MemberSerialization.OptIn)]
    public class AccountResponseMessage : GeneralMessage
    {

        public AccountResponseMessage()
        {
            action = Constant.MSG_ACCOUNT_RESP;
        }

        [JsonProperty]
        public UInt64 account_id { get; set; }

        [JsonProperty]
        public bool success { get; set; }

        [JsonProperty]
        public string fail_reason { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }

}
