using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using shareshare.Business;


namespace shareshare.Message
{
    public abstract class RequestMessage : GeneralMessage
    {
        BusinessUnit _bunit;
        public BusinessUnit Bunit
        {
            get { return _bunit; }
            set { _bunit = value; }
        }

        public virtual GeneralMessage ValidRequest()
        {
            return null;
        }

        public abstract bool HasResponse();

        public virtual string HandleMessage()
        {
            return string.Empty;
        }

    }
}
