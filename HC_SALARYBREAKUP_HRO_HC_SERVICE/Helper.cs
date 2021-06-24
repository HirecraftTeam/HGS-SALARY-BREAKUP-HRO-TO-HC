using Applicant_Integration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HireCraft.HM_APIService
{
    internal class Helper
    {
        internal static string ConString
        {
            get
            {
                if (Helper.IsEncrypted == 1)
                {
                    Cryptographer crypto = new Cryptographer();
                    return crypto.opDecryptPasswordBase64(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);
                }
                else
                {
                    return ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                }
                //return ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            }
        }
        
        internal static string client_id
        {
            get
            {
                return ConfigurationManager.AppSettings["client_id"].ToString();
            }
        }
        internal static string client_secret
        {
            get
            {
                return ConfigurationManager.AppSettings["client_secret"].ToString();
            }
        }

        internal static string grant_type
        {
            get
            {
                return ConfigurationManager.AppSettings["grant_type"].ToString();
            }
        }


        internal static string CTCSalaryURL
        {
            get
            {
                return ConfigurationManager.AppSettings["CTCSalaryURL"].ToString();
            }
        }
        internal static string client_key
        {
            get
            {
                return ConfigurationManager.AppSettings["client_key"].ToString();
            }
        }

        internal static string client_code
        {
            get
            {
                return ConfigurationManager.AppSettings["client_code"].ToString();
            }
        }
        internal static string access_identifier
        {
            get
            {
                return ConfigurationManager.AppSettings["access_identifier"].ToString();
            }
        }

        internal static string Authorization
        {
            get
            {
                return ConfigurationManager.AppSettings["Authorization"].ToString();
            }
        }

        internal static string AuthorizationURL
        {
            get
            {
                return ConfigurationManager.AppSettings["AuthorizationURL"].ToString();
            }
        }

        internal static Int32 IsEncrypted
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["IsEncrypted"]);
            }
        }


        internal static Int64 JobMins
        {
            get
            {
                return Convert.ToInt64(ConfigurationManager.AppSettings["JobMins"]);
            }

        }
        

        internal static Int16 IsPushToHCtoIndium
        {
            get
            {
                return Convert.ToInt16(ConfigurationManager.AppSettings["IsPushToHCtoIndium"]);
            }

        }

        internal static Int16 IsPullFromIndiumtoHC
        {
            get
            {
                return Convert.ToInt16(ConfigurationManager.AppSettings["IsPullFromIndiumtoHC"]);
            }

        }

        internal static string EmployeeDetailsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["EmployeeDetailsUrl"];
            }

        }

        internal static string GetEmployeeDetailsFromIndium
        {
            get
            {
                return ConfigurationManager.AppSettings["GetEmployeeDetailsFromIndium"];
            }

        }

        internal static Int64 ServiceMinsRun
        {
            get
            {
                return Convert.ToInt64(ConfigurationManager.AppSettings["ServiceMinsRun"]);
            }

        }


        internal static string Accessid
        {
            get
            {
                return ConfigurationManager.AppSettings["AccessID"];
            }
        }

        internal static Boolean EnableReqSync
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableReqSync"]);
            }
        }

        


    }

}