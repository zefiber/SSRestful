using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;

namespace SSService
{
    public partial class SSService
    {
        public GeneralMessage RegisterUser(Customer cus)
        {
            GeneralMessage gm = new GeneralMessage();

            gm.success = true;
            return gm;
        }

    }
}
