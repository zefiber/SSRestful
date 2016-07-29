using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace SSService
{


    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var ss = new SSService();
                string uri = Util.ReadSetting(Util.KEY_SERVICEURL) ?? "http://localhost:8777/";
                var host = new ServiceHost(ss, new Uri(uri));
                host.Open();
                Console.WriteLine("Security Service is running....");
                Console.ReadKey();
                host.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
