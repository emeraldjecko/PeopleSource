using System;
using System.Web.Mvc;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class CookieHtmlHelper.
    /// </summary>
    public static class CookieHtmlHelper
    {
        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString GetCookieValue(this HtmlHelper htmlHelper, string cookieName)
        {
            return htmlHelper.GetCookieValue(cookieName, true);
        }

        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="slide">if set to <c>true</c> [slide].</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString GetCookieValue(this HtmlHelper htmlHelper, string cookieName, bool slide)
        {
            var request = htmlHelper.ViewContext.HttpContext.Request;
            var response = htmlHelper.ViewContext.HttpContext.Response;

            var cookie = request.Cookies[cookieName];

            if (cookie != null)
            {
                if (slide)
                {
                    cookie.Expires = DateTime.Now.AddHours(1);
                    response.Cookies.Set(cookie);
                }
                return MvcHtmlString.Create(cookie.Value);
            }
            return MvcHtmlString.Create(string.Empty);
        }
    }
}