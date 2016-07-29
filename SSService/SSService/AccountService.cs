using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Database;

namespace SSService
{
    public partial class SSService
    {

        public List<Account> GetAccounts()
        {
            ServiceSecurityContext securityCtx;
            securityCtx = OperationContext.Current.ServiceSecurityContext;
            string userName = securityCtx.PrimaryIdentity.Name;
            return _db.GetAccountsById(userName);
        }


        public string ActivateAccount(string token)
        {
            RegisterToken tk = _db.GetRegisterToken(token);
            if (tk != null)
            {
                Customer cs = _db.GetCustomerByUser(tk.username);
                if (cs != null && cs.is_activated == false)
                {
                    cs.is_activated = true;
                    _db.SaveCustomer(cs);
                    _db.DeleteRegisterToken(token);
                    return "Successfully activated your account!";
                }
                else
                {
                    _db.DeleteRegisterToken(token);
                    return "Server error, already activated";
                }
            }
            else
            {
                return "Invalid token!";
            }
        }


        public List<Account> GetAccounts(string username)
        {
            return _db.GetAccountsById(username);
        }


    }
}
