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
    /*login message*/
    [JsonObject(MemberSerialization.OptIn)]
    public class LoginRequestMessage : RequestMessage
    {
        private DbAccess _dataAccess;
        private EmailService _emailService;
        public LoginRequestMessage(DbAccess dba, EmailService emailservice)
        {
            action = Constant.MSG_LOGINREQ;
            _dataAccess = dba;
            _emailService = emailservice;
        }

        public LoginRequestMessage()
        {
            action = Constant.MSG_LOGINREQ;
        }

        [JsonProperty]
        public string username { get; set; }

        [JsonProperty]
        public string password { get; set; }

        [JsonProperty]
        public bool kickothersession { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override GeneralMessage ValidRequest()
        {
            LoginResponseMessage ret = null;
            if(string.IsNullOrEmpty(username))
            {
                ret = new LoginResponseMessage();
                ret.failreason = "please input your username";
                ret.success = false;
                return ret;
            }
            if (string.IsNullOrEmpty(password))
            {
                ret = new LoginResponseMessage();
                ret.failreason = "please input your password";
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
            LoginResponseMessage ret = new LoginResponseMessage();
            Customer customer = _dataAccess.GetCustomerByUser(username);
            if (customer == null)
            {
                ret.success = false;
                ret.failreason = string.Format("Invalid Username: {0}", username);
            }
            else
            {
                if (customer.password == password)
                {
                    if (customer.is_activated)
                    {
                        if (!kickothersession && customer.connect_status)
                        {
                            ret.success = false;
                            ret.failreason = string.Format("Device {0} is currently connected", customer.last_device_id);
                        }
                        else
                        {
                            ret.success = true;
                            ret.token = Guid.NewGuid().ToString();
                            AccessToken tk = new AccessToken { token = ret.token, username = username };
                            _dataAccess.SaveAccessToken(tk);
                            Bunit.IfLogin = true;
                            Bunit.UserName = username;
                        }
                    }
                    else
                    {
                        ret.success = false;
                        ret.failreason = string.Format("Please check your email:{0} to activate the account", customer.email);
                        _emailService.SendEmail(customer.email, customer.token);
                        //send email to activate token
                    }
                }
                else
                {
                    ret.success = false;
                    ret.failreason = string.Format("Invalid Password", customer.last_device_id);
                }
            }
            return ret.GetMessage();
        }
    }



    [JsonObject(MemberSerialization.OptIn)]
    public class LoginResponseMessage : GeneralMessage
    {

        public LoginResponseMessage()
        {
            action = Constant.MSG_LOGINREQ;
        }

        [JsonProperty]
        public bool success { get; set; }

        [JsonProperty]
        public string token { get; set; }

        [JsonProperty]
        public string failreason { get; set; }

        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }
    }




    [JsonObject(MemberSerialization.OptIn)]
    public class LogoutRequestMessage : RequestMessage
    {
        public LogoutRequestMessage()
        {
            action = Constant.MSG_LOGOUTREQ;
        }

        
        public override string GetMessage()
        {
            return JsonConvert.SerializeObject(this) + Constant.MSG_DELIMETER;
        }

        public override string HandleMessage()
        {
            Bunit.LogoutToServer();
            return string.Empty;
        }
     
        public override bool HasResponse()
        {
            return false;
        }
    }

}
