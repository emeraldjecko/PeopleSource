using System.Collections.Generic;
using System.Web.Mvc;
using StructureMap;

namespace PeoplesSource
{
    /// <summary>
    /// Class CustomActionInvoker.
    /// </summary>
    public class CustomActionInvoker : ControllerActionInvoker
    {
        /// <summary>
        /// The _container
        /// </summary>
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomActionInvoker"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CustomActionInvoker(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Invokes the action method with filters.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>ActionExecutedContext.</returns>
        protected override ActionExecutedContext InvokeActionMethodWithFilters(
            ControllerContext controllerContext,
            IList<IActionFilter> filters,
            ActionDescriptor actionDescriptor,
            IDictionary<string, object> parameters)
        {
            foreach (var actionFilter in filters)
            {
                _container.BuildUp(actionFilter);
            }
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }
}