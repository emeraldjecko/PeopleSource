using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace PeoplesSource.Helpers
{
    public class APICall
    {
        /// <summary>
        /// Call the HttpWebRequest Method along with Developers details for Ebay Api
        /// </summary>
        /// <param name="apirequest">
        /// Input Request Body
        /// </param>
        /// <param name="callname">
        /// X-EBAY-API-CALL-NAME to distingush and find out the type of request
        /// </param>
        /// <param name="RequestType">
        /// POST or GET
        /// </param>
        /// <param name="error">
        /// Error Message 
        /// </param>
        /// <returns></returns>
        public static XmlDocument MakeAPIRequest(string apirequest, string callname, string RequestType, string error,
            bool useProxy = false, string ProxyIP = "", string ProxyPort = "", string ProxyUserName = "", string ProxyPassword = "")
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                string APIServerURL = ConfigurationManager.AppSettings["ebayApiUrl"];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(APIServerURL);

                if (useProxy && !string.IsNullOrEmpty(ProxyIP) && !string.IsNullOrEmpty(ProxyPort))
                {
                    WebProxy proxy = new WebProxy();

                    proxy.Address = new Uri("http://" + ProxyIP + ":" + ProxyPort + "");
                    proxy.BypassProxyOnLocal = false;
                    if (!string.IsNullOrEmpty(ProxyUserName) && !string.IsNullOrEmpty(ProxyPassword))
                    {
                        proxy.Credentials = new NetworkCredential(ProxyUserName, ProxyPassword);
                    }
                    else
                    {
                        proxy.UseDefaultCredentials = true;
                    }
                    request.Proxy = proxy;
                }

                request.Headers.Add("X-EBAY-API-DEV-NAME", ConfigurationManager.AppSettings["DevID"]);
                request.Headers.Add("X-EBAY-API-APP-NAME", ConfigurationManager.AppSettings["AppID"]);
                request.Headers.Add("X-EBAY-API-CERT-NAME", ConfigurationManager.AppSettings["CertID"]);

                request.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", ConfigurationManager.AppSettings["Version"]);
                request.Headers.Add("X-EBAY-API-SITEID", ConfigurationManager.AppSettings["SiteID"]);
                request.Headers.Add("X-EBAY-API-CALL-NAME", callname);
                request.Method = "POST";
                request.ContentType = "text/xml";
                UTF8Encoding encoding = new UTF8Encoding();
                int dataLen = encoding.GetByteCount(apirequest);
                byte[] utf8Bytes = new byte[dataLen];
                Encoding.UTF8.GetBytes(apirequest, 0, apirequest.Length, utf8Bytes, 0);

                Stream str = null;
                str = request.GetRequestStream();
                str.Write(utf8Bytes, 0, utf8Bytes.Length);
                str.Close();
                WebResponse response = request.GetResponse();
                str = response.GetResponseStream();

                StreamReader sr = new StreamReader(str);
                xmlDoc.LoadXml(sr.ReadToEnd());
                sr.Close();
                str.Close();
            }
            catch (Exception Ex)
            {
                error = Ex.Message;
                Console.Write("\nError Occurs in APICall.cs File..");
                Console.Write("\nError Message : "+Ex.Message+"");
            }

            return xmlDoc;
        }
    }
}