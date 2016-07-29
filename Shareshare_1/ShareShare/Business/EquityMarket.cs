using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shareshare.Business
{
    public class EquityMarket
    {
        public EquityMarket()
        {
            bidsize = 0;
            asksize = 0;
            ask = -1;
            bid = -1;
        }
       
        public double ask { get; set; }
     
        public double bid { get; set; }
        
        public int bidsize { get; set; }

        public int asksize { get; set; }

    }


}
