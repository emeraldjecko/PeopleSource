using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Security;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class CookieHelper.
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// Gets or sets the current impact.
        /// </summary>
        /// <value>The current impact.</value>
        public static int CurrentIndicatorId
        {
            get
            {
                return ((HttpContext.Current.Request.Cookies["CurrentIndicatorId"] != null) ? Convert.ToInt32(HttpContext.Current.Request.Cookies["CurrentIndicatorId"].Value) : 1);
            }
            set
            {
                var currentIndicatorC = HttpContext.Current.Request.Cookies["CurrentIndicatorId"];

                if (currentIndicatorC == null)
                {
                    currentIndicatorC = new HttpCookie("CurrentIndicatorId");
                }
                currentIndicatorC.Value = value.ToString(CultureInfo.InvariantCulture);
                currentIndicatorC.Expires = DateTime.Now.AddHours(FormsAuthentication.Timeout.TotalHours);
                HttpContext.Current.Response.Cookies.Add(currentIndicatorC);
            }
        }

        public static HttpCookie GetCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
                return cookie;
            else
                return null;
        }

        public static int DeleteCookie(string cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                var c = new HttpCookie(cookieName);
                c.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(c);
                return 1;
            }
            return 0;
        }

        public static void CreateCookie(string cookieName, Dictionary<string, string> values)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            foreach (var item in values)
            {
                cookie[item.Key] = item.Value;
            }
            cookie.Expires = DateTime.Now.AddHours(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}