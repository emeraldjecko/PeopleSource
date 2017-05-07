using StructureMap;
//using log4net;
using System.Web.Mvc;
using System.Web.Routing;

namespace PeoplesSource
{
    /// <summary>
    /// Class CustomControllerFactory.
    /// </summary>
    public class CustomControllerFactory : DefaultControllerFactory
    {
        // MK: this is not used in the web.config anymore
        //private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates the controller.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>IController.</returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            // MK: this is not used in the web.config anymore
            //_log.Info("RawUrl: " + requestContext.HttpContext.Request.RawUrl);

            try
            {
                var controllerType = base.GetControllerType(requestContext, controllerName);
                if (null == controllerType && controllerName != "toastr.js.map")
                {
                    return base.CreateController(requestContext, controllerName);
                }
                var controller = ObjectFactory.GetInstance(controllerType) as Controller;
                controller.ActionInvoker = ObjectFactory.GetInstance<CustomActionInvoker>();
                return controller;
            }
            catch
            {
                return null;
            }
        }
    }
}