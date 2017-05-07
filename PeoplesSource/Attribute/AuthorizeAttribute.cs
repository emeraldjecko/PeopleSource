using System;
using System.Web.Mvc;
using System.Web.Routing;
using PeoplesSource.Common;
using StructureMap;

namespace PeoplesSource.Attribute
{
    /// <summary>
    /// Class AuthorizeAttribute.
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
     
        /// <summary>
        /// The _id parameter
        /// </summary>
        private readonly string _idParameter;
        /// <summary>
        /// The _id parameter property
        /// </summary>
        private readonly string _idParameterProperty;
        /// <summary>
        /// The _authorization service type
        /// </summary>
        private readonly Type _authorizationServiceType;

        /// <summary>
        /// Gets or sets the persistence.
        /// </summary>
        /// <value>The persistence.</value>
        public IPersistence Persistence { get; set; }
        /// <summary>
        /// Gets or sets the user service.
        /// </summary>
        /// <value>The user service.</value>
      

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        public AuthorizeAttribute()
            : this(null, null, null)
        {
        }

     
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="idParameter">The identifier parameter.</param>
        /// <param name="authorizationServiceType">Type of the authorization service.</param>
        public AuthorizeAttribute(string idParameter, Type authorizationServiceType)
            : this(idParameter, null, authorizationServiceType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="idParameter">The identifier parameter.</param>
        /// <param name="idParameterProperty">The identifier parameter property.</param>
        /// <param name="authorizationServiceType">Type of the authorization service.</param>
        public AuthorizeAttribute(string idParameter, string idParameterProperty, Type authorizationServiceType)
        {
            _idParameter = idParameter;
            _idParameterProperty = idParameterProperty;
            _authorizationServiceType = authorizationServiceType;
            Order = (int)ActionAttributeOrder.Authorize;
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var createdSession = false;
                if (Persistence.Session == null || !Persistence.Session.IsOpen)
                {
                    createdSession = true;
                    ObjectFactory.GetInstance<IPersistence>().OpenSession();
                }

                //is user authenticated
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    // auth failed, redirect to login page
                    filterContext.Result = new HttpUnauthorizedResult();
                }
           

                //else if (!((filterContext.Controller is ProfileController) || (filterContext.Controller is LookupController)) && !UserCompletedRequiredFields(filterContext))
                //{
                //    filterContext.Result = RedirectToAction("Index", "Profile", new { area = "User" });
                //}

                if (createdSession)
                {
                    Persistence.Session.Dispose();
                }
            }
        }

     
        /// <summary>
        /// Redirects to action.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>RedirectToRouteResult.</returns>
        private RedirectToRouteResult RedirectToAction(string actionName, string controllerName, object routeValues)
        {
            var routeValues2 = new RouteValueDictionary(routeValues);
            routeValues2["action"] = actionName;
            routeValues2["controller"] = controllerName;
            return new RedirectToRouteResult(routeValues2);
        }
    }
}
