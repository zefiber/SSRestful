using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace shareshare.Business
{
    public class BusinessOrder
    {

        public int server_order_id { get; set; }
        //open //cancel // filled // partial filled //expired
        public string status { get; set; }

        public int filled { get; set; }

        public int remaining { get; set; }

        public decimal avgFillPrice { get; set; }

        public decimal lastFillPrice { get; set; }
    }



}
