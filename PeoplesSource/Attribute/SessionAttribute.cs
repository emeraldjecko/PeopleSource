using System.Web.Mvc;
using  PeoplesSource.Common;

namespace PeoplesSource.Attribute
{
    /// <summary>
    /// Class SessionAttribute.
    /// </summary>
    public class SessionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets the persistence.
        /// </summary>
        /// <value>The persistence.</value>
        public IPersistence Persistence { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionAttribute"/> class.
        /// </summary>
        public SessionAttribute()
        {
            Order = (int)ActionAttributeOrder.Session;
        }

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Persistence.Session == null || !Persistence.Session.IsOpen)
            {
                Persistence.OpenSession();
                Persistence.Session.BeginTransaction(System.Data.IsolationLevel.Serializable);
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Called when [result executed].
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                base.OnResultExecuted(filterContext);
            }
            finally
            {
                if (!filterContext.IsChildAction)
                {
                    if (Persistence.Session != null)
                    {
                        Persistence.Session.Transaction.Commit();
                        Persistence.Session.Transaction.Dispose();
                        Persistence.Session.Dispose();
                    }
                }
            }
        }

    }
}