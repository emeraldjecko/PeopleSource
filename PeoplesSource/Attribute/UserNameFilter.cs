using System.Web.Mvc;

namespace PeoplesSource.Attribute
{
    /// <summary>
    /// Class UserNameFilter.
    /// </summary>
    public class UserNameFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNameFilter"/> class.
        /// </summary>
        public UserNameFilter()
        {
            Order = (int)ActionAttributeOrder.UserNameFilter;
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string key = "userName";

            if (filterContext.ActionParameters.ContainsKey(key))
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.ActionParameters[key] = filterContext.HttpContext.User.Identity.Name;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}