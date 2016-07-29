using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Net.Mail;

namespace Infrastructure
{
    public class Util
    {

        public const string KEY_RESTFULURL = "SSServiceURL";
        public const string KEY_CLIENTNUM = "ClientNum";
        public const string KEY_WEBSERVERPORT = "WebServerPort";
        public const string KEY_WEBSERVERURL = "WebServerURL";


        public const string KEY_EMAILSERVER = "EmailServerName";
        public const string KEY_EMAILPORT = "EmailServerPort";
        public const string KEY_EMAILUSERNAME = "EmailUserName";
        public const string KEY_EMAILPASSWORD = "EmailPassword";


        static readonly TimeSpan MARKET_OPEN = new TimeSpan(09, 30, 0);
        static readonly TimeSpan MARKET_CLOSE = new TimeSpan(16, 0, 0);
        static readonly TimeSpan PRE_MARKET_OPEN = new TimeSpan(04, 0, 0);
        static readonly TimeSpan POST_MARKET_CLOSE = new TimeSpan(20, 0, 0);


        /*
        public static bool IfPreMarketOpenNow()
        {
            DateTime dt = GetCurrentExchangeTime();
            if ((dt.Day >= (int)DayOfWeek.Monday) && (dt.Day <= (int)DayOfWeek.Friday))
            {
                if (dt.TimeOfDay > MARKET_OPEN && dt.TimeOfDay < MARKET_CLOSE)
                {
                    return true;
                }
            }
            return false;
        }*/


        public static DateTime GetNextAvailableTradeMarketDate()
        {
            DateTime dt = GetCurrentExchangeTime();
            DateTime ret;
            if ((dt.DayOfWeek >= DayOfWeek.Monday) && (dt.DayOfWeek <= DayOfWeek.Friday))
            {
                if (dt.TimeOfDay >= MARKET_CLOSE)
                {

                    if (dt.DayOfWeek == DayOfWeek.Friday)
                    {
                        ret = dt.AddDays(3);
                    }
                    else
                    {
                        ret = dt.AddDays(1);
                    }
                }
                else if (dt.TimeOfDay < MARKET_OPEN)
                {
                    ret = dt;
                }
                else
                {
                    return dt;
                }

            }
            else if (dt.DayOfWeek == DayOfWeek.Saturday)//weekend
            {
                ret = dt.AddDays(2);
            }
            else
            {
                ret = dt.AddDays(1);
            }
            return new DateTime(ret.Year, ret.Month, ret.Day, 9, 30, 0, 0, ret.Kind);

        }

        public static DateTime GetNextAvailableTradeLimitDate()
        {
            DateTime dt = GetCurrentExchangeTime();
            DateTime ret;
            if ((dt.DayOfWeek >= DayOfWeek.Monday) && (dt.DayOfWeek <= DayOfWeek.Friday))
            {
                if (dt.TimeOfDay >= POST_MARKET_CLOSE)
                {

                    if (dt.DayOfWeek == DayOfWeek.Friday)
                    {
                        ret = dt.AddDays(3);
                    }
                    else
                    {
                        ret =dt.AddDays(1);
                    }
                }
                else if (dt.TimeOfDay < PRE_MARKET_OPEN)
                {
                    ret = dt;
                }
                else
                {
                    return dt;
                }

            }
            else if (dt.DayOfWeek == DayOfWeek.Saturday)//weekend
            {
                ret =dt.AddDays(2);
            }
            else
            {
                ret = dt.AddDays(1);
            }
            return  new DateTime(ret.Year,ret.Month, ret.Day,4,0,0,0,ret.Kind);

        }

        public static bool IfMarketOpenNow()
        {
            DateTime dt = GetCurrentExchangeTime();
            if ((dt.Day >= (int)DayOfWeek.Monday) && (dt.Day <= (int)DayOfWeek.Friday))
            {
                if (dt.TimeOfDay > MARKET_OPEN && dt.TimeOfDay < MARKET_CLOSE)
                {
                    return true;
                }
            }
            return false;
        }



        public static DateTime GetCurrentExchangeTime()
        {
            DateTime currentTime = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Utc.Id, "Eastern Standard Time");
        }

        public static decimal GetLastTradePrice(int equityid)
        {
            Random r = new Random(equityid);
            int p = r.Next(100) + 1;
            double c = (r.NextDouble() + 0.1)*p;
            return (decimal)(Math.Round(c, 2));
            
        }

        public static double GetHistoryJumpPercentageOneDay(int equityid)
        {
            Random r = new Random(equityid);
            double c = r.NextDouble() + 0.1; 
            return c;

        }

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string ReadSetting(string key)
        {
            string ret = null;
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

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                string ret = Path.GetDirectoryName(path);
                System.Console.WriteLine("log file folder is:" + ret);
                return ret;
            }
        }

    }
}
