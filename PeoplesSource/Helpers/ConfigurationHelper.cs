using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PeoplesSource.Helpers
{
    public class ConfigurationHelper
    {
        public static string GetTeapplixDownloadFolderPath()
        {
            return ConfigurationManager.AppSettings["TeapplixDownloadFolderPath"].ToString();
        }
        public static string GetTeapplixProductURL()
        {
            return ConfigurationManager.AppSettings["TeapplixProductURL"].ToString();
        }
        public static string GetTeapplixQuantityURL()
        {
            return ConfigurationManager.AppSettings["TeapplixQuantityURL"].ToString();
        }
        public static string GetTeapplixOrderURL()
        {
            return ConfigurationManager.AppSettings["TeapplixOrderURL"].ToString();
        }
        public static string GetSSISProductPath()
        {
            return ConfigurationManager.AppSettings["SSISProductPath"].ToString();
        }
        public static string GetSSISQuantityPath()
        {
            return ConfigurationManager.AppSettings["SSISQuantityPath"].ToString();
        }
        public static string GetSSISOrderPath()
        {
            return ConfigurationManager.AppSettings["SSISOrderPath"].ToString();
        }
    }
}