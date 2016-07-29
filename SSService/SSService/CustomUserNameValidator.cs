using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IdentityModel.Selectors;
using Database;

namespace SSService
{
    public class CustomUserNameValidator : UserNamePasswordValidator
    {
        // This method validates users. It allows in two users, user1 and user2
        // This code is for illustration purposes only and
        // must not be used in a production environment because it is not secure.

        DbAccess _db = Resource.Instance._Database;

        public override void Validate(string userName, string password)
        {

            if (null == userName || null == password)
            {
                throw new ArgumentNullException("You must provide both the username and token to access this service");
            }
            AccessToken tk = _db.GetAccessToken(userName, password);
            if (tk == null || tk.token != password)
            {
                // This throws an informative fault to the client.
                throw new FaultException("Unknown Username or Incorrect Password");
                // When you do not want to throw an informative fault to the client,
                // throw the following exception.
                // throw new SecurityTokenException("Unknown Username or Incorrect Password");
            }
        }
    }
}
