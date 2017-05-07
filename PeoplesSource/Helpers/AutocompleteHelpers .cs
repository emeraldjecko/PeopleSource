using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PeoplesSource.Helpers
{
    public static class AutocompleteHelpers
    {
        public static MvcHtmlString AutocompleteFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string autoCompleteActionName, string selectActionName, string controllerName, string callBackFunction)
        {
            string autocompleteUrl = UrlHelper.GenerateUrl(null, autoCompleteActionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);

            string selectUrl = UrlHelper.GenerateUrl(null, selectActionName, controllerName,
                                                          null,
                                                          html.RouteCollection,
                                                          html.ViewContext.RequestContext,
                                                          includeImplicitMvcValues: true);

            //var myAttributes = new Dictionary<string, object>{
            //    {"zip", "value"},
            //    {"test2", "value2"},
            //};

            return html.TextBoxFor(expression, new { data_autocomplete_url = autocompleteUrl, data_select_url = selectUrl, data_callbackfunction = callBackFunction });
        }
    }
}