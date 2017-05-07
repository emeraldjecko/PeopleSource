using System;
using System.Web.Mvc;
using System.Web.Security;

namespace PeoplesSource
{
    public class UserIdFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdFilter"/> class.
        /// </summary>
        public UserIdFilter()
        {
            Order = (int)ActionAttributeOrder.UserIdFilter;
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string key = "userId";

            if (filterContext.ActionParameters.ContainsKey(key))
            {
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.ActionParameters[key] = filterContext.HttpContext.User.Identity.Name;

                    MembershipUser mu = Membership.GetUser(filterContext.HttpContext.User.Identity.Name);

                    string userid11 = mu.ProviderUserKey.ToString();
                    filterContext.ActionParameters[key] = Guid.Parse(userid11);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}