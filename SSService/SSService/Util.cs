using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace SSService
{
    public class Util
    {
        public const string KEY_SERVICEURL = "SERVICEURI";

        public static string ReadSetting(string key)
        {
            string ret = null ;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                ret = appSettings[key] ?? null;
                
            }
            catch (ConfigurationErrorsException)
            {
                
            }
            return ret;
        }

    }
}
