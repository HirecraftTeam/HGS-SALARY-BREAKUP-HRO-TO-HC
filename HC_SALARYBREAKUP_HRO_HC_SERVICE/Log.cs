using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HireCraft.HM_APIService
{
    public class Log
    {
        #region Declaration

        public enum Status { Fatal = 1, Error, Debug, Info }

        #endregion

        public static void LogData(string logDataString, Status st)
        {
            try
            {
                
                log4net.Config.XmlConfigurator.Configure();
                log4net.ILog log = log4net.LogManager.GetLogger("HireCraft.HM_APIService API Logger");
                switch (st)
                {
                    case Status.Fatal:
                        log.Fatal(System.DateTime.Now + ": " + logDataString);
                        break;
                    case Status.Error:
                        log.Error(System.DateTime.Now + ": " + logDataString);
                        break;
                    case Status.Debug:
                        log.Debug(System.DateTime.Now + ": " + logDataString);
                        break;
                    case Status.Info:
                        log.Info(System.DateTime.Now + ": " + logDataString);
                        break;
                    default:
                        log.Info(System.DateTime.Now + ": " + logDataString);
                        break;
                }
            }
            catch
            {
                //Debug.Assert(false, ex.Message, ex.StackTrace);
            }
        }
    }
}