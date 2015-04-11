using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Hsr.Core.Log
{
    public class Log4Manager
    {
        public static readonly Log4Manager Instance = new Log4Manager();
        private static ILog _loggerInfo;
        private static ILog _loggerError;
        
        static Log4Manager()
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "log4Net.config");
            XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(path));
            _loggerInfo = LogManager.GetLogger("Logger_Info");
            _loggerError = LogManager.GetLogger("Logger_Error");
        }

        public void Error(string msg, Exception err)
        {
            _loggerError.Error(msg, err);
        }

        public void Info(string message)
        {
            _loggerInfo.Info(message);
        }
    }
 
}
