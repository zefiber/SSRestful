using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using shareshare.PriceServer;
using Infrastructure;
using Database;
using shareshare.Business;

namespace shareshare.Message
{
    /*register message*/
    [JsonObject(MemberSerialization.OptIn)]
    public class RegisterRequestMessage : RequestMessage
    {
        const int MININUM_USERNAME = 5;
        const int MINIMUN_PASSWORD = 4;
        const decimal REGISTER_AWARD = 100000.00M;

        private DbAccess _dataAccess;
        private EmailService _emailService;

        public RegisterRequestMessage(DbAccess dba, EmailService emailservice)
        {
            _dataAccess = dba;
            _emailService = emailservice;
            action = Constant.MSG_REGISTERREQ;
        }

        public RegisterRequestMessage()
        {
            action = Constant.MSG_REGISTERREQ;
        }

        [JsonProperty]
        public string username { get; set; }

        [JsonProperty]
        public string password { get; set; }

        [JsonProperty]
        public string email { get; set; }

        public override GeneralMessage ValidRequest()
        {
            RegisterResponseMessage resp = null;
            if (username.Length < MININUM_USERNAME)
            {
                resp = new RegisterResponseMessage();
                resp.failreason = string.Format("username length at least is {0}! ", MININUM_USERNAME);
                resp.success = false;
                return resp;

            }
            if (password.Length < MINIMUN_PASSWORD)
            {
                resp = new RegisterResponseMessage();
                resp.success = false;
                resp.failreason = string.Format("password length at least is {0}! ", MINIMUN_PASSWORD);
                return resp;
            }
            if (!Util.IsValidEmail(email))
            {
                resp = new RegisterResponseMessage();
                resp.success = false;
                resp.failreason = string.Format("email format should be XXX@XXX.XXX");
                return resp;
            }
            return resp;
        }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override bool HasResponse()
        {
            return true;
        }

        public override string HandleMessage()
        {
            RegisterResponseMessage resp = new RegisterResponseMessage();
            Customer cs = new Customer()
            {
                username = username,
                password = password,
                email = email
            };
            if (!_dataAccess.SaveCustomer(cs))
            {
                resp.success = false;
                resp.failreason = string.Format("username already exists please try another one");
            }
            else
            {
                resp.account = _dataAccess.GetNewAvailbleAccount(username,REGISTER_AWARD, ConstantV.ACCOUNT_TYPE_REGULAR).account_id;
                resp.success = true;
                cs.accounts = new HashSet<ulong>();
                cs.accounts.Add(resp.account);
                string wlname = "watchlist1";
                cs.watch_list = new Dictionary<string, HashSet<int>>();
                cs.watch_list.Add(wlname,null);
                RegisterToken tk = new RegisterToken()
                {
                    username = cs.username,
                    token_id = Guid.NewGuid().ToString()
                };
                cs.token = tk.token_id;
                cs.is_activated = false;
                _dataAccess.SaveRegisterToken(tk);
                _dataAccess.SaveCustomer(cs);
                _emailService.SendEmail(cs.email, cs.token);

            }
            return resp.GetMessage();
        }


    }



    [JsonObject(MemberSerialization.OptIn)]
    public class RegisterResponseMessage : GeneralMessage
    {

        public RegisterResponseMessage()
        {
            action = Constant.MSG_REGISTERRESP;
        }

        [JsonProperty]
        public bool success { get; set; }

        [JsonProperty]
        public UInt64 account { get; set; }

        [JsonProperty]
        public string failreason { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }
}
