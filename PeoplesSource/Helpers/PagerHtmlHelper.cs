using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace PeoplesSource.Helpers
{
    /// <summary>
    /// Class PagerHtmlHelper.
    /// </summary>
    public static class PagerHtmlHelper
    {
        #region Pager

        /// <summary>
        /// Pagers the specified HTML helper.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="pageInfo">The page information.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString Pager(this HtmlHelper htmlHelper, string name, PageInfo pageInfo, Func<int, object> attributes)
        {
            var isFirstPageEnabled = false;
            var isPreviousPageEnabled = false;
            var isNextPageEnabled = false;
            var isLastPageEnabled = false;

            const int bracketSize = 10;

            var numberOfPages = Convert.ToInt32(Math.Ceiling(pageInfo.Total / (decimal)pageInfo.PageSize));

            var bracketStart = 1;
            var bracketEnd = (bracketStart + bracketSize - 1);

            // If necessary slide the visible page numbers to the right5
            if (pageInfo.Page > (bracketSize / 2))
            {
                bracketStart = pageInfo.Page - (bracketSize / 2);
                bracketEnd = bracketStart + bracketSize;
            }
            // If visible page number end is too big then set as last page
            if (bracketEnd > numberOfPages)
            {
                bracketEnd = numberOfPages;
            }

            if (bracketStart > (numberOfPages - bracketSize))
            {
                if (numberOfPages <= bracketSize)
                {
                    bracketStart = 1;
                }
                else
                {
                    bracketStart = numberOfPages - bracketSize;
                }
            }


            if (pageInfo.Total > 0)
            {
                isFirstPageEnabled = 1 != pageInfo.Page && bracketStart != 1;
                isPreviousPageEnabled = 1 != pageInfo.Page;

                isNextPageEnabled = numberOfPages != pageInfo.Page;
                isLastPageEnabled = numberOfPages != pageInfo.Page && numberOfPages != bracketEnd;
            }

            var builder = new StringBuilder();
            var pagerLinks = new List<string>();

            if (pageInfo.Total > pageInfo.PageSize)
            {
                //builder.Append("<div class=\"pager\">");

                // First //
                if (isFirstPageEnabled)
                {
                    pagerLinks.Add(PagerLink(htmlHelper, "First Page", attributes(1), new { title = "Go to the first page" }));
                }

                // Previous //
                if (isPreviousPageEnabled)
                {
                    pagerLinks.Add(PagerLink(htmlHelper, "Prev", attributes(pageInfo.Page - 1), new { title = "Go to the previous page" }));
                }

                // Pages //
                for (var i = bracketStart; i <= bracketEnd; i++)
                {
                    var title = string.Concat("Go to page ", i.ToString(CultureInfo.InvariantCulture));
                    if (pageInfo.Page == i)
                    {
                        pagerLinks.Add(PagerLink(htmlHelper, i.ToString(CultureInfo.InvariantCulture), attributes(i), new { @class = "selected", title, style = " background:#f05a28" }));
                    }
                    else pagerLinks.Add(PagerLink(htmlHelper, i.ToString(CultureInfo.InvariantCulture), attributes(i), new { title }));
                }

                // Next //
                if (isNextPageEnabled)
                {
                    pagerLinks.Add(PagerLink(htmlHelper, "Next", attributes(pageInfo.Page + 1), new { title = "Go to the next page" }));
                }

                // Last //
                if (isLastPageEnabled)
                {
                    pagerLinks.Add(PagerLink(htmlHelper, "Last Page", attributes(numberOfPages), new { title = "Go to the last page" }));
                }

                builder.Append(string.Join("&nbsp;", pagerLinks.ToArray()));

                builder.Append("</div>");

                //if (pageInfo.Total > 0)
                //{
                //    builder.Append("<div class=\"pager_summary\">");
                //    builder.Append(string.Format(
                //    "{0} - {1} of {2} results",
                //    (pageInfo.Page - 1) * pageInfo.PageSize + 1,
                //    (pageInfo.Page * pageInfo.PageSize) > pageInfo.Total ? pageInfo.Total : (pageInfo.Page * pageInfo.PageSize),
                //    pageInfo.Total));
                //    builder.Append("</div>");
                //}
            }
            else
            {
                builder.Append("<div class=\"pager_summary\">");
                if (pageInfo.Total > 0)
                {
                    builder.Append(string.Format("{0} result(s)", pageInfo.Total));
                }
                builder.Append("</div>");
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// Pagers the link.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="userAttributes">The user attributes.</param>
        /// <param name="pagerAttributes">The pager attributes.</param>
        /// <returns>System.String.</returns>
        private static string PagerLink(HtmlHelper htmlHelper, string linkText, object userAttributes, object pagerAttributes)
        {
            var attributes = new Dictionary<string, object>();
            foreach (var kvp in new RouteValueDictionary(userAttributes) as IDictionary<string, object>)
            {
                attributes.MergeAttribute(kvp);
            }
            foreach (var kvp in new RouteValueDictionary(pagerAttributes) as IDictionary<string, object>)
            {
                attributes.MergeAttribute(kvp);
            }

            var link = htmlHelper.ActionLink(string.Concat("*", linkText, "*"), "", null, attributes).ToString();
            return link.Replace("*", "&nbsp;");
        }

        /// <summary>
        /// Merges the attribute.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="attribute">The attribute.</param>
        private static void MergeAttribute(this IDictionary<string, object> dictionary, KeyValuePair<string, object> attribute)
        {
            if (dictionary.ContainsKey(attribute.Key))
            {

                dictionary[attribute.Key] = dictionary[attribute.Key] + " " + attribute.Value;
            }
            else
            {
                dictionary.Add(attribute.Key, attribute.Value);
            }
        }

        #endregion

        #region ViewData

        /// <summary>
        /// Sets the specified view data dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewDataDictionary">The view data dictionary.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        public static void Set<T>(this ViewDataDictionary viewDataDictionary, T value, string type = "")
        {
            var key = type + typeof(T).GUID;
            if (viewDataDictionary.ContainsKey(key))
            {
                viewDataDictionary[key] = value;
            }
            else
            {
                viewDataDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the specified view data dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewDataDictionary">The view data dictionary.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// ``0.
        /// </returns>
        public static T Get<T>(this ViewDataDictionary viewDataDictionary, string type = "")
        {
            var key = type + typeof(T).GUID;
            if (!viewDataDictionary.ContainsKey(key))
            {
                return default(T);
            }
            return (T)viewDataDictionary[key];
        }

        #endregion

    }
}
