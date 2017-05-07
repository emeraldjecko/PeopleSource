using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class UrlHtmlHelper.
    /// </summary>
    public static class UrlHtmlHelper
    {

        /// <summary>
        /// Actions the with list.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="routeData">The route data.</param>
        /// <param name="protocol">The protocol.</param>
        /// <returns>System.String.</returns>
        public static string ActionWithList(this UrlHelper helper, string action, string controller, object routeData, string protocol)
        {

            var rv = new RouteValueDictionary(routeData);

            var newRv = new RouteValueDictionary();
            var arrayRv = new RouteValueDictionary();
            foreach (var kvp in rv)
            {
                var nrv = newRv;
                var val = kvp.Value;
                if (val is IEnumerable && !(val is string))
                {
                    nrv = arrayRv;
                }

                nrv.Add(kvp.Key, val);

            }

            var href = UrlHelper.GenerateUrl(null, action, controller, protocol, null, null, newRv, helper.RouteCollection, helper.RequestContext, true);
            
            foreach (var kvp in arrayRv)
            {
                var lst = kvp.Value as IEnumerable;
                var key = kvp.Key;
                foreach (var val in lst)
                {
                    href = href.AddQueryString(key, val);
                }

            }
            return href;
        }

        /// <summary>
        /// Adds the query string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string AddQueryString(this string url, string name, object value)
        {
            url = url ?? "";

            var join = '?';
            if (url.Contains('?'))
                join = '&';

            return string.Concat(url, join, name, "=", HttpUtility.UrlEncode(value.ToString()));
        }
    }
}