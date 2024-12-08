using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProfideSedayuOp.Models.Helper
{
    public static class OwinLibrary
    {
        public static string GetDB()
        {
            return ConfigurationManager.AppSettings["AppMitsui"].ToString();
        }
        public static string GetDBP()
        {
            return ConfigurationManager.AppSettings["AppDBP"].ToString();
        }

        public static string GetAPIMTF()
        {
            return ConfigurationManager.AppSettings["AppAPIMTF"].ToString();
        }
        public static string GetAPIMitsu()
        {
            return ConfigurationManager.AppSettings["AppAPIMitsui"].ToString();
        }
        public static string GetAPILokal()
        {
            return ConfigurationManager.AppSettings["AppAPILokal"].ToString();
        }
        public static string APIKey()
        {
            return ConfigurationManager.AppSettings["AppAPIKey"].ToString();
        }
        public static string APIParam()
        {
            return ConfigurationManager.AppSettings["AppAPIParam"].ToString();
        }
    }
}