using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Threading;

namespace Infrastructure
{
    public enum LogMessageType { DEBUG, ERROR, WARNING, INFO };
    public delegate void GlobalLogChangedEventHandler(object sender, GlobalLogAttributes e);



    public static class MyLogExtension
    {
        public static void LogInfo(this Object obj, string message, params object[] args)
        {

            string sLogType;

            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty("IP");
            if (pi == null)
            {
                sLogType = obj.GetType().Name;
            }
            else
            {
                sLogType = pi.GetValue(obj, null).ToString();

            }

            string msg = String.Format(message, args);
            GlobalLog.LogInfo(sLogType, msg);
        }


        public static void LogError(this Object obj, string message, params object[] args)
        {
            string sLogType;

            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty("IP");
            if (pi == null)
            {
                sLogType = obj.GetType().Name;
            }
            else
            {
                sLogType = pi.GetValue(obj, null).ToString();

            }

            string msg = String.Format(message, args);
            GlobalLog.LogError(sLogType, msg);
        }

        public static void LogException(this Object obj, Exception e, string message, params object[] args)
        {
            string sLogType;

            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty("IP");
            if (pi == null)
            {
                sLogType = obj.GetType().Name;
            }
            else
            {
                sLogType = pi.GetValue(obj, null).ToString();

            }

            string msg = String.Format(message, args);
            GlobalLog.LogError(sLogType, msg);
            GlobalLog.LogError("EXCEPTION MESSAGE", e.Message);

            if (e.StackTrace != null)
            {
                GlobalLog.LogError("EXCEPTION STACKTRACE", e.StackTrace);
            }
            if (e.InnerException != null)
            {
                if (e.InnerException.Message != null)
                {
                    GlobalLog.LogError("EXCEPTION INNER MESSAGE", e.InnerException.Message);
                }
                if (e.InnerException.StackTrace != null)
                {
                    GlobalLog.LogError("EXCEPTION INNERSTACKTRACE", e.InnerException.StackTrace);
                }
            }

        }

        public static void LogWarning(this Object obj, string message, params object[] args)
        {
            string sLogType;

            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty("IP");
            if (pi == null)
            {
                sLogType = obj.GetType().Name;
            }
            else
            {
                sLogType = pi.GetValue(obj, null).ToString();

            }

            string msg = String.Format(message, args);
            GlobalLog.LogWarning(sLogType, msg);
        }


        public static void LogDebug(this Object obj, string message, params object[] args)
        {
            string sLogType;

            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty("IP");
            if (pi == null)
            {
                sLogType = obj.GetType().Name;
            }
            else
            {
                sLogType = pi.GetValue(obj, null).ToString();

            }

            string msg = String.Format(message, args);
            GlobalLog.LogDebug(sLogType, msg);
        }

    }



    public class GlobalLogAttributes
    {
        public string Tag
        {
            get;
            set;
        }
        public string ThreadId
        {
            get;
            set;
        }
        public string Time
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public override string ToString()
        {
            return String.Format("{0}: [tid:{1}] [{2}] {3} {4}", Tag, ThreadId, Time, Category, Message);
        }
    }



    public static class GlobalLog
    {

        public static string FileName = null;
        private static StreamWriter _writer;
        public static event GlobalLogChangedEventHandler LogChanged;
        static GlobalLog()
        {
            FileName = String.Format(@"{0}\{1}-{2}.log",Util.AssemblyDirectory, Process.GetCurrentProcess().ProcessName, System.DateTime.Today.DayOfWeek);

            FileInfo fInfo = new FileInfo(FileName);

            bool bCreateNew;
            if (fInfo.Exists)
            {
                bCreateNew = false;
                TimeSpan ts = System.DateTime.Now - fInfo.LastWriteTime;
                if (ts.Days > 2)
                    bCreateNew = true;
            }
            else
            {
                bCreateNew = true;
            }


            if (bCreateNew)
            {
                _writer = new StreamWriter(FileName, false);
            }
            else
            {
                _writer = new StreamWriter(FileName, true);
            }
            _writer.AutoFlush = true;
        }

        public static void close()
        {
            _writer.Close();
        }

        public static void LogMessage(LogMessageType messageType, string message, params object[] args)
        {
            string msg = String.Format(message, args);
            if (messageType == LogMessageType.DEBUG)
            {
                LogDebug("", msg);
            }
            else if (messageType == LogMessageType.ERROR)
            {
                LogError("", msg);
            }
            else if (messageType == LogMessageType.WARNING)
            {
                LogWarning("", msg);
            }
            else if (messageType == LogMessageType.INFO)
            {
                LogInfo("", msg);
            }
        }

        [Conditional("DEBUG")]
        public static void LogDebug(string stype, string message)
        {
            GenerateMessage("DEBUG", stype, message);
        }

        [Conditional("DEBUG")]
        public static void LogDebug(string stype, string message, params object[] args)
        {
            string msg = String.Format(message, args);
            LogDebug(stype, msg);
        }

        public static void LogError(string stype, string message)
        {
            GenerateMessage("Error", stype, message);
        }

        public static void LogError(string stype, string message, params object[] args)
        {
            string msg = String.Format(message, args);
            LogError(stype, msg);
        }

        public static void LogWarning(string stype, string message)
        {
            GenerateMessage("Warning", stype, message);
        }

        public static void LogWarning(string stype, string message, params object[] args)
        {
            string msg = String.Format(message, args);
            LogWarning(stype, msg);
        }


        public static void LogInfo(string stype, string message)
        {
            GenerateMessage("Info", stype, message);
        }


        public static void LogInfo(string stype, string message, params object[] args)
        {
            string msg = String.Format(message, args);
            LogInfo(stype, msg);
        }


        private static void GenerateMessage(string tag, string stype, string message)
        {
            
            GlobalLogAttributes attri = new GlobalLogAttributes { Tag = tag, ThreadId = Thread.CurrentThread.ManagedThreadId.ToString(), Time = System.DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss.fff"), Category = stype, Message = message };

            lock (_writer)
            {
                if (_writer.BaseStream != null)
                {
                    _writer.WriteLine(attri.ToString());
                    _writer.Flush();
                }
            }

            RaiseLogChangeEvent(attri);

        }

        public static void RaiseLogChangeEvent(GlobalLogAttributes log)
        {
            if (LogChanged != null)
            {
                LogChanged(null, log);
            }
        }

    }
}
