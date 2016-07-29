using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shareshare.PriceServer
{
    interface ISSPriceServer
    {
        void RegisterToService();
        bool Start();
    }
}
